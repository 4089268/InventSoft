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

            Dim h4 = sheet.Cells(nRow, 4)
            h4.Style.Font.Size = 12
            h4.Style.Font.Bold = True
            h4.Value = "MARCA"

            Dim h5 = sheet.Cells(nRow, 5)
            h5.Style.Font.Size = 12
            h5.Style.Font.Bold = True
            h5.Value = "TIPO"

            Dim h6 = sheet.Cells(nRow, 6)
            h6.Style.Font.Size = 12
            h6.Style.Font.Bold = True
            h6.Value = "MODELO"

            Dim h7 = sheet.Cells(nRow, 7)
            h7.Style.Font.Size = 12
            h7.Style.Font.Bold = True
            h7.Value = "UBICACION"

            Dim h8 = sheet.Cells(nRow, 8)
            h8.Style.Font.Size = 12
            h8.Style.Font.Bold = True
            h8.Value = "ANAQUEL"

            Dim h9 = sheet.Cells(nRow, 9)
            h9.Style.Font.Size = 12
            h9.Style.Font.Bold = True
            h9.Value = "NUM PIEZA"

            Dim h10 = sheet.Cells(nRow, 10)
            h10.Style.Font.Size = 12
            h10.Style.Font.Bold = True
            h10.Value = "FAMILIA"

            Dim h11 = sheet.Cells(nRow, 11)
            h11.Style.Font.Size = 12
            h11.Style.Font.Bold = True
            h11.Value = "ASEGURADORA"
            nRow += 1


            '// Generera rows
            For Each data As Vw_Opr_Productos In Me.datos
                Dim c1 = sheet.Cells(nRow, 1)
                c1.Value = data.codigo.ToString

                Dim c2 = sheet.Cells(nRow, 2)
                c2.Value = data.descripcion.ToString

                Dim c3 = sheet.Cells(nRow, 3)
                c3.Value = data.existencia

                Dim c4 = sheet.Cells(nRow, 4)
                c4.Value = data.marca.ToString

                Dim c5 = sheet.Cells(nRow, 5)
                c5.Value = data.tipo.ToString

                Dim c6 = sheet.Cells(nRow, 6)
                c6.Value = data.modelo.ToString

                Dim c7 = sheet.Cells(nRow, 7)
                c7.Value = data.ubicacion.ToString

                Dim c8 = sheet.Cells(nRow, 8)
                c8.Value = data.anaquel.ToString

                Dim c9 = sheet.Cells(nRow, 9)
                c9.Value = data.num_pieza.ToString

                Dim c10 = sheet.Cells(nRow, 10)
                c10.Value = data.familia.ToString

                Dim c11 = sheet.Cells(nRow, 11)
                c11.Value = data.aseguradora.ToString

                nRow += 1
            Next


            sheet.Column(1).BestFit = True
            sheet.Column(2).AutoFit()
            sheet.Column(3).BestFit = True
            sheet.Column(4).Width = 18
            sheet.Column(5).Width = 18
            sheet.Column(6).AutoFit()
            sheet.Column(7).Width = 18
            sheet.Column(8).Width = 18
            sheet.Column(9).AutoFit()
            sheet.Column(10).Width = 18
            sheet.Column(11).Width = 18



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
