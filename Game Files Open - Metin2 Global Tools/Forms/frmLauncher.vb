Imports System.IO
Imports System.Xml
Imports System.ComponentModel

Public Class frmLauncher
    Private Struct() As String
    Private Pointer() As Integer
    Private MaxLength() As Integer
    Private bytes() As Byte
    Private LoadedLauncher As String

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            Dim sr As FileStream
            Dim reader As BinaryReader
            sr = New FileStream(OpenFileDialog1.FileName, FileMode.Open)
            LoadedLauncher = OpenFileDialog1.FileName.ToString
            reader = New BinaryReader(sr)
            bytes = reader.ReadBytes(sr.Length)
            sr.Close()

            If Not bytes.Length = 2279936 Then
                Dim FileSize As String = bytes.Length.ToString
                For i = 1 To 4
                    If FileSize.Length - 1 >= i * 3 + (i - 1) Then
                        FileSize = FileSize.Insert(FileSize.Length - (i * 3 + (i - 1)), ".")
                    End If
                Next i

                AddListBoxItem("Invalid 2010 Launcher, probably the file is encrypted or isn't a metin2 launcher!")
                AddListBoxItem("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")
                AddListBoxItem("Files Size: " & FileSize)
                AddListBoxItem("2010 Launcher Size: 2.279.936")
                Exit Sub
            End If

            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\DataBases\Launcher\Launcher2010.xml") Then
                Dim database As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\DataBases\Launcher\Launcher2010.xml")

                Dim x As New XmlDocument()
                x.LoadXml(database)
                Dim infos As XmlNodeList = x.GetElementsByTagName("info")

                For i = 0 To infos.Count - 1
                    ReDim Preserve Struct(i)
                    ReDim Preserve Pointer(i)
                    ReDim Preserve MaxLength(i)
                    Struct(i) = infos.Item(i).Attributes("name").Value.ToString
                    Pointer(i) = infos.Item(i).Attributes("PTR").Value.ToString
                    MaxLength(i) = infos.Item(i).Attributes("MaxLength").Value.ToString
                Next i

                DataGridView1.Rows.Clear()

                For i = 0 To Struct.Length - 1
                    Dim Values() As String = AddValue(i, bytes)
                    DataGridView1.Rows.Add(Values(0), Values(1), AscWords(Values(1)))
                Next i
                AddListBoxItem("Launcher loaded without errors")
            End If
        Else
            AddListBoxItem("Cannot find the DataBase!")
            AddListBoxItem("Path: ..\DataBases\Launcher\Launcher2010 DB.xml")
        End If
    End Sub

    Private Function AddValue(ByVal i As Integer, ByVal bytes() As Byte)
        Dim val As String = ""
        For s = 0 To MaxLength(i) - 1
            If Chr(bytes(Pointer(i) + s)) = Chr(0) Then
                Exit For
            End If

            val += Chr(bytes(Pointer(i) + s))
        Next s
        Return {Struct(i), val}
    End Function

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If IsNothing(bytes) = True Then
            AddListBoxItem("First you must load a launcher!")
            Exit Sub
        End If

        If SaveLauncher() = False Then
            Exit Sub
        End If

        If IsNothing(LoadedLauncher) = True Or My.Computer.FileSystem.FileExists(LoadedLauncher) = False Then
            AddListBoxItem("Impossible to find the launcher!")
            AddListBoxItem("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")
            AddListBoxItem("Path: " & LoadedLauncher)
            AddListBoxItem("Try to use the option 'Save As'...")
            Exit Sub
        End If

        Dim aFileStream As FileStream
        aFileStream = New FileStream(LoadedLauncher, FileMode.OpenOrCreate, FileAccess.Write)
        Dim myBinaryWriter As New BinaryWriter(aFileStream)
        myBinaryWriter.Write(bytes)
        aFileStream.Close()
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        If IsNothing(bytes) = True Then
            AddListBoxItem("First you must load a launcher!")
            Exit Sub
        End If

        If SaveLauncher() = False Then
            Exit Sub
        End If

        Dim SaveDialog As New SaveFileDialog
        SaveDialog.DefaultExt = ".exe"
        SaveDialog.FileName = "Launcher"
        SaveDialog.Filter = "Executable (.exe)|*.exe|Binary File (.bin)|*.bin|All Files|*.*"

        If SaveDialog.ShowDialog = DialogResult.OK Then
            Dim aFileStream As FileStream
            aFileStream = New FileStream(SaveDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write)
            Dim myBinaryWriter As New BinaryWriter(aFileStream)
            myBinaryWriter.Write(bytes)
            aFileStream.Close()
        End If
    End Sub

    Private Function SaveLauncher() As Boolean
        Dim ToWrite() As String

        For i = 0 To Struct.Length - 1
            ToWrite = DataGridView1.Rows(i).Cells(2).Value.ToString.Split("-")
            For s = 0 To MaxLength(i) - 1
                If s > ToWrite.Length - 1 Then
                    bytes(Pointer(i) + s) = 0
                Else
                    If IsNumeric(ToWrite(s)) = False Then
                        AddListBoxItem("Failed to save the launcher!")
                        AddListBoxItem("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")
                        AddListBoxItem("Invalid numeric value: " & ToWrite(s))
                        AddListBoxItem("Info: s = " & s.ToString & " i = " & i.ToString)
                        Return False
                    Else
                        bytes(Pointer(i) + s) = Convert.ToInt32(ToWrite(s))
                    End If
                End If
            Next s
        Next i

        Return True
    End Function

    Private Function AscWords(ByVal Word As String)
        Dim AscWord As String = ""
        For i = 0 To Word.Length - 1
            AscWord = AscWord & Asc(Word.Substring(i, 1)) & "-"
        Next i
        Return AscWord.Substring(0, AscWord.Length - 1)
    End Function

    Private Sub DataGridView1_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        If e.ColumnIndex = 2 Then
            If IsNothing(DataGridView1.Rows(e.RowIndex).Cells(2).Value) = True Then
                DataGridView1.Rows(e.RowIndex).Cells(1).Value = Nothing
                Exit Sub
            End If

            Dim TextEdited As String = DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString
            Dim EditedArray() As String

            If TextEdited.Contains("-") Then
                EditedArray = TextEdited.Split("-")
            Else
                EditedArray = {TextEdited}
            End If

            If EditedArray.Length > MaxLength(e.RowIndex) Then
                AddListBoxItem("Too long word (Max. " & MaxLength(e.RowIndex) & ") at the Row " & (e.RowIndex + 1) & " and Column " & (e.ColumnIndex + 1))
                ReDim Preserve EditedArray(MaxLength(e.RowIndex) - 1)
                'Clear
                DataGridView1.Rows(e.RowIndex).Cells(2).Value = ""
                For i = 0 To EditedArray.Length - 1
                    DataGridView1.Rows(e.RowIndex).Cells(2).Value = DataGridView1.Rows(e.RowIndex).Cells(2).Value & EditedArray(i) & "-"
                Next i
                DataGridView1.Rows(e.RowIndex).Cells(2).Value = DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString.Substring(0, DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString.Length - 1)
            End If

            'Check if all the values are integer
            For i = 0 To EditedArray.Length - 1
                If IsNumeric(EditedArray(i)) = False Then
                    AddListBoxItem("Not numeric Value at the Row " & (e.RowIndex + 1) & " and Column 3")
                    DataGridView1.Rows(e.RowIndex).Cells(2).Value = AscWords(DataGridView1.Rows(e.RowIndex).Cells(1).Value)
                    Exit Sub
                End If
            Next i

            'Check if all the values are < 256
            For i = 0 To EditedArray.Length - 1
                If Convert.ToInt32(EditedArray(i)) > 255 Then
                    AddListBoxItem("Invalid Value (0 - 255) at the Row " & (e.RowIndex + 1) & " and Column " & (e.ColumnIndex + 1))
                    DataGridView1.Rows(e.RowIndex).Cells(2).Value = AscWords(DataGridView1.Rows(e.RowIndex).Cells(1).Value)
                    Exit Sub
                End If
            Next i

            'Clear the cell
            DataGridView1.Rows(e.RowIndex).Cells(1).Value = ""
            'Insert the new values
            For i = 0 To EditedArray.Length - 1
                DataGridView1.Rows(e.RowIndex).Cells(1).Value += Chr(EditedArray(i))
            Next i
        ElseIf e.ColumnIndex = 1 Then
            If IsNothing(DataGridView1.Rows(e.RowIndex).Cells(1).Value) = True Then
                DataGridView1.Rows(e.RowIndex).Cells(2).Value = Nothing
                Exit Sub
            End If

            If DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString.Length > MaxLength(e.RowIndex) Then
                AddListBoxItem("Too long word (Max. " & MaxLength(e.RowIndex) & ") at the Row " & (e.RowIndex + 1) & " and Column " & (e.ColumnIndex + 1))
                DataGridView1.Rows(e.RowIndex).Cells(1).Value = DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString.Substring(0, MaxLength(e.RowIndex))
            End If

            Dim Bytes As String = AscWords(DataGridView1.Rows(e.RowIndex).Cells(1).Value)
            DataGridView1.Rows(e.RowIndex).Cells(2).Value = Bytes
        End If
    End Sub

    Private Sub AddListBoxItem(ByVal ItemToAdd As String)
        ListBox1.Items.Add(ItemToAdd)
        ListBox1.TopIndex = ListBox1.Items.Count - 1
    End Sub

    Private Sub SettingsForEncrypterEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsForEncrypterEditorToolStripMenuItem.Click
        Dim TextToWrite As String = "#################################################################" & Chr(13) & Chr(10)
        TextToWrite += "# " & Chr(13) & Chr(10)
        TextToWrite += "# Settings for Encrypter" & Chr(13) & Chr(10)
        TextToWrite += "# File generated with Game Files Open - Metin2 Global Tools" & Chr(13) & Chr(10)
        TextToWrite += "# " & Chr(13) & Chr(10)
        TextToWrite += "#################################################################" & Chr(13) & Chr(10) & Chr(13) & Chr(10)
        TextToWrite += "[SETTINGS]" & Chr(13) & Chr(10)
        TextToWrite += "MCOZ Key = """ & Chr(13) & Chr(10)

        Dim SaveDialog As New SaveFileDialog
        SaveDialog.DefaultExt = ".ini"
        SaveDialog.FileName = "Launcher"
        SaveDialog.Filter = "INI File (.ini)|*.ini|All Files|*.*"

        If SaveDialog.ShowDialog = DialogResult.OK Then
            Dim aFileStream As FileStream
            aFileStream = New FileStream(SaveDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write)
            Dim myBinaryWriter As New BinaryWriter(aFileStream)
            myBinaryWriter.Write(bytes)
            aFileStream.Close()
        End If
    End Sub

    Private Sub Launcher2010ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Launcher2010ToolStripMenuItem.Click
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\DataBases\Launcher\Downloads.xml") Then
            Dim down As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\DataBases\Launcher\Downloads.xml")

            Dim x As New XmlDocument()
            x.LoadXml(down)
            Dim infos As XmlNodeList = x.GetElementsByTagName("info")
            Dim destination As String = infos.Item(0).Attributes("dest").Value.ToString.Replace("..\", Application.StartupPath & "\")
            Dim url As String = infos.Item(0).Attributes("url").Value.ToString.Replace("../", "http://www.gamefilesopen.com/global_tools/")
            Dim fname As String = infos.Item(0).Attributes("name").Value.ToString

            AddListBoxItem("The download of the " & fname & " is started!")

            Control.CheckForIllegalCrossThreadCalls = False
            BackgroundWorker1.RunWorkerAsync({url, destination, fname, infos.Item(0).Attributes("dest").Value.ToString})

        Else
            AddListBoxItem("Cannot find the Downloads!")
            AddListBoxItem("Path: ..\DataBases\Launcher\Downloads.xml")
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            My.Computer.Network.DownloadFile(e.Argument(0), e.Argument(1))
            AddListBoxItem("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")
            AddListBoxItem("The download of the " & e.Argument(2) & " is finished!")
            AddListBoxItem("The file was downlaoded in " & e.Argument(3))
        Catch ex As Exception
            AddListBoxItem("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")
            AddListBoxItem(ex.ToString)
        End Try
    End Sub
End Class