Imports GFO_Map_Helper
Imports Paloma
Imports GFO_Regen_Helper.LoadRegen
Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D

Module LoadMap
    Public Function LoadRawMap(ByVal path As String, ByVal NumberOfChannel As Integer, ByVal ChannelToRead As Integer, ByVal FilesName As String, ByVal SizeX As Integer, ByVal SizeY As Integer)
        Dim picRaw As PictureBox = New PictureBox()
        If Not MapSize.X = Nothing And Not MapSize.Y = Nothing Then
            frmMapEditor.Panel1.Controls.Clear()
            ' Interface
            frmMapEditor.ToolStripProgressBar1.Value = 0
            frmMapEditor.ToolStripProgressBar1.Maximum = MapSize.Y
            Dim bm As New Bitmap(SizeX * MapSize.X, SizeY * MapSize.Y)
            Dim gr As Graphics = Graphics.FromImage(bm)
            Dim Image As Image = New Bitmap(SizeX * MapSize.X, SizeY * MapSize.Y)
            For y = 0 To MapSize.Y - 1
                For x = 0 To MapSize.X - 1
                    Dim QuadrantName As String = path & "\00" & x & "00" & y & "\" & FilesName
                    If My.Computer.FileSystem.FileExists(QuadrantName) Then
                        Image = LoadImage.LoadRAW(QuadrantName, NumberOfChannel, ChannelToRead, SizeX, SizeY)
                    Else
                        MessageBox.Show("Quadrant not found: " & QuadrantName)
                    End If
                    gr.DrawImage(Image, x * SizeX, y * SizeY)
                    Image = bm
                Next x
                frmMapEditor.ToolStripProgressBar1.Value += 1
                frmMapEditor.ToolStrip1.Refresh()
            Next y

            picRaw.Size = New Size(SizeX * MapSize.X, SizeY * MapSize.Y)
            picRaw.Image = Image
            Return picRaw
        End If

        Return picRaw
    End Function

    Public Function LoadHeightMap(ByVal path As String, ByVal ChannelToRead As Integer)
        Dim Height As PictureBox = New PictureBox()
        If Not MapSize.Y = Nothing And Not MapSize.X = Nothing Then
            frmMapEditor.Panel1.Controls.Clear()
            ' Interface
            frmMapEditor.ToolStripProgressBar1.Value = 0
            frmMapEditor.ToolStripProgressBar1.Maximum = MapSize.Y
            Dim bm As New Bitmap(128 * MapSize.X + 3, 128 * MapSize.Y + 3)
            Dim gr As Graphics = Graphics.FromImage(bm)
            Dim Image As Image = New Bitmap(128 * MapSize.X + 3, 128 * MapSize.Y + 3)
            For y = 0 To MapSize.Y - 1
                For x = 0 To MapSize.X - 1
                    Dim QuadrantName As String = path & "\00" & x & "00" & y & "\height.raw"
                    If My.Computer.FileSystem.FileExists(QuadrantName) Then
                        Image = LoadImage.LoadRAW(QuadrantName, 2, ChannelToRead, 131, 131)
                    Else
                        MessageBox.Show("Quadrant not found: " & QuadrantName)
                    End If
                    gr.DrawImage(Image, x * 128, y * 128)
                    Image = bm
                Next x
                frmMapEditor.ToolStripProgressBar1.Value += 1
                frmMapEditor.ToolStrip1.Refresh()
            Next y

            Height.Size = New Size(128 * MapSize.X + 3, 128 * MapSize.Y + 3)
            Height.Image = Image
        End If
        Return Height
    End Function

    Public Function LoadWTRMap(ByVal path As String)
        Dim Water As PictureBox = New PictureBox()
        If Not MapSize.Y = Nothing And Not MapSize.X = Nothing Then
            frmMapEditor.Panel1.Controls.Clear()
            ' Interface
            frmMapEditor.ToolStripProgressBar1.Value = 0
            frmMapEditor.ToolStripProgressBar1.Maximum = MapSize.Y
            Dim bm As New Bitmap(128 * MapSize.X, 128 * MapSize.Y)
            Dim gr As Graphics = Graphics.FromImage(bm)
            Dim Image As Image = New Bitmap(128 * MapSize.X, 128 * MapSize.Y)
            For y = 0 To MapSize.Y - 1
                For x = 0 To MapSize.X - 1
                    Dim QuadrantName As String = path & "\00" & x & "00" & y & "\water.wtr"
                    If My.Computer.FileSystem.FileExists(QuadrantName) Then
                        Image = LoadImage.LoadWTR(QuadrantName)
                    Else
                        MessageBox.Show("Quadrant not found: " & QuadrantName)
                    End If
                    gr.DrawImage(Image, x * 128, y * 128)
                    Image = bm
                Next x
                frmMapEditor.ToolStripProgressBar1.Value += 1
                frmMapEditor.ToolStrip1.Refresh()
            Next y

            Water.Size = New Size(128 * MapSize.X, 128 * MapSize.Y)
            Water.Image = Image
        End If
        Return Water
    End Function

    Public Function LoadATRMap(ByVal path As String)
        Dim picATR As PictureBox = New PictureBox()
        If Not MapSize.Y = Nothing And Not MapSize.X = Nothing Then
            frmMapEditor.Panel1.Controls.Clear()
            ' Interface
            frmMapEditor.ToolStripProgressBar1.Value = 0
            frmMapEditor.ToolStripProgressBar1.Maximum = MapSize.Y
            Dim bm As New Bitmap(256 * MapSize.X, 256 * MapSize.Y)
            Dim gr As Graphics = Graphics.FromImage(bm)
            Dim Image As Image = New Bitmap(256 * MapSize.X, 256 * MapSize.Y)
            For y = 0 To MapSize.Y - 1
                For x = 0 To MapSize.X - 1
                    Dim QuadrantName As String = path & "\00" & x & "00" & y & "\attr.atr"
                    If My.Computer.FileSystem.FileExists(QuadrantName) Then
                        Image = LoadImage.LoadATR(QuadrantName)
                    Else
                        MessageBox.Show("Quadrant not found: " & QuadrantName)
                    End If
                    gr.DrawImage(Image, x * 256, y * 256)
                    Image = bm
                Next x
                frmMapEditor.ToolStripProgressBar1.Value += 1
                frmMapEditor.ToolStrip1.Refresh()
            Next y

            picATR.Size = New Size(256 * MapSize.X, 256 * MapSize.Y)
            picATR.Image = Image
        End If

        Return picATR
    End Function

    Public Function LoadHeightTargaMap(ByVal path As String)
        Dim picHeight As New PictureBox
        If Not MapSize.Y = Nothing And Not MapSize.X = Nothing Then
            frmMapEditor.Panel1.Controls.Clear()
            ' Interface
            frmMapEditor.ToolStripProgressBar1.Value = 0
            frmMapEditor.ToolStripProgressBar1.Maximum = MapSize.Y
            Dim bm As New Bitmap(129 * MapSize.X, 129 * MapSize.Y)
            Dim gr As Graphics = Graphics.FromImage(bm)
            Dim Image As Image = New Bitmap(129 * MapSize.X, 129 * MapSize.Y)
            For y = 0 To MapSize.Y - 1
                For x = 0 To MapSize.X - 1
                    Dim QuadrantName As String = path & "\00" & x & "00" & y & "\height.tga"
                    If My.Computer.FileSystem.FileExists(QuadrantName) Then
                        Image = Paloma.TargaImage.LoadTargaImage(QuadrantName)
                    Else
                        MessageBox.Show("Quadrant not found: " & QuadrantName)
                    End If
                    gr.DrawImage(Image, x * 129, y * 129)
                    Image = bm
                Next x
                frmMapEditor.ToolStripProgressBar1.Value += 1
                frmMapEditor.ToolStrip1.Refresh()
            Next y

            picHeight.Size = Image.Size
            picHeight.Image = Image

        End If
        Return picHeight
    End Function

    'Public Sub LoadDDSMap(ByVal path As String, ByVal Type As String)
    'Dim s As String = LoadSettings.CheckString("MAPSIZE", path & "\setting.txt", "Impossible to check the Map Size")
    'If Not s = "ERROR" Then
    'Dim MapSize As String() = s.Split("x")
    'Dim MapSizeX As Integer = Convert.ToInt32(MapSize(1).Replace(" ", ""))
    'Dim MapSizeY As Integer = Convert.ToInt32(MapSize(0).Replace(" ", ""))
    'frmMain.Panel1.Controls.Clear()
    ' Interface
    'frmMain.ToolStripProgressBar1.Value = 0
    'frmMain.ToolStripProgressBar1.Maximum = MapSizeY
    'Dim bm As New Bitmap(256 * MapSizeY, 256 * MapSizeX)
    'Dim gr As Graphics = Graphics.FromImage(bm)
    'Dim Image As Image = New Bitmap(256 * MapSizeY, 256 * MapSizeX)

    'Dim GraphicsCard As New Device
    'GraphicsCard.SetCooperativeLevel(frmMain, CooperativeLevelFlags.FullscreenExclusive)
    'GraphicsCard.SetDisplayMode(256, 256, 16, 0, False)
    'For y = 0 To MapSizeY - 1
    'For x = 0 To MapSizeX - 1
    'Dim QuadrantName As String = path & "\00" & y & "00" & x & "\" & Type
    'If My.Computer.FileSystem.FileExists(QuadrantName) Then
    'Dim sur As New Surface(QuadrantName, New SurfaceDescription, GraphicsCard)
    'Image
    'Else
    'MessageBox.Show("Quadrant not found: " & QuadrantName)
    'End If
    'gr.DrawImage(Image, 0 + (y * 256), 0 + (x * 256))
    'Image = bm
    'Next x
    'frmMain.ToolStripProgressBar1.Value += 1
    'frmMain.ToolStrip1.Refresh()
    'Next y

    'frmMain.Picture = New PictureBox()
    'frmMain.Picture.Size = New Size(256 * MapSizeY, 256 * MapSizeX)
    'frmMain.Picture.Image = Image
    'frmMain.Panel1.Controls.Add(frmMain.Picture)
    'End If
    'End Sub

    Public Function SetShowName()
        Dim ShowName As String = "Show: Attribute, "
        If frmMapEditor.ShowSRegen.Checked = True Then
            ShowName += "Server Regen, "
        End If
        If frmMapEditor.ShowCRegen.Checked = True Then
            ShowName += "Client Regen, "
        End If
        If frmMapEditor.ShowMAI.Checked = True Then
            ShowName += "Monster Area Info, "
        End If
        If frmMapEditor.ShowNPC.Checked = True Then
            ShowName += "NPC, "
        End If
        If frmMapEditor.ShowStone.Checked = True Then
            ShowName += "Stone, "
        End If
        If ShowName.Length > 2 Then
            ShowName = ShowName.Substring(0, ShowName.Length - 2)
        End If
        Return ShowName
    End Function
End Module
