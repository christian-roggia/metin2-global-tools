Imports GFO_Regen_Helper.LoadRegen
Imports GFO_Regen_Helper.EditRegen
Imports GFO_Regen_Helper.Common
Imports GFO_Regen_Helper

Public Class frmRegen
    Private WithEvents MyAttribute As New PictureBox()
    Private WithEvents MyRegen As New PictureBox()

    Private MyRegenBitmap As New Bitmap(256, 256)

    Public Shared GCPath As String = ""
    Public Shared GSPath As String = ""
    Private GCDirName As String = ""

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

    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            GCPath = FolderBrowserDialog1.SelectedPath
            GCDirName = GCPath.Remove(0, GCPath.LastIndexOf("\") + 1)

            Dim MySettingsArray() As String = LoadSettings.LoadSettings(GCPath)
            Dim MapSizeArray() As String = MySettingsArray(3).Replace(" ", Nothing).Split("x")

            If IsNothing(MapSizeArray(0)) = False And IsNumeric(MapSizeArray(0)) = True Then
                MapSize.X = Convert.ToInt32(MapSizeArray(0))
            End If

            MapSize.X = MySettingsArray(3).Replace(" ", Nothing).Split("x")(0)
            MapSize.Y = MySettingsArray(3).Replace(" ", Nothing).Split("x")(1)

            MyAttribute = LoadMap.LoadATRMap(GCPath)
            MyRegen = MyAttribute

            If My.Computer.FileSystem.FileExists(GCPath & "\regen.txt") Then
                ClientRegenExists = True
                ShowCRegen.Enabled = True
                ComboFilesType.Text = "Regen (Client)"
                ClientRegenArray = LoadRegen.LoadArrayRegen(GCPath & "\regen.txt")
                AddListBoxItem("MapOutdoor::Load - LoadMonsterAreaInfo Loaded(regen.txt)")
            ElseIf My.Computer.FileSystem.FileExists(GCPath & "\monsterareainfo.txt") Then
                ClientMAIExists = True
                ShowMAI.Enabled = True
                ComboFilesType.Text = "MonsterAreaInfo"
                MAIArray = LoadRegen.LoadArrayRegen(GCPath & "\monsterareainfo.txt")
                AddListBoxItem("MapOutdoor::Load - LoadMonsterAreaInfo Loaded(monsterareainfo.txt)")
            End If

            SaveClientMaps.Enabled = True
            SaveAsClient.Enabled = True

            Panel1.Controls.Clear()
            Panel1.Controls.Add(MyAttribute)

            AddHandler MyRegen.MouseMove, AddressOf MyRegen_MouseMove
            AddHandler MyRegen.MouseDown, AddressOf MyRegen_MouseDown
        End If
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

    Private Sub LoadRegenFiles(ByVal LName As String, ByVal LColor As Color, ByRef QuadrantsArray() As QuadrantRegen, ByVal LCheck As CheckBox, ByVal LPath As String)
        MyRegen.Size = New Size(MapSize.X * 256, MapSize.Y * 256)
        If LCheck.Checked = True Then
            If IsNothing(QuadrantsArray) = True Then
                QuadrantsArray = LoadRegen.LoadArrayRegen(LPath & LName)
                If IsNothing(QuadrantsArray) = False Then
                    MyRegen.Image = EditRegen.ConvertArrayToImage(QuadrantsArray, MyRegen.Image, LColor)
                End If
                Panel1.Controls.Clear()
                Panel1.Controls.Add(MyRegen)
            Else
                MyRegen.Image = EditRegen.ConvertArrayToImage(QuadrantsArray, MyRegen.Image, LColor)
                Panel1.Controls.Clear()
                Panel1.Controls.Add(MyRegen)
            End If
        Else
            MyRegen.Image = EditRegen.DeleteArrayFromImage(QuadrantsArray, MyAttribute.Image, MyRegen.Image)
            Panel1.Controls.Clear()
            Panel1.Controls.Add(MyRegen)
        End If
        MyRegenBitmap = MyRegen.Image
    End Sub

    Private Sub AddListBoxItem(ByVal ItemToAdd As String)
        ListBox1.Items.Add(ItemToAdd)
        ListBox1.TopIndex = ListBox1.Items.Count - 1
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

        'RefreshCheckBox(True)
    End Sub

    Private Sub MyRegen_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyRegen.MouseDown
        If IsNothing(MyRegen.Image) = False Then
            MyRegenBitmap = MyRegen.Image
            If MyRegenBitmap.Width > e.X And MyRegenBitmap.Height > e.Y Then
                Dim LocalColor As Color = Common.SwitchColor(ComboFilesType.Text)
                If Not MyRegenBitmap.GetPixel(e.X, e.Y) = LocalColor Then
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
                        MyRegenBitmap.SetPixel(e.X, e.Y, LocalColor)
                        MyRegen.Image = MyRegenBitmap
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub MyRegen_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyRegen.MouseMove
        Dim Localization() As Integer = LocalizeRegen(e.X, e.Y)
        Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
        If MyRegenBitmap.Width > e.X And MyRegenBitmap.Height > e.Y Then
            Dim ColorToArgb As Integer = MyRegenBitmap.GetPixel(e.X, e.Y).ToArgb
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

                With QuadrantsArray(RegenLocalizate).Regen(i)
                    Dim TextRegen() As TextBox = {TextX, TextY, TextSX, TextSY, TextZ, TextDir, TextTime, TextPercent, TextCount, TextVnum}
                    Dim RegenValue() As String = {.X, .Y, .SX, .SY, .Z, .Dir, .Time, .Percent, .Count, .Vnum}
                    For v = 0 To TextRegen.Length - 1
                        TextRegen(v).Text = RegenValue(v)
                    Next v
                End With

                RegenLoadID = i
                RegenLoadX = QuadrantsArray(RegenLocalizate).Regen(i).X
                RegenLoadY = QuadrantsArray(RegenLocalizate).Regen(i).Y
                RegenLoadArray = QuadrantsArray
                RegenLoadColor = TColor

                TabControl1.SelectedTab = TabPage2
                Exit For
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
                    MyRegen.Image = EditRegen.DeleteArrayFromImage(Nothing, MyAttribute.Image, MyRegen.Image, {RegenLoadX, RegenLoadY})
                    Dim TextRegen() As TextBox = {TextX, TextY, TextSX, TextSY, TextZ, TextDir, TextTime, TextPercent, TextCount, TextVnum}
                    For c = 0 To TextRegen.Length - 1
                        TextRegen(c).Text = ""
                    Next c
                    Exit Sub
                End If

                Dim X As Integer = Convert.ToInt32(TextY.Text)
                Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)

                If X < MyRegen.Image.Width Then
                    If RegenLoadID < RegenLoadArray(RegenLocalizate).Regen.Length - 1 Then
                        If IsNothing(MyRegen.Image) = False Then
                            MyRegenBitmap = MyRegen.Image.Clone

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
                            MyRegenBitmap = EditRegen.DeleteArrayFromImage(Nothing, MyAttribute.Image, MyRegen.Image, Coordinates)
                            RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).X = X
                            MyRegenBitmap.SetPixel(X, RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Y, RegenLoadColor)
                            MyRegen.Image = MyRegenBitmap
                            MyRegen.Refresh()
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
                                MyRegen.Image = EditRegen.DeleteArrayFromImage(Nothing, MyAttribute.Image, MyRegen.Image, {RegenLoadX, RegenLoadY})
                            End If
                        Else
                            MyRegen.Image = EditRegen.DeleteArrayFromImage(Nothing, MyAttribute.Image, MyRegen.Image, {RegenLoadX, RegenLoadY})
                        End If
                    Else
                        MyRegen.Image = EditRegen.DeleteArrayFromImage(Nothing, MyAttribute.Image, MyRegen.Image, {RegenLoadX, RegenLoadY})
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

                If Y < MyRegen.Image.Width Then
                    If RegenLoadID < RegenLoadArray(RegenLocalizate).Regen.Length - 1 Then
                        If IsNothing(MyRegen.Image) = False Then
                            MyRegenBitmap = MyRegen.Image.Clone

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
                            MyRegenBitmap = EditRegen.DeleteArrayFromImage(Nothing, MyAttribute.Image, MyRegen.Image, Coordinates)
                            MyRegenBitmap.SetPixel(RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).X, Y, RegenLoadColor)
                            MyRegen.Image = MyRegenBitmap
                            MyRegen.Refresh()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub TextZ_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextZ.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                If IsNumeric(TextZ.Text) = True And IsNothing(TextZ.Text) = False Then
                    Dim Z As Integer = Convert.ToInt32(TextZ.Text)
                    Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                    Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                    RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Z = Z
                End If
            End If
        End If
    End Sub

    Private Sub TextSX_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSX.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                If IsNumeric(TextSX.Text) = True And IsNothing(TextSX.Text) = False Then
                    Dim SX As Integer = Convert.ToInt32(TextSX.Text)
                    Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                    Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                    RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).SX = SX
                End If
            End If
        End If
    End Sub

    Private Sub TextSY_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSY.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                If IsNumeric(TextSY.Text) = True And IsNothing(TextSY.Text) = False Then
                    Dim SY As Integer = Convert.ToInt32(TextSY.Text)
                    Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                    Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                    RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).SY = SY
                End If
            End If
        End If
    End Sub

    Private Sub TextDir_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDir.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                If IsNumeric(TextDir.Text) = True And IsNothing(TextDir.Text) = False Then
                    Dim Dir As Integer = Convert.ToInt32(TextDir.Text)
                    Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                    Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                    RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Dir = Dir
                End If
            End If
        End If
    End Sub

    Private Sub TextTime_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextTime.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                If IsNothing(TextTime.Text) = False Then
                    Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                    Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                    RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Time = TextTime.Text
                End If
            End If
        End If
    End Sub

    Private Sub TextPercent_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextPercent.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                If IsNumeric(TextPercent.Text) = True And IsNothing(TextPercent.Text) = False Then
                    Dim Percent As Integer = Convert.ToInt32(TextPercent.Text)
                    Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                    Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                    RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Percent = Percent
                End If
            End If
        End If
    End Sub

    Private Sub TextCount_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCount.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not RegenLoadID = -1 Then
                If IsNumeric(TextCount.Text) = True And IsNothing(TextCount.Text) = False Then
                    Dim MCount As Integer = Convert.ToInt32(TextCount.Text)
                    Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                    Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                    RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Count = MCount
                End If
            End If
        End If
    End Sub

    Private Sub TextVnum_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextVnum.KeyPress
        If CheckChangeRegen(e.KeyChar, TextVnum.Text, True) = True Then
            RegenLoadArray(FindLocalization()).Regen(RegenLoadID).Vnum = Convert.ToInt32(TextVnum.Text)
        End If
    End Sub

    Private Sub ComboType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboType.SelectedIndexChanged
        If Not RegenLoadID = -1 Then
            If IsNothing(TextVnum.Text) = False Then
                Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
                Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)
                RegenLoadArray(RegenLocalizate).Regen(RegenLoadID).Type = ComboType.Text.ToLower.Substring(0, 1)
            End If
        End If
    End Sub

    Private Function CheckChangeRegen(ByVal MyKey As Char, ByVal MyText As String, ByVal IsNumber As Boolean) As Boolean
        If Not MyKey = Chr(13) Then
            Return False
        End If

        If CheckRegen(MyText, IsNumber) = False Then
            Return False
        End If

        If FindLocalization() = -1 Then
            Return False
        End If

        Return True
    End Function

    Private Function FindLocalization() As Integer
        If IsNothing({RegenLoadX, RegenLoadY}) = False And IsNumeric({RegenLoadX, RegenLoadY}) = True And {RegenLoadX, RegenLoadY}.Contains(".") = False Then
            Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
            Return (Localization(0) * MapSize.X) + Localization(1)
        Else
            AddListBoxItem("Invalid Quadrant parameters:")
            AddListBoxItem("X = " & RegenLoadX.ToString)
            AddListBoxItem("Y = " & RegenLoadY.ToString)
            Return -1
        End If
    End Function


    Private Function CheckRegen(ByVal RegenText As String, ByVal CheckNumber As Boolean) As Boolean
        If RegenLoadID = -1 Then
            Return False
        End If

        If IsNothing(RegenText) = True Then
            Return False
        End If

        If CheckNumber = True And IsNumeric(RegenText) = False Then
            Return False
        End If

        If CheckNumber = True And RegenText.Contains(".") = True Then
            Return False
        End If

        Return True
    End Function

    Private Sub ComboFilesType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboFilesType.SelectedIndexChanged
        If Not RegenLoadID = -1 Then
            If IsNothing(ComboFilesType.Text) = False And IsNothing(MyRegen.Image) = False Then
                Dim MyColor As New Color
                Dim ArrayType() As QuadrantRegen = Nothing
                Select Case ComboFilesType.Text
                    Case "Regen (Server)"
                        MyColor = Color.Red
                        ArrayType = ServerRegenArray
                        Exit Select
                    Case "Regen (Client)"
                        MyColor = Color.FromArgb(255, 128, 0)
                        ArrayType = ClientRegenArray
                        Exit Select
                    Case "MonsterAreaInfo"
                        MyColor = Color.Cyan
                        ArrayType = MAIArray
                        Exit Select
                    Case "NPC"
                        MyColor = Color.Lime
                        ArrayType = ServerNPCArray
                        Exit Select
                    Case "Stone"
                        MyColor = Color.Silver
                        ArrayType = ServerStoneArray
                        Exit Select
                    Case "Boss"
                        MyColor = Color.DodgerBlue
                        ArrayType = ServerBossArray
                        Exit Select
                End Select

                If IsNothing(ArrayType) = True Then
                    AddListBoxItem("Invalid Regen Type, reload all the regen files!")
                    Exit Sub
                End If

                Dim RegenLocalizate As Integer = FindLocalization()

                If IsNothing(ArrayType(RegenLocalizate).Regen) = False Then
                    ReDim Preserve ArrayType(RegenLocalizate).Regen(ArrayType(RegenLocalizate).Regen.Length)
                Else
                    ReDim Preserve ArrayType(RegenLocalizate).Regen(0)
                End If

                ArrayType(RegenLocalizate).Regen(ArrayType(RegenLocalizate).Regen.Length - 1) = RegenLoadArray(RegenLocalizate).Regen(RegenLoadID)
                RegenLoadArray(RegenLocalizate).Regen = Common.RegenDeleteItem(RegenLoadX, RegenLoadY, RegenLoadArray(RegenLocalizate).Regen)

                MyRegenBitmap = MyRegen.Image
                MyRegenBitmap = EditRegen.DeleteArrayFromImage(Nothing, MyAttribute.Image, MyRegen.Image, {RegenLoadX, RegenLoadY})
                MyRegenBitmap.SetPixel(RegenLoadX, RegenLoadY, MyColor)
                MyRegen.Image = MyRegenBitmap
                MyRegen.Refresh()

                RegenLoadArray = ArrayType
                RegenLoadColor = MyColor
            End If
        End If
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


    Private Sub ToolStripMenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem14.Click
        CheckServerDir()
        SaveSMapRegen(ServerRegenArray, "Maps Exported\" & GCDirName & "\Server", "Regen")
        SaveSMapRegen(ServerNPCArray, "Maps Exported\" & GCDirName & "\Server", "NPC")
        SaveSMapRegen(ServerStoneArray, "Maps Exported\" & GCDirName & "\Server", "Stone")
        SaveSMapRegen(ServerBossArray, "Maps Exported\" & GCDirName & "\Server", "Boss")
    End Sub

    Private Sub CheckServerDir()
        CheckDirExists("Maps Exported")
        CheckDirExists("Maps Exported\" + GCDirName)
        CheckDirExists("Maps Exported\" + GCDirName + "\Server")
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

    Private Sub ServerMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            GSPath = FolderBrowserDialog1.SelectedPath
            If My.Computer.FileSystem.FileExists(GSPath & "\regen.txt") Then
                ServerRegenExists = True
            End If
            If My.Computer.FileSystem.FileExists(GSPath & "\stone.txt") Then
                ServerStoneExists = True
            End If
            If My.Computer.FileSystem.FileExists(GSPath & "\boss.txt") Then
                ServerBossExists = True
            End If
            If My.Computer.FileSystem.FileExists(GSPath & "\npc.txt") Then
                ServerNPCExists = True
            End If
            'RefreshCheckBox()
            ServerExportMaps.Enabled = True
        End If
    End Sub

    Private Sub ObjectsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles ObjectsToolStripMenuItem1.Click
        'CheckSettingsDir()
        If ClientRegenExists = True Then
            SaveAll.SaveCMapRegen(ClientRegenArray, "Maps Exported\" & GCDirName & "\Settings", "Regen")
        ElseIf ClientMAIExists = True Then
            SaveAll.SaveCMapRegen(MAIArray, "Maps Exported\" & GCDirName & "\Settings", "Monster Area Info")
        End If
    End Sub
End Class