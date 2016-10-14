Imports System.Xml
Imports System.Net

Public Class frmSharing
    Structure Downloads
        Public ID As Integer
        Public Name As String
        Public Language As String
        Public Developers As String
        Public Downloads As String
        Public Vote As String
        Public Sharing As String
        Public Price As Integer
        Public WebSite As String
        Public Team As String
        Public ServerName As String
        Public Contacts As String
        Public OtherInfo As String
        Public Documentation As String
        Public ScreenShotsLinks As String
        Public DownID As Integer
        Public License As String
        Public UploadDate As String
        Public Uploader As String
        Public Version As Integer
        Public LastUpdate As String
    End Structure
    Shared DownloadsArray(0) As Downloads
    Private BW1Argument As String

    Private DownloadedID(0) As Integer
    Public UserName As String
    Public Password As String
    Public DownID As Integer = -1

    Private Sub frmSharing_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UserNameText.Text = "Welcome back " & UserName
        Control.CheckForIllegalCrossThreadCalls = False
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim client As New WebClient()
        Dim x As New XmlDocument()
        Try
            Dim MyXML As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\DataBases\Sharing\" & e.Argument & ".xml")
            'x.LoadXml(client.DownloadString("http://www.gamefilesopen.com/global_tools/global.php?type=" & e.Argument & "&page=" & NumericUpDown1.Value))
            x.LoadXml(MyXML)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
        Dim files As XmlNodeList = x.GetElementsByTagName("file")
        DataGridView1.Rows.Clear()

        Dim xValue() As String = {"id", "name", "language", "developers", "downloads", "vote", "sharing", "price",
                          "website", "team", "servername", "contacts", "otherinfo", "documentation",
                          "screenshotslinks", "downid", "license", "uploaddate", "uploader", "version", "lastupdate"}
        Dim i As Integer = 0
        ReDim DownloadsArray(files.Count - 1)

        For Each f As XmlNode In files
            DownloadsArray(i).ID = Convert.ToInt32(f.Attributes(xValue(0)).Value)
            DownloadsArray(i).Name = f.Attributes(xValue(1)).Value
            DownloadsArray(i).Language = f.Attributes(xValue(2)).Value
            DownloadsArray(i).Developers = f.Attributes(xValue(3)).Value
            DownloadsArray(i).Downloads = Convert.ToInt32(f.Attributes(xValue(4)).Value)
            DownloadsArray(i).Vote = Convert.ToInt32(f.Attributes(xValue(5)).Value)
            DownloadsArray(i).Sharing = f.Attributes(xValue(6)).Value
            DownloadsArray(i).Price = Convert.ToInt32(f.Attributes(xValue(7)).Value)
            DownloadsArray(i).WebSite = f.Attributes(xValue(8)).Value
            DownloadsArray(i).Team = f.Attributes(xValue(9)).Value
            DownloadsArray(i).ServerName = f.Attributes(xValue(10)).Value
            DownloadsArray(i).Contacts = f.Attributes(xValue(11)).Value
            DownloadsArray(i).OtherInfo = f.Attributes(xValue(12)).Value
            DownloadsArray(i).Documentation = f.Attributes(xValue(13)).Value
            DownloadsArray(i).ScreenShotsLinks = f.Attributes(xValue(14)).Value
            DownloadsArray(i).DownID = f.Attributes(xValue(15)).Value
            DownloadsArray(i).License = f.Attributes(xValue(16)).Value
            DownloadsArray(i).UploadDate = f.Attributes(xValue(17)).Value
            DownloadsArray(i).Uploader = f.Attributes(xValue(18)).Value
            DownloadsArray(i).Version = Convert.ToInt32(f.Attributes(xValue(19)).Value)
            DownloadsArray(i).LastUpdate = f.Attributes(xValue(20)).Value
            i += 1
            DataGridView1.Rows.Add(f.Attributes("name").Value, f.Attributes("language").Value, f.Attributes("developers").Value)
        Next f
    End Sub

    Private Sub DataGridView1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged
        Dim idx As Integer = DataGridView1.SelectedRows(0).Index
        With DownloadsArray(idx)
            DownID = .DownID
            TextBox3.Text = .Language
            TextBox2.Text = .Name
            TextBox8.Text = .Sharing
            TextBox1.Text = .Price
            TextBox4.Text = .WebSite
            TextBox11.Text = .Uploader
            TextBox14.Text = .UploadDate
            TextBox16.Text = .LastUpdate
            TextBox5.Text = .Team
            TextBox6.Text = .ServerName
            TextBox9.Text = .Contacts.Replace("%n", vbCrLf)
            TextBox7.Text = .OtherInfo.Replace("%n", vbCrLf)
            RichTextBox1.Text = .Documentation.Replace("%n", vbCrLf)
            RichTextBox2.Text = .License.Replace("%n", vbCrLf)
            TextBox12.Text = Nothing
            TextBox13.Text = Nothing
            TextBox15.Text = Nothing
            CheckBox1.Checked = False

            Dim ScreenArray() As String = .ScreenShotsLinks.Split("%n")
            For i = 0 To ScreenArray.Length - 1
                ListBox1.Text = ScreenArray(i)
            Next i
        End With
    End Sub

    Private Sub WaitForBW1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WaitForBW1.Tick
        If BackgroundWorker1.IsBusy = False And frmWait.Visible = False Then
            BackgroundWorker1.RunWorkerAsync(BW1Argument)
            WaitForBW1.Stop()
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Try
            Dim request As Net.HttpWebRequest = DirectCast(Net.HttpWebRequest.Create(ListBox1.SelectedItems.Item(0).ToString), Net.HttpWebRequest)
            Dim response As Net.HttpWebResponse = DirectCast(request.GetResponse, Net.HttpWebResponse)
            Dim img As Image = Image.FromStream(response.GetResponseStream())
            response.Close()
            PictureBox1.Size = New Size(img.Width, img.Height)
            PictureBox1.Image = img
        Catch ex As Exception
            PictureBox1.Image = PictureBox1.ErrorImage
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ReDim Preserve DownloadedID(DownloadedID.Length)
        DownloadedID(DownloadedID.Length - 1) = DownID

        Dim client As New WebClient()
        Dim x As New XmlDocument()

        Try
            x.LoadXml(client.DownloadString("http://www.gamefilesopen.com/global_tools/download.php?ID=" & UserName & "&PSW=" & Password & "&DOWNID=" & DownID))
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try

        Dim files As XmlNodeList = x.GetElementsByTagName("file")

        Dim xValue() As String = {"direct", "mirror", "torrent"}
        TextBox12.Text = files(0).Attributes(xValue(0)).Value
        TextBox13.Text = files(0).Attributes(xValue(1)).Value
        TextBox15.Text = files(0).Attributes(xValue(2)).Value
    End Sub

    Private Sub TextBox10_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(13) Then
            BW1Argument = ComboBox1.Text
            WaitForBW1.Start()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        BW1Argument = ComboBox1.Text
        WaitForBW1.Start()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        BW1Argument = ComboBox1.Text
        WaitForBW1.Start()
    End Sub
End Class