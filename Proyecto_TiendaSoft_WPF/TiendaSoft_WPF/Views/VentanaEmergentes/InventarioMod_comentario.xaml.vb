Public Class InventarioMod_comentario
    Private xinventario As InventarioMod

    Public Sub New(x As InventarioMod)
        InitializeComponent()
        xinventario = x
    End Sub

    Public Sub salir() Handles btn_salir.Click
        Me.Close()
    End Sub
    Public Sub Saliendo() Handles Me.Closing
        xinventario.comentario = tb_coment.Text
    End Sub
End Class
