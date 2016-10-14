Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing

Public Class clsRegen
    Shared Function LoadImageRegen(ByVal Path As String, ByVal Picture As Image)
        Dim Img As New Bitmap(Picture)
        If My.Computer.FileSystem.FileExists(Path) Then
            Dim RegenText As String = My.Computer.FileSystem.ReadAllText(Path)
            Dim RegenLines() As String = RegenText.Split(New Char() {ChrW(10)})
            Dim i As Integer = 0
            For i = 0 To RegenLines.Length - 1
                If isComment(RegenLines(i), i) = False Then
                    Dim CurLine As String = RegenLines(i).Replace(ChrW(9), " ")
                    Dim SingleRegen() As String = CurLine.Split(New Char() {" "c})
                    If Not SingleRegen(0) = "" Then
                        Try
                            Img.SetPixel(SingleRegen(1), SingleRegen(2), Color.Red)
                        Catch ex As Exception

                        End Try
                    End If
                End If
            Next i
        End If
        Return Img
    End Function

    Shared Function LoadArrayRegen(ByVal Path As String)
        Dim RegenArray(10) As Array
        If My.Computer.FileSystem.FileExists(Path) Then
            Dim RegenText As String = My.Computer.FileSystem.ReadAllText(Path)
            Dim RegenLines() As String = RegenText.Split(New Char() {ChrW(10)})
            Dim i As Integer = 0

            Dim ArrayType(RegenLines.Length - 1) As String
            Dim ArrayX(RegenLines.Length - 1) As Integer
            Dim ArrayY(RegenLines.Length - 1) As Integer
            Dim ArraySX(RegenLines.Length - 1) As Integer
            Dim ArraySY(RegenLines.Length - 1) As Integer
            Dim ArrayZ(RegenLines.Length - 1) As Integer
            Dim ArrayDir(RegenLines.Length - 1) As Integer
            Dim ArrayTime(RegenLines.Length - 1) As String
            Dim ArrayPercent(RegenLines.Length - 1) As Integer
            Dim ArrayCount(RegenLines.Length - 1) As Integer
            Dim ArrayVnum(RegenLines.Length - 1) As Integer

            For i = 0 To RegenLines.Length - 1
                If isComment(RegenLines(i), i) = False Then
                    Dim CurLine As String = RegenLines(i).Replace(ChrW(9), " ")
                    Dim SingleRegen() As String = CurLine.Split(New Char() {" "c})
                    If Not SingleRegen(0) = "" Then
                        ArrayType(i) = SingleRegen(0)
                        ArrayX(i) = Convert.ToInt32(SingleRegen(1))
                        ArrayY(i) = Convert.ToInt32(SingleRegen(2))
                        ArraySX(i) = Convert.ToInt32(SingleRegen(3))
                        ArraySY(i) = Convert.ToInt32(SingleRegen(4))
                        ArrayZ(i) = Convert.ToInt32(SingleRegen(5))
                        ArrayDir(i) = Convert.ToInt32(SingleRegen(6))
                        ArrayTime(i) = SingleRegen(7)
                        ArrayPercent(i) = Convert.ToInt32(SingleRegen(8))
                        ArrayCount(i) = Convert.ToInt32(SingleRegen(9))
                        ArrayVnum(i) = Convert.ToInt32(SingleRegen(10))
                    End If
                End If
            Next i
            RegenArray(0) = ArrayType
            RegenArray(1) = ArrayX
            RegenArray(2) = ArrayY
            RegenArray(3) = ArraySX
            RegenArray(4) = ArraySY
            RegenArray(5) = ArrayZ
            RegenArray(6) = ArrayDir
            RegenArray(7) = ArrayTime
            RegenArray(8) = ArrayPercent
            RegenArray(9) = ArrayCount
            RegenArray(10) = ArrayVnum
        End If
        Return RegenArray
    End Function

    Shared Function isComment(ByVal PStream As String, ByVal line As Integer)
        If PStream.Length > 2 Then
            If PStream.Substring(0, 2) = "//" Then
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function
End Class
