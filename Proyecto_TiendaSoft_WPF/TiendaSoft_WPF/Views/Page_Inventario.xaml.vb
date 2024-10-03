Imports System.Data.SqlClient
Imports System.Data

Class Page_Inventario
    Dim xop As String = "NUEVO"
    Public Sub New()
        InitializeComponent()

    End Sub

    Public Sub New(x As String)
        InitializeComponent()
        xop = x
    End Sub

    Private Sub rootLayout() Handles rotLayout.Loaded
        If (xop = "NUEVO") Then
            btn_nuevoProducto.IsEnabled = False
            navigationframe.Navigate(New Invetario_Nuevo)
        Else
            btn_remplazarInv.IsEnabled = False
            navigationframe.Navigate(New Page_inv_remplazar)
        End If
    End Sub

    Private Sub btn_clicks(sender As Object, e As RoutedEventArgs) Handles btn_nuevoProducto.Click, btn_remplazarInv.Click, btn_MovimientoInv.Click, btn_editarProducto.Click
        actividadButons()
        CType(sender, Button).IsEnabled = False

        Select Case sender.name
            Case "btn_nuevoProducto"
                navigationframe.Navigate(New Invetario_Nuevo)

            Case "btn_remplazarInv"
                navigationframe.Navigate(New Page_inv_remplazar)

            Case "btn_MovimientoInv"
                navigationframe.Navigate(New Page_inv_movimientos)

            Case "btn_editarProducto"
                navigationframe.Navigate(New InventarioMod)

        End Select

    End Sub

    Private Sub actividadButons()
        btn_nuevoProducto.IsEnabled = True
        btn_remplazarInv.IsEnabled = True
        btn_MovimientoInv.IsEnabled = True
        btn_editarProducto.IsEnabled = True
    End Sub

End Class
