Imports System.Xml

Public Class frmPatcher
    Private X As Integer
    Private Y As Integer
    Public FilesToDownload() As Integer
    Public files As XmlNodeList

    Private Sub frmPatcher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(frmMain.Location.X + frmMain.Width \ 2 - Me.Width \ 2,
                         frmMain.Location.Y + frmMain.Height \ 2 - Me.Height \ 2)
        Control.CheckForIllegalCrossThreadCalls = False
        AutoPatcher.RunWorkerAsync()
        Timer1.Start()
    End Sub

    Private Sub AutoPatcher_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles AutoPatcher.DoWork
        For i = 0 To FilesToDownload.Length - 1
            With files.Item(FilesToDownload(i))
                Label1.Text = "Downloading file [" & (i + 1) & "/" & FilesToDownload.Length & "] (" &
                    .Attributes("filename").Value.ToString & ")" & " Update: " & .Attributes("data").Value.ToString

                If My.Computer.FileSystem.FileExists(.Attributes("destination").Value.ToString.Replace("..\", Application.StartupPath & "\")) Then
                    My.Computer.FileSystem.DeleteFile(.Attributes("destination").Value.ToString.Replace("..\", Application.StartupPath & "\"))
                End If

                My.Computer.Network.DownloadFile(.Attributes("url").Value.ToString.Replace("../", "http://gamefilesopen.com/global_tools/updates/"),
                                                 .Attributes("destination").Value.ToString.Replace("..\", Application.StartupPath & "\"))
            End With
        Next i
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If AutoPatcher.IsBusy = False Then
            Me.Close()
            Timer1.Stop()
        End If
    End Sub
End Class