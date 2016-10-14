Imports GFO_Regen_Helper.LoadRegen
Imports System.IO

Module SaveAll
    Public Sub SaveCMapSettings(ByVal TextBoxes() As TextBox, ByVal TextSettings() As String, ByVal GCDirName As String)
        Dim TextToWrite As String = "ScriptType" & Chr(9) & "MapSetting" & Chr(13) & Chr(10) & Chr(13) & Chr(10)

        For t = 0 To TextBoxes.Length - 1
            TextToWrite += TextSettings(t) & Chr(9) & TextBoxes(t).Text.Replace(" x ", Chr(9)) & Chr(13) & Chr(10)
        Next t

        My.Computer.FileSystem.WriteAllText(Application.StartupPath + "\" + GCDirName + "\setting.txt", TextToWrite, False)
    End Sub

    Public Sub CheckDirExists(ByVal DirectoryName As String)
        If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\" & DirectoryName) Then
            My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\" & DirectoryName)
        End If
    End Sub

    Public Sub SaveAllImages(ByVal PicturesName() As PictureBox, ByVal ImgNames() As String, ByVal GCDirName As String)
        Dim PathName As String
        For i = 0 To PicturesName.Length - 1
            PathName = Application.StartupPath & "\" & GCDirName & "\Images\" & ImgNames(i) & ".png"
            PicturesName(i).Image.Save(PathName, Imaging.ImageFormat.Png)
        Next i
    End Sub

    Public Sub SaveCMapRegen(ByVal ClientRegenArray() As QuadrantRegen, ByVal GCDirName As String, ByVal RType As String)
        Dim TextToWrite As String = "///////////////////////////////////////////////////////////////////////" & Chr(13) & Chr(10) & "//" & Chr(13) & Chr(10)
        TextToWrite += "//  " & RType & " generated with Game Files Open - Map Editor" & Chr(13) & Chr(10)
        TextToWrite += "//  All thanks to the Game Files Open Team" & Chr(13) & Chr(10) & "//" & Chr(13) & Chr(10)
        TextToWrite += "//  Map Name: " & GCDirName.Replace("\", "").Replace("Maps Exported", "").Replace("Settings", "") & Chr(13) & Chr(10)
        TextToWrite += "//  Type: Client " & RType & Chr(13) & Chr(10) & "//" & Chr(13) & Chr(10)
        TextToWrite += "///////////////////////////////////////////////////////////////////////" & Chr(13) & Chr(10) & Chr(13) & Chr(10)


        For q = 0 To ClientRegenArray.Length - 1
            If IsNothing(ClientRegenArray(q).Regen) = False Then
                'TextToWrite += "//Quadrant X: " & ClientRegenArray(q).QX.ToString & " Y: " & ClientRegenArray(q).QY.ToString & " Objects Count: " & ClientRegenArray(q).ObjectsNumber & Chr(13) & Chr(10) & Chr(13) & Chr(10)
                For r = 0 To ClientRegenArray(q).Regen.Length - 1
                    TextToWrite += ClientRegenArray(q).Regen(r).Type & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).X.ToString & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).Y.ToString & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).SX.ToString & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).SY.ToString & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).Z.ToString & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).Dir.ToString & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).Time & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).Percent.ToString & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).Count.ToString & Chr(9)
                    TextToWrite += ClientRegenArray(q).Regen(r).Vnum.ToString & Chr(13) & Chr(10)
                Next
            End If
        Next q

        My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\" & GCDirName & "\" & RType.Replace(" ", "").ToLower & ".txt", TextToWrite, False)
    End Sub

    Public Sub SaveCSettings(ByVal Regen As Boolean, ByVal GCDirName As String, ByVal SaveSettings As Boolean)
        Dim TextToWrite As String = "##################################################" & Chr(13) & Chr(10) & "#" & Chr(13) & Chr(10)
        TextToWrite += "#  Game Files Open - Map Editor" & Chr(13) & Chr(10)
        TextToWrite += "#  All rights reserved" & Chr(13) & Chr(10) & "#" & Chr(13) & Chr(10)
        TextToWrite += "##################################################" & Chr(13) & Chr(10) & Chr(13) & Chr(10)
        TextToWrite += "[Images Paths]" & Chr(13) & Chr(10)
        TextToWrite += "Attribute Path = ./Images/Attribute.png" & Chr(13) & Chr(10)
        TextToWrite += "Height CH1 Path = ./Images/Height CH1.png" & Chr(13) & Chr(10)
        TextToWrite += "Height CH2 Path = ./Images/Height CH2.png" & Chr(13) & Chr(10)
        TextToWrite += "Shadow CH1 Path = ./Images/Shadow CH1.png" & Chr(13) & Chr(10)
        TextToWrite += "Shadow CH2 Path = ./Images/Shadow CH2.png" & Chr(13) & Chr(10)
        TextToWrite += "Tile Path = ./Images/Tile.png" & Chr(13) & Chr(10)
        TextToWrite += "Water Path = ./Images/Water.png" & Chr(13) & Chr(10) & Chr(13) & Chr(10)

        If SaveSettings = True Then
            TextToWrite += "[Settings Paths]" & Chr(13) & Chr(10)
            TextToWrite += "Setting Path = ./Settings/setting.txt" & Chr(13) & Chr(10)

            If Regen = True Then
                TextToWrite += "Regen Path = ./Settings/regen.txt" & Chr(13) & Chr(10)
            Else
                TextToWrite += "Monster Area Info Path = ./Settings/monsterareainfo.txt" & Chr(13) & Chr(10)
            End If
        End If

        My.Computer.FileSystem.WriteAllText(Application.StartupPath + "\Maps Exported\" + GCDirName + "\Paths.ini", TextToWrite, False)
    End Sub

    Public Sub ImportAll(ByVal FilePath As String, ByVal TempGDirName As String)
        Dim DirExported As String = FilePath.Substring(0, FilePath.LastIndexOf("\"))
        Dim ImagesPath(7) As String
        Dim PicturesToSave(7) As Bitmap
        ImagesPath(0) = FindImagesPaths("ATTRIBUTE PATH = ./", FilePath)
        ImagesPath(1) = FindImagesPaths("HEIGHT CH1 PATH = ./", FilePath)
        ImagesPath(2) = FindImagesPaths("HEIGHT CH2 PATH = ./", FilePath)
        ImagesPath(3) = FindImagesPaths("SHADOW CH1 PATH = ./", FilePath)
        ImagesPath(4) = FindImagesPaths("SHADOW CH2 PATH = ./", FilePath)
        ImagesPath(5) = FindImagesPaths("TILE PATH = ./", FilePath)
        ImagesPath(6) = FindImagesPaths("WATER PATH = ./", FilePath)
        Dim settingpath As String = FindImagesPaths("SETTING PATH = ./", FilePath)
        Dim regenpath As String = FindImagesPaths("REGEN PATH = ./", FilePath)
        If regenpath = "" Then
            regenpath = FindImagesPaths("MONSTER AREA INFO PATH = ./", FilePath)
        End If

        For i = 0 To ImagesPath.Length - 1
            If Not ImagesPath(i) = "" Then
                If My.Computer.FileSystem.FileExists(DirExported & "\" & ImagesPath(i)) Then
                    PicturesToSave(i) = New Bitmap(DirExported & "\" & ImagesPath(i))
                End If
            Else
                PicturesToSave(i) = New Bitmap(256, 256)
            End If
        Next
        SaveMap.SaveMapATR(PicturesToSave(0), TempGDirName)
        SaveMap.SaveMapHeight(PicturesToSave(1), 1, TempGDirName)
        SaveMap.SaveMapHeight(PicturesToSave(2), 2, TempGDirName)
        'SaveMap.SaveMapTarga(Img)
        SaveMap.SaveMapRAW(PicturesToSave(3), 2, 1, 256, 256, "shadowmap.raw", TempGDirName)
        SaveMap.SaveMapRAW(PicturesToSave(4), 2, 2, 256, 256, "shadowmap.raw", TempGDirName)
        SaveMap.SaveMapRAW(PicturesToSave(5), 1, 1, 258, 258, "tile.raw", TempGDirName)
        SaveMap.SaveMapWTR(PicturesToSave(6), TempGDirName)

        If Not settingpath = "" Then
            Dim PathSetting As String = Application.StartupPath & "\Maps Imported\" & TempGDirName & "\" & settingpath.Replace("SETTINGS/", "").ToLower
            If My.Computer.FileSystem.FileExists(PathSetting) Then
                My.Computer.FileSystem.DeleteFile(PathSetting)
            End If

            If My.Computer.FileSystem.FileExists(DirExported & "\" & settingpath) Then
                My.Computer.FileSystem.CopyFile(DirExported & "\" & settingpath, PathSetting)
            End If
        End If

        If Not regenpath = "" Then
            If My.Computer.FileSystem.FileExists(DirExported & "\" & regenpath) Then
                Dim PathRegen As String = Application.StartupPath & "\Maps Imported\" & TempGDirName & "\" & regenpath.Replace("SETTINGS/", "").ToLower
                If My.Computer.FileSystem.FileExists(PathRegen) Then
                    My.Computer.FileSystem.DeleteFile(PathRegen)
                End If

                My.Computer.FileSystem.CopyFile(DirExported & "\" & regenpath, PathRegen)
            End If
        End If
    End Sub

    Private Function FindImagesPaths(ByVal str As String, ByVal path As String)
        str = str.ToUpper
        If My.Computer.FileSystem.FileExists(path) Then
            Dim Settings As New StreamReader(path)
            Do While (Settings.Peek >= 0)
                Dim ScriptType As String = Settings.ReadLine()
                If ScriptType.ToUpper.Contains(str) = True Then
                    ' Stringa trovata
                    If ScriptType.ToUpper.Substring(0, str.Length) = str Then
                        Return ScriptType.ToUpper.Replace(str, "")
                        Exit Do
                        Exit Function
                    End If
                End If
            Loop
        End If
        Return ""
    End Function

    Public Sub SaveSMapRegen(ByVal ServerArray() As QuadrantRegen, ByVal GCDirName As String, ByVal RType As String)
        Dim TextToWrite As String = "///////////////////////////////////////////////////////////////////////" & Chr(13) & Chr(10) & "//" & Chr(13) & Chr(10)
        TextToWrite += "//  " & RType & " generated with Game Files Open - Map Editor" & Chr(13) & Chr(10)
        TextToWrite += "//  All thanks to the Game Files Open Team" & Chr(13) & Chr(10) & "//" & Chr(13) & Chr(10)
        TextToWrite += "//  Map Name: " & GCDirName.Replace("\", "").Replace("Maps Exported", "").Replace("Server", "") & Chr(13) & Chr(10)
        TextToWrite += "//  Type: Server " & RType & Chr(13) & Chr(10) & "//" & Chr(13) & Chr(10)
        TextToWrite += "///////////////////////////////////////////////////////////////////////" & Chr(13) & Chr(10) & Chr(13) & Chr(10)


        For q = 0 To ServerArray.Length - 1
            If IsNothing(ServerArray(q).Regen) = False Then
                'TextToWrite += "//Quadrant X: " & ClientRegenArray(q).QX.ToString & " Y: " & ClientRegenArray(q).QY.ToString & " Objects Count: " & ClientRegenArray(q).ObjectsNumber & Chr(13) & Chr(10) & Chr(13) & Chr(10)
                For r = 0 To ServerArray(q).Regen.Length - 1
                    TextToWrite += ServerArray(q).Regen(r).Type & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).X.ToString & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).Y.ToString & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).SX.ToString & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).SY.ToString & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).Z.ToString & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).Dir.ToString & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).Time & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).Percent.ToString & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).Count.ToString & Chr(9)
                    TextToWrite += ServerArray(q).Regen(r).Vnum.ToString & Chr(13) & Chr(10)
                Next
            End If
        Next q

        My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\" & GCDirName & "\" & RType.Replace(" ", "").ToLower & ".txt", TextToWrite, False)
    End Sub
End Module
