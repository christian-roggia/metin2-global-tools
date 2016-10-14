Imports System.Drawing
Imports GFO_Regen_Helper.LoadRegen
Imports GFO_Regen_Helper.EditRegen

Public Class Common
    Shared Function SwitchColor(ByVal TypeText As String)
        Select Case TypeText
            Case "Regen (Server)"
                Return Color.Red
                Exit Select
            Case "Regen (Client)"
                Return Color.FromArgb(255, 128, 0)
                Exit Select
            Case "MonsterAreaInfo"
                Return Color.Cyan
                Exit Select
            Case "NPC"
                Return Color.Lime
                Exit Select
            Case "Stone"
                Return Color.Silver
                Exit Select
            Case "Boss"
                Return Color.DodgerBlue
                Exit Select
            Case Else
                Return Color.FromArgb(255, 128, 0)
                Exit Select
        End Select
    End Function

    Shared Function SwitchType(ByVal TypeText As String)
        Select Case TypeText
            Case "Mob"
                Return "m"
                Exit Select
            Case "Group"
                Return "g"
                Exit Select
            Case "Regen"
                Return "r"
                Exit Select
            Case Else
                Return "r"
                Exit Select
        End Select
    End Function


    Shared Function RegenDeleteItem(ByVal RegenLoadX As Integer, ByVal RegenLoadY As Integer, ByVal RegenArray() As Regen)
        Dim Localization() As Integer = LocalizeRegen(RegenLoadX, RegenLoadY)
        Dim RegenLocalizate As Integer = (Localization(0) * MapSize.X) + Localization(1)

        If IsNothing(RegenArray) Then
            Return Nothing
            Exit Function
        End If

        If RegenArray.Length - 1 >= 0 Then
            If Not RegenLoadX = -1 Then
                If Not RegenLoadY = -1 Then
                    Dim RegenIndex As Integer = GetRegenIndexFromXY(RegenArray, RegenLoadX, RegenLoadY)
                    RegenArray = DeleteItemFromRegenArray(RegenArray, RegenIndex)
                    Return RegenArray
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

    Shared Function CheckDuplicateRegen(ByVal RegenArray() As Regen, ByVal RegenLoadX As Integer, ByVal RegenLoadY As Integer)
        If MsgBox("There is a duplicate entry for the coordinates " & RegenLoadX & " x " &
                   RegenLoadY & ", do you want to remove all the duplicates?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Do While (GetRegenIndexFromXY(RegenArray, RegenLoadX, RegenLoadY) = Nothing)
                Dim RegenIndex As Integer = GetRegenIndexFromXY(RegenArray, RegenLoadX, RegenLoadY)
                RegenArray = DeleteItemFromRegenArray(RegenArray, RegenIndex)
            Loop
            Return RegenArray
        Else
            Return RegenArray
        End If
    End Function
End Class
