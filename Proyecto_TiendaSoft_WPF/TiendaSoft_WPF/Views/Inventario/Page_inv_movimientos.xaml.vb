Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Class Page_inv_movimientos
    Dim ds As DataSet

    Sub loaded_done() Handles Me.Loaded
        cargarUI()
        cargarCatalogo()
        cargarDatos()
    End Sub

    Private Sub cargarCatalogo()
        Try
            Dim dataFamilia As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "MOVIMIENTOS"}).Fill(dataFamilia)
            dataFamilia.Tables(0).Rows(0).Item("Descripcion") = "TODOS"
            cb_mov.ItemsSource = dataFamilia.Tables(0).DefaultView
            cb_mov.SelectedIndex = 0
        Catch ex As Exception
            Dim x As New msg_error("ALGO SALIO MAL")
        End Try

    End Sub

    Private Sub cargarUI()
        sp_fecha.Visibility = Visibility.Collapsed

        myDatagrid.IsReadOnly = True
        myDatagrid.AutoGenerateColumns = False
        myDatagrid.Columns.Clear()
        myDatagrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed
        myDatagrid.CanUserAddRows = False

        myDatagrid.Columns.Add(crear_datagridColumn("producto", 200, "PRODUCTO", False, False))
        myDatagrid.Columns.Add(crear_datagridColumn("Movimientos", 200, "MOVIMIENTO", False, False))
        myDatagrid.Columns.Add(crear_datagridColumn("fecha", 200, "FECHA", False, True))
        myDatagrid.Columns.Add(crear_datagridColumn("nombre", 200, "OPERADOR", False, False))
        myDatagrid.Columns.Add(crear_datagridColumn("invt_anterior", 110, "INV. ANTERIOR", False, False))
        myDatagrid.Columns.Add(crear_datagridColumn("invt_nuevo", 110, "INV. NUEVO", False, False))
        myDatagrid.Columns.Add(crear_datagridColumn("comentario", 110, "COMENTARIO", False, False))
        myDatagrid.Columns.Add(crear_datagridColumn("codigoRecibo", 110, "RECIBO", False, False))

        dp_fecha1.SelectedDate = Date.Now
        dp_fecha2.SelectedDate = Date.Now
    End Sub

    Private Function crear_datagridColumn(binding As String, ancho As Integer, cabecera As String, moneda As Boolean, fecha As Boolean) As DataGridTextColumn
        Dim myStyle As New Style
        myStyle.Setters.Add(New Setter(Label.FontWeightProperty, FontWeights.Medium))

        Dim myColumn As New DataGridTextColumn
        myColumn.FontSize = 12
        Dim myBin As New Binding(binding)
        If (moneda) Then
            myBin.StringFormat = "{0:C}"
        End If
        If (fecha) Then
            myBin.StringFormat = "{0:dd/MM/yyyy hh:mm}"
        End If

        myColumn.Binding = myBin
        myColumn.Header = cabecera
        myColumn.HeaderStyle = myStyle
        myColumn.Width = ancho

        Return myColumn
    End Function

    Private Sub cargarDatos() Handles btn_buscar.Click
        myDatagrid.ItemsSource = Nothing
        Try
            ds = New DataSet

            If (chb_fecha.IsChecked) Then
                Dim xf1() As String = dp_fecha1.Text.Split("/")
                Dim xf2() As String = dp_fecha2.Text.Split("/")

                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Movimientos]", {"cAlias", "id_movimiento", "xf1", "xf2"}, {"CARGARF", cb_mov.SelectedValue, xf1(2) & "-" & xf1(1) & "-" & xf1(0), xf2(2) & "-" & xf2(1) & "-" & xf2(0)}).Fill(ds)
                myDatagrid.ItemsSource = ds.Tables(0).DefaultView
            Else
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Movimientos]", {"cAlias", "id_movimiento"}, {"CARGAR", cb_mov.SelectedValue}).Fill(ds)
                myDatagrid.ItemsSource = ds.Tables(0).DefaultView
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub tb_search_changes(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        If (e.Key = Key.Enter) Then
            If (tb_search.SearchText <> "") Then
                Dim dataview As DataView = myDatagrid.ItemsSource
                dataview.RowFilter = String.Format("producto like '%" & tb_search.SearchText & "%'")
                myDatagrid.ItemsSource = dataview
            Else
                cargarDatos()
            End If
        End If
    End Sub

    Private Sub checkBox_changed() Handles chb_fecha.Checked, chb_fecha.Unchecked
        If (chb_fecha.IsChecked) Then
            sp_fecha.Visibility = Visibility.Visible
        Else
            sp_fecha.Visibility = Visibility.Collapsed
        End If
    End Sub


End Class

