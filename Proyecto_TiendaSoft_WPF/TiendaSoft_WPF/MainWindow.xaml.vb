Imports DevExpress.Xpf.Bars
Imports System.Threading
Imports System.Globalization
Imports System.Data.SqlClient
Imports System.Data
Imports System

Class MainWindow
    Dim dispatcherTimer As System.Windows.Threading.DispatcherTimer

    ''UI Loaded
    Private Sub rootGrid_onLoaded() Handles rootGrid.Loaded
        Me.Title = String.Format("INVENTSOFT {0}", strVer)

        Thread.CurrentThread.CurrentCulture = New CultureInfo("es-MX")
        MostrarBotones(False)

        lb_name.Content = strName
        lb_ver.Content = "V." + strVer

        If (xOpererador > 0 And xNombreUsuario.Length > 0 And xOpciones.Length > 0) Then

            dispatcherTimer = New System.Windows.Threading.DispatcherTimer()
            AddHandler dispatcherTimer.Tick, AddressOf actualizar_FechaHora
            dispatcherTimer.Interval = New TimeSpan(0, 0, 1)
            dispatcherTimer.Start()

            label_NombreOperador.Content = xNombreUsuario
            VerificarOpciones()
        Else
            Me.Close()
        End If

        btn_corte.IsEnabled = False
    End Sub

    Private Sub VerificarOpciones()
        navFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden
        navFrame.Navigate(New Page_Busqueda(Me))
        btn_ventas.IsEnabled = False

        barManager.Bars(0).ItemLinks(0).IsEnabled = True
        barManager.Bars(0).ItemLinks(1).IsEnabled = True
        barManager.Bars(0).ItemLinks(2).IsEnabled = True
        barManager.Bars(0).ItemLinks(3).IsEnabled = True
        barManager.Bars(0).ItemLinks(4).IsEnabled = True
        barManager.Bars(0).ItemLinks(5).IsEnabled = True
        barManager.Bars(0).ItemLinks(6).IsEnabled = True

        'barManager.Bars(0).ItemLinks(9).IsEnabled = True

        barManager.Bars(0).ItemLinks(0).IsVisible = True
        barManager.Bars(0).ItemLinks(1).IsVisible = True
        barManager.Bars(0).ItemLinks(2).IsVisible = True
        barManager.Bars(0).ItemLinks(3).IsVisible = True
        barManager.Bars(0).ItemLinks(4).IsVisible = True
        barManager.Bars(0).ItemLinks(5).IsVisible = True
        barManager.Bars(0).ItemLinks(6).IsVisible = True

        barManager.Bars(0).ItemLinks(8).IsEnabled = True
        barManager.Bars(0).ItemLinks(8).IsVisible = True

        'barManager.Bars(0).ItemLinks(9).IsVisible = True
        If xAdmin Then
            barManager.Bars(0).ItemLinks(7).IsEnabled = True
            barManager.Bars(0).ItemLinks(7).IsVisible = True
        End If

    End Sub

    Private Sub actualizar_FechaHora(ByVal sender As Object, ByVal e As EventArgs)
        labelHora.Content = Date.Now.ToString("F")
        CommandManager.InvalidateRequerySuggested()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles btn_ventas.ItemClick, btn_catalagos.ItemClick, btn_Salir.ItemClick, btn_configuracion.ItemClick, btn_corte.ItemClick, btn_inventario.ItemClick
        restaurarBotones()
        sender.IsEnabled = False

        Select Case sender.name
            Case "btn_ventas"
                navFrame.Navigate(New Page_Busqueda(Me))

            Case "btn_configuracion"
                navFrame.Source = New Uri("Views/Page_Configuracion.xaml", UriKind.Relative)

            Case "btn_catalagos"
                navFrame.Navigate(New Page_Catalagos)

            Case "btn_corte"
                navFrame.Source = New Uri("Views/Page_Corte.xaml", UriKind.Relative)

            Case "btn_inventario"
                navFrame.Navigate(New Page_Inventario())

            Case "btn_Salir"
                restaurarBotones()
                Me.Close()
        End Select
    End Sub

    Private Sub restaurarBotones()
        btn_ventas.IsEnabled = True
        btn_ventas.IsEnabled = True
        btn_catalagos.IsEnabled = True
        btn_inventario.IsEnabled = True
        btn_corte.IsEnabled = False

        btn_configuracion.IsEnabled = True
    End Sub

    Private Sub MostrarBotones(val As Boolean)
        For Each item As BarItemLinkBase In barManager.Bars(0).ItemLinks
            item.IsVisible = val
            item.IsEnabled = val
        Next
    End Sub
        
    Private Sub saliendo(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        Dim confir As New Saliendo_Confirmar

        If confir.ShowDialog Then
            Salir(e)
        Else
            btn_Salir.IsEnabled = True
            e.Cancel = True
        End If
    End Sub

    Sub Salir(e As ComponentModel.CancelEventArgs)
        Mi_conexion.cerrarConexion()
        
    End Sub


    ''LLAMADAS EXTERNAS
    Sub NuevaPieza()
        restaurarBotones()
        btn_inventario.IsEnabled = False
        navFrame.Navigate(New Page_Inventario("NUEVO"))
    End Sub

    Sub EditarPieza(id_producto As Integer)
        restaurarBotones()
        btn_inventario.IsEnabled = False
        Dim x As New InventarioMod
        x.id_producto = id_producto
        navFrame.Navigate(x)
    End Sub

    Sub AjustarInventarioPieza(id_producto As Integer)
        restaurarBotones()
        btn_inventario.IsEnabled = False
        Dim x As New Page_inv_remplazar
        x.id_producto = id_producto
        navFrame.Navigate(x)
    End Sub

    Sub activarPanel(x As Boolean)
        Me.navFrame.IsEnabled = x
    End Sub

End Class
