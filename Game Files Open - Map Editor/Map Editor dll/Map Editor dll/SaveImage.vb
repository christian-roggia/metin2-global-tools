Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing

Public Class SaveImage
    Shared Function SaveATR(ByVal imgBitmap As Bitmap)
        Dim x, y, CurrentByte, i As Integer
        Dim Bytes(imgBitmap.Height * imgBitmap.Width + 5) As Byte
        Dim Magic() As Byte = {74, 10, 0, 1, 0, 1}
        Array.Copy(Magic, Bytes, 6)

        Try
            y = x = i = 0
            CurrentByte = 6
            For y = 0 To 255
                For x = 0 To 255
                    Bytes(CurrentByte) = imgBitmap.GetPixel(x, y).R
                    CurrentByte += 1
                Next x
                x = 0
            Next y
        Catch ex As Exception
            'Error Handle
        End Try
        Return Bytes
    End Function

    Shared Function SaveRAW(ByVal imgBitmap As Bitmap, ByVal Channel As Integer, ByVal ChannelToRead As Integer, Optional ByVal Bytes() As Byte = Nothing)
        Dim x, y, CurrentByte, i As Integer
        If IsNothing(Bytes) = True Then
            ReDim Preserve Bytes(imgBitmap.Height * imgBitmap.Width * Channel - 1)
        End If

        Try
            y = x = i = 0
            CurrentByte = ChannelToRead - 1
            For y = 0 To imgBitmap.Width - 1
                For x = 0 To imgBitmap.Height - 1
                    Bytes(CurrentByte) = imgBitmap.GetPixel(x, y).R
                    CurrentByte += Channel
                Next x
                x = 0
            Next y
        Catch ex As Exception
            'Error Handle
        End Try
        Return Bytes
    End Function

    Shared Function SaveWTR(ByVal imgBitmap As Bitmap)
        Dim x, y, CurrentByte, i As Integer
        Dim Bytes(imgBitmap.Height * imgBitmap.Width + 6) As Byte
        Dim Magic() As Byte = {50, 21, 128, 0, 128, 0, 1}
        Array.Copy(Magic, Bytes, 6)

        Try
            y = x = i = 0
            CurrentByte = 7
            For y = 0 To 127
                For x = 0 To 127
                    Select Case (imgBitmap.GetPixel(x, y))
                        Case Color.FromArgb(0, 255, 255)
                            Bytes(CurrentByte) = 0
                        Case Color.FromArgb(0, 148, 255)
                            Bytes(CurrentByte) = 1
                        Case Color.FromArgb(0, 0, 255)
                            Bytes(CurrentByte) = 2
                        Case Color.White
                            Bytes(CurrentByte) = 255
                        Case Else
                            Bytes(CurrentByte) = imgBitmap.GetPixel(x, y).R
                    End Select
                    CurrentByte += 1
                Next x
                x = 0
            Next y
        Catch ex As Exception
            'Error Handle
        End Try
        Return Bytes
    End Function
End Class
