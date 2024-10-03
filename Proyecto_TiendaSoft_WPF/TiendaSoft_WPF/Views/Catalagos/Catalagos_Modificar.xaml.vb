Imports System.Data

Class Catalagos_Modificar
    Dim opcion As String = ""
    Dim xid As Int64 = -1
    Dim ds As DataSet

    Public Sub New(_op As String)
        InitializeComponent()
        opcion = _op
    End Sub

    Private Sub rooteLayout_loaded() Handles rootLayout.Loaded
        activarBotones_editar(False)
        sp_editar.IsEnabled = False
        DataGrid1.IsEnabled = True

        lb_title.Content = "CATALOGO " & opcion

        If (opcion = "UBICACIONES") Then
            sp_activo.Visibility = Visibility.Collapsed
        End If

        If (opcion = "TIPO") Then
            label_marcar.IsEnabled = True
            cb_marca.IsEnabled = True
            label_marcar.Visibility = Visibility.Visible
            cb_marca.Visibility = Visibility.Visible

        Else
            label_marcar.IsEnabled = False
            cb_marca.IsEnabled = False
            label_marcar.Visibility = Visibility.Collapsed
            cb_marca.Visibility = Visibility.Collapsed
        End If

        DataGrid1.AutoGenerateColumns = False
        DataGrid1.BorderBrush = System.Windows.Media.Brushes.LightGray
        DataGrid1.CanUserAddRows = False
        DataGrid1.CanUserDeleteRows = False
        DataGrid1.VerticalGridLinesBrush = System.Windows.Media.Brushes.LightGray
        DataGrid1.HorizontalGridLinesBrush = System.Windows.Media.Brushes.LightGray
        DataGrid1.IsReadOnly = True


        ''DATAGRID COLUMNS
        'Dim col1 As New DataGridTextColumn
        'col1.Header = "ID"
        'col1.FontSize = 12
        'col1.Binding = New Binding("id")
        'DataGrid1.Columns.Add(col1)

        Dim col2 As New DataGridTextColumn
        col2.Header = "NOMBRE"
        col2.FontSize = 12
        col2.Binding = New Binding("Descripcion")
        col2.Width = 220
        DataGrid1.Columns.Add(col2)

        If (opcion = "TIPO") Then
            Dim col4 As New DataGridTextColumn
            col4.Header = "MARCA"
            col4.Binding = New Binding("Marca")
            DataGrid1.Columns.Add(col4)
        End If

        If (opcion <> "UBICACIONES") Then
            Dim col3 As New DataGridCheckBoxColumn
            col3.Header = "ACTIVO"
            col3.Binding = New Binding("activo")
            DataGrid1.Columns.Add(col3)
        End If


        CargarDatos()

    End Sub

    Private Sub CargarDatos()
        Try
            ds = New DataSet
            If (opcion = "TIPO") Then

                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xActivo"}, {"CARGAR", "TIPO", 1}).Fill(ds)
                CargarDatosGrid()

                Dim ds2 As New DataSet
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xActivo"}, {"CARGAR", "MARCA", 1}).Fill(ds2)
                cb_marca.ItemsSource = ds2.Tables(0).DefaultView
            Else
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xActivo"}, {"CARGAR", opcion, 1}).Fill(ds)
                CargarDatosGrid()
            End If
        Catch ex As Exception
            ''Err:00368
        End Try
    End Sub

    Private Sub CargarDatosGrid()
        DataGrid1.ItemsSource = ds.Tables(0).DefaultView

    End Sub
    Private Sub activarBotones_editar(x As Boolean)
        btn_nuevo.IsEnabled = Not x
        btn_modif.IsEnabled = Not x

        btn_Guardar.IsEnabled = x
        btn_Cancelar.IsEnabled = x

        If (x) Then
            btn_nuevo.Visibility = Visibility.Collapsed
            btn_modif.Visibility = Visibility.Collapsed

            btn_Guardar.Visibility = Visibility.Visible
            btn_Cancelar.Visibility = Visibility.Visible
        Else
            btn_nuevo.Visibility = Visibility.Visible
            btn_modif.Visibility = Visibility.Visible

            btn_Guardar.Visibility = Visibility.Collapsed
            btn_Cancelar.Visibility = Visibility.Collapsed

            tb_nombre.Text = ""
            cb_activo.IsChecked = False
            xid = -1

        End If
    End Sub

    Private Sub Nuevo_Elemento() Handles btn_nuevo.Click
        xid = -1
        activarBotones_editar(True)
        DataGrid1.IsEnabled = False
        sp_editar.IsEnabled = True
    End Sub
    Private Sub editarValor() Handles btn_modif.Click
        Try
            xid = DataGrid1.SelectedItem.row.item(0)
        Catch ex As Exception
        End Try

        If (xid > -1) Then
            activarBotones_editar(True)
            sp_editar.IsEnabled = True
            DataGrid1.IsEnabled = False

            tb_nombre.Text = DataGrid1.SelectedItem.row.item(1)

            Select Case opcion
                Case "TIPO"
                    cb_marca.SelectedValue = CType(DataGrid1.SelectedItem.row.item(4), Integer)
                    cb_activo.IsChecked = CType(DataGrid1.SelectedItem.row.item(3), Boolean)
                Case "UBICACIONES"
                Case Else
                    cb_activo.IsChecked = CType(DataGrid1.SelectedItem.row.item(2), Boolean)
            End Select
        Else
            Dim x As New msg_information("Seleccione un elemento")
        End If

    End Sub
    Private Sub guardarCambios() Handles btn_Guardar.Click
        Dim ds As New DataSet
        If (opcion = "TIPO") Then
            If (tb_nombre.Text <> "" And cb_marca.SelectedValue > 0) Then
                If (xid > -1) Then
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xDescripcion", "xActivo", "xid_marca", "xid"}, {"MODIFICAR", "TIPO", tb_nombre.Text, cb_activo.IsChecked, cb_marca.SelectedValue, xid}).Fill(ds)
                Else
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xDescripcion", "xActivo", "xid_marca"}, {"AGREGAR", "TIPO", tb_nombre.Text, 1, cb_marca.SelectedValue}).Fill(ds)
                End If
                Dim x As New msg_completado(ds.Tables(0).Rows(0).Item(0))
                CargarDatos()
            Else
                Dim x As New msg_information("Verifique que todos los campos este llenos")
            End If


        Else
            If (tb_nombre.Text <> "") Then
                If (xid > -1) Then
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xDescripcion", "xActivo", "xid"}, {"MODIFICAR", opcion, tb_nombre.Text, cb_activo.IsChecked, xid}).Fill(ds)
                Else
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xDescripcion", "xActivo"}, {"AGREGAR", opcion, tb_nombre.Text, cb_activo.IsChecked}).Fill(ds)
                End If

                Dim x As New msg_completado(ds.Tables(0).Rows(0).Item(0))
                CargarDatos()
            Else
                Dim x As New msg_information("Verifique que todos los campos este llenos")
            End If
        End If



        cancelar_editado()
    End Sub
    Private Sub cancelar_editado() Handles btn_Cancelar.Click
        activarBotones_editar(False)
        sp_editar.IsEnabled = False
        DataGrid1.IsEnabled = True

    End Sub

    Private Sub tb_search_search(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        If (e.Key = Key.Enter) Then
            If (tb_search.SearchText <> "") Then
                Dim dataview As DataView = DataGrid1.ItemsSource
                dataview.RowFilter = String.Format("Descripcion like '%" & tb_search.SearchText & "%'")
                DataGrid1.ItemsSource = dataview
            Else
                CargarDatosGrid()
            End If
        End If
    End Sub


End Class
