Imports System.Data

Public Class DarBaja_Confirmar
    Public Property id_producto As Integer = 0

    Public Sub layout_loaded() Handles grid_form.Loaded
        cargarCatalagos()
    End Sub

    Private Sub venta_checked(sender As Object, e As RoutedEventArgs) Handles cb_salida.SelectionChanged
        If (cb_salida.SelectedValue = 4) Then
            grid_form.RowDefinitions(2).Height = New System.Windows.GridLength(25)

        Else
            grid_form.RowDefinitions(2).Height = New System.Windows.GridLength(0)
        End If
    End Sub

    Private Sub darBaja_click() Handles btn_ok.Click
        If (cb_salida.SelectedIndex <> -1 And cb_existe.SelectedIndex <> -1) Then
            Dim dt As New DataSet

            Dim ticket As String = tb_ticket.Text

            If (cb_salida.SelectedValue <> 4) Then
                ticket = "NINGUNO"
            End If

            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto", "existen", "id_movimiento", "coment", "cod_recib", "id_operador"}, _
                                                       {"DARBAJA", id_producto, cb_existe.SelectedValue, cb_salida.SelectedValue, tb_coment.Text, ticket, xOpererador}).Fill(dt)
            MessageBox.Show(dt.Tables(0).Rows(0).Item("mensaje"), "", MessageBoxButton.OK, MessageBoxImage.Information)

            Me.DialogResult = True

            Me.Close()
        Else
            Dim x As New msg_information("SELECCIONE EL MOVIMIENTO Y/O LA CANTIDAD ")
        End If
    End Sub

    Private Sub cargarCatalagos()
        Try
            Dim dataFamilia As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat"}, {"CARGAR", "SMOVIMIENTOS"}).Fill(dataFamilia)
            cb_salida.ItemsSource = dataFamilia.Tables(0).DefaultView

        Catch ex As Exception
        End Try

        Try
            Dim producto As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto"}, {"CARGARXID", id_producto}).Fill(producto)
            Dim xTotal As Integer = producto.Tables(0).Rows(0).Item("existencia")

            Dim table As New DataTable
            table.Columns.Add("existencia", GetType(Integer))

            If (xTotal <> 0) Then
                For i As Integer = 1 To xTotal
                    table.Rows.Add(i)
                Next
            End If

            cb_existe.ItemsSource = table.DefaultView

        Catch ex As Exception
        End Try

    End Sub

    Private Sub salir() Handles btn_cancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

End Class
