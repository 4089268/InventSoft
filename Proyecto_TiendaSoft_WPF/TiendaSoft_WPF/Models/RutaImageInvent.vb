Public Class RutaImageInvent
    Property ImagenCompleta As String
    Property ImagenPeque As String
    Public Sub New()
    End Sub
    Public Sub New(ic As String, ip As String)
        Me.ImagenCompleta = ic
        Me.ImagenPeque = ip
    End Sub

End Class