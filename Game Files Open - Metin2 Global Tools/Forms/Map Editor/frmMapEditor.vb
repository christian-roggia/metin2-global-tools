Imports GFO_Map_Helper
Imports GFO_Regen_Helper
Imports GFO_Regen_Helper.LoadRegen
Imports GFO_Regen_Helper.Common
Imports GFO_Regen_Helper.EditRegen
Imports System.IO

Public Class frmMapEditor
    Public WithEvents Attribute As PictureBox = New PictureBox()
    Public WithEvents HeightCH1 As PictureBox = New PictureBox()
    Public WithEvents HeightCH2 As PictureBox = New PictureBox()
    Public WithEvents HeightTarga As PictureBox = New PictureBox()
    Public WithEvents ShadowCH1 As PictureBox = New PictureBox()
    Public WithEvents ShadowCH2 As PictureBox = New PictureBox()
    Public WithEvents Tile As PictureBox = New PictureBox()
    Public WithEvents Water As PictureBox = New PictureBox()

    Private IsChangingRadio As Boolean = False
    Private PType As String
    Private WithEvents LabelToolTip As New Label()
    Private HeightTargaExists As Boolean = False

    Public Shared GCPath As String = ""
    Private GCDirName As String = ""

    Public Shared CellScale As String = ""
    Public Shared HeightScale As String = ""
    Public Shared ViewRadius As String = ""
    Public Shared SMapSize As String = ""
    Public Shared BasePosition As String = ""
    Public Shared TextureSet As String = ""
    Public Shared Environment As String = ""

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LabelToolTip.ForeColor = Color.Blue
        LabelToolTip.BackColor = Color.Transparent
        Panel1.Size = New Size(Me.Size.Width - 250, Me.Size.Height - 101)
        TabControl1.Location = New Point(Panel1.Location.X + Panel1.Width + 9, Panel1.Location.Y)
        TabControl1.Size = New Size(210, Me.Size.Height - 100)
    End Sub

    Private Sub ClientMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            ClearAll()
            GCPath = FolderBrowserDialog1.SelectedPath
            GCDirName = GCPath.Remove(0, GCPath.LastIndexOf("\") + 1)

            Dim MySettingsArray() As String = LoadSettings.LoadSettings(GCPath)
            Dim MapSizeArray() As String = MySettingsArray(3).Replace(" ", Nothing).Split("x")

            If IsNothing(MapSizeArray(0)) = False And IsNumeric(MapSizeArray(0)) = True Then
                MapSize.X = Convert.ToInt32(MapSizeArray(0))
            End If

            MapSize.X = MySettingsArray(3).Replace(" ", Nothing).Split("x")(0)
            MapSize.Y = MySettingsArray(3).Replace(" ", Nothing).Split("x")(1)

            CellScale = MySettingsArray(0)
            HeightScale = MySettingsArray(1)
            ViewRadius = MySettingsArray(2)
            SMapSize = MySettingsArray(3)
            BasePosition = MySettingsArray(4)
            TextureSet = MySettingsArray(5)
            Environment = MySettingsArray(6)

            Panel1.Controls.Clear()
            Panel1.Controls.Add(Attribute)
        End If
    End Sub

    Private Sub LoadAllMaps()
        Attribute = LoadMap.LoadATRMap(GCPath)
        ShadowCH1 = LoadMap.LoadRawMap(GCPath, 2, 0, "shadowmap.raw", 256, 256)
        ShadowCH2 = LoadMap.LoadRawMap(GCPath, 2, 1, "shadowmap.raw", 256, 256)
        Tile = LoadMap.LoadRawMap(GCPath, 1, 0, "tile.raw", 258, 258)
        HeightCH1 = LoadMap.LoadHeightMap(GCPath, 0)
        HeightCH2 = LoadMap.LoadHeightMap(GCPath, 1)
        Water = LoadMap.LoadWTRMap(GCPath)

        If My.Computer.FileSystem.FileExists(GCPath + "\000000\height.tga") Then
            HeightTarga = LoadMap.LoadHeightTargaMap(GCPath)
            HeightTargaExists = True
        End If

        Attribute.Controls.Add(LabelToolTip)

        Dim PicturesName() As PictureBox = {Attribute, HeightCH1, HeightCH2, ShadowCH1, ShadowCH2, Tile, Water}

        For i = 0 To 6
            AddHandler PicturesName(i).MouseMove, AddressOf Panel1_MouseMove
            PicturesName(i).Controls.Add(LabelToolTip)
        Next

        AddHandler LabelToolTip.MouseMove, AddressOf Panel1_MouseMove
    End Sub

    Private Sub SaveImage(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveClientMaps.Click
        Dim PicturesName(6) As PictureBox
        Dim ImgNames(6) As String
        Dim PathName As String
        PicturesName = {Attribute, HeightCH1, HeightCH2, ShadowCH1, ShadowCH2, Tile, Water}
        ImgNames = {"Attribute", "Height CH1", "Height CH2", "Shadow CH1", "Shadow CH2", "Tile", "Water"}

        CheckImagesDir()

        For i = 0 To 6
            If Panel1.Controls.Contains(PicturesName(i)) Then
                PathName = Application.StartupPath + "\Maps Exported\" + GCDirName + "\Images\" + ImgNames(i) + ".png"
                PicturesName(i).Image.Save(PathName, Imaging.ImageFormat.Png)
            End If
        Next i
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem3.Click
        ClearAll()
    End Sub

    Private Sub ClearAll()
        GCPath = Nothing

        ClientClearImages()

        IsChangingRadio = True
        SetShowRadioButton(RadioButton1)
        IsChangingRadio = False

        PType = Nothing
    End Sub

    Private Sub ClientClearImages()
        Dim ImgToDispose() As PictureBox = {Attribute, HeightCH1, HeightCH2, ShadowCH1, ShadowCH2, Tile, Water, HeightTarga}
        For i = 0 To ImgToDispose.Length - 1
            If IsNothing(ImgToDispose(i)) = False Then
                ImgToDispose(i) = Nothing
            End If
        Next i

        HeightTargaExists = False

        LabelToolTip.Text = Nothing
        Panel1.Controls.Clear()
        RadioButton7.Enabled = False
        RadioButton8.Enabled = False
        RadioButton9.Enabled = False

        SaveClientMaps.Enabled = False
        ExportClientMaps.Enabled = False
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Application.Exit()
    End Sub

    Private Sub OurSiteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OurSiteToolStripMenuItem.Click
        Process.Start("http://gamefilesopen.com")
    End Sub


    Private Sub MapEditorWebSiteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MapEditorWebSiteToolStripMenuItem.Click
        Process.Start("http://map-editor.gamefilesopen.com")
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        frmSettings.ShowDialog()
    End Sub

    Private Sub frmMain_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Panel1.Size = New Size(Me.Size.Width - 250, Me.Size.Height - 101)
        TabControl1.Location = New Point(Panel1.Location.X + Panel1.Width + 9, Panel1.Location.Y)
        TabControl1.Size = New Size(210, Me.Size.Height - 100)
    End Sub

    Private Sub SetShowRadioButton(ByVal TrueRadio As RadioButton)
        If IsChangingRadio = True Then
            Dim SpecialRadioButton() As RadioButton = {RadioButton7, RadioButton8, RadioButton9}
            Dim ShowRadioButton() As RadioButton = {RadioButton1, RadioButton2, RadioButton3, RadioButton4,
                                                    RadioButton5, RadioButton6, RadioButton7, RadioButton8, RadioButton9}

            For i = 0 To ShowRadioButton.Length - 1
                ShowRadioButton(i).Checked = False
            Next i

            For s = 0 To SpecialRadioButton.Length - 1
                SpecialRadioButton(s).Enabled = False
            Next s

            TrueRadio.Checked = True
            IsChangingRadio = False
        End If
    End Sub

    Private Sub SetChannelRadioButton(ByVal TrueRadio As RadioButton)
        If IsChangingRadio = True Then
            Dim SpecialRadioButton() As RadioButton = {RadioButton7, RadioButton8, RadioButton9}
            For s = 0 To SpecialRadioButton.Length - 1
                SpecialRadioButton(s).Checked = False
            Next s
            TrueRadio.Checked = True
            IsChangingRadio = False
        End If
    End Sub

    Private Sub SetMultiChannelImage(ByVal GPType As String, ByVal SetImgTrue As Boolean)
        RadioButton7.Enabled = True
        RadioButton8.Enabled = True
        RadioButton9.Enabled = SetImgTrue
        PType = GPType
        RadioButton7.Checked = True
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton1)

            Panel1.Controls.Clear()
            Panel1.Controls.Add(Attribute)
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton2)

            Panel1.Controls.Clear()
            Panel1.Controls.Add(HeightCH1)
            SetMultiChannelImage("Height", HeightTargaExists)
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton3)
        End If
    End Sub

    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton4)

            Panel1.Controls.Clear()
            Panel1.Controls.Add(ShadowCH1)

            SetMultiChannelImage("Shadow", False)
        End If
    End Sub

    Private Sub RadioButton5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton5)

            Panel1.Controls.Clear()
            Panel1.Controls.Add(Tile)
        End If
    End Sub

    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton6)

            Panel1.Controls.Clear()
            Panel1.Controls.Add(Water)
        End If
    End Sub

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetChannelRadioButton(RadioButton7)

            If PType = "Height" Then
                Panel1.Controls.Clear()
                Panel1.Controls.Add(HeightCH1)
            ElseIf PType = "Shadow" Then
                Panel1.Controls.Clear()
                Panel1.Controls.Add(ShadowCH1)
            End If
        End If
    End Sub

    Private Sub RadioButton8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton8.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetChannelRadioButton(RadioButton8)

            If PType = "Height" Then
                Panel1.Controls.Clear()
                Panel1.Controls.Add(HeightCH2)
            ElseIf PType = "Shadow" Then
                Panel1.Controls.Clear()
                Panel1.Controls.Add(ShadowCH2)
            End If
        End If
    End Sub

    Private Sub RadioButton9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton9.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetChannelRadioButton(RadioButton9)

            If PType = "Height" Then
                Panel1.Controls.Clear()
                Panel1.Controls.Add(HeightTarga)
            ElseIf PType = "Shadow" Then
                MsgBox("PType = Shadow, Image")
            End If
        End If
    End Sub

    Private Sub frmMain_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        LabelToolTip.Visible = False
    End Sub

    Private Sub Panel1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If My.Settings.Coordinates = True Then
            LabelToolTip.Location = New Point(e.X + 2, e.Y + 2)
            LabelToolTip.Text = e.X & " " & e.Y
            LabelToolTip.Visible = True
        Else
            LabelToolTip.Visible = False
        End If
    End Sub

    Private Sub SaveImage(ByVal Picture As PictureBox, ByVal Type As String)
        Dim opnDialog As New OpenFileDialog
        Dim TempGDirName As String
        opnDialog.Filter = "File Image(*.png; *.jpg; *.jpeg)|*.png;*.jpg;*.jpeg|All Files|*.*"

        If opnDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            TempGDirName = opnDialog.FileName.Remove(0, opnDialog.FileName.LastIndexOf("\") + 1)
            TempGDirName = opnDialog.FileName.Replace("\" & TempGDirName, "")
            TempGDirName = TempGDirName.Remove(0, TempGDirName.LastIndexOf("\") + 1)

            Dim Img As Image = New Bitmap(opnDialog.FileName)
            Panel1.Controls.Clear()
            Picture.Size = Img.Size
            Picture.Image = New Bitmap(Img)
            Panel1.Controls.Add(Picture)
            Panel1.Refresh()
            Select Case Type
                Case "Water"
                    SaveMap.SaveMapWTR(Img, TempGDirName)
                Case "Height1"
                    SaveMap.SaveMapHeight(Img, 1, TempGDirName)
                Case "Height2"
                    SaveMap.SaveMapHeight(Img, 2, TempGDirName)
                Case "HeightTarga"
                    'SaveMap.SaveMapTarga(Img)
                Case "ShadowMap1"
                    SaveMap.SaveMapRAW(Img, 2, 1, 256, 256, "shadowmap.raw", TempGDirName)
                Case "ShadowMap2"
                    SaveMap.SaveMapRAW(Img, 2, 2, 256, 256, "shadowmap.raw", TempGDirName)
                Case "Tile"
                    SaveMap.SaveMapRAW(Img, 1, 1, 258, 258, "tile.raw", TempGDirName)
                Case "Attribute"
                    SaveMap.SaveMapATR(Img, TempGDirName)
            End Select
        End If
    End Sub

    Private Sub GenerateWTRToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem69.Click
        SaveImage(Water, "Water")
    End Sub

    Private Sub CH1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem61.Click
        SaveImage(HeightCH1, "Height1")
    End Sub

    Private Sub CH2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem62.Click
        SaveImage(HeightCH2, "Height2")
    End Sub

    Private Sub CH1ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem67.Click
        SaveImage(ShadowCH1, "Shadow1")
    End Sub

    Private Sub CH2ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem66.Click
        SaveImage(ShadowCH2, "Shadow2")
    End Sub

    Private Sub GenerateTileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem68.Click
        SaveImage(Tile, "Tile")
    End Sub

    Private Sub GenerateATRToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem58.Click
        SaveImage(Attribute, "Attribute")
    End Sub

    Private Sub ToolStripMenuItem55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem55.Click
        CheckDirExists("Maps Imported")

        Dim opnDialog As New OpenFileDialog
        Dim TempGDirName As String
        opnDialog.Filter = "Paths Settings(*.ini)|*.ini|All Files|*.*"

        If opnDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            TempGDirName = opnDialog.FileName.Remove(0, opnDialog.FileName.LastIndexOf("\") + 1)
            TempGDirName = opnDialog.FileName.Replace("\" & TempGDirName, "")
            TempGDirName = TempGDirName.Remove(0, TempGDirName.LastIndexOf("\") + 1)
            ImportAll(opnDialog.FileName, TempGDirName)
        End If
    End Sub

    Private Sub ImagesToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImagesToolStripMenuItem1.Click
        ClientClearImages()
    End Sub

    Private Sub AllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem.Click
        Dim PicturesName() As PictureBox = {Attribute, HeightCH1, HeightCH2, ShadowCH1, ShadowCH2, Tile, Water}
        Dim ImgNames() As String = {"Attribute", "Height CH1", "Height CH2", "Shadow CH1", "Shadow CH2", "Tile", "Water"}

        CheckImagesDir()

        SaveAll.SaveAllImages(PicturesName, ImgNames, "Maps Exported\" & GCDirName)
        SaveCSettings(False, GCDirName, False)
    End Sub

    Private Sub ExportAll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem2.Click
        Dim PicturesName() As PictureBox = {Attribute, HeightCH1, HeightCH2, ShadowCH1, ShadowCH2, Tile, Water}
        Dim ImgNames() As String = {"Attribute", "Height CH1", "Height CH2", "Shadow CH1", "Shadow CH2", "Tile", "Water"}
        Dim TextBoxes() As String = {CellScale, HeightScale, ViewRadius, SMapSize, BasePosition, TextureSet, Environment}
        Dim TextSettings() As String = {"CellScale", "HeightScale", Chr(13) & Chr(10) & "ViewRadius", Chr(13) & Chr(10) & "MapSize", "BasePosition", "TextureSet", "Environment"}
        Dim TextToWrite As String = "ScriptType" & Chr(9) & "MapSetting" & Chr(13) & Chr(10) & Chr(13) & Chr(10)

        CheckImagesDir()
        CheckSettingsDir()

        SaveAll.SaveAllImages(PicturesName, ImgNames, "Maps Exported\" & GCDirName)
        SaveAll.SaveCMapSettings(TextBoxes, TextSettings, "Maps Exported\" & GCDirName & "\Settings")
    End Sub

    Private Sub CheckImagesDir()
        CheckDirExists("Maps Exported")
        CheckDirExists("Maps Exported\" + GCDirName)
        CheckDirExists("Maps Exported\" + GCDirName + "\Images")
    End Sub

    Private Sub CheckSettingsDir()
        CheckDirExists("Maps Exported")
        CheckDirExists("Maps Exported\" + GCDirName)
        CheckDirExists("Maps Exported\" + GCDirName + "\Settings")
    End Sub

    Private Sub SettingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingToolStripMenuItem.Click
        Dim TextBoxes() As String = {CellScale, HeightScale, ViewRadius, SMapSize, BasePosition, TextureSet, Environment}
        Dim TextSettings() As String = {"CellScale", "HeightScale", Chr(13) & Chr(10) & "ViewRadius", Chr(13) & Chr(10) & "MapSize", "BasePosition", "TextureSet", "Environment"}
        CheckSettingsDir()
        SaveAll.SaveCMapSettings(TextBoxes, TextSettings, "Maps Exported\" & GCDirName & "\Settings")
    End Sub

    Private Sub AttributeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AttributeToolStripMenuItem.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({Attribute}, {"Attribute"}, "Maps Exported\" & GCDirName)
    End Sub

    Private Sub AllToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem5.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({HeightCH1, HeightCH2}, {"Height CH1", "Height CH2"}, "Maps Exported\" & GCDirName)
    End Sub

    Private Sub CH1ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CH1ToolStripMenuItem2.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({HeightCH1}, {"Height CH1"}, "Maps Exported\" & GCDirName)
    End Sub

    Private Sub CH2ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CH2ToolStripMenuItem2.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({HeightCH2}, {"Height CH2"}, "Maps Exported\" & GCDirName)
    End Sub

    Private Sub AllToolStripMenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem6.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({ShadowCH1, ShadowCH2}, {"Shadow CH1", "Shadow CH2"}, "Maps Exported\" & GCDirName)
    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({ShadowCH1}, {"Shadow CH1"}, "Maps Exported\" & GCDirName)
    End Sub

    Private Sub ToolStripMenuItem54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem54.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({ShadowCH2}, {"Shadow CH2"}, "Maps Exported\" & GCDirName)
    End Sub

    Private Sub TileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TileToolStripMenuItem.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({Tile}, {"Tile"}, "Maps Exported\" & GCDirName)
    End Sub

    Private Sub WaterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WaterToolStripMenuItem.Click
        CheckImagesDir()
        SaveAll.SaveAllImages({Water}, {"Water"}, "Maps Exported\" & GCDirName)
    End Sub
End Class
