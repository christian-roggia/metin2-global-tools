Imports System.Net
Imports System.Xml

Public Class frmWait
    Private X As Integer
    Private Y As Integer
    Public FormLoader As Form = frmLogin
    Public Parameters() As String = {"Connecting to the Server...", "true"}

    Private Sub frmWait_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TopLevel = True
        Label1.Text = Parameters(0)
        Me.Location = New Point(FormLoader.Location.X + FormLoader.Width \ 2 - Me.Width \ 2,
                                 FormLoader.Location.Y + FormLoader.Height \ 2 - Me.Height \ 2)
        If Parameters(1) = "true" Then
            BackgroundWorker1.RunWorkerAsync()
            Timer1.Start()
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim client As New WebClient()
        Dim x As New XmlDocument()
        Try
            x.LoadXml(client.DownloadString("http://www.gamefilesopen.com/global_tools/init.php"))
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
        Dim init As XmlNodeList = x.GetElementsByTagName("init")
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If BackgroundWorker1.IsBusy = False Then
            Me.Close()
            Timer1.Stop()
        End If
    End Sub

    Private Sub RectangleShape1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RectangleShape1.MouseDown
        X = Control.MousePosition.X - Me.Location.X
        Y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub RectangleShape1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RectangleShape1.MouseMove
        Dim NewPoint As System.Drawing.Point
        If e.Button = Windows.Forms.MouseButtons.Left Then
            NewPoint = Control.MousePosition
            NewPoint.X -= X
            NewPoint.Y -= Y
            Me.Location = NewPoint
        End If
    End Sub
End Class