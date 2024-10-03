Public Class Saliendo_Confirmar


    Private Sub continuarClick() Handles btn_ok.Click
        Me.DialogResult = True
        Me.Close()
    End Sub

    Private Sub cancelarClick() Handles btn_cancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

End Class
