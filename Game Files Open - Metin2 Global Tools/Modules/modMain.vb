Imports AlphaBG
Imports PerPixelAlphaForm
Module modMain
    Private WithEvents Domain As AppDomain = AppDomain.CurrentDomain
    Private WithEvents bg As AlphaBG
    Sub Main()
        Application.EnableVisualStyles()
        bg = New AlphaBG(My.Resources.splash, frmSplash, AlphaBG.Settings.DrawControls)
        bg.StartPosition = FormStartPosition.CenterScreen
        Application.Run(bg)
    End Sub

    Private Sub Domain_UnhandledException(ByVal sender As Object, ByVal e As System.UnhandledExceptionEventArgs) Handles Domain.UnhandledException
        MessageBox.Show(e.ToString, "Unhandled Exception")
    End Sub

    Private Sub bg_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles bg.Load
        bg.Fade(0)
        bg.Show()
        Dim t As Stopwatch
        For i As Double = 0 To Math.PI / 2 Step Math.PI / 90
            t = Stopwatch.StartNew()
            bg.Fade(Math.Sin(i))
            Do Until t.ElapsedMilliseconds >= 13
                Application.DoEvents()
            Loop
        Next
        finish()
    End Sub

    Public Sub finish()
        Dim t As Stopwatch
        t = Stopwatch.StartNew()
        For i As Double = Math.PI / 2 To 0 / 2 Step (Math.PI / 90) * -1
            t = Stopwatch.StartNew()
            bg.Fade(Math.Sin(i))
            Do Until t.ElapsedMilliseconds >= 13
                Application.DoEvents()
            Loop
        Next
        bg.Hide()
        bg.ShowInTaskbar = False
        frmMain.Show()
    End Sub
End Module
