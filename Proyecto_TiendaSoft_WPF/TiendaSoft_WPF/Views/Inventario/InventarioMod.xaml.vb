Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Class InventarioMod
    Dim ximagenes As New List(Of RutaImageInvent)

    Property id_producto As Int64 = 0
    Property comentario As String = ""
    Private modificaronImagenes As Boolean = False

    Dim backWorker As BackgroundWorker
    Dim backWorkerCargar As BackgroundWorker
    Dim ListaParametros As New List(Of SqlParameter)

    Private Sub rootLayout_loaded() Handles rootLayout.Loaded
        cargarCatalagos()
        limpiarCampos()

        If id_producto > 0 Then
            Try
                tb_search.Text = id_producto
                InicializarWorkerCargar()
            Catch ex As Exception
            End Try
        End If

    End Sub
    Private Sub cargarCatalagos()
        Dim dataFamilia As New DataSet
        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "FAMILIAS"}).Fill(dataFamilia)
        cb_Familia.ItemsSource = dataFamilia.Tables(0).DefaultView

        Dim dataMarca As New DataSet
        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "MARCA"}).Fill(dataMarca)
        'cb_Marca.ItemsSource = dataMarca.Tables(0).DefaultView

        Dim dataUbicacion As New DataSet
        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "UBICACIONES"}).Fill(dataUbicacion)
        cb_Ubicacion.ItemsSource = dataUbicacion.Tables(0).DefaultView

        Dim dataAseguradora As New DataSet
        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "ASEGURADORA"}).Fill(dataAseguradora)
        'cb_aseguradora.ItemsSource = dataAseguradora.Tables(0).DefaultView

    End Sub
    'Private Sub cargarCatalago_Tipo() Handles cb_Marca.SelectionChanged
    '    Try
    '        Dim dataTipo As New DataSet
    '        Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xid_marca"}, {"CARGARTIPOXMARCA", cb_Marca.SelectedValue}).Fill(dataTipo)
    '        cb_Tipo.ItemsSource = dataTipo.Tables(0).DefaultView
    '    Catch ex As Exception
    '    End Try
    'End Sub
    Private Sub InicializarWorker()
        id_producto = CType(tb_search.Text, Integer)
        busyIndicator1.IsBusy = True
        Me.backWorker = New BackgroundWorker
        backWorker.WorkerReportsProgress = True
        AddHandler backWorker.DoWork, AddressOf Worker_onWork
        AddHandler backWorker.ProgressChanged, AddressOf Worker_reportProgress
        backWorker.RunWorkerAsync()
    End Sub
    Private Sub InicializarWorkerCargar()
        limpiarCampos()
        ximagenes.Clear()
        id_producto = CType(tb_search.Text, Integer)
        busyIndicator1.IsBusy = True
        backWorkerCargar = New BackgroundWorker
        backWorkerCargar.WorkerReportsProgress = True
        AddHandler backWorkerCargar.DoWork, AddressOf backGroundWorkeCargar_doWork
        AddHandler backWorkerCargar.ProgressChanged, AddressOf backGroundWorkeCargar_reportProgres
        backWorkerCargar.RunWorkerAsync()
    End Sub


    '********* BOTON AGREGAR *********
    Private Sub AgregarNuevaPieza_click() Handles btn_agregar.Click
        If (tb_Cantidad.Text <> "" And tB_descripcion.Text <> "" And tb_Cantidad.Text <> "" And cb_Familia.SelectedValue >= 0 And cb_Ubicacion.SelectedValue >= 0) Then

            '***** Valirdar que los modelos sean numeros de 4 digitos *****
            'Dim regex As New System.Text.RegularExpressions.Regex("^[0-9]{4}$")
            'If Not regex.IsMatch(tb_modelo.Text) Then
            '    Dim x As New msg_information("SOLO SE PERMITEN MODELOS DE 4 DIGITOS NUMERICOS")
            '    tb_modelo.Focus()
            '    Return
            'End If

            'If tb_modelo2.Text <> "" And Not regex.IsMatch(tb_modelo2.Text) Then
            '    Dim x As New msg_information("SOLO SE PERMITEN MODELOS DE 4 DIGITOS NUMERICOS")
            '    tb_modelo2.Focus()
            '    Return
            'End If


            '******** GENERAR CADENA DE MODELOS ********
            'Dim xmodelos As String = ""
            'If tb_modelo2.Text <> "" Then
            '    Dim xm1 As Integer = Int(tb_modelo.Text)
            '    Dim xm2 As Integer = Int(tb_modelo2.Text)

            '    If (xm1 < xm2) Then
            '        For cont As Integer = xm1 To xm2
            '            xmodelos = xmodelos & cont
            '            If (cont <> xm2) Then
            '                xmodelos = xmodelos & ","
            '            End If
            '        Next
            '    Else
            '        xmodelos = xm1 & ""
            '    End If
            'Else
            '    xmodelos = tb_modelo.Text
            'End If

            '****** Generando lista de parametros ******
            ListaParametros = New List(Of SqlParameter)
            ListaParametros.Add(New SqlParameter("@cAlias", "MODIFICAR"))
            ListaParametros.Add(New SqlParameter("@id_producto", tb_search.Text))
            ListaParametros.Add(New SqlParameter("@Codigo", tb_Codigo.Text))
            ListaParametros.Add(New SqlParameter("@Descripcion", tB_descripcion.Text))
            ListaParametros.Add(New SqlParameter("@existen", tb_Cantidad.Text))
            ListaParametros.Add(New SqlParameter("@id_familia", cb_Familia.SelectedValue))
            'ListaParametros.Add(New SqlParameter("@id_marca", cb_Marca.SelectedValue))
            'ListaParametros.Add(New SqlParameter("@id_tipo", cb_Tipo.SelectedValue))
            'ListaParametros.Add(New SqlParameter("@id_aseguradora", cb_aseguradora.SelectedValue))
            ListaParametros.Add(New SqlParameter("@id_ubicacion", cb_Ubicacion.SelectedValue))
            'ListaParametros.Add(New SqlParameter("@modelo", xmodelos))
            'ListaParametros.Add(New SqlParameter("@xplaca", tb_placa.Text))
            ListaParametros.Add(New SqlParameter("@Numero_Pieza", tb_numPieza.Text))
            ListaParametros.Add(New SqlParameter("@anaquel", tb_anaquel.Text))

            InicializarWorker()


        Else
            Dim x As New msg_information("Verifique que los campos esten llenos.")
        End If

    End Sub
    Private Sub Eliminar() Handles btn_eliminar.Click
        Try
            If tb_search.Text <> "" Then
                Dim xcoment As New InventarioMod_comentario(Me)
                xcoment.ShowDialog()

                If (MessageBox.Show("¿DESEA DAR DE BAJA LA PIEZA?", "CONFIRMAR", MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes) Then
                    Dim dt As New DataSet
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto", "coment", "id_movimiento", "id_operador"},
                                                               {"DARBAJA", tb_search.Text, comentario, 5, xOpererador}).Fill(dt)
                    limpiarCampos()

                    Dim xm As New msg_completado(dt.Tables(0).Rows(0).Item("mensaje"))
                End If
            End If

        Catch ex As Exception
        End Try
    End Sub
    Public Sub actualizarCodigo()
        tb_search.Text = id_producto & ""
        InicializarWorkerCargar()
    End Sub
    Private Sub modal_Buscar() Handles btn_search.Click
        Dim f As New ModalBuscar(Me)
        f.Show()
    End Sub
    Private Sub cargarDatos(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        If (e.Key = Key.Enter And tb_search.Text <> "") Then
            modificaronImagenes = False
            InicializarWorkerCargar()
        End If
    End Sub

    Private Sub limpiarCampos()

        modificaronImagenes = False

        'tb_search.Text = ""
        tb_Codigo.Text = ""
        tb_numPieza.Text = ""
        tB_descripcion.Text = ""
        tb_Cantidad.Text = ""
        'tb_modelo.Text = ""
        'tb_modelo2.Text = ""
        tb_anaquel.Text = ""
        'cb_aseguradora.SelectedIndex = 0
        cb_Familia.SelectedIndex = 0
        'cb_Marca.SelectedIndex = 0
        'cb_Tipo.SelectedIndex = 0
        cb_Ubicacion.SelectedIndex = 0

        img1.Source = Nothing
        ximagenes.Clear()
        sp_imagenes.Children.Clear()


    End Sub


    ''**** VALIDAR CARACTERES ***
    Private Sub validar_numbers(sender As Object, e As TextCompositionEventArgs) Handles tb_Cantidad.PreviewTextInput
        Try
            Dim regex As New System.Text.RegularExpressions.Regex("[^0-9]+")

            e.Handled = regex.IsMatch(e.Text)
        Catch ex As Exception
        End Try

    End Sub


    ''**** IMAGENES *****
    Private Sub CargarArchivoImagen() Handles btn_cargarImagen.Click
        Try
            Dim openFile As New System.Windows.Forms.OpenFileDialog
            openFile.Multiselect = True
            openFile.Title = "Cargar Imagen"
            openFile.Filter = "Todos(*.*)|*.*|Imagenes|*.jpg;*.gif;*.png;*.bmp"
            If (openFile.ShowDialog()) Then


                modificaronImagenes = True

                Dim i As Integer = 0
                For Each xFileName In openFile.FileNames

                    Using ms As New MemoryStream(File.ReadAllBytes(xFileName))
                        Dim imagenOriginal As New System.Drawing.Bitmap(ms)
                        Dim imagenReducido As System.Drawing.Bitmap

                        '******** Generar imagen reducida ********
                        Dim xw = 200
                        Dim xh = 200
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

                    modificaronImagenes = True
                Next
                actualizar_panelImagenes()

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
            modificaronImagenes = True

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


            If modificaronImagenes Then

                '****** Limpriar  Imagenes en la base de datos ******
                Using xq = New SqlCommand
                    xq.CommandTimeout = 500
                    xq.CommandType = CommandType.StoredProcedure
                    xq.CommandText = "[Global].[Opr_ProductosP]"
                    xq.Parameters.Clear()
                    xq.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "LIMPIARIMAGNES"))
                    xq.Parameters.Add(New SqlClient.SqlParameter("@id_producto", id_producto))
                    xq.Connection = Mi_conexion.conexion
                    Dim reader = xq.ExecuteReader
                    reader.Close()
                End Using

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

            End If

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

    Private Sub backGroundWorkeCargar_doWork(sender As Object, e As DoWorkEventArgs)
        System.Threading.Thread.Sleep(400)

        Try
            '********** Cargar Datos ***********
            Dim ds As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto"}, {"CARGARXID", id_producto}).Fill(ds)
            CType(sender, BackgroundWorker).ReportProgress(0, ds.Tables(0))

            '********** Cargar Imagenes ***********
            Dim ds2 As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto"}, {"CARGARIMAGENES", id_producto}).Fill(ds2)

            Dim i = 0
            For Each xRow In ds2.Tables(0).Rows

                Using ms As New MemoryStream(CType(xRow.Item("imagen"), Byte()))
                    Dim imagenOriginal As New System.Drawing.Bitmap(ms)
                    Dim imagenReducido As System.Drawing.Bitmap

                    '******** Generar imagen reducida ********
                    Dim xw = 200
                    Dim xh = 200
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
                    CType(sender, BackgroundWorker).ReportProgress(1, urlOriginal & ";" & urlReducido)
                    i = i + 1

                End Using

            Next

        Catch ex As Exception
            CType(sender, BackgroundWorker).ReportProgress(-1, ex)
        End Try

        System.Threading.Thread.Sleep(100)
        CType(sender, BackgroundWorker).ReportProgress(2)

    End Sub
    Private Sub backGroundWorkeCargar_reportProgres(sender As Object, e As ProgressChangedEventArgs)
        Select Case e.ProgressPercentage
            Case 0
                Dim dt = CType(e.UserState, DataTable)
                tb_Codigo.Text = dt.Rows(0).Item("codigo")
                tB_descripcion.Text = dt.Rows(0).Item("descripcion")
                tb_numPieza.Text = dt.Rows(0).Item("num_pieza")

                tb_Cantidad.Text = dt.Rows(0).Item("existencia")
                'cb_aseguradora.SelectedValue = CType(dt.Rows(0).Item("id_aseguradora"), Integer)
                cb_Familia.SelectedValue = CType(dt.Rows(0).Item("id_familia"), Integer)
                'cb_Marca.SelectedValue = CType(dt.Rows(0).Item("id_marca"), Integer)
                'cargarCatalago_Tipo()
                'tb_placa.Text = dt.Rows(0).Item("placa")
                'cb_Tipo.SelectedValue = CType(dt.Rows(0).Item("id_tipo"), Integer)
                cb_Ubicacion.SelectedValue = CType(dt.Rows(0).Item("id_ubicacion"), Integer)
                tb_anaquel.Text = dt.Rows(0).Item("anaquel").ToString

                Dim xStringModelo As String = dt.Rows(0).Item("modelo")
                'If xStringModelo.Length = 4 Then
                '    tb_modelo.Text = xStringModelo.Substring(0, 4)
                'Else
                '    Try
                '        tb_modelo.Text = xStringModelo.Substring(0, 4)
                '        tb_modelo2.Text = xStringModelo.Substring(xStringModelo.Length - 4, 4)
                '    Catch ex As Exception
                '        MessageBox.Show(ex.ToString)
                '    End Try
                'End If

            Case 1
                Dim tmpUrl = e.UserState.ToString.Split(";")

                ximagenes.Add(New RutaImageInvent(tmpUrl(0), tmpUrl(1)))

            Case 2
                Actualizar_panelImagenes()
                busyIndicator1.IsBusy = False

            Case -1
                Dim xf As New msg_error("Error al cargar datos", CType(e.UserState, Exception))

        End Select

    End Sub
End Class