'------------------------------------------------------------------------------
' <auto-generated>
'    Este código se generó a partir de una plantilla.
'
'    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
'    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class Sys_Usuarios
    Public Property id_operador As Integer
    Public Property nombre As String
    Public Property usuario As String
    Public Property id_departamento As Decimal
    Public Property activo As Boolean
    Public Property caducidad As Date
    Public Property id_jerarquia As Decimal
    Public Property id_sucursal As Decimal
    Public Property id_empleado As Decimal
    Public Property cambia_sucursal As Boolean
    Public Property id_poblacion As Nullable(Of Decimal)
    Public Property psw_ant As String
    Public Property password As String
    Public Property LastPswChanged As Nullable(Of Date)
    Public Property opciones As String
    Public Property isAdmin As Nullable(Of Boolean)
    Public Property status As Nullable(Of Integer)
    Public Property sesionActual As Nullable(Of Integer)

End Class
