Imports GFO_Map_Helper.LoadImage
Imports GFO_Regen_Helper.EditRegen
Imports System.Math
Imports System.IO

Module modAreaData
    Public Structure MyObject
        Public X As String
        Public Y As String
        Public Z As String
        Public ID As String
        Public RotateX As String
        Public RotateY As String
        Public RotateZ As String
        Public Unknown As String
    End Structure

    Public Structure PRB
        Public PTR As Double
        Public BuildingFile As String
        Public PropertyName As String
        Public PropertyType As String
        Public ShadowFlag As Integer
    End Structure

    Public Structure PRT
        Public PTR As Double
        Public PropertyName As String
        Public PropertyType As String
        Public TreeFile As String
        Public TreeSize As String
        Public TreeVariance As String
    End Structure

    Public DirectoryPath As String
    Public SaveYPRTPath As String

    Public Function FindObjectCount(ByVal MyDataLines() As String) As Integer
        For s = 0 To MyDataLines.Length - 1
            If MyDataLines(s).ToLower.Contains("objectcount") = True Then
                Dim ObjectCountString As String = MyDataLines(s).Replace(" ", "").ToLower.Replace("objectcount", "")

                If IsNumeric(ObjectCountString) = True And ObjectCountString.Contains(".") = False Then
                    Return Convert.ToInt32(ObjectCountString)
                End If
            End If
        Next s

        Return 0
    End Function

    Public Function SplitToObjectArray(ByVal MyDataLines() As String) As String()
        Dim IsObject As Boolean = False
        Dim LoadedObjectArray(0) As String

        For i = 0 To MyDataLines.Length - 1
            If MyDataLines(i).ToLower.Contains("start") Then
                IsObject = True
            ElseIf MyDataLines(i).ToLower.Replace(" ", Nothing).Contains("endobject") Then
                IsObject = False
            ElseIf IsObject = True Then
                Dim ObjectArrayLength As Integer = LoadedObjectArray.Length
                ReDim Preserve LoadedObjectArray(ObjectArrayLength)
                LoadedObjectArray(ObjectArrayLength - 1) = MyDataLines(i)
            End If
        Next i

        Return LoadedObjectArray
    End Function

    Public Function ConvertObjectArrayToMyObject(ByVal ObjectCount As Integer, ByVal LoadedObjectArray() As String, ByVal ObjectArray() As MyObject)
        For i = 0 To ObjectCount - 1
            With ObjectArray(i)
                Dim XYZ() As String = LoadedObjectArray(i * 4).Split(" ")
                If XYZ.Length - 1 >= 6 And IsNumeric(XYZ(4)) And IsNumeric(XYZ(5)) And IsNumeric(XYZ(6)) Then
                    .X = XYZ(4)
                    .Y = XYZ(5)
                    .Z = XYZ(6)
                Else
                    .X = "Invalid X, Object: " & i.ToString
                    .Y = "Invalid Y, Object: " & i.ToString
                    .Z = "Invalid Z, Object: " & i.ToString
                End If

                If IsNumeric(LoadedObjectArray(i * 4 + 1).Replace(" ", "")) Then
                    .ID = LoadedObjectArray(i * 4 + 1).Replace(" ", "")
                Else
                    .ID = "Invalid ID, Object: " & i.ToString
                End If

                Dim RotateXYZ() As String = LoadedObjectArray(i * 4 + 2).Replace(" ", "").Split("#")

                If RotateXYZ.Length - 1 >= 2 And IsNumeric(RotateXYZ(0)) And IsNumeric(RotateXYZ(1)) And IsNumeric(RotateXYZ(2)) Then
                    .RotateX = RotateXYZ(0)
                    .RotateY = RotateXYZ(1)
                    .RotateZ = RotateXYZ(2)
                Else
                    .RotateX = "Invalid Rotate X, Object: " & i.ToString
                    .RotateY = "Invalid Rotate X, Object: " & i.ToString
                    .RotateZ = "Invalid Rotate X, Object: " & i.ToString
                End If

                If IsNumeric(LoadedObjectArray(i * 4 + 3).Replace(" ", "")) Then
                    .Unknown = LoadedObjectArray(i * 4 + 3).Replace(" ", "")
                Else
                    .Unknown = "Invalid Unknownw Value, Object: " & i.ToString
                End If
            End With
        Next i

        Return ObjectArray
    End Function

    Public Function LoadDoubleAttribute(ByVal AreaPath As String) As Bitmap
        If My.Computer.FileSystem.FileExists(AreaPath) = False Then
            Return New Bitmap(512, 512)
            Exit Function
        End If

        Dim SpecialColors(,) As String = {{1, Color.DarkRed.Name}, {2, Color.Black.Name}, {3, Color.Red.Name},
                                         {4, Color.Green.Name}, {5, Color.Blue.Name}, {6, Color.DodgerBlue.Name},
                                         {192, Color.Black.Name}, {201, Color.DarkRed.Name}, {195, Color.DarkRed.Name},
                                         {200, Color.DarkRed.Name}, {205, Color.DarkRed.Name}, {193, Color.DarkRed.Name}}

        Dim MyAttribute As Bitmap = LoadATR(AreaPath, SpecialColors)
        Dim DoubleAttribute As New Bitmap(MyAttribute.Width * 2, MyAttribute.Height * 2)

        Dim x, y As Integer

        For x = 0 To MyAttribute.Width * 2 - 1 Step 2
            For y = 0 To MyAttribute.Height * 2 - 1 Step 2
                DoubleAttribute.SetPixel(x, y, MyAttribute.GetPixel(x \ 2, y \ 2))
                DoubleAttribute.SetPixel(x, y + 1, MyAttribute.GetPixel(x \ 2, y \ 2))
                DoubleAttribute.SetPixel(x + 1, y, MyAttribute.GetPixel(x \ 2, y \ 2))
                DoubleAttribute.SetPixel(x + 1, y + 1, MyAttribute.GetPixel(x \ 2, y \ 2))
            Next y

            y = 0
        Next x

        Return DoubleAttribute
    End Function

    Public Function LoadPRBDataBase(ByRef MyDataGridView As DataGridView, ByVal MyDataBasePath As String, ByRef PRBArray() As PRB) As DataGridView
        If My.Computer.FileSystem.FileExists(MyDataBasePath) = False Then
            Return MyDataGridView
            Exit Function
        End If

        Dim database As String = My.Computer.FileSystem.ReadAllText(MyDataBasePath)
        Dim MyPRBArray(0) As PRB

        Dim x As New Xml.XmlDocument()
        x.LoadXml(database)
        Dim MyYPRT As Xml.XmlNodeList = x.GetElementsByTagName("PRB")

        MyDataGridView.Rows.Clear()

        If MyYPRT.Count - 1 = 0 Then
            Return MyDataGridView
            Exit Function
        End If

        For i = 0 To MyYPRT.Count - 1
            ReDim Preserve MyPRBArray(i)
            With MyPRBArray(i)
                .PTR = Convert.ToDouble(MyYPRT.Item(i).Attributes("ptr").Value)
                .BuildingFile = MyYPRT.Item(i).Attributes("buildingfile").Value.ToString
                .PropertyName = MyYPRT.Item(i).Attributes("propertyname").Value.ToString
                .PropertyType = MyYPRT.Item(i).Attributes("propertytype").Value.ToString
                .ShadowFlag = Convert.ToInt32(MyYPRT.Item(i).Attributes("shadowflag").Value)
            End With
        Next i

        MyDataGridView.Rows.Clear()

        For i = 0 To MyPRBArray.Length - 1
            With MyPRBArray(i)
                MyDataGridView.Rows.Add(.PTR, .BuildingFile, .PropertyName, .PropertyType, .ShadowFlag)
            End With
        Next i

        PRBArray = MyPRBArray

        Return MyDataGridView
    End Function

    Public Function LoadPRTDataBase(ByRef MyDataGridView As DataGridView, ByVal MyDataBasePath As String, ByRef PRTArray() As PRT) As DataGridView
        If My.Computer.FileSystem.FileExists(MyDataBasePath) = False Then
            Return MyDataGridView
            Exit Function
        End If

        Dim database As String = My.Computer.FileSystem.ReadAllText(MyDataBasePath)
        Dim MyPRTArray(0) As PRT

        Dim x As New Xml.XmlDocument()
        x.LoadXml(database)
        Dim MyYPRT As Xml.XmlNodeList = x.GetElementsByTagName("PRT")

        MyDataGridView.Rows.Clear()

        If MyYPRT.Count - 1 = 0 Then
            Return MyDataGridView
            Exit Function
        End If

        For i = 0 To MyYPRT.Count - 1
            ReDim Preserve MyPRTArray(i)
            With MyPRTArray(i)
                .PTR = Convert.ToDouble(MyYPRT.Item(i).Attributes("ptr").Value)
                .PropertyName = MyYPRT.Item(i).Attributes("propertyname").Value.ToString
                .PropertyType = MyYPRT.Item(i).Attributes("propertytype").Value.ToString
                .TreeFile = MyYPRT.Item(i).Attributes("treefile").Value.ToString
                .TreeSize = MyYPRT.Item(i).Attributes("treesize").Value.ToString
                .TreeVariance = MyYPRT.Item(i).Attributes("treevariance").Value.ToString
            End With
        Next i

        MyDataGridView.Rows.Clear()

        For i = 0 To MyPRTArray.Length - 1
            With MyPRTArray(i)
                MyDataGridView.Rows.Add(.PTR, .PropertyName, .PropertyType, .TreeFile, .TreeSize, .TreeVariance)
            End With
        Next i

        PRTArray = MyPRTArray

        Return MyDataGridView
    End Function

    Public Function SetObjectsPoints(ByVal DoubleAttribute As Bitmap, ByVal MyObjects() As MyObject, ByVal MapSize() As Integer) As Boolean
        If IsNothing(DoubleAttribute) = True Then
            Return False
        End If

        If IsNothing(MyObjects) = True Then
            Return False
        End If

        If MyObjects.Length - 1 = 0 Then
            Return False
        End If

        For i = 0 To MyObjects.Length - 1
            Dim X As Double = Abs((Convert.ToInt32(MyObjects(i).X.Remove(MyObjects(i).X.LastIndexOf("."))) \ 100) * 2 - (MapSize(0) * 512))
            Dim Y As Double = Abs(Abs(Convert.ToInt32(MyObjects(i).Y.Remove(MyObjects(i).Y.LastIndexOf("."))) \ 100) * 2 - (MapSize(1) * 512))

            If X < 512 Or Y < 512 Then
                DoubleAttribute.SetPixel(X, Y, Color.White)
            End If
        Next i

        Return True
    End Function

    Public Function FindMapSize(ByVal SettingsPath As String) As Integer()
        Dim MySettings() As String = My.Computer.FileSystem.ReadAllText(SettingsPath).Replace(Chr(10), Chr(13)).Split(Chr(13))
        Dim MapSize() As String

        For i = 0 To MySettings.Length - 1
            If MySettings(i).ToLower.Contains("mapsize") = True Then
                MapSize = MySettings(i).ToLower.Replace("mapsize", "").Replace(" ", "").Split(Chr(9))
            End If
        Next i

        If IsNothing(MapSize) = False And IsNumeric(MapSize(1)) And IsNumeric(MapSize(2)) Then
            Return {Convert.ToInt32(MapSize(1)), Convert.ToInt32(MapSize(2))}
        Else
            Return {0, 0}
        End If
    End Function

    Public Function FromMapSizeToComboBox(ByVal MapSize() As Integer, ByRef MyCombo As ToolStripComboBox) As Boolean
        If IsNothing(MapSize) = True Or MapSize(0) = 0 Or MapSize(1) = 0 Then
            Return False
        End If

        MyCombo.Text = "000000"

        For x = 0 To MapSize(0) - 1
            For y = 0 To MapSize(1) - 1
                MyCombo.Items.Add("00" & x & "00" & y)
            Next y
        Next x

        Return True
    End Function

    Public Function LoadAreaData(ByVal AreaDataPath As String, ByRef ObjectArray() As MyObject) As Boolean
        Dim MyAreaData As String = My.Computer.FileSystem.ReadAllText(AreaDataPath)
        MyAreaData = MyAreaData.Replace(Chr(10), "").Replace(Chr(13) & Chr(13), Chr(13))
        Dim MyDataLines() As String = MyAreaData.Split(Chr(13))
        Dim ObjectCount As Integer = 0

        If MyDataLines.Length - 1 < 9 Then
            Return False
            Exit Function
        End If

        If Not MyDataLines(0).ToLower.Replace(" ", "") = "areadatafile" Then
            Return False
            Exit Function
        End If

        ObjectCount = FindObjectCount(MyDataLines)

        If ObjectCount = 0 Then
            Return False
            Exit Function
        End If

        ReDim Preserve ObjectArray(ObjectCount - 1)

        ObjectArray = ConvertObjectArrayToMyObject(ObjectCount, SplitToObjectArray(MyDataLines), ObjectArray)

        Return True
    End Function

    Public Sub ProcessDir(ByVal Dir As String)
        Dim fileEntries() As String = Directory.GetFiles(Dir)
        For Each fileName As String In fileEntries
            ProcessFile(fileName)
        Next

        Dim subdirectoryEntries As String() = Directory.GetDirectories(Dir)
        For Each subdirectory As String In subdirectoryEntries
            ProcessDir(subdirectory)
        Next
    End Sub

    Public Sub ProcessFile(ByVal FilePath As String)
        Dim FileExtension = FilePath.Substring(FilePath.LastIndexOf("."))

        If FileExtension = ".prb" Then
            LoadPRBFiles(FilePath)
        ElseIf FileExtension = ".prt" Then
            LoadPRTFiles(FilePath)
        End If
    End Sub

    Public Sub LoadPRBFiles(ByVal PRBPath As String)
        Dim MyPRB As String = My.Computer.FileSystem.ReadAllText(PRBPath)
        Dim MyPRBLines() As String = MyPRB.Replace(Chr(10), Chr(13)).Split(Chr(13))
        Dim MyValue(5) As String

        If Not MyPRBLines(0).ToUpper = "YPRT" Then
            Exit Sub
        End If

        MyValue(0) = ".." & PRBPath.Replace(DirectoryPath, "")

        For i = 0 To MyPRBLines.Length - 1
            Select Case MyPRBLines(i).Split(Chr(9))(0).ToLower
                Case "buildingfile"
                    MyValue(2) = MyPRBLines(i).Replace(Chr(9), "").Replace("buildingfile", "").Replace(Chr(34), "")
                Case "propertyname"
                    MyValue(3) = MyPRBLines(i).Replace(Chr(9), "").Replace("propertyname", "").Replace(Chr(34), "")
                Case "propertytype"
                    MyValue(4) = MyPRBLines(i).Replace(Chr(9), "").Replace("propertytype", "").Replace(Chr(34), "")
                Case "shadowflag"
                    MyValue(5) = MyPRBLines(i).Replace(Chr(9), "").Replace("shadowflag", "").Replace(Chr(34), "")
                Case Else
                    If IsNumeric(MyPRBLines(i).Split(Chr(9))(0)) = True Then
                        MyValue(1) = MyPRBLines(i).Replace(Chr(9), "")
                    End If
            End Select
        Next i

        My.Computer.FileSystem.WriteAllText(SaveYPRTPath, Chr(9) & "<PRB path=""" & MyValue(0) & """ ptr=""" & MyValue(1) &
                                             """ buildingfile=""" & MyValue(2) & """ propertyname=""" & MyValue(3) &
                                             """ propertytype=""" & MyValue(4) & """ shadowflag=""" & MyValue(5) & """ />" & vbCrLf, True)
    End Sub

    Public Sub LoadPRTFiles(ByVal PRTPath As String)
        Dim MyPRT As String = My.Computer.FileSystem.ReadAllText(PRTPath)
        Dim MyPRTLines() As String = MyPRT.Replace(Chr(10), Chr(13)).Split(Chr(13))
        Dim MyValue(6) As String

        If Not MyPRTLines(0).ToUpper = "YPRT" Then
            Exit Sub
        End If

        MyValue(0) = ".." & PRTPath.Replace(DirectoryPath, "")

        For i = 0 To MyPRTLines.Length - 1
            Select Case MyPRTLines(i).Split(Chr(9))(0).ToLower
                Case "propertyname"
                    MyValue(2) = MyPRTLines(i).Replace(Chr(9), "").Replace("propertyname", "").Replace(Chr(34), "")
                Case "propertytype"
                    MyValue(3) = MyPRTLines(i).Replace(Chr(9), "").Replace("propertytype", "").Replace(Chr(34), "")
                Case "treefile"
                    MyValue(4) = MyPRTLines(i).Replace(Chr(9), "").Replace("treefile", "").Replace(Chr(34), "")
                Case "treesize"
                    MyValue(5) = MyPRTLines(i).Replace(Chr(9), "").Replace("treesize", "").Replace(Chr(34), "")
                Case "treevariance"
                    MyValue(6) = MyPRTLines(i).Replace(Chr(9), "").Replace("treevariance", "").Replace(Chr(34), "")
                Case Else
                    If IsNumeric(MyPRTLines(i).Split(Chr(9))(0)) = True Then
                        MyValue(1) = MyPRTLines(i).Replace(Chr(9), "")
                    End If
            End Select
        Next i

        My.Computer.FileSystem.WriteAllText(SaveYPRTPath, Chr(9) & "<PRT path=""" & MyValue(0) & """ ptr=""" & MyValue(1) &
                                             """ propertyname=""" & MyValue(2) & """ propertytype=""" & MyValue(3) &
                                             """ treefile=""" & MyValue(4) & """ treesize=""" & MyValue(5) &
                                             """ treevariance=""" & MyValue(6) & """ />" & vbCrLf, True)
    End Sub
End Module
