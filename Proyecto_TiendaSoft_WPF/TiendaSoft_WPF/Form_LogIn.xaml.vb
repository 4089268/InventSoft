Imports System.Data.SqlClient
Imports System.Data

Public Class Form_LogIn

    Private Sub Form_LogIn_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ''Verificar La conexion de la base de Datos

        Mi_conexion = New Conexion_Sql
        If (Not Mi_conexion.Conectar) Then
            ''Formulario Confugurar Conexion
            Dim conexionForm As New Frm_Conexion
            Dim xres As Boolean = False
            xres = conexionForm.ShowDialog()
            If (xres) Then
                Dim xl As New Form_LogIn
                xl.Show()
                Me.Close()
            Else
                Me.Close()
            End If
        End If

        tb_usuario.Focus()

    End Sub

    Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs) Handles OKButton.Click

        Dim reader As SqlDataReader = Mi_conexion.Ejecutar_Procedimiento("[dbo].[Sys_Login]", {"cUsu", "cPass", "cApp"}, {tb_usuario.Text, tb_password.Password, strApp})
        Dim Logearse As String = ""
        Try
            While reader.Read
                Logearse = reader("Usuario")
            End While
            reader.Close()

        Catch ex As Exception
            MessageBox.Show("Se Perdio la Conexion con la Base de Datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
            Me.Close()
        End Try

        If (Logearse <> "") Then
            xOpererador = Val(Split(Logearse.ToString, "#")(0).ToString)
            xNombreUsuario = Split(Logearse.ToString, "#")(1).ToString
            xOpciones = Split(Logearse.ToString, "#")(2).ToString
            xAdmin = IIf(Split(Logearse.ToString, "#")(3).ToString = "1", True, False)

            IniciarSesion()

        Else
            Me.tb_usuario.Focus()
            MessageBox.Show("Usuario y/o contraseña no validos", "", MessageBoxButton.OK, MessageBoxImage.Stop)
            Me.tb_usuario.Focus()
        End If


    End Sub

    Private Sub btn_salir_onclic() Handles btn_salir.Click
        Me.Close()
    End Sub

    Private Sub IniciarSesion()
        Dim yform As New MainWindow
        yform.Show()
        Me.Close()
    End Sub

    Private Sub TextBlock_KeyUp(sender As Object, e As KeyEventArgs) Handles tb_usuario.KeyUp, tb_password.KeyUp

        If e.Key = Key.Enter Then
            Select Case sender.Name
                Case "tb_usuario"
                    tb_password.Focus()
                Case "tb_password"
                    OKButton_Click(sender, Nothing)
            End Select

        End If

    End Sub


End Class
