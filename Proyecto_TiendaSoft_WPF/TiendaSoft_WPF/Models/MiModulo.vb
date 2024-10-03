Imports System.Data.SqlClient

Module MiModulo

#Region " Variables... "
    Public Const strName As String = "InventSOFT"
    Public Const strVer As String = "1.1.16"

    Public Const strApp As String = "POINT"
    Public Const strBasedeDatos As String = "InventarioPDV"

    Public xOpererador As Int32 = 0
    Public xNombreUsuario As String = ""
    Public xOpciones As String = ""
    Public xAdmin As Boolean = False

    Public Mi_conexion As Conexion_Sql
#End Region

#Region " Variables Ventas"
    Public imp_cambio As Double = 0
    Public imp_pagar As Double = 0
    Public imp_pago As Double = 0
#End Region

End Module