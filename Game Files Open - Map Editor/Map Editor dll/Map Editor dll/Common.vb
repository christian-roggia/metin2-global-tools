Imports System.Drawing

Public Class Common
    Shared Function CheckColor(ByVal MyColor As Integer, ByVal ColorsArray(,) As String)
        Dim ReturnColor As New Color
        ReturnColor = Color.FromArgb(MyColor, MyColor, MyColor)

        For i = 0 To ColorsArray.Length \ 2 - 1
            If MyColor.ToString = ColorsArray(i, 0) Then
                ReturnColor = Color.FromName(ColorsArray(i, 1))
            End If
        Next i

        Return ReturnColor
    End Function
End Class
