Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing

Public Class LoadImage
    Shared Function LoadRAW(ByVal path As String, ByVal Channel As Integer, ByVal ChannelToRead As Integer, ByVal SizeX As Integer, ByVal SizeY As Integer)
        Dim sr As FileStream
        Dim reader As BinaryReader
        Dim Bytes() As Byte
        Dim x, y, CurrentByte, i As Integer
        Dim imgBitmap As New Bitmap(SizeX, SizeY)

        If My.Computer.FileSystem.FileExists(path) Then
            sr = New FileStream(path, FileMode.Open)
            reader = New BinaryReader(sr)
            Bytes = reader.ReadBytes(sr.Length)
            sr.Close()
            y = x = i = 0
            CurrentByte = ChannelToRead
            For y = 0 To SizeY - 1
                For x = 0 To SizeX - 1
                    imgBitmap.SetPixel(x, y, Color.FromArgb(Bytes(CurrentByte), Bytes(CurrentByte), Bytes(CurrentByte)))
                    CurrentByte += Channel
                Next x
                x = 0
            Next y
        End If
        Return imgBitmap
    End Function

    Shared Function LoadWTR(ByVal path As String)
        Dim sr As FileStream
        Dim reader As BinaryReader
        Dim Bytes() As Byte
        Dim x, y, CurrentByte, i As Integer
        Dim imgBitmap As New Bitmap(128, 128)
        Dim col As New Color

        If My.Computer.FileSystem.FileExists(path) Then
            sr = New FileStream(path, FileMode.Open)
            reader = New BinaryReader(sr)
            Bytes = reader.ReadBytes(sr.Length)
            sr.Close()
            y = x = i = 0
            CurrentByte = 7
            For y = 0 To 127
                For x = 0 To 127
                    Select Case (Bytes(CurrentByte))
                        Case 0
                            col = Color.FromArgb(0, 255, 255)
                        Case 1
                            col = Color.FromArgb(0, 148, 255)
                        Case 2
                            col = Color.FromArgb(0, 0, 255)
                        Case Else
                            col = Color.FromArgb(Bytes(CurrentByte), Bytes(CurrentByte), Bytes(CurrentByte))
                    End Select
                    imgBitmap.SetPixel(x, y, col)
                    CurrentByte += 1
                Next x
                x = 0
            Next y
        End If
        Return imgBitmap
    End Function

    Shared Function LoadATR(ByVal path As String, Optional ByVal SpecialColors(,) As String = Nothing)
        Dim sr As FileStream
        Dim reader As BinaryReader
        Dim Bytes() As Byte
        Dim x, y, CurrentByte, i As Integer
        Dim imgBitmap As New Bitmap(256, 256)

        If My.Computer.FileSystem.FileExists(path) Then
            sr = New FileStream(path, FileMode.Open)
            reader = New BinaryReader(sr)
            Bytes = reader.ReadBytes(sr.Length)
            sr.Close()
            y = x = i = 0
            CurrentByte = 6
            For y = 0 To 255
                For x = 0 To 255
                    If IsNothing(SpecialColors) = True Then
                        imgBitmap.SetPixel(x, y, Color.FromArgb(Bytes(CurrentByte), Bytes(CurrentByte), Bytes(CurrentByte)))
                    Else
                        imgBitmap.SetPixel(x, y, Common.CheckColor(Bytes(CurrentByte), SpecialColors))
                    End If
                    CurrentByte += 1
                Next x
                x = 0
            Next y
        End If
        Return imgBitmap
    End Function
End Class