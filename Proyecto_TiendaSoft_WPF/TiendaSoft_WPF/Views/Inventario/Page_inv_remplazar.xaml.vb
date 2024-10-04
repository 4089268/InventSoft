Imports System.Data.SqlClient
Imports System.Data
Imports System.Linq

Class Page_inv_remplazar
    Property id_producto As Int64 = 0
    Private Sub rootLayout_loaded() Handles rootLayout.Loaded

        If id_producto > 0 Then
            Try
                tb_search.Text = id_producto

                Dim dataSet As New DataSet
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto", "id_operador"}, {"CARGARXID", tb_search.Text, xOpererador}).Fill(dataSet)
                lb_descripcion.Content = dataSet.Tables(0).Rows(0).Item("descripcion")
                tb_ACantidad.Text = dataSet.Tables(0).Rows(0).Item("existencia")

            Catch ex As Exception
            End Try
        End If

    End Sub
    Private Sub buscarmodal_click() Handles btn_search.Click
        Me.IsEnabled = False
        Dim f As New ModalBuscar(Me)
        f.Show()
    End Sub

    Public Sub actualizarCodigo()
        tb_search.Text = id_producto & ""
        Me.IsEnabled = True
    End Sub

    Private Sub Buscar(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        Try
            If (e.Key = Key.Enter And tb_search.Text <> "") Then
                Dim dataSet As New DataSet
                Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto"}, {"CARGARXID", tb_search.Text}).Fill(dataSet)

                If dataSet.Tables.Count <= 0 Then
                    Throw New KeyNotFoundException
                End If

                If dataSet.Tables(0).Rows.Count <= 0 Then
                    Throw New KeyNotFoundException
                End If

                lb_descripcion.Content = dataSet.Tables(0).Rows(0).Item("descripcion")
                tb_ACantidad.Text = dataSet.Tables(0).Rows(0).Item("existencia")

            End If
        Catch nf As KeyNotFoundException
            Dim x As New msg_error("No existe registros con este Id")
        Catch ex As Exception
            Dim x As New msg_error("Error al conectarse con el servidor")
        End Try
    End Sub

    '' ** VALIDAR NUMEROS
    Private Sub validar_numbers(sender As Object, e As TextCompositionEventArgs) Handles tb_nCantidad.PreviewTextInput, tb_search.PreviewTextInput
        Try
            Dim regex As System.Text.RegularExpressions.Regex
            regex = New System.Text.RegularExpressions.Regex("[^0-9]+")

            e.Handled = regex.IsMatch(e.Text)
        Catch ex As Exception
        End Try

    End Sub

    Private Sub ajustar() Handles btn_agregar.Click
        Try
            Dim ds As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_ProductosP]", {"cAlias", "id_producto", "existen", "coment"}, _
                                                       {"AJUSTARINVENTARIO", tb_search.Text, tb_nCantidad.Text, tb_nComent.Text}).Fill(ds)
            limpiar_UI()
            Dim xm As New msg_completado(ds.Tables(0).Rows(0).Item("mensaje"))
        Catch ex As Exception
            Dim xm As New msg_error("ERROR AL TRATAR DE AJUSTAR EL INVENTARIO")
        End Try
    End Sub

    Private Sub limpiar_UI()
        lb_descripcion.Content = ""
        tb_search.Text = ""
        tb_ACantidad.Text = 0
        tb_nCantidad.Text = 0
        tb_nComent.Text = ""

    End Sub

End Class
