Imports System.Data

Public Class ModalBuscar
    Dim ds As DataSet

    Dim formRempl As Page_inv_remplazar
    Dim formMod As InventarioMod
    Public Sub New(d As InventarioMod)
        InitializeComponent()
        formMod = d
        Me.ResizeMode = ResizeMode.NoResize
    End Sub

    Public Sub New(d As Page_inv_remplazar)
        InitializeComponent()
        formRempl = d
        Me.ResizeMode = ResizeMode.NoResize
    End Sub

    Private Sub layyout_loaded() Handles rootLayout.Loaded

        grd_venta.AutoGenerateColumns = False
        grd_venta.BorderBrush = System.Windows.Media.Brushes.LightGray
        grd_venta.CanUserAddRows = False
        grd_venta.CanUserDeleteRows = False
        grd_venta.VerticalGridLinesBrush = System.Windows.Media.Brushes.LightGray
        grd_venta.HorizontalGridLinesBrush = System.Windows.Media.Brushes.LightGray
        grd_venta.IsReadOnly = True
        grd_venta.CanUserSortColumns = False
        grd_venta.FontSize = "10"

        Dim col1 As New DataGridTextColumn
        col1.Header = "Codigo"
        col1.Binding = New Binding("codigo")
        col1.Width = 150

        Dim col2 As New DataGridTextColumn
        col2.Header = "Descripcion"
        col2.Binding = New Binding("descripcion")
        col2.Width = 300

        Dim col3 As New DataGridTextColumn
        col3.Header = "Existencia"
        col3.Binding = New Binding("existencia")
        col3.Width = 70

        Dim col4 As New DataGridTextColumn
        col4.Header = "Familia"
        col4.Binding = New Binding("familia")
        col4.Width = 70

        Dim col5 As New DataGridTextColumn
        col5.Header = "Marca"
        col5.Binding = New Binding("Marca")
        col5.Width = 100

        Dim col6 As New DataGridTextColumn
        col6.Header = "Tipo"
        col6.Binding = New Binding("Tipo")
        col6.Width = 70

        Dim col7 As New DataGridTextColumn
        col7.Header = "Modelo"
        col7.Binding = New Binding("modelo")
        col7.Width = 70

        Dim col8 As New DataGridTextColumn
        col8.Header = "Aseguradora"
        col8.Binding = New Binding("Aseguradora")
        col8.Width = 100

        Dim col9 As New DataGridTextColumn
        col9.Header = "Ubicacion"
        col9.Binding = New Binding("Ubicacion")
        col9.Width = 70

        Dim col10 As New DataGridTextColumn
        col10.Header = "Anaquel"
        col10.Binding = New Binding("anaquel")
        col10.Width = 70

        grd_venta.Columns.Add(col1)
        grd_venta.Columns.Add(col2)
        grd_venta.Columns.Add(col3)
        grd_venta.Columns.Add(col4)
        grd_venta.Columns.Add(col5)
        grd_venta.Columns.Add(col6)
        grd_venta.Columns.Add(col7)
        grd_venta.Columns.Add(col8)
        grd_venta.Columns.Add(col9)
        grd_venta.Columns.Add(col10)

    End Sub

    Private Sub Buscar(sender As Object, e As KeyEventArgs) Handles txt_buscar.KeyDown
        If (e.Key = Key.Enter) Then
            ds = New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Busqueda]", {"xAlias", "xPalabra"}, {"BUSCAR", txt_buscar.Text}).Fill(ds)
            grd_venta.ItemsSource = ds.Tables(0).DefaultView

        End If
    End Sub

    Private Sub salir() Handles btn_ok.Click
        Me.Close()
    End Sub

    Private Sub salir_Escape(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Key = Key.Escape Then
            Me.Close()
        End If
    End Sub
    Private Sub saliendo() Handles Me.Closing

        If (Not formRempl Is Nothing) Then
            Try
                formRempl.id_producto = CType(ds.Tables(0).Rows(grd_venta.SelectedIndex).Item(0), Int64)
                formRempl.actualizarCodigo()
            Catch ex As Exception
                formRempl.actualizarCodigo()
            End Try
        End If


        If (Not formMod Is Nothing) Then
            Try
                formMod.id_producto = CType(ds.Tables(0).Rows(grd_venta.SelectedIndex).Item(0), Int64)
                formMod.actualizarCodigo()
            Catch ex As Exception
                formMod.actualizarCodigo()
            End Try

        End If

    End Sub

End Class
