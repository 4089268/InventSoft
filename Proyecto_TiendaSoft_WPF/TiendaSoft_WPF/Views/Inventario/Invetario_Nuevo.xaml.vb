Imports System
Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports System.ComponentModel

Class Invetario_Nuevo

    Dim ximagenes As New List(Of RutaImageInvent)

    Dim backWorker As BackgroundWorker
    Dim ListaParametros As New List(Of SqlParameter)

    Private Sub rootLayout_loaded() Handles rootLayout.Loaded
        cargarCatalagos()
        limpiarCampos()
        tb_modelo.Text = DateTime.Now.Year
    End Sub

    Private Sub cargarCatalagos()
        Dim dataFamilia As New DataSet
        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "FAMILIAS"}).Fill(dataFamilia)
        cb_Familia.ItemsSource = dataFamilia.Tables(0).DefaultView

        Dim dataMarca As New DataSet
        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "MARCA"}).Fill(dataMarca)
        cb_Marca.ItemsSource = dataMarca.Tables(0).DefaultView

        Dim dataUbicacion As New DataSet
        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "UBICACIONES"}).Fill(dataUbicacion)
        cb_Ubicacion.ItemsSource = dataUbicacion.Tables(0).DefaultView

        Dim dataAseguradora As New DataSet
        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "ASEGURADORA"}).Fill(dataAseguradora)
        cb_aseguradora.ItemsSource = dataAseguradora.Tables(0).DefaultView

    End Sub
    Private Sub cargarCatalagos(tipo As String)
        Select Case tipo
            Case "Familia"
                Dim dataFamilia As New DataSet
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "FAMILIAS"}).Fill(dataFamilia)
                cb_Familia.ItemsSource = dataFamilia.Tables(0).DefaultView
                cb_Familia.SelectedIndex = 0

            Case "Marca"
                Dim dataMarca As New DataSet
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "MARCA"}).Fill(dataMarca)
                cb_Marca.ItemsSource = dataMarca.Tables(0).DefaultView
                cb_Marca.SelectedIndex = 0

            Case "Ubicacion"
                Dim dataUbicacion As New DataSet
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "UBICACIONES"}).Fill(dataUbicacion)
                cb_Ubicacion.ItemsSource = dataUbicacion.Tables(0).DefaultView
                cb_Ubicacion.SelectedIndex = 0

            Case "Aseguradora"
                Dim dataAseguradora As New DataSet
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "ASEGURADORA"}).Fill(dataAseguradora)
                cb_aseguradora.ItemsSource = dataAseguradora.Tables(0).DefaultView
                cb_aseguradora.SelectedIndex = 0

        End Select

    End Sub
    Private Sub cargarCatalago_Tipo() Handles cb_Marca.SelectionChanged
        Try
            Dim dataTipo As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xid_marca"}, {"CARGARTIPOXMARCA", cb_Marca.SelectedValue}).Fill(dataTipo)

            cb_Tipo.ItemsSource = dataTipo.Tables(0).DefaultView
        Catch ex As Exception
        End Try

    End Sub
    Private Sub InicializarWorker()
        busyIndicator1.IsBusy = True
        Me.backWorker = New BackgroundWorker
        backWorker.WorkerReportsProgress = True
        AddHandler backWorker.DoWork, AddressOf Worker_onWork
        AddHandler backWorker.ProgressChanged, AddressOf Worker_reportProgress
        backWorker.RunWorkerAsync()
    End Sub


    '********* BOTON AGREGAR *********
    Private Sub AgregarNuevaPieza_click() Handles btn_agregar.Click
        If (tb_Cantidad.Text <> "" And tB_descripcion.Text <> "" And tb_Cantidad.Text <> "" And cb_Familia.SelectedValue >= 0 And cb_Tipo.SelectedValue >= 0 And tb_modelo.Text <> "" And cb_aseguradora.SelectedValue >= 0 And cb_Ubicacion.SelectedValue >= 0 And cb_Marca.SelectedValue >= 0) Then

            '***** Valirdar que los modelos sean numeros de 4 digitos *****
            Dim regex As New System.Text.RegularExpressions.Regex("^[0-9]{4}$")
            If Not regex.IsMatch(tb_modelo.Text) Then
                Dim x As New msg_information("SOLO SE PERMITEN MODELOS DE 4 DIGITOS NUMERICOS")
                tb_modelo.Focus()
                Return
            End If

            If tb_modelo2.Text <> "" And Not regex.IsMatch(tb_modelo2.Text) Then
                Dim x As New msg_information("SOLO SE PERMITEN MODELOS DE 4 DIGITOS NUMERICOS")
                tb_modelo2.Focus()
                Return
            End If


            '****** Generar cadena de modelos ******
            Dim xmodelos As String = ""
            If tb_modelo2.Text <> "" Then
                Dim xm1 As Integer = Int(tb_modelo.Text)
                Dim xm2 As Integer = Int(tb_modelo2.Text)

                If (xm1 < xm2) Then
                    For cont As Integer = xm1 To xm2
                        xmodelos = xmodelos & cont
                        If (cont <> xm2) Then
                            xmodelos = xmodelos & ","
                        End If
                    Next
                Else
                    xmodelos = xm1 & ""
                End If
            Else
                xmodelos = tb_modelo.Text
            End If

            '****** Generando lista de parametros ******
            ListaParametros = New List(Of SqlParameter)
            ListaParametros.Add(New SqlParameter("@cAlias", "NUEVO"))
            ListaParametros.Add(New SqlParameter("@Codigo", tb_Codigo.Text))
            ListaParametros.Add(New SqlParameter("@Descripcion", tB_descripcion.Text))
            ListaParametros.Add(New SqlParameter("@existen", tb_Cantidad.Text))
            ListaParametros.Add(New SqlParameter("@id_familia", cb_Familia.SelectedValue))
            ListaParametros.Add(New SqlParameter("@id_marca", cb_Marca.SelectedValue))
            ListaParametros.Add(New SqlParameter("@id_tipo", cb_Tipo.SelectedValue))
            ListaParametros.Add(New SqlParameter("@id_aseguradora", cb_aseguradora.SelectedValue))
            ListaParametros.Add(New SqlParameter("@id_ubicacion", cb_Ubicacion.SelectedValue))
            ListaParametros.Add(New SqlParameter("@modelo", xmodelos))
            ListaParametros.Add(New SqlParameter("@xplaca", tb_placa.Text))
            ListaParametros.Add(New SqlParameter("@Numero_Pieza", tb_numPieza.Text))
            ListaParametros.Add(New SqlParameter("@anaquel", tb_anaquel.Text))

            InicializarWorker()

        Else
            Dim x As New msg_information("Verifique que los campos esten llenos.")
        End If

    End Sub


    '********* Validar Entradas *********
    Private Sub validar_numbers(sender As Object, e As TextCompositionEventArgs) Handles tb_Cantidad.PreviewTextInput, tb_modelo.PreviewTextInput, tb_modelo2.PreviewTextInput
        Try
            Dim regex As New System.Text.RegularExpressions.Regex("[^0-9]+")

            e.Handled = regex.IsMatch(e.Text)
        Catch ex As Exception
        End Try

    End Sub
    Public Sub nuevosCatalogos(sender As Object, e As RoutedEventArgs) Handles btn_nFamilias.Click, btn_nAsegu.Click, btn_nMarca.Click, btn_nUbic.Click
        Dim xnuevo As Inventario_NuevoCatalago
        Dim n As String = sender.name

        Select Case n
            Case "btn_nFamilias"
                xnuevo = New Inventario_NuevoCatalago("Familia")
                If (xnuevo.ShowDialog) Then
                    cargarCatalagos("Familia")
                End If
            Case "btn_nMarca"
                xnuevo = New Inventario_NuevoCatalago("Marca")
                If (xnuevo.ShowDialog) Then
                    cargarCatalagos("Marca")
                End If
            Case "btn_nUbic"
                xnuevo = New Inventario_NuevoCatalago("Ubicacion")
                If (xnuevo.ShowDialog) Then
                    cargarCatalagos("Ubicacion")
                End If
            Case "btn_nAsegu"
                xnuevo = New Inventario_NuevoCatalago("Aseguradora")
                If (xnuevo.ShowDialog) Then
                    cargarCatalagos("Aseguradora")
                End If
        End Select
        Try

        Catch ex As Exception

        End Try

    End Sub
    Private Sub limpiarCampos()
        tb_Codigo.Text = ""
        tB_descripcion.Text = ""
        tb_Cantidad.Text = "1"
        tb_modelo.Text = ""
        tb_modelo2.Text = ""
        tb_numPieza.Text = ""
        tb_placa.Text = ""
        tb_anaquel.Text = ""

        cb_aseguradora.SelectedIndex = 0
        cb_Familia.SelectedIndex = 0
        cb_Marca.SelectedIndex = 0
        cb_Tipo.SelectedIndex = 0
        cb_Ubicacion.SelectedIndex = 0
        img1.Source = Nothing

        ximagenes.Clear()
        sp_imagenes.Children.Clear()
    End Sub


    '********** IMAGENES **********
    Private Sub CargarArchivoImagen() Handles btn_cargarImagen.Click
        Try
            Dim openFile As New System.Windows.Forms.OpenFileDialog
            openFile.Multiselect = True
            openFile.Title = "Cargar Imagen"
            openFile.Filter = "Todos(*.*)|*.*|Imagenes|*.jpg;*.gif;*.png;*.bmp"
            If (openFile.ShowDialog()) Then

                Dim i As Integer = 0
                For Each xFileName In openFile.FileNames

                    Using ms As New MemoryStream(File.ReadAllBytes(xFileName))
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
                        Dim rnd As New Random
                        Dim nombre = String.Format("img{0}{1}{2}{3}{4}{5}", rnd.Next(666), rnd.Next(666), rnd.Next(666), rnd.Next(666), rnd.Next(666), rnd.Next(666))
                        Dim urlOriginal = String.Format("{0}inventSoft/{1}.jpg", System.IO.Path.GetTempPath(), nombre)
                        Dim urlReducido = String.Format("{0}inventSoft/{1}_{2}x{3}.jpg", System.IO.Path.GetTempPath(), nombre, xw, xh)

                        '******** guardar imagen original ********
                        Dim xfileO As New FileInfo(urlOriginal)
                        xfileO.Directory.Create()
                        Using xfileStream = xfileO.OpenWrite
                            imagenOriginal.Save(xfileStream, Drawing.Imaging.ImageFormat.Jpeg)
                            xfileStream.Close()
                        End Using

                        '******** guardar imagen reducida ********
                        Dim xfileP As New FileInfo(urlReducido)
                        xfileP.Directory.Create()
                        Using xfileStream = xfileP.OpenWrite
                            imagenReducido.Save(xfileStream, Drawing.Imaging.ImageFormat.Jpeg)
                            xfileStream.Close()
                        End Using

                        '******** guardar las rutas en la lista ********
                        ximagenes.Add(New RutaImageInvent(urlOriginal, urlReducido))

                        i = i + 1

                    End Using

                Next
                Actualizar_panelImagenes()

            End If

        Catch ex As Exception
            Dim xf As New msg_error("Error al cargar las imagenes", ex)
        End Try

    End Sub
    Public Sub ImagenPrevisualizar_click() Handles img1.MouseLeftButtonDown
        Try
            Dim tmpIndex As Integer = Integer.Parse(img1.Tag.ToString)
            'Dim xvisor As New Visor_Imagen(ximagenes.ElementAt(tmpIndex))
            Dim xvisor As New Visor_Imagen(ximagenes, tmpIndex)
            xvisor.cambiarTitulo(tB_descripcion.Text)
            xvisor.ShowDialog()
        Catch ex As Exception
        End Try

    End Sub
    Public Sub Actualizar_panelImagenes()
        Try
            sp_imagenes.Children.Clear()
            If (ximagenes.Count > 0) Then
                Dim index = 0
                For Each xRutaImage As RutaImageInvent In ximagenes

                    Dim imageControl As New Image
                    imageControl.Margin = New Thickness(2, 2, 2, 2)
                    Using xStream = File.OpenRead(xRutaImage.ImagenPeque)
                        Dim xtmpImage As New BitmapImage
                        xtmpImage.BeginInit()
                        xtmpImage.CacheOption = BitmapCacheOption.OnLoad
                        xtmpImage.StreamSource = xStream
                        xtmpImage.EndInit()
                        imageControl.Source = xtmpImage
                    End Using
                    imageControl.Tag = index

                    AddHandler imageControl.MouseLeftButtonDown, AddressOf PanelImagenes_fotoClick
                    AddHandler imageControl.MouseRightButtonDown, AddressOf PanelImagenes_fotoRigthClick

                    sp_imagenes.Children.Add(imageControl)
                    index = index + 1
                Next
                img1.Source = CType(sp_imagenes.Children(0), Image).Source
                img1.Tag = CType(sp_imagenes.Children(0), Image).Tag
            End If

        Catch ex As Exception
            Dim x As New msg_error("Algo salio mal :(", ex)
        End Try
    End Sub
    Private Sub PanelImagenes_fotoClick(sender As Object, e As MouseButtonEventArgs)
        Me.img1.Source = sender.source
        Me.img1.Tag = sender.Tag
    End Sub
    Private Sub PanelImagenes_fotoRigthClick(sender As Object, e As MouseButtonEventArgs)
        If (MessageBox.Show("¿DESEAS QUITAR ESTA IMAGEN?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes) Then

            ximagenes.RemoveAt(sp_imagenes.Children.IndexOf(sender))
            Actualizar_panelImagenes()

            img1.Source = Nothing

        End If
    End Sub


    '********* EVENTOS BACKGROUNDWORKER *********
    Private Sub Worker_onWork(sender As Object, e As EventArgs)
        System.Threading.Thread.Sleep(400)

        Try

            '***** Guardar Datos *****
            Dim res As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", ListaParametros).Fill(res)
            Dim id_producto = res.Tables(0).Rows(0).Item("id_producto")

            '****** Guardar Imagenes ******
            For Each xRutaImage As RutaImageInvent In ximagenes
                Try
                    Using xq = New SqlCommand
                        xq.CommandTimeout = 500
                        xq.CommandType = CommandType.StoredProcedure
                        xq.CommandText = "[Global].[Opr_ProductosP]"
                        xq.Parameters.Clear()
                        xq.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "GUARDARIMAGEN"))
                        xq.Parameters.Add(New SqlClient.SqlParameter("@id_producto", id_producto))
                        xq.Parameters.Add(New SqlClient.SqlParameter("@foto1", File.ReadAllBytes(xRutaImage.ImagenCompleta)))
                        xq.Connection = Mi_conexion.conexion
                        Dim reader = xq.ExecuteReader
                        reader.Close()
                    End Using

                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            Next

            CType(sender, BackgroundWorker).ReportProgress(0, res.Tables(0).Rows(0).Item("mensaje").ToString)

        Catch ex As Exception
            CType(sender, BackgroundWorker).ReportProgress(-1, ex)
        End Try

        System.Threading.Thread.Sleep(100)
        CType(sender, BackgroundWorker).ReportProgress(1)
    End Sub
    Private Sub Worker_reportProgress(sender As Object, e As ProgressChangedEventArgs)
        Select Case e.ProgressPercentage
            Case 0
                Dim xm As New msg_completado(e.UserState.ToString)
                limpiarCampos()

            Case 1
                busyIndicator1.IsBusy = False

            Case -1
                Dim x2 As New msg_error("ALGO SALIO MAL :(", CType(e.UserState, Exception))
        End Select
    End Sub

End Class
