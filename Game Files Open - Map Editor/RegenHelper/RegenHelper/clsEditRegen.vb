Imports GFO_Regen_Helper.LoadRegen
Imports System.Drawing

Public Class EditRegen
    Shared Function ConvertArrayToImage(ByVal arrArray() As QuadrantRegen, ByVal Picture As Bitmap, ByVal arrColor As Color)
        If IsNothing(Picture) = False Then
            Dim PictureRegen As New Bitmap(Picture)
            If IsNothing(arrArray) = False Then
                If IsNothing(arrColor) = False Then
                    For i = 0 To arrArray.Length - 1
                        If IsNothing(arrArray(i).Regen) = False Then
                            For s = 0 To arrArray(i).Regen.Length - 1
                                PictureRegen.SetPixel(arrArray(i).Regen(s).X, arrArray(i).Regen(s).Y, arrColor)
                            Next s
                        End If
                    Next i
                    Return PictureRegen
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Shared Function DeleteArrayFromImage(ByVal arrArray() As QuadrantRegen, ByVal PictureBase As Bitmap, ByVal PictureToEdit As Bitmap, Optional ByVal OnlyOne() As Integer = Nothing)
        If IsNothing(PictureToEdit) = False Then
            Dim PictureOut As New Bitmap(PictureToEdit)
            If IsNothing(OnlyOne) = True Then
                If IsNothing(arrArray) = False Then
                    For i = 0 To arrArray.Length - 1
                        If IsNothing(arrArray(i).Regen) = False Then
                            For s = 0 To arrArray(i).Regen.Length - 1
                                PictureOut.SetPixel(arrArray(i).Regen(s).X, arrArray(i).Regen(s).Y, Color.FromArgb(PictureBase.GetPixel(arrArray(i).Regen(s).X, arrArray(i).Regen(s).Y).ToArgb))
                            Next s
                        End If
                    Next i
                    Return PictureOut
                Else
                    Return Nothing
                End If
            Else
                PictureOut.SetPixel(OnlyOne(0), OnlyOne(1), Color.FromArgb(PictureBase.GetPixel(OnlyOne(0), OnlyOne(1)).ToArgb))
                Return PictureOut
            End If
        Else
            Return Nothing
        End If
    End Function

    Shared Function GetRegenIndexFromXY(ByVal RegenArray() As Regen, ByVal X As Integer, ByVal Y As Integer)
        Dim RegenIndex As Integer = -1
        If IsNothing(RegenArray) = False Then
            For i = 0 To RegenArray.Length - 1
                If RegenArray(i).X = X And RegenArray(i).Y = Y Then
                    Return i
                End If
            Next
        End If

        Return RegenIndex
    End Function


    Shared Function DeleteItemFromRegenArray(ByVal RegenArray() As Regen, ByVal RegenIndex As Integer)
        Dim TempRegen(RegenArray.Length - 2) As Regen
        Dim Counter As Integer = 0
        For i = 0 To RegenArray.Length - 1
            If Not i = RegenIndex Then
                TempRegen(Counter) = RegenArray(i)
                Counter += 1
            End If
        Next i

        ReDim Preserve RegenArray(TempRegen.Length - 1)

        If RegenArray.Length - 1 >= 0 Then
            Array.Copy(TempRegen, RegenArray, RegenArray.Length)
        Else
            RegenArray = Nothing
        End If

        Return RegenArray
    End Function

    Shared Function AddItemToArray(ByVal RegenArray() As Regen, ByVal RegenItem As Regen)
        ReDim Preserve RegenArray(RegenArray.Length)
        RegenArray(RegenArray.Length - 1) = RegenItem
        Return RegenArray
    End Function
End Class