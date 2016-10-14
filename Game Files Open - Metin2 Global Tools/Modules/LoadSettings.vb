Imports System.IO
Module LoadSettings
    Public Function LoadSettings(ByVal path As String) As String()
        Dim PathSettings As String = path & "\setting.txt"
        Dim ArrayString() As String = {}
        If Not CheckString("SCRIPTTYPEMAPSETTING", PathSettings, "Impossible to check the ScriptType") = "ERROR" Then
            Dim CellScale As String = CheckString("CELLSCALE", PathSettings, "Impossible to check the CellScale")
            Dim HeightScale As String = CheckString("HEIGHTSCALE", PathSettings, "Impossible to check the HeightScale")
            Dim ViewRadius As String = CheckString("VIEWRADIUS", PathSettings, "Impossible to check the ViewRadius")
            Dim MapSize As String = CheckString("MAPSIZE", PathSettings, "Impossible to check the MapSize")
            Dim BasePosition As String = CheckString("BASEPOSITION", PathSettings, "Impossible to check the BasePosition")
            Dim TextureSet As String = CheckString("TEXTURESET", PathSettings, "Impossible to check the TextureSet")
            Dim Environment As String = CheckString("ENVIRONMENT", PathSettings, "Impossible to check the Environment")
            ArrayString = {CellScale, HeightScale, ViewRadius, MapSize, BasePosition, TextureSet, Environment}
        End If
        Return ArrayString
    End Function

    Public Function CheckString(ByVal str As String, ByVal path As String, ByVal InvalidFile As String)
        str = str.ToUpper.Replace(" ", "")
        If My.Computer.FileSystem.FileExists(path) Then
            Dim Settings As New StreamReader(path)
            Do While (Settings.Peek >= 0)
                Dim ScriptType As String = Settings.ReadLine()
                If ScriptType.ToUpper.Replace(" ", "").Replace("	", "").Contains(str) = True Then
                    ' Stringa trovata
                    If ScriptType.ToUpper.Replace(" ", "").Replace("	", "").Substring(0, str.Length) = str Then
                        Return ScriptType.Replace(" ", "").Remove(0, str.Length + 1).Replace("	", " x ")
                    Else
                        MessageBox.Show(InvalidFile & "asd", "Load settings error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return "ERROR"
                    End If
                End If
            Loop
            MessageBox.Show(InvalidFile, "Load settings error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return "ERROR"
        End If
        MessageBox.Show("File settings not found!", "Load settings error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return "ERROR"
    End Function
End Module
