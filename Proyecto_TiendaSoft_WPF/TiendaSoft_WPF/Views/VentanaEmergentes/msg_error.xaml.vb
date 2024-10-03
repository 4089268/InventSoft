Public Class msg_error
    Private mensaje As String = ""
    Dim x = False
    Dim xErrr As Exception


    Public Sub New(x As String)
        InitializeComponent()
        Me.mensaje = x

        If x.Length <= 0 Then
            Me.Close()
        End If
        Me.ShowDialog()

    End Sub

    Public Sub New(x As String, xee As Exception)
        InitializeComponent()
        Me.mensaje = x

        If x.Length <= 0 Then
            Me.Close()
        End If

        xErrr = xee

        Me.ShowDialog()

    End Sub


    Private Sub me_loaded_Done() Handles Me.Loaded
        lb_msg.Content = mensaje

        Try
            tb_trace.Text = xErrr.Message & vbNewLine & xErrr.StackTrace
        Catch ex As Exception
        End Try

    End Sub

    Private Sub okClick() Handles btn_ok.Click
        Me.Close()
    End Sub

    Private Sub imageError_cick() Handles imageError.MouseLeftButtonUp
        If Not xErrr Is Nothing Then
            If x Then
                rootLayout.RowDefinitions(1).Height = New GridLength(0)
                Me.Height = 115
                Me.Width = 340
                Me.WindowStyle = WindowStyle.None
            Else
                rootLayout.RowDefinitions(1).Height = New GridLength(400)
                Me.Height = Me.Height + 400
                Me.Width = Me.Width + 100
                Me.WindowStyle = WindowStyle.ToolWindow
            End If

            x = Not x
        End If

    End Sub

End Class
