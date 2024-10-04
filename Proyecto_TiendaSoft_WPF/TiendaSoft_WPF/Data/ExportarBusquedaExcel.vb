Imports System.IO
Imports OfficeOpenXml


Public Class ExportarBusquedaExcel

    Private datos As IEnumerable(Of Vw_Opr_Productos)
    Dim rutaArchivo As String = ""

    Public Sub New(productos As IEnumerable(Of Vw_Opr_Productos))
        Me.datos = productos
    End Sub

    Public Sub Exportar()
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial

        Using package As New ExcelPackage()
            Dim sheet As ExcelWorksheet = package.Workbook.Worksheets.Add("InventSoft")

            Dim nRow As Integer = 1

            'Generar cabeceras
            Dim h1 = sheet.Cells(nRow, 1)
            h1.Style.Font.Size = 12
            h1.Style.Font.Bold = True
            h1.Value = "CODIGO"

            Dim h2 = sheet.Cells(nRow, 2)
            h2.Style.Font.Size = 12
            h2.Style.Font.Bold = True
            h2.Value = "DESCRIPCION"

            Dim h3 = sheet.Cells(nRow, 3)
            h3.Style.Font.Size = 12
            h3.Style.Font.Bold = True
            h3.Value = "EXISTENCIA"

            Dim h7 = sheet.Cells(nRow, 4)
            h7.Style.Font.Size = 12
            h7.Style.Font.Bold = True
            h7.Value = "UBICACION"

            Dim h8 = sheet.Cells(nRow, 5)
            h8.Style.Font.Size = 12
            h8.Style.Font.Bold = True
            h8.Value = "ANAQUEL"

            Dim h10 = sheet.Cells(nRow, 6)
            h10.Style.Font.Size = 12
            h10.Style.Font.Bold = True
            h10.Value = "FAMILIA"

            nRow += 1


            '// Generera rows
            For Each data As Vw_Opr_Productos In Me.datos
                Dim c1 = sheet.Cells(nRow, 1)
                c1.Value = data.codigo.ToString

                Dim c2 = sheet.Cells(nRow, 2)
                c2.Value = data.descripcion.ToString

                Dim c3 = sheet.Cells(nRow, 3)
                c3.Value = data.existencia

                Dim c7 = sheet.Cells(nRow, 4)
                c7.Value = data.ubicacion.ToString

                Dim c8 = sheet.Cells(nRow, 5)
                c8.Value = data.anaquel.ToString

                Dim c10 = sheet.Cells(nRow, 6)
                c10.Value = data.familia.ToString

                nRow += 1
            Next


            sheet.Column(1).BestFit = True
            sheet.Column(2).AutoFit()
            sheet.Column(3).BestFit = True
            sheet.Column(4).BestFit = True
            sheet.Column(5).Width = 18
            sheet.Column(6).Width = 18


            '// Geneara nombre documento
            Dim nombreArchivo = String.Format("datosBusqueda_{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMdd"), Guid.NewGuid().ToString().Replace("-", "").Substring(0, 7))
            Dim downloadsFolder = System.IO.Path.GetTempPath
            rutaArchivo = Path.Combine(downloadsFolder, nombreArchivo)

            '// Exportar datos
            package.SaveAs(New FileInfo(rutaArchivo))

        End Using

        System.Diagnostics.Process.Start(rutaArchivo)

    End Sub

End Class
