Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Data
Imports System.Linq
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Windows.Markup
Imports System.Xaml
Imports System.Globalization
Imports SUT.PrintEngine.Utils
Imports System.Threading.Tasks

Class Page_Busqueda

    Dim invetnService As New InventSoftService
    Dim busquedaAvzModel As BusquedaAvzModel

    Dim mostraraBusquedaAvz As Boolean = False
    Dim mainForm As MainWindow
    Dim ximagenes As New List(Of RutaImageInvent)
    Dim backGroundWorker1 As BackgroundWorker
    Dim productoSeleccionado As Vw_Opr_Productos

    Dim catalogoFamilias As New List(Of Cat_Familia)
    Dim catalogoMarcas As New List(Of Cat_Marca)
    Dim catalogoTipos As New List(Of Cat_Tipo)
    Dim catalogoUbicaciones As New List(Of Cat_Ubicaciones)
    Dim catalogoAseguradoras As New List(Of Cat_aseguradora)

    Public Sub New(m As MainWindow)
        InitializeComponent()
        Me.busquedaAvzModel = New BusquedaAvzModel
        Me.grid_avanza.DataContext = busquedaAvzModel
        mainForm = m
    End Sub
    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        CargarCatalagos()
        Limpiar_UI()
    End Sub
    Public Sub WorkerImagenes()
        '******** Hilo para cargar las imagenes de la pieza seleccionada ********
        busyIndicator1.IsBusy = True

        img1.Source = Nothing
        'For Each xi As Image In sp_imagenes.Children
        '    xi.Source = Nothing
        'Next
        sp_imagenes.Children.Clear()
        ximagenes = New List(Of RutaImageInvent)

        backGroundWorker1 = New BackgroundWorker
        backGroundWorker1.WorkerReportsProgress = True
        AddHandler backGroundWorker1.DoWork, AddressOf backGroundWorker1_doWork
        AddHandler backGroundWorker1.ProgressChanged, AddressOf backGroundWorker1_reportProgres
        backGroundWorker1.RunWorkerAsync()
    End Sub
    Public Sub Limpiar_UI()
        grd_venta.ItemsSource = Nothing
        txt_buscar.Text = ""

        busquedaAvzModel.Anaquel = ""
        busquedaAvzModel.Codigo = ""
        busquedaAvzModel.Descripcion = ""
        busquedaAvzModel.Id_Aseguradora = 0
        busquedaAvzModel.Id_Familia = 0
        busquedaAvzModel.Id_Marca = 0
        busquedaAvzModel.Id_Tipo = 0
        busquedaAvzModel.Id_Ubicacion = 0
        busquedaAvzModel.Modelo = ""
        busquedaAvzModel.Numero_Pieza = ""
        busquedaAvzModel.Placa = ""

    End Sub
    Private Sub CargarCatalagos()
        Using dbContext As New InventarioSoftDB
            catalogoFamilias = dbContext.Cat_Familia.Where(Function(item) item.activo = True).ToList()
            catalogoFamilias.First().Descripcion = "TODOS"

            catalogoMarcas = dbContext.Cat_Marca.Where(Function(item) item.activo = True).ToList()
            catalogoMarcas.First().Descripcion = "TODOS"

            catalogoTipos = dbContext.Cat_Tipo.Where(Function(item) item.activo = True).ToList()
            catalogoTipos.First().Descripcion = "TODOS"

            catalogoUbicaciones = dbContext.Cat_Ubicaciones.ToList()
            catalogoUbicaciones.First().descripcion = "TODOS"

            catalogoAseguradoras = dbContext.Cat_aseguradora.Where(Function(item) item.activo = True).ToList()
            catalogoAseguradoras.First().Descripcion = "TODOS"
        End Using

        avz_C_fam.ItemsSource = catalogoFamilias
        avz_c_ubic.ItemsSource = catalogoUbicaciones
    End Sub


    '*********** EVENTOS ***********
    Private Async Sub Busqueda_Click(sender As Object, e As RoutedEventArgs) Handles btn_busquedaN.Click, btn_busquedaAvz.Click
        grd_venta.ItemsSource = Nothing

        busyIndicator1.IsBusy = True
        Me.Cursor = Cursors.Wait
        Await Task.Delay(100)

        Dim itemsBusqueda As New List(Of Vw_Opr_Productos)
        If (sender.Name = "btn_busquedaN") Then
            itemsBusqueda = invetnService.BusquedaBasica(txt_buscar.Text).ToList
        End If
        If (sender.Name = "btn_busquedaAvz") Then
            itemsBusqueda = invetnService.BusquedaAvanzada(busquedaAvzModel).ToList
        End If
        grd_venta.ItemsSource = itemsBusqueda
        lbl_articulos.Content = itemsBusqueda.Count

        Await Task.Delay(100)
        Me.Cursor = Cursors.Arrow
        busyIndicator1.IsBusy = False

    End Sub
    Private Sub dataGrid_selectChanged() Handles grd_venta.SelectionChanged
        Try
            productoSeleccionado = CType(grd_venta.SelectedItem, Vw_Opr_Productos)
            WorkerImagenes()
        Catch ex As Exception
            'Dim xf = New msg_error("Error", ex)
        End Try
    End Sub
    Private Sub txt_buscar_keyDown(sender As Object, e As KeyEventArgs) Handles txt_buscar.KeyDown
        If (e.Key = Key.Enter) Then
            Busqueda_Click(btn_busquedaN, Nothing)
            btn_busquedaN.Focus()
        Else
            grd_venta.ItemsSource = Nothing
        End If
    End Sub

    Private Sub Activar_BusquedaAvanzada() Handles btn_cambiarBusqueda.Click
        Limpiar_UI()

        If (mostraraBusquedaAvz) Then
            RootLayout.RowDefinitions(1).Height = New System.Windows.GridLength(50)
            grid_avanza.Visibility = Visibility.Collapsed
            grid_rap.Visibility = Visibility.Visible
        Else
            RootLayout.RowDefinitions(1).Height = New System.Windows.GridLength(120)
            grid_avanza.Visibility = Visibility.Visible
            grid_rap.Visibility = Visibility.Collapsed
        End If

        mostraraBusquedaAvz = Not mostraraBusquedaAvz
    End Sub

    Private Sub BotonExportarExcel_Click(sender As Object, e As RoutedEventArgs) Handles btn_exportExcel.Click
        Dim datos As IEnumerable(Of Vw_Opr_Productos) = grd_venta.ItemsSource
        If IsNothing(datos) Then
            Return
        End If

        If datos.Count <= 0 Then
            Return
        End If

        Me.busyIndicator1.IsBusy = True
        Dim exportarDatos As New ExportarBusquedaExcel(datos)
        exportarDatos.Exportar()
        Me.busyIndicator1.IsBusy = False

    End Sub

    '*********** BOTONES ACCIONES ***********
    Private Sub btnEliminar_click() Handles btn_eliminar.Click
        Try
            Dim xbaja As New DarBaja_Confirmar
            xbaja.id_producto = productoSeleccionado.id_producto

            If (xbaja.ShowDialog()) Then
                grd_venta.ItemsSource = Nothing
            End If

            Busqueda_Click(btn_busquedaN, Nothing)
            img1.Source = Nothing
        Catch ex As Exception
            Dim xm As New msg_information("SELECCIONE UNA ELEMENTO")
        End Try
    End Sub
    Private Sub btnNuevo_click() Handles btn_nuevo.Click
        mainForm.NuevaPieza()
    End Sub
    Private Sub btn_editar_click() Handles btn_editar.Click
        Try
            mainForm.EditarPieza(productoSeleccionado.id_producto)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btn_ajustar_click() Handles btn_ajustar.Click
        Try
            mainForm.AjustarInventarioPieza(productoSeleccionado.id_producto)
        Catch ex As Exception
        End Try
    End Sub



    '*********** IMAGENES ***********
    Private Sub Img1_click() Handles img1.MouseLeftButtonDown
        Try
            Dim tmpIndex As Integer = Integer.Parse(img1.Tag.ToString)
            'Dim xvisor As New Visor_Imagen(ximagenes.ElementAt(tmpIndex))
            Dim xvisor As New Visor_Imagen(ximagenes, tmpIndex)
            xvisor.cambiarTitulo(productoSeleccionado.descripcion)
            xvisor.ShowDialog()
        Catch ex As Exception
        End Try
    End Sub
    Public Sub Actualizar_panelImagenes()
        Try
            tb_totalImagenes.Text = String.Format("{0} Imagenes", ximagenes.Count)
            sp_imagenes.Children.Clear()

            Dim index = 0
            For Each xRutaImg As RutaImageInvent In ximagenes

                Dim imageControl As New Image
                imageControl.Margin = New Thickness(2, 2, 2, 2)

                '**** cargar imagen en memoria
                Using xstream = File.OpenRead(xRutaImg.ImagenPeque)
                    Dim tmpImage As New BitmapImage()
                    tmpImage.BeginInit()
                    tmpImage.CacheOption = BitmapCacheOption.OnLoad
                    tmpImage.StreamSource = xstream
                    tmpImage.EndInit()
                    imageControl.Source = tmpImage
                End Using
                imageControl.Tag = index

                AddHandler imageControl.MouseLeftButtonDown, AddressOf PanelImagenes_fotoClick
                sp_imagenes.Children.Add(imageControl)
                index = index + 1
            Next
            If sp_imagenes.Children.Count > 0 Then
                img1.Source = CType(sp_imagenes.Children(0), Image).Source
                img1.Tag = CType(sp_imagenes.Children(0), Image).Tag
            End If

        Catch ex As Exception
            'Dim x As New msg_error("ALGO SALIO MAL", ex)
        End Try
    End Sub
    Private Sub PanelImagenes_fotoClick(sender As Object, e As MouseButtonEventArgs)
        Me.img1.Source = sender.source
        Me.img1.Tag = sender.Tag
    End Sub


    '*********** Eventos BackgroundWorker ***********
    Private Sub backGroundWorker1_doWork(sender As Object, e As DoWorkEventArgs)

        Try
            Dim ds2 As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto"}, {"CARGARIMAGENES", productoSeleccionado.id_producto}).Fill(ds2)

            Dim i = 0

            For Each r As DataRow In ds2.Tables(0).Rows

                Using ms As New MemoryStream(CType(r.Item("imagen"), Byte()))
                    Dim imagenOriginal As New System.Drawing.Bitmap(ms)
                    Dim imagenReducido As System.Drawing.Bitmap

                    '******** Generar imagen reducida ********
                    Dim xw = 170
                    Dim xh = 170
                    imagenReducido = New System.Drawing.Bitmap(xw, xh)
                    Using g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(imagenReducido)
                        g.DrawImage(imagenOriginal, 0, 0, xw, xh)
                    End Using

                    '******** generar rutas de las imagenes ********
                    Dim urlOriginal = String.Format("{0}inventSoft/image_{1}.jpg", System.IO.Path.GetTempPath(), i)
                    Dim urlReducido = String.Format("{0}inventSoft/image_{1}_{2}x{3}.jpg", System.IO.Path.GetTempPath(), i, xw, xh)

                    '******** guardar imagen original ********
                    Dim xfileO As New FileInfo(urlOriginal)
                    xfileO.Directory.Create()

                    Using xfileStream = xfileO.OpenWrite
                        imagenOriginal.Save(xfileStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                        xfileStream.Close()
                    End Using

                    '******** guardar imagen reducida ********
                    Dim xfileP As New FileInfo(urlReducido)
                    Using xfileStream = xfileP.OpenWrite
                        imagenReducido.Save(xfileStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                        xfileStream.Close()
                    End Using

                    '******** guardar las rutas en la lista ********
                    CType(sender, BackgroundWorker).ReportProgress(0, urlOriginal & ";" & urlReducido)
                    i = i + 1

                End Using

            Next

        Catch ex As Exception
            CType(sender, BackgroundWorker).ReportProgress(-1, ex)
        End Try

        System.Threading.Thread.Sleep(200)
        CType(sender, BackgroundWorker).ReportProgress(1)

    End Sub
    Private Sub backGroundWorker1_reportProgres(sender As Object, e As ProgressChangedEventArgs)
        Select Case e.ProgressPercentage
            Case 0
                Dim tmpUrl = e.UserState.ToString.Split(";")
                ximagenes.Add(New RutaImageInvent(tmpUrl(0), tmpUrl(1)))

            Case 1
                Actualizar_panelImagenes()
                busyIndicator1.IsBusy = False

                'Case -1
                'Dim xf As New msg_error("Error al cargar datos", CType(e.UserState, Exception))
        End Select

    End Sub


End Class