Imports System.Data

Public Class Inventario_NuevoCatalago
    Private xtipo As String = ""
    Public Sub New(tipo As String)
        InitializeComponent()
        Me.xtipo = tipo
    End Sub

    Public Sub Loaded_done() Handles Me.Loaded
        Select Case xtipo
            Case "Familia"
                title.Content = "AGREGAR NUEVA FAMILIA"
            Case "Marca"
                title.Content = "AGREGAR NUEVA MARCA"
            Case "Aseguradora"
                title.Content = "AGREGAR NUEVA ASEGURADORA"
            Case "Ubicacion"
                title.Content = "AGREGAR NUEVA UBICACION"
            Case Else
                Me.Close()
        End Select
    End Sub

    Public Sub agregarClick() Handles btn_ok.Click
        Dim ds As New DataSet
        Try

        
            Select Case xtipo

                Case "Familia"
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xDescripcion", "xActivo"}, {"AGREGAR", "FAMILIAS", tb_descripcion.Text, 1}).Fill(ds)
                Case "Marca"
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xDescripcion", "xActivo"}, {"AGREGAR", "MARCA", tb_descripcion.Text, 1}).Fill(ds)
                Case "Aseguradora"
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xDescripcion", "xActivo"}, {"AGREGAR", "ASEGURADORA", tb_descripcion.Text, 1}).Fill(ds)
                Case "Ubicacion"
                    Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Opr_Catalagos]", {"xAlias", "xCat", "xDescripcion", "xActivo"}, {"AGREGAR", "UBICACIONES", tb_descripcion.Text, 1}).Fill(ds)

            End Select

            Me.DialogResult = True

        Catch ex As Exception
            Dim xm As New msg_error("ALGO SALIO MAL")
            Me.DialogResult = False
        End Try



    End Sub

    Public Sub CancelarClick() Handles btn_cancel.Click
        Me.DialogResult = False
    End Sub
End Class
