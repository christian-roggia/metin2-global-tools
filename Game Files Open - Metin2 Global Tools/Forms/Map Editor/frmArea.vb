Imports System.Math

Public Class frmArea
    Private ObjectArray() As MyObject
    Private PRBArray() As PRB
    Private PRTArray() As PRT
    Private MapPath As String
    Private MapSize() As Integer

    Private Sub frmArea_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadPRBDataBase(DataGridView1, Application.StartupPath & "\DataBases\Area Data\default_YPRT.xml", PRBArray)
        LoadPRTDataBase(DataGridView3, Application.StartupPath & "\DataBases\Area Data\default_YPRT.xml", PRTArray)
    End Sub

    Private Sub ImportAreaDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportAreaDataToolStripMenuItem.Click
        Dim OpenFileDLG As New OpenFileDialog
        OpenFileDLG.Filter = "Settings File (.txt)|*.txt|All Files|*.*"
        If OpenFileDLG.ShowDialog = DialogResult.OK Then
            ClearAll()
            MapPath = OpenFileDLG.FileName.Remove(OpenFileDLG.FileName.LastIndexOf("\") + 1)

            If FromMapSizeToComboBox(FindMapSize(OpenFileDLG.FileName), ComboQuadrants) = False Then
                Exit Sub
            End If

            LoadAreaData(MapPath & "000000\areadata.txt", ObjectArray)

            InsertAllValues()

            PictureBox1.Image = LoadDoubleAttribute(MapPath & "000000\attr.atr")

            SetObjectsPoints(PictureBox1.Image, ObjectArray, {0, 0})
        End If
    End Sub

    Private Function InsertAllValues() As Boolean
        If IsNothing(ObjectArray) = True Then
            Return False
        End If

        For i = 0 To ObjectArray.Length - 1
            With ObjectArray(i)
                DataGridView2.Rows.Add(i.ToString, .X.ToString, .Y.ToString, .Z, .ID, .RotateX, .RotateY, .RotateZ, .Unknown)
            End With
        Next i

        Return True
    End Function

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        ClearAll()
    End Sub

    Private Sub ComboQuadrants_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboQuadrants.SelectedIndexChanged
        PictureBox1.Image = LoadDoubleAttribute(MapPath & ComboQuadrants.Text & "\attr.atr")
        LoadAreaData(MapPath & ComboQuadrants.Text & "\areadata.txt", ObjectArray)
        SetObjectsPoints(PictureBox1.Image, ObjectArray, {ComboQuadrants.Text.Substring(2, 1), ComboQuadrants.Text.Substring(5, 1)})
        InsertAllValues()
    End Sub

    Private Sub ClearAll()
        DataGridView2.Rows.Clear()
        ObjectArray = Nothing
        ComboQuadrants.Items.Clear()
        ComboQuadrants.Text = ""
        PictureBox1.Image = Nothing
        MapPath = ""
        MapSize = Nothing
    End Sub

    Private Sub ImportFromDirectoyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportFromDirectoyToolStripMenuItem.Click
        Dim OpenFileDLG As New OpenFileDialog
        OpenFileDLG.Filter = "DataBase XML (.xml)|*.xml|All Files|*.*"

        If OpenFileDLG.ShowDialog = DialogResult.OK Then
            LoadPRBDataBase(DataGridView1, OpenFileDLG.FileName, PRBArray)
        End If
    End Sub

    Private Sub NewDataBaseFromDirectoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewDataBaseFromDirectoryToolStripMenuItem.Click
        Control.CheckForIllegalCrossThreadCalls = False
        frmWait.FormLoader = Me
        frmWait.Parameters = {"Wait until the databasing process is finished...", "false"}
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            frmWait.Show()
            Dim TimeStamp As String = Now.Day.ToString & "." & Now.Month.ToString & "." & Now.Year.ToString & "_" & Now.Hour & Now.Minute & Now.Second
            SaveYPRTPath = Application.StartupPath & "\DataBases\Area Data\db_YPRT_" & TimeStamp & ".xml"
            DirectoryPath = FolderBrowserDialog1.SelectedPath

            BackgroundWorker1.RunWorkerAsync(FolderBrowserDialog1.SelectedPath)
            Timer1.Start()
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        My.Computer.FileSystem.WriteAllText(SaveYPRTPath, "<?xml version='1.0' encoding='GB2312'?>" & vbCrLf & "<areadata>" & vbCrLf, False)
        ProcessDir(e.Argument)
        My.Computer.FileSystem.WriteAllText(SaveYPRTPath, "</areadata>", True)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If BackgroundWorker1.IsBusy = False Then
            frmWait.Close()
            Timer1.Stop()
        End If
    End Sub
End Class