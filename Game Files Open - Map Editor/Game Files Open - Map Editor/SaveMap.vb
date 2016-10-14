Imports GFO_Map_Helper
Imports System.IO
Module SaveMap
    Public Sub SaveMapATR(ByVal ATRImage As Image, ByVal MapName As String)
        Dim Path As String
        Dim _SourceImage As Bitmap = New Bitmap(ATRImage)

        For x As Integer = 0 To ATRImage.Width / 256 - 1
            For y As Integer = 0 To ATRImage.Height / 256 - 1
                Path = Application.StartupPath & "\Maps Imported\" & MapName & "\00" & x & "00" & y
                If Not My.Computer.FileSystem.DirectoryExists(Path) Then
                    My.Computer.FileSystem.CreateDirectory(Path)
                End If

                Path += "\attr.atr"
                Dim rect As New Rectangle(x * 256, y * 256, 256, 256)
                Dim Bytes() As Byte = SaveImage.SaveATR(_SourceImage.Clone(rect, _SourceImage.PixelFormat))
                FileOpen(1, Path, OpenMode.Binary, OpenAccess.Write)
                FilePut(1, Bytes)
                FileClose(1)
            Next y
        Next x

    End Sub

    Public Sub SaveMapWTR(ByVal WTRImage As Image, ByVal MapName As String)
        Dim Path As String
        Dim _SourceImage As Bitmap = New Bitmap(WTRImage)

        For x As Integer = 0 To WTRImage.Width / 128 - 1
            For y As Integer = 0 To WTRImage.Height / 128 - 1
                Path = Application.StartupPath & "\Maps Imported\" & MapName & "\00" & x & "00" & y
                If Not My.Computer.FileSystem.DirectoryExists(Path) Then
                    My.Computer.FileSystem.CreateDirectory(Path)
                End If

                Path += "\water.wtr"
                Dim rect As New Rectangle(x * 128, y * 128, 128, 128)
                Dim Bytes() As Byte = SaveImage.SaveWTR(_SourceImage.Clone(rect, _SourceImage.PixelFormat))
                FileOpen(1, Path, OpenMode.Binary, OpenAccess.Write)
                FilePut(1, Bytes)
                FileClose(1)
            Next y
        Next x
    End Sub

    Public Sub SaveMapHeight(ByVal RAWImage As Image, ByVal ChannelToSave As Integer, ByVal MapName As String)
        Dim Path As String
        Dim _SourceImage As Bitmap = New Bitmap(RAWImage)

        For x As Integer = 0 To (RAWImage.Width - 3) / 128 - 1
            For y As Integer = 0 To (RAWImage.Height - 3) / 128 - 1
                Path = Application.StartupPath & "\Maps Imported\" & MapName & "\00" & x & "00" & y
                If Not My.Computer.FileSystem.DirectoryExists(Path) Then
                    My.Computer.FileSystem.CreateDirectory(Path)
                End If

                Path += "\height.raw"
                Dim Bytes() As Byte
                Dim rect As New Rectangle(x * 128, y * 128, 131, 131)
                If My.Computer.FileSystem.FileExists(Path) And ChannelToSave > 1 Then
                    Dim streamBinary As New FileStream(Path, FileMode.Open)
                    Dim readerInput As BinaryReader = New BinaryReader(streamBinary)
                    Dim lengthFile As Integer = streamBinary.Length - 1
                    Bytes = readerInput.ReadBytes(lengthFile)
                    streamBinary.Close()
                    readerInput.Close()
                Else
                    Bytes = Nothing
                End If
                FileOpen(1, Path, OpenMode.Binary, OpenAccess.Write)
                Bytes = SaveImage.SaveRAW(_SourceImage.Clone(rect, _SourceImage.PixelFormat), 2, ChannelToSave, Bytes)
                FilePut(1, Bytes)
                FileClose(1)
            Next y
        Next x
    End Sub

    Public Sub SaveMapRAW(ByVal RAWImage As Image, ByVal NumberOfChannel As Integer, ByVal ChannelToRead As Integer, ByVal RAWWidth As Integer, ByVal RAWHeight As Integer, ByVal RAWFileName As String, ByVal MapName As String)
        Dim Path As String
        Dim _SourceImage As Bitmap = New Bitmap(RAWImage)

        For x As Integer = 0 To RAWImage.Width / RAWWidth - 1
            For y As Integer = 0 To RAWImage.Height / RAWHeight - 1
                Path = Application.StartupPath & "\Maps Imported\" & MapName & "\00" & x & "00" & y
                If Not My.Computer.FileSystem.DirectoryExists(Path) Then
                    My.Computer.FileSystem.CreateDirectory(Path)
                End If

                Path += "\" & RAWFileName
                Dim Bytes() As Byte
                Dim rect As New Rectangle(x * RAWWidth, y * RAWHeight, RAWWidth, RAWHeight)
                If My.Computer.FileSystem.FileExists(Path) And ChannelToRead > 1 Then
                    Dim streamBinary As New FileStream(Path, FileMode.Open)
                    Dim readerInput As BinaryReader = New BinaryReader(streamBinary)
                    Dim lengthFile As Integer = streamBinary.Length - 1
                    Bytes = readerInput.ReadBytes(lengthFile)
                    streamBinary.Close()
                    readerInput.Close()
                Else
                    Bytes = Nothing
                End If
                FileOpen(1, Path, OpenMode.Binary, OpenAccess.Write)
                Bytes = SaveImage.SaveRAW(_SourceImage.Clone(rect, _SourceImage.PixelFormat), NumberOfChannel, ChannelToRead, Bytes)
                FilePut(1, Bytes)
                FileClose(1)
            Next y
        Next x
    End Sub
End Module
