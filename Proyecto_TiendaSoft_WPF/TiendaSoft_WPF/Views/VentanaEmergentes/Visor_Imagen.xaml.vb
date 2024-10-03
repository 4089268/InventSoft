Imports System
Imports System.IO

Public Class Visor_Imagen

    Property Imagenes As List(Of RutaImageInvent)
    Private xTmmpImage As BitmapImage
    Private index As Integer = 0

    Public Sub New(xr As List(Of RutaImageInvent), i As Integer)
        InitializeComponent()
        Imagenes = xr
        index = i
    End Sub

    Public Sub me_loadedDone() Handles Me.Loaded
        Actualizar_panelImagenes()
        slider_valueChanged()
        ActualizarImagenPrincipal()
    End Sub
    Public Sub cambiarTitulo(title As String)
        Me.Title = title
    End Sub
    Public Sub ActualizarImagenPrincipal()
        Dim tmpRuta = Imagenes.ElementAt(index).ImagenCompleta
        Using xstream = File.OpenRead(tmpRuta)
            Dim tmpImage As New BitmapImage()
            tmpImage.BeginInit()
            tmpImage.CacheOption = BitmapCacheOption.OnLoad
            tmpImage.StreamSource = xstream
            tmpImage.EndInit()
            Me.img1.Source = tmpImage
        End Using

    End Sub


    '************ Eventos UI ************
    Private Sub BotonAnterior_click(sender As Object, e As RoutedEventArgs) Handles btn_anterior.Click
        btn_siguiente.IsEnabled = True
        btn_siguiente.Opacity = 1
        If index > 0 Then
            index = index - 1
            slider1.Value = 1
            ActualizarImagenPrincipal()

            If index = 0 Then
                btn_anterior.IsEnabled = False
                btn_anterior.Opacity = 0.3
            End If

        End If
    End Sub
    Private Sub BotonSiguiente_click(sender As Object, e As RoutedEventArgs) Handles btn_siguiente.Click
        btn_anterior.IsEnabled = True
        btn_anterior.Opacity = 1
        If index < (Imagenes.Count - 1) Then
            index = index + 1
            slider1.Value = 1
            ActualizarImagenPrincipal()

            If index = (Imagenes.Count - 1) Then
                btn_siguiente.IsEnabled = False
                btn_siguiente.Opacity = 0.3
            End If

        End If
    End Sub
    Private Sub Me_keyPress(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.Key = Key.Left Then
            BotonAnterior_click(Nothing, Nothing)
        End If
        If e.Key = Key.Right Then
            BotonSiguiente_click(Nothing, Nothing)
        End If
    End Sub
    Private Sub slider_valueChanged() Handles slider1.ValueChanged
        img1.Width = scrollviewer1.ActualWidth * slider1.Value
        img1.Height = scrollviewer1.ActualHeight * slider1.Value
    End Sub


    '************ IMAGENES ************
    Public Sub Actualizar_panelImagenes()
        Try

            Dim index = 0
            For Each xRutaImg As RutaImageInvent In Imagenes

                Dim imageControl As New Image
                imageControl.Margin = New Thickness(2, 2, 2, 2)

                '**** cargar imagen en memoria
                Using xstream = File.OpenRead(xRutaImg.ImagenCompleta)
                    Dim tmpImage As New BitmapImage()
                    tmpImage.BeginInit()
                    tmpImage.CacheOption = BitmapCacheOption.OnLoad
                    tmpImage.StreamSource = xstream
                    tmpImage.EndInit()
                    imageControl.Source = tmpImage
                End Using
                imageControl.Tag = index

                AddHandler imageControl.MouseLeftButtonDown, AddressOf PanelImagenes_fotoClick
                sp_imagenes.Children.Add(imageControl)
                index = index + 1
            Next


        Catch ex As Exception
            Dim x As New msg_error("Error al cargar al mostrar las imagenes", ex)
        End Try
    End Sub
    Private Sub PanelImagenes_fotoClick(sender As Object, e As MouseButtonEventArgs)
        Me.img1.Source = sender.source
        Me.img1.Tag = sender.Tag
    End Sub



End Class
