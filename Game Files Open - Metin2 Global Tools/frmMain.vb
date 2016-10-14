Imports System.Xml

Public Class frmMain
    Private level1 As String = "HKEY_"
    Private level2 As String = "CLASSES_"
    Private level3 As String = "ROOT\"

    Private Sub MapEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MapEditorToolStripMenuItem.Click
        frmMapEditor.Show()
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Application.Exit()
    End Sub

    Private Sub DownloadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadToolStripMenuItem.Click
        If frmSharing.UserName = Nothing Then
            frmLogin.ShowDialog()
        Else
            frmSharing.ShowDialog()
        End If
    End Sub

    Private Sub DecrypterDecompresserToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DecrypterDecompresserToolStripMenuItem.Click
        frmLZO.Show()
    End Sub

    Private Sub QuestEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuestEditorToolStripMenuItem.Click

    End Sub

    Private Sub LauncherEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LauncherEditorToolStripMenuItem.Click
        frmLauncher.ShowDialog()
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Updates\Update.exe") Then
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Updates\Restart.res") Then
                My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\Updates\Restart.res")
                Shell(Application.StartupPath & "\Updates\Update.exe", AppWinStyle.MinimizedFocus)
                Application.Exit()
            Else
                My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\Updates\Restart.exe")
            End If
        End If

        If My.Computer.Network.IsAvailable = True Then
            AutoPatcher.RunWorkerAsync()
        End If
    End Sub

    Private Sub AutoPatcher_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles AutoPatcher.DoWork

        Try
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Data\Updates\Updates.xml") Then
                My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\Data\Updates\Updates.xml")
            End If

            My.Computer.Network.DownloadFile("http://www.gamefilesopen.com/global_tools/updates/updates.xml", Application.StartupPath & "\Data\Updates\Updates.xml")

            Dim x As New XmlDocument()
            Dim XMLstring As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\Data\Updates\Updates.xml")
            x.LoadXml(XMLstring)
            Dim files As XmlNodeList = x.GetElementsByTagName("file")
            Dim Counter As Integer = 0
            Dim FilesToDownload(-1) As Integer

            For i = 0 To files.Count - 1
                If Not ChFile_md5(files.Item(i).Attributes("destination").Value.ToString.Replace("..\", Application.StartupPath & "\")) = files.Item(i).Attributes("check_md5").Value.ToString Then
                    Counter += 1
                    ReDim Preserve FilesToDownload(FilesToDownload.Length)
                    FilesToDownload(FilesToDownload.Length - 1) = i
                End If
            Next i

            If Counter > 0 Then
                frmPatcher.FilesToDownload = FilesToDownload
                frmPatcher.files = files
                frmPatcher.ShowDialog()
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub RegenEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegenEditorToolStripMenuItem.Click
        frmRegen.Show(Me)
    End Sub

    Private Sub AreaDataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AreaDataEditorToolStripMenuItem.Click
        frmArea.Show(Me)
    End Sub
End Class