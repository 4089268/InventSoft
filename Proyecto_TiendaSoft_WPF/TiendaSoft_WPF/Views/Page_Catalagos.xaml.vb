Class Page_Catalagos

    Private _idPieza As String = ""

    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        btn_Aseguradoras.IsEnabled = False
        If (_idPieza.Length > 0) Then
            'Product_Frame.Navigate(New Page_product_catalagoProducto(_idPieza))
        Else
            Product_Frame.Navigate(New Catalagos_Modificar("ASEGURADORA"))
        End If

    End Sub

    Sub buttonsClick(sender As Object, e As RoutedEventArgs) Handles btn_Aseguradoras.Click, btn_grupos.Click, btn_Marcas.Click, btn_tipos.Click, btn_Ubicaciones.Click
        restaurarBotones()
        sender.IsEnabled = False

        Select Case sender.name
            Case "btn_Aseguradoras"
                Product_Frame.Navigate(New Catalagos_Modificar("ASEGURADORA"))
            Case "btn_grupos"
                Product_Frame.Navigate(New Catalagos_Modificar("FAMILIAS"))
            Case "btn_Marcas"
                Product_Frame.Navigate(New Catalagos_Modificar("MARCA"))
            Case "btn_tipos"
                Product_Frame.Navigate(New Catalagos_Modificar("TIPO"))
            Case "btn_Ubicaciones"
                Product_Frame.Navigate(New Catalagos_Modificar("UBICACIONES"))
        End Select
    End Sub

    Private Sub restaurarBotones()
        btn_Aseguradoras.IsEnabled = True
        btn_grupos.IsEnabled = True
        btn_Marcas.IsEnabled = True
        btn_tipos.IsEnabled = True
        btn_Ubicaciones.IsEnabled = True
    End Sub

End Class
