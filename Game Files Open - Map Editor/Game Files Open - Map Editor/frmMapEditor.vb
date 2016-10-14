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
    Public WithEvents TempImg As PictureBox = New PictureBox()

    Private IsChangingRadio As Boolean = False
    Private PType As String
    Private WithEvents LabelToolTip As New Label()
    Private TempBitmap As New Bitmap(256, 256)
    Private HeightTargaExists As Boolean = False

    Private GCPath As String
    Private GSPath As String
    Private GCDirName As String

    Private RegenLoadID As Integer = -1
    Private RegenLoadX As Integer = -1
    Private RegenLoadY As Integer = -1
    Private RegenLoadArray() As QuadrantRegen
    Private RegenLoadColor As Color

    Private ClientRegenExists As Boolean = False
    Private ClientMAIExists As Boolean = False
    Private ServerRegenExists As Boolean = False
    Private ServerBossExists As Boolean = False
    Private ServerNPCExists As Boolean = False
    Private ServerStoneExists As Boolean = False

    Private ServerRegenArray() As QuadrantRegen
    Private ServerNPCArray() As QuadrantRegen
    Private ServerBossArray() As QuadrantRegen
    Private ServerStoneArray() As QuadrantRegen
    Private ClientRegenArray() As QuadrantRegen
    Private MAIArray() As QuadrantRegen

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
            TextGCPath.Text = FolderBrowserDialog1.SelectedPath
            GCPath = FolderBrowserDialog1.SelectedPath
            GCDirName = GCPath.Remove(0, GCPath.LastIndexOf("\") + 1)
            LoadSettings.LoadSettings(GCPath)
            Dim X As Integer = Convert.ToInt32(ToolStripLabel2.Text.Replace("Map Size:", "").Replace(" ", "").LastIndexOf("x"))
            MapSize.X = Convert.ToInt32(ToolStripLabel2.Text.Replace("Map Size:", "").Replace(" ", "").Substring(0, X))
            MapSize.Y = Convert.ToInt32(ToolStripLabel2.Text.Replace("Map Size:", "").Replace(" ", "").Replace(MapSize.X & "x", ""))
            LoadAllMaps()
            If My.Computer.FileSystem.FileExists(GCPath & "\regen.txt") Then
                ClientRegenExists = True
                TextCRegen.Enabled = True
                ShowCRegen.Enabled = True
                ComboFilesType.Text = "Regen (Client)"
                ClientRegenArray = LoadRegen.LoadArrayRegen(GCPath & "\regen.txt")
            End If
            If My.Computer.FileSystem.FileExists(GCPath & "\monsterareainfo.txt") Then
                ClientMAIExists = True
                TextCMAI.Enabled = True
                ShowMAI.Enabled = True
                ComboFilesType.Text = "MonsterAreaInfo"
                MAIArray = LoadRegen.LoadArrayRegen(GCPath & "\monsterareainfo.txt")
            End If

            SaveClientMaps.Enabled = True
            ExportClientMaps.Enabled = True

            Panel1.Controls.Clear()
            Panel1.Controls.Add(Attribute)
        End If
    End Sub


    Private Sub ServerMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            TextGSPath.Text = FolderBrowserDialog1.SelectedPath
            GSPath = FolderBrowserDialog1.SelectedPath
            If My.Computer.FileSystem.FileExists(GSPath & "\regen.txt") Then
                ServerRegenExists = True
                TextCRegen.Enabled = True
            End If
            If My.Computer.FileSystem.FileExists(GSPath & "\stone.txt") Then
                ServerStoneExists = True
                TextSStone.Enabled = True
            End If
            If My.Computer.FileSystem.FileExists(GSPath & "\boss.txt") Then
                ServerBossExists = True
                TextSBoss.Enabled = True
            End If
            If My.Computer.FileSystem.FileExists(GSPath & "\npc.txt") Then
                ServerNPCExists = True
                TextSNPC.Enabled = True
            End If
            RefreshCheckBox()
            ServerExportMaps.Enabled = True
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
        TempImg.Image = New Bitmap(Attribute.Image)
        TempBitmap = New Bitmap(TempImg.Image)

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
        AddHandler TempImg.MouseDown, AddressOf TempImage_MouseDown
        AddHandler TempImg.MouseMove, AddressOf TempImage_MouseMove
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

    Private Sub SetToolStripName(ByVal QuadrantSize As String, ByVal ShowDetails As String, Optional ByVal Channel As String = "No Channel")
        ToolStripLabel1.Text = "Status: Viewing"
        ToolStripLabel3.Text = "Quadrant Size: " + QuadrantSize
        ToolStripLabel4.Text = "Show: " + ShowDetails
        ToolStripLabel5.Text = "Channel: " + Channel
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Application.Exit()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem3.Click
        ClearAll()
    End Sub

    Private Sub ClearAll()
        GCPath = Nothing

        ClientClearImages()
        ClientClearObjects()
        ServerClear()

        IsChangingRadio = True
        SetShowRadioButton(RadioButton1)
        IsChangingRadio = False

        PType = Nothing
        Panel1.Controls.Add(PictureBackground)
    End Sub

    Private Sub ServerClear()
        GSPath = Nothing

        ServerRegenArray = Nothing
        ServerBossArray = Nothing
        ServerNPCArray = Nothing
        ServerStoneArray = Nothing

        ServerBossExists = False
        ServerNPCExists = False
        ServerRegenExists = False
        ServerStoneExists = False

        ServerExportMaps.Enabled = False

        RefreshCheckBox(True)
    End Sub

    Private Sub ClientClearObjects()
        ClientMAIExists = False
        ClientRegenExists = False

        ClientRegenArray = Nothing
        MAIArray = Nothing

        RefreshCheckBox(True)

        RemoveHandler TempImg.MouseMove, AddressOf TempImage_MouseMove
        RemoveHandler TempImg.MouseDown, AddressOf TempImage_MouseDown

        RegenLoadID = -1
    End Sub

    Private Sub ClientClearImages()
        Dim ImgToDispose() As PictureBox = {Attribute, HeightCH1, HeightCH2, ShadowCH1, ShadowCH2, Tile, Water, TempImg, HeightTarga}
        For i = 0 To ImgToDispose.Length - 1
            If IsNothing(ImgToDispose(i)) = False Then
                ImgToDispose(i) = Nothing
            End If
        Next i

        HeightTargaExists = False

        LabelToolTip.Text = Nothing
        SetToolStripName("0 x 0", "Nothing")
        ToolStripLabel1.Text = "Status: Map Closed"
        ToolStripLabel2.Text = "Map Size: 0 x 0"
        Panel1.Controls.Clear()
        RadioButton7.Enabled = False
        RadioButton8.Enabled = False
        RadioButton9.Enabled = False

        SaveClientMaps.Enabled = False
        ExportClientMaps.Enabled = False
    End Sub

    Private Sub RefreshCheckBox(Optional ByVal ClearCheck As Boolean = False)
        ShowBoss.Enabled = ServerBossExists
        ShowSRegen.Enabled = ServerRegenExists
        ShowCRegen.Enabled = ClientRegenExists
        ShowMAI.Enabled = ClientMAIExists
        ShowNPC.Enabled = ServerNPCExists
        ShowStone.Enabled = ServerStoneExists
        If ClearCheck = True Then
            ShowBoss.Checked = False
            ShowSRegen.Checked = False
            ShowCRegen.Checked = False
            ShowMAI.Checked = False
            ShowNPC.Checked = False
            ShowStone.Checked = False
        End If
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

    Private Sub frmMain_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Panel1.Size = New Size(Me.Size.Width - 250, Me.Size.Height - 101)
        TabControl1.Location = New Point(Panel1.Location.X + Panel1.Width + 9, Panel1.Location.Y)
        TabControl1.Size = New Size(210, Me.Size.Height - 100)
    End Sub

    Public Sub DisplayValue(ByVal CellScale As String, ByVal HeightScale As String, ByVal ViewRadius As String, ByVal SMapSize As String, ByVal BasePosition As String, ByVal TextureSet As String, ByVal Environment As String)
        TextBox1.Text = CellScale
        TextBox2.Text = HeightScale
        TextBox3.Text = ViewRadius
        TextBox4.Text = SMapSize
        TextBox5.Text = BasePosition
        TextBox6.Text = TextureSet
        TextBox7.Text = Environment
        ToolStripLabel2.Text = "Map Size: " + SMapSize
        ToolStripLabel1.Text = "Status: Map Loaded"
    End Sub

    Private Sub SetShowRadioButton(ByVal TrueRadio As RadioButton)
        If IsChangingRadio = True Then
            Dim SpecialRadioButton() As RadioButton = {RadioButton7, RadioButton8, RadioButton9}
            Dim ShowCheckBox() As CheckBox = {ShowSRegen, ShowCRegen, ShowMAI, ShowNPC, ShowStone}
            Dim ShowRadioButton() As RadioButton = {RadioButton1, RadioButton2, RadioButton3, RadioButton4,
                                                    RadioButton5, RadioButton6, RadioButton7, RadioButton8, RadioButton9}

            For i = 0 To ShowRadioButton.Length - 1
                ShowRadioButton(i).Checked = False
            Next i

            For s = 0 To SpecialRadioButton.Length - 1
                SpecialRadioButton(s).Enabled = False
            Next s

            For c = 0 To ShowCheckBox.Length - 1
                ShowCheckBox(c).Enabled = False
            Next c

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

            SetToolStripName("256 x 256", "Attribute")
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Attribute)

            RefreshCheckBox()
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton2)

            SetToolStripName("128 (+ 3) x 128 (+ 3)", "Height", "CH1")
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

            SetToolStripName("256 x 256", "Shadow Map", "CH1")
            Panel1.Controls.Clear()
            Panel1.Controls.Add(ShadowCH1)

            SetMultiChannelImage("Shadow", False)
        End If
    End Sub

    Private Sub RadioButton5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton5)

            SetToolStripName("258 x 258", "Tile")
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Tile)
        End If
    End Sub

    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetShowRadioButton(RadioButton6)

            SetToolStripName("128 x 128", "Water")
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Water)
        End If
    End Sub

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        If IsChangingRadio = False Then
            IsChangingRadio = True
            SetChannelRadioButton(RadioButton7)

            If PType = "Height" Then
                SetToolStripName("128 (+ 3) x 128 (+ 3)", "Height", "CH1")
                Panel1.Controls.Clear()
                Panel1.Controls.Add(HeightCH1)
            ElseIf PType = "Shadow" Then
                SetToolStripName("256 x 256", "Shadow Map", "CH1")
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
                SetToolStripName("128 (+ 3) x 128 (+ 3)", "Height", "CH2")
                Panel1.Controls.Clear()
                Panel1.Controls.Add(HeightCH2)
            ElseIf PType = "Shadow" Then
                SetToolStripName("256 x 256", "Shadow Map", "CH2")
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
                SetToolStripName("129 x 129", "Height", "TARGA")
                Panel1.Controls.Clear()
                Panel1.Controls.Add(HeightTarga)
            ElseIf PType = "Shadow" Then
                MsgBox("PType = Shadow, Image")
            End If
        End If
    End Sub


    Private Sub LoadRegenFiles(ByVal LName As String, ByVal LColor As Color, ByRef QuadrantsArray() As QuadrantRegen, ByVal LCheck As CheckBox, ByVal LPath As String)
        TempImg.Size = New Size(MapSize.X * 256, MapSize.Y * 256)
        If LCheck.Checked = True And RadioButton1.Checked = True Then
            If IsNothing(QuadrantsArray) = True Then
                ToolStripLabel4.Text = SetShowName()
                QuadrantsArray = LoadRegen.LoadArrayRegen(LPath & LName)
                If IsNothing(QuadrantsArray) = False Then
                    TempImg.Image = EditRegen.ConvertArrayToImage(QuadrantsArray, TempImg.Image, LColor)
                End If
                Panel1.Controls.Clear()
                Panel1.Controls.Add(TempImg)
            Else
                TempImg.Image = EditRegen.ConvertArrayToImage(QuadrantsArray, TempImg.Image, LColor)
                ToolStripLabel4.Text = SetShowName()
                Panel1.Controls.Clear()
                Panel1.Controls.Add(TempImg)
            End If
        Else
            TempImg.Image = EditRegen.DeleteArrayFromImage(QuadrantsArray, Attribute.Image, TempImg.Image)
            ToolStripLabel4.Text = SetShowName()
            Panel1.Controls.Clear()
            Panel1.Controls.Add(TempImg)
        End If
        TempBitmap = TempImg.Image
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowSRegen.CheckedChanged
        LoadRegenFiles("\regen.txt", Color.Red, ServerRegenArray, ShowSRegen, GSPath)
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCRegen.CheckedChanged
        LoadRegenFiles("\regen.txt", Color.FromArgb(255, 128, 0), ClientRegenArray, ShowCRegen, GCPath)
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowMAI.CheckedChanged
        LoadRegenFiles("\monsterareainfo.txt", Color.Cyan, MAIArray, ShowMAI, GCPath)
    End Sub

    Private Sub ShowNPC_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowNPC.CheckedChanged
        LoadRegenFiles("\npc.txt", Color.Lime, ServerNPCArray, ShowNPC, GSPath)
    End Sub

    Private Sub ShowStone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowStone.CheckedChanged
        LoadRegenFiles("\stone.txt", Color.Silver, ServerStoneArray, ShowStone, GSPath)
    End Sub

    Private Sub ShowBoss_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowBoss.CheckedChanged
        LoadRegenFiles("\boss.txt", Color.DodgerBlue, ServerBossArray, ShowBoss, GSPath)
    End Sub

    Private Sub frmMain_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        LabelToolTip.Visible = False
    End Sub

    Private Sub CheckBox2_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCoordintes.CheckedChanged
        If ShowCoordintes.Checked = False Then
            LabelToolTip.Visible = False
        End If
    End Sub

    Private Sub CursorCross_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CursorCross.CheckedChanged
        If CursorCross.Checked = True Then
            Panel1.Cursor = Cursors.Cross
        Else
            Panel1.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub Panel1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove
        If ShowCoordintes.Checked = True Then
            LabelToolTip.Location = New Point(e.X + 2, e.Y + 2)
            LabelToolTip.Text = e.X & " " & e.Y
            LabelToolTip.Visible = True
        End If
    End Sub

    Private Function SwitchArray()
        Select Case ComboFilesType.Text
            Case "Regen (Server)"
                Return ServerRegenArray
                Exit Select
            Case "Regen (Client)"
                Return ClientRegenArray
                Exit Select
            Case "MonsterAreaInfo"
                Return MAIArray
                Exit Select
            Case "Boss"
                Return ServerBossArray
                Exit Select
            Case "NPC"
                Return ServerNPCArray
                Exit Select
            Case "Stone"
                Return ServerStoneArray
                Exit Select
            Case Else
                Return ClientRegenArray
                Exit Select
        End Select
    End Function

    Private Sub TempImage_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseDown
        If IsNothing(TempImg.Image) = False Then
            TempBitmap = TempImg.Image
            If TempBitmap.Width > e.X And TempBitmap.Height > e.Y Then
                Dim LocalColor As Color = Common.SwitchColor(ComboFilesType.Text)
                If Not TempBitmap.GetPixel(e.X, e.Y) = LocalColor Then
                    Dim LocalArray() As QuadrantRegen = SwitchArray()
                    If IsNothing(LocalArray) = False Then
                        Dim Localization() As Integer = LocalizeRegen(e.X, e.Y)
                        Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                        'Dim TextRegen() As TextBox = {TextSX, TextSY, TextZ, TextDir, TextTime, TextPercent, TextCount, TextVnum}
                        Dim TextRegen() As TextBox = {TextSX, TextSY, TextZ, TextDir, TextPercent, TextCount, TextVnum}

                        If IsNothing(LocalArray(RegenLocalizate)) Then
                            LocalArray(RegenLocalizate) = New QuadrantRegen
                        End If

                        Dim TempArrayLength As Integer = 0

                        If IsNothing(LocalArray(RegenLocalizate).Regen) Then
                            ReDim Preserve LocalArray(RegenLocalizate).Regen(0)
                        Else
                            TempArrayLength = LocalArray(RegenLocalizate).Regen.Length
                            ReDim Preserve LocalArray(RegenLocalizate).Regen(TempArrayLength)
                        End If

                        For i = 0 To TextRegen.Length - 1
                            If TextRegen(i).Text = Nothing Then
                                TextRegen(i).Text = "0"
                            End If
                        Next

                        LocalArray(RegenLocalizate).Regen(TempArrayLength).Type = Common.SwitchType(ComboType.Text)
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).X = e.X
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).Y = e.Y
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).SX = Convert.ToInt32(TextSX.Text)
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).SY = Convert.ToInt32(TextSY.Text)
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).Z = Convert.ToInt32(TextZ.Text)
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).Dir = Convert.ToInt32(TextDir.Text)
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).Time = TextTime.Text
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).Percent = Convert.ToInt32(TextPercent.Text)
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).Count = Convert.ToInt32(TextCount.Text)
                        LocalArray(RegenLocalizate).Regen(TempArrayLength).Vnum = Convert.ToInt32(TextVnum.Text)
                        LocalArray(RegenLocalizate).ObjectsNumber += 1
                        TempBitmap.SetPixel(e.X, e.Y, LocalColor)
                        TempImg.Image = TempBitmap
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub TempImage_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TempImg.MouseMove
        Dim Localization() As Integer = LocalizeRegen(e.X, e.Y)
        Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
        If TempBitmap.Width > e.X And TempBitmap.Height > e.Y Then
            Dim ColorToArgb As Integer = TempBitmap.GetPixel(e.X, e.Y).ToArgb
            Select Case ColorToArgb
                Case Color.Red.ToArgb
                    ComboFilesType.Text = "Regen (Server)"
                    RegenSearchID(e, ServerRegenArray, Color.Red)
                    Exit Select
                Case Color.FromArgb(255, 128, 0).ToArgb
                    ComboFilesType.Text = "Regen (Client)"
                    RegenSearchID(e, ClientRegenArray, Color.FromArgb(255, 128, 0))
                    Exit Select
                Case Color.Cyan.ToArgb
                    ComboFilesType.Text = "MonsterAreaInfo"
                    RegenSearchID(e, MAIArray, Color.Cyan)
                    Exit Select
                Case Color.Lime.ToArgb
                    ComboFilesType.Text = "NPC"
                    RegenSearchID(e, ServerNPCArray, Color.Lime)
                    Exit Select
                Case Color.Silver.ToArgb
                    ComboFilesType.Text = "Stone"
                    RegenSearchID(e, ServerStoneArray, Color.Silver)
                    Exit Select
                Case Color.DodgerBlue.ToArgb
                    ComboFilesType.Text = "Boss"
                    RegenSearchID(e, ServerBossArray, Color.DodgerBlue)
                    Exit Select
            End Select
        End If
    End Sub

    Private Sub RegenSearchID(ByVal e As System.Windows.Forms.MouseEventArgs, ByVal QuadrantsArray() As QuadrantRegen, ByVal TColor As Color)
        Dim Localization() As Integer = LocalizeRegen(e.X, e.Y)
        Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
        For i = 0 To QuadrantsArray(RegenLocalizate).Regen.Length - 1
            If QuadrantsArray(RegenLocalizate).Regen(i).X = e.X And QuadrantsArray(RegenLocalizate).Regen(i).Y = e.Y Then
                Select Case QuadrantsArray(RegenLocalizate).Regen(i).Type
                    Case "r"
                        ComboType.Text = "Regen"
                        Exit Select
                    Case "m"
                        ComboType.Text = "Mob"
                        Exit Select
                    Case "g"
                        ComboType.Text = "Group"
                        Exit Select
                End Select
                'Dim TextRegen() As TextBox = {TextX, TextY, TextSX, TextSY, TextZ, TextDir, TextTime, TextPercent, TextCount, TextVnum}
                TextX.Text = QuadrantsArray(RegenLocalizate).Regen(i).X
                TextY.Text = QuadrantsArray(RegenLocalizate).Regen(i).Y
                TextSX.Text = QuadrantsArray(RegenLocalizate).Regen(i).SX
                TextSY.Text = QuadrantsArray(RegenLocalizate).Regen(i).SY
                TextZ.Text = QuadrantsArray(RegenLocalizate).Regen(i).Z
                TextDir.Text = QuadrantsArray(RegenLocalizate).Regen(i).Dir
                TextTime.Text = QuadrantsArray(RegenLocalizate).Regen(i).Time
                TextPercent.Text = QuadrantsArray(RegenLocalizate).Regen(i).Percent
                TextCount.Text = QuadrantsArray(RegenLocalizate).Regen(i).Count
                TextVnum.Text = QuadrantsArray(RegenLocalizate).Regen(i).Vnum
                RegenLoadID = i
                RegenLoadX = QuadrantsArray(RegenLocalizate).Regen(i).X
                RegenLoadY = QuadrantsArray(RegenLocalizate).Regen(i).Y
                RegenLoadArray = QuadrantsArray
                RegenLoadColor = TColor
                TabControl1.SelectedTab = TabPage3
                Exit Sub
            End If
        Next i
    End Sub

    Private Sub TextX_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextX.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                Dim OldLocalization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                Dim OldRegenLocalizate As Integer = (OldLocalization(0) * MapSize.X) + OldLocalization(1)

                If TextX.Text = "" Then
                    RegenLoadArray(OldRegenLocalizate).Regen = Common.RegenDeleteItem(RegenLoadX, RegenLoadY, RegenLoadArray(OldRegenLocalizate).Regen)
                    TempImg.Image = EditRegen.DeleteArrayFromImage(Nothing, Attribute.Image, TempImg.Image, {RegenLoadX, RegenLoadY})
                    Dim TextRegen() As TextBox = {TextX, TextY, TextSX, TextSY, TextZ, TextDir, TextTime, TextPercent, TextCount, TextVnum}
                    For c = 0 To TextRegen.Length - 1
                        TextRegen(c).Text = ""
                    Next c
                    Exit Sub
                End If

                Dim X As Integer = Convert.ToInt32(TextY.Text)
                Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)

                If X < TempImg.Image.Width Then
                    If RegenLoadID < RegenLoadArray(RegenLocalizate).Regen.Length - 1 Then
                        If IsNothing(TempImg.Image) = False Then
                            TempBitmap = TempImg.Image.Clone

                            If Not OldRegenLocalizate = RegenLocalizate Then
                                Dim RegenIndex As Integer = GetRegenIndexFromXY(RegenLoadArray(OldRegenLocalizate).Regen, RegenLoadX, RegenLoadY)
                                RegenLoadArray(RegenLocalizate).Regen = AddItemToArray(RegenLoadArray(RegenLocalizate).Regen, RegenLoadArray(OldRegenLocalizate).Regen(RegenIndex))
                                RegenLoadArray(OldRegenLocalizate).Regen = DeleteItemFromRegenArray(RegenLoadArray(OldRegenLocalizate).Regen, RegenIndex)
                                RegenLoadArray(RegenLocalizate).Regen(RegenLoadArray(RegenLocalizate).Regen.Length - 1).X = X
                            Else
                                RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).X = X
                            End If

                            Dim Coordinates() As Integer = {RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).X,
                                                            RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Y}
                            TempBitmap = EditRegen.DeleteArrayFromImage(Nothing, Attribute.Image, TempImg.Image, Coordinates)
                            RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).X = X
                            TempBitmap.SetPixel(X, RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Y, RegenLoadColor)
                            TempImg.Image = TempBitmap
                            TempImg.Refresh()
                        End If
                    End If
                End If
            End If
        End If
    End Sub


    Private Sub TextY_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextY.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                Dim OldLocalization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                Dim OldRegenLocalizate As Integer = (OldLocalization(0) * MapSize.X) + OldLocalization(1)

                If TextY.Text = "" Then
                    RegenLoadArray(OldRegenLocalizate).Regen = Common.RegenDeleteItem(RegenLoadX, RegenLoadY, RegenLoadArray(OldRegenLocalizate).Regen)
                    If IsNothing(RegenLoadArray(OldRegenLocalizate).Regen) = False Then
                        If Not GetRegenIndexFromXY(RegenLoadArray(OldRegenLocalizate).Regen, RegenLoadX, RegenLoadY) = -1 Then
                            RegenLoadArray(OldRegenLocalizate).Regen = Common.CheckDuplicateRegen(RegenLoadArray(OldRegenLocalizate).Regen, RegenLoadX, RegenLoadY)
                            If GetRegenIndexFromXY(RegenLoadArray(OldRegenLocalizate).Regen, RegenLoadX, RegenLoadY) = -1 Then
                                TempImg.Image = EditRegen.DeleteArrayFromImage(Nothing, Attribute.Image, TempImg.Image, {RegenLoadX, RegenLoadY})
                            End If
                        Else
                            TempImg.Image = EditRegen.DeleteArrayFromImage(Nothing, Attribute.Image, TempImg.Image, {RegenLoadX, RegenLoadY})
                        End If
                    Else
                        TempImg.Image = EditRegen.DeleteArrayFromImage(Nothing, Attribute.Image, TempImg.Image, {RegenLoadX, RegenLoadY})
                    End If

                    Dim TextRegen() As TextBox = {TextX, TextY, TextSX, TextSY, TextZ, TextDir, TextTime, TextPercent, TextCount, TextVnum}
                    For c = 0 To TextRegen.Length - 1
                        TextRegen(c).Text = ""
                    Next c
                    Exit Sub
                End If

                Dim Y As Integer = Convert.ToInt32(TextY.Text)
                Dim Localization() As Integer = LocalizeRegen(RegenLoadX, Y)
                Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)

                If Y < TempImg.Image.Width Then
                    If RegenLoadID < RegenLoadArray(RegenLocalizate).Regen.Length - 1 Then
                        If IsNothing(TempImg.Image) = False Then
                            TempBitmap = TempImg.Image.Clone

                            If Not OldRegenLocalizate = RegenLocalizate Then
                                Dim RegenIndex As Integer = GetRegenIndexFromXY(RegenLoadArray(OldRegenLocalizate).Regen, RegenLoadX, RegenLoadY)
                                RegenLoadArray(RegenLocalizate).Regen = AddItemToArray(RegenLoadArray(RegenLocalizate).Regen, RegenLoadArray(OldRegenLocalizate).Regen(RegenIndex))
                                RegenLoadArray(OldRegenLocalizate).Regen = DeleteItemFromRegenArray(RegenLoadArray(OldRegenLocalizate).Regen, RegenIndex)
                                RegenLoadArray(RegenLocalizate).Regen(RegenLoadArray(RegenLocalizate).Regen.Length - 1).Y = Y
                            Else
                                RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Y = Y
                            End If

                            Dim Coordinates() As Integer = {RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).X,
                                                            RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Y}
                            TempBitmap = EditRegen.DeleteArrayFromImage(Nothing, Attribute.Image, TempImg.Image, Coordinates)
                            TempBitmap.SetPixel(RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).X, Y, RegenLoadColor)
                            TempImg.Image = TempBitmap
                            TempImg.Refresh()
                        End If
                    End If
                End If
            End If
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

    Private Sub ObjectsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ObjectsToolStripMenuItem.Click
        ClientClearObjects()
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

    Private Sub ObjectsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ObjectsToolStripMenuItem1.Click
        CheckSettingsDir()
        If ClientRegenExists = True Then
            SaveAll.SaveCMapRegen(ClientRegenArray, "Maps Exported\" & GCDirName & "\Settings", "Regen")
        ElseIf ClientMAIExists = True Then
            SaveAll.SaveCMapRegen(MAIArray, "Maps Exported\" & GCDirName & "\Settings", "Monster Area Info")
        End If
    End Sub


    Private Sub ExportAll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem2.Click
        Dim PicturesName() As PictureBox = {Attribute, HeightCH1, HeightCH2, ShadowCH1, ShadowCH2, Tile, Water}
        Dim ImgNames() As String = {"Attribute", "Height CH1", "Height CH2", "Shadow CH1", "Shadow CH2", "Tile", "Water"}
        Dim TextBoxes() As TextBox = {TextBox1, TextBox2, TextBox3, TextBox4, TextBox5, TextBox6, TextBox7}
        Dim TextSettings() As String = {"CellScale", "HeightScale", Chr(13) & Chr(10) & "ViewRadius", Chr(13) & Chr(10) & "MapSize", "BasePosition", "TextureSet", "Environment"}
        Dim TextToWrite As String = "ScriptType" & Chr(9) & "MapSetting" & Chr(13) & Chr(10) & Chr(13) & Chr(10)

        CheckImagesDir()
        CheckSettingsDir()

        SaveAll.SaveAllImages(PicturesName, ImgNames, "Maps Exported\" & GCDirName)
        SaveAll.SaveCMapSettings(TextBoxes, TextSettings, "Maps Exported\" & GCDirName & "\Settings")
        If ClientRegenExists = True Then
            SaveCSettings(True, GCDirName, True)
            SaveAll.SaveCMapRegen(ClientRegenArray, "Maps Exported\" & GCDirName & "\Settings", "Regen")
        ElseIf ClientMAIExists = True Then
            SaveCSettings(False, GCDirName, True)
            SaveAll.SaveCMapRegen(MAIArray, "Maps Exported\" & GCDirName & "\Settings", "Monster Area Info")
        End If
    End Sub

    Private Sub CheckImagesDir()
        CheckDirExists("Maps Exported")
        CheckDirExists("Maps Exported\" + GCDirName)
        CheckDirExists("Maps Exported\" + GCDirName + "\Images")
    End Sub

    Private Sub CheckServerDir()
        CheckDirExists("Maps Exported")
        CheckDirExists("Maps Exported\" + GCDirName)
        CheckDirExists("Maps Exported\" + GCDirName + "\Server")
    End Sub

    Private Sub CheckSettingsDir()
        CheckDirExists("Maps Exported")
        CheckDirExists("Maps Exported\" + GCDirName)
        CheckDirExists("Maps Exported\" + GCDirName + "\Settings")
    End Sub

    Private Sub SettingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingToolStripMenuItem.Click
        Dim TextBoxes() As TextBox = {TextBox1, TextBox2, TextBox3, TextBox4, TextBox5, TextBox6, TextBox7}
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

    Private Sub ToolStripMenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem14.Click
        CheckServerDir()
        SaveSMapRegen(ServerRegenArray, "Maps Exported\" & GCDirName & "\Server", "Regen")
        SaveSMapRegen(ServerNPCArray, "Maps Exported\" & GCDirName & "\Server", "NPC")
        SaveSMapRegen(ServerStoneArray, "Maps Exported\" & GCDirName & "\Server", "Stone")
        SaveSMapRegen(ServerBossArray, "Maps Exported\" & GCDirName & "\Server", "Boss")
    End Sub

    Private Sub RegenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegenToolStripMenuItem.Click
        CheckServerDir()
        SaveSMapRegen(ServerRegenArray, "Maps Exported\" & GCDirName & "\Server", "Regen")
    End Sub

    Private Sub NPCToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NPCToolStripMenuItem.Click
        CheckServerDir()
        SaveSMapRegen(ServerNPCArray, "Maps Exported\" & GCDirName & "\Server", "NPC")
    End Sub

    Private Sub StoneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StoneToolStripMenuItem.Click
        CheckServerDir()
        SaveSMapRegen(ServerStoneArray, "Maps Exported\" & GCDirName & "\Server", "Stone")
    End Sub

    Private Sub BossToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BossToolStripMenuItem.Click
        CheckServerDir()
        SaveSMapRegen(ServerBossArray, "Maps Exported\" & GCDirName & "\Server", "Boss")
    End Sub

    Private Sub ToolStripMenuItem97_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem97.Click
        ServerClear()
    End Sub
End Class
