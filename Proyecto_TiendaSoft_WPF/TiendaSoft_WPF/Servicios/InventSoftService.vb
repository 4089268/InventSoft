Public Class InventSoftService

    Public Function Catalogo_Familia() As Cat_Familia()
        Dim tmpData = New List(Of Cat_Familia)
        Using dbContext As New InventarioSoftDB
            tmpData = dbContext.Cat_Familia.Where(Function(item) item.activo = True).ToList()
        End Using
        Return tmpData.ToArray()
    End Function
    Public Function Catalogo_Marcas() As Cat_Marca()
        Dim tmpData = New List(Of Cat_Marca)
        Using dbContext As New InventarioSoftDB
            tmpData = dbContext.Cat_Marca.Where(Function(item) item.activo = True).ToList()
        End Using
        Return tmpData.ToArray
    End Function
    Public Function Catalogo_Tipos() As Cat_Tipo()
        Dim tmpData = New List(Of Cat_Tipo)
        Using dbContext As New InventarioSoftDB
            tmpData = dbContext.Cat_Tipo.Where(Function(item) item.activo = True).ToList()
        End Using
        Return tmpData.ToArray
    End Function
    Public Function Catalogo_Ubicaciones() As Cat_Ubicaciones()
        Dim tmpData = New List(Of Cat_Ubicaciones)
        Using dbContext As New InventarioSoftDB
            tmpData = dbContext.Cat_Ubicaciones.ToList()
        End Using
        Return tmpData.ToArray
    End Function
    Public Function Catalogo_Aseguradoras() As Cat_aseguradora()
        Dim tmpData = New List(Of Cat_aseguradora)
        Using dbContext As New InventarioSoftDB
            tmpData = dbContext.Cat_aseguradora.Where(Function(item) item.activo = True).ToList()
        End Using
        Return tmpData.ToArray
    End Function

    Public Function BusquedaBasica(busquedaString As String) As Vw_Opr_Productos()
        Dim tmpOri As New List(Of Vw_Opr_Productos)
        Dim busquedaArr = busquedaString.ToLower.Split(" ")
        Using dbContext As New InventarioSoftDB
            tmpOri = dbContext.Vw_Opr_Productos.ToList
        End Using
        For Each s In busquedaArr
            tmpOri = tmpOri.Where(Function(item) item.descripcion.ToLower.Contains(s) Or item.anaquel.ToLower.Contains(s) Or item.aseguradora.ToLower.Contains(s) Or item.codigo.ToLower.Contains(s) Or item.familia.ToLower.Contains(s) Or item.marca.ToLower.Contains(s) Or item.tipo.ToLower.Contains(s) Or item.ubicacion.ToLower.Contains(s)).ToList
        Next
        Return tmpOri.ToArray
    End Function
    Public Function BusquedaAvanzada(busqueda As BusquedaAvzModel) As Vw_Opr_Productos()
        Dim tmpData As New List(Of Vw_Opr_Productos)
        Using dbContext As New InventarioSoftDB
            tmpData = dbContext.Vw_Opr_Productos.ToList
        End Using

        If busqueda.Descripcion.Length > 0 Then
            tmpData = tmpData.Where(Function(item) item.descripcion.ToLower.Contains(busqueda.Descripcion.ToLower)).ToList
        End If

        If busqueda.Modelo.Length > 0 Then
            tmpData = tmpData.Where(Function(item) item.modelo.ToLower.Contains(busqueda.Modelo.ToLower)).ToList
        End If

        If busqueda.Placa.Length > 0 Then
            tmpData = tmpData.Where(Function(item) item.placa.ToLower.Contains(busqueda.Placa.ToLower)).ToList
        End If

        If busqueda.Codigo.Length > 0 Then
            tmpData = tmpData.Where(Function(item) item.codigo.ToLower.Contains(busqueda.Codigo.ToLower)).ToList
        End If

        If busqueda.Numero_Pieza.Length > 0 Then
            tmpData = tmpData.Where(Function(item) item.num_pieza.ToLower.Contains(busqueda.Numero_Pieza.ToLower)).ToList
        End If

        If busqueda.Anaquel.Length > 0 Then
            tmpData = tmpData.Where(Function(item) item.anaquel.ToLower.Contains(busqueda.Anaquel.ToLower)).ToList
        End If

        If busqueda.Id_Marca > 0 Then
            tmpData = tmpData.Where(Function(item) item.id_marca = busqueda.Id_Marca).ToList
        End If

        If busqueda.Id_Tipo > 0 Then
            tmpData = tmpData.Where(Function(item) item.id_tipo = busqueda.Id_Tipo).ToList
        End If

        If busqueda.Id_Aseguradora > 0 Then
            tmpData = tmpData.Where(Function(item) item.id_aseguradora = busqueda.Id_Aseguradora).ToList
        End If

        If busqueda.Id_Familia > 0 Then
            tmpData = tmpData.Where(Function(item) item.id_familia = busqueda.Id_Familia).ToList
        End If

        If busqueda.Id_Ubicacion > 0 Then
            tmpData = tmpData.Where(Function(item) item.id_ubicacion = busqueda.Id_Ubicacion).ToList
        End If

        Return tmpData.ToArray
    End Function


End Class
