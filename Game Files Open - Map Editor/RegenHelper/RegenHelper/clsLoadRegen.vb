Imports System.Drawing
Imports GFO_Regen_Helper.LoadRegen

Public Class LoadRegen
    Public Structure Regen
        Public Type As String
        Public X As Integer
        Public Y As Integer
        Public SX As Integer
        Public SY As Integer
        Public Z As Integer
        Public Dir As Integer
        Public Time As String
        Public Percent As Integer
        Public Count As Integer
        Public Vnum As Integer
    End Structure

    Public Structure QuadrantRegen
        Public QX As Integer
        Public QY As Integer
        Public ObjectsNumber As Integer
        Public Regen() As Regen
    End Structure

    Public Structure MapSize
        Shared X As Integer
        Shared Y As Integer
    End Structure

    Shared Function LoadImageRegen(ByVal Path As String, ByVal Picture As Image)
        Dim Img As New Bitmap(Picture)
        If My.Computer.FileSystem.FileExists(Path) Then
            Dim RegenText As String = My.Computer.FileSystem.ReadAllText(Path).Replace(ChrW(13) & ChrW(10), ChrW(10))
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
        Dim QuadrantsArray() As QuadrantRegen
        If My.Computer.FileSystem.FileExists(Path) Then
            Dim RegenText As String = My.Computer.FileSystem.ReadAllText(Path).Replace(ChrW(13) & ChrW(10), ChrW(10))
            Dim RegenLines() As String = RegenText.Split(New Char() {ChrW(10)})
            Dim i As Integer = 0

            Dim TempRegenArray(RegenLines.Length - 1) As Regen
            Dim Counter As Integer = 0

            For i = 0 To RegenLines.Length - 1
                If isComment(RegenLines(i), i) = False Then
                    Dim CurLine As String = RegenLines(i).Replace(ChrW(9), " ")
                    Dim SingleRegen() As String = CurLine.Split(New Char() {" "c})
                    If SingleRegen.Length = 11 Then
                        TempRegenArray(Counter).Type = SingleRegen(0)
                        TempRegenArray(Counter).X = Convert.ToInt32(SingleRegen(1))
                        TempRegenArray(Counter).Y = Convert.ToInt32(SingleRegen(2))
                        TempRegenArray(Counter).SX = Convert.ToInt32(SingleRegen(3))
                        TempRegenArray(Counter).SY = Convert.ToInt32(SingleRegen(4))
                        TempRegenArray(Counter).Z = Convert.ToInt32(SingleRegen(5))
                        TempRegenArray(Counter).Dir = Convert.ToInt32(SingleRegen(6))
                        TempRegenArray(Counter).Time = SingleRegen(7)
                        TempRegenArray(Counter).Percent = Convert.ToInt32(SingleRegen(8))
                        TempRegenArray(Counter).Count = Convert.ToInt32(SingleRegen(9))
                        TempRegenArray(Counter).Vnum = Convert.ToInt32(SingleRegen(10))
                        Counter += 1
                    End If
                End If
            Next i
            Dim RegenArray(Counter - 1) As Regen
            Array.Copy(TempRegenArray, RegenArray, Counter)

            QuadrantsArray = SplitArrayInQuadrants(RegenArray)
        Else
            QuadrantsArray = Nothing
        End If
        Return QuadrantsArray
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

    Shared Function SplitArrayInQuadrants(ByVal RegenArray() As Regen)
        'Initialziate the array
        Dim QuadrantArray(MapSize.X * MapSize.Y + 1) As QuadrantRegen
        Dim RegenLocalizate(1) As Integer
        Dim QuadrantLocalize As Integer
        For x = 0 To MapSize.X - 1
            For y = 0 To MapSize.Y - 1
                QuadrantArray(y + (x * MapSize.X)).QX = x
                QuadrantArray(y + (x * MapSize.X)).QY = y
                QuadrantArray(y + (x * MapSize.X)).ObjectsNumber = -1
            Next y
        Next x

        For i = 0 To RegenArray.Length - 1
            If IsNothing(RegenArray(i)) = False Then
                RegenLocalizate = LocalizeRegen(RegenArray(i).X, RegenArray(i).Y)
                QuadrantArray((RegenLocalizate(0) * MapSize.X) + RegenLocalizate(1)).ObjectsNumber += 1
            End If
        Next i

        For s = 0 To QuadrantArray.Length - 1
            Dim TempRegenArray(QuadrantArray(s).ObjectsNumber) As Regen
            QuadrantArray(s).Regen = TempRegenArray
            QuadrantArray(s).ObjectsNumber = 0
        Next s

        For c = 0 To RegenArray.Length - 1
            If IsNothing(RegenArray(c)) = False Then
                RegenLocalizate = LocalizeRegen(RegenArray(c).X, RegenArray(c).Y)
                If IsNothing(RegenLocalizate) = False Then
                    QuadrantLocalize = (RegenLocalizate(0) * MapSize.X) + RegenLocalizate(1)
                    QuadrantArray(QuadrantLocalize).Regen(QuadrantArray(QuadrantLocalize).ObjectsNumber) = RegenArray(c)
                    QuadrantArray(QuadrantLocalize).ObjectsNumber += 1
                End If
            End If
        Next c

        For n = 0 To QuadrantArray.Length - 1
            If IsNothing(QuadrantArray(n)) = False Then
                If QuadrantArray(n).ObjectsNumber = 0 Then
                    QuadrantArray(n).Regen = Nothing
                End If
            End If
        Next n

        Return QuadrantArray
    End Function

    Shared Function LocalizeRegen(ByVal X As Integer, ByVal Y As Integer)
        Dim LocalizationArray(1) As Integer
        If IsNothing(X) = False And IsNothing(Y) = False Then
            LocalizationArray(0) = X \ 256
            LocalizationArray(1) = Y \ 256
            Return LocalizationArray
        Else
            Return Nothing
        End If
    End Function
End Class
