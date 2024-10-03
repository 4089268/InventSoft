Imports OfficeOpenXml

Class Application

    ' Los eventos de nivel de aplicación, como Startup, Exit y DispatcherUnhandledException
    ' se pueden controlar en este archivo.

    Private Sub OnAppStartup_UpdateThemeName(sender As Object, e As StartupEventArgs)

        DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName()

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial
    End Sub
End Class
