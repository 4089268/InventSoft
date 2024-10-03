Public Class msg_information
    Private mensaje As String = ""
    Public Sub New(x As String)
        InitializeComponent()
        Me.mensaje = x

        If x.Length <= 0 Then
            Me.Close()
        End If
        Me.ShowDialog()

    End Sub
    Private Sub me_loaded_Done() Handles Me.Loaded
        lb_msg.Content = mensaje
    End Sub

    Private Sub okClick() Handles btn_ok.Click
        Me.Close()
    End Sub

End Class
