Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Xml

Public Class frmLogin
    Private TempRegistryKey As String
    Private level1 As String = "HKEY_"
    Private level2 As String = "CLASSES_"
    Private level3 As String = "ROOT\"
    Private RndNumber As Integer

    Function getMD5Hash(ByVal strToHash As String) As String
        Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = md5Obj.ComputeHash(bytesToHash)

        Dim strResult As String = ""

        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2").ToLower
        Next

        Return strResult
    End Function

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If TempRegistryKey = My.Computer.Registry.GetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "SecureKey", String.Empty) Then
            If My.Computer.Registry.GetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "LimitPackets", String.Empty) = String.Empty Then
                frmWait.Parameters = {"Connecting to the Server...", "true"}
                frmWait.FormLoader = Me
                frmWait.Show()
                WaitForfrmWait.Start()
            Else
                Dim baseDate As New DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second)
                Dim TimeToWait As Integer = 300 - (GetTimeStamp(baseDate) - Convert.ToInt32(My.Computer.Registry.GetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "LimitPackets", String.Empty)))
                If TimeToWait < 0 Then
                    My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "IncomingPackets", "0", Microsoft.Win32.RegistryValueKind.String)
                    My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "LimitPackets", String.Empty, Microsoft.Win32.RegistryValueKind.String)
                    frmWait.Show()
                    WaitForfrmWait.Start()
                Else
                    ErrorText.Text = "Login failed 5 times, please wait " & TimeToWait \ 60 & " minutes and retry!"
                End If
            End If
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim MyRnd As New Random
        RndNumber = MyRnd.Next(256, 2147483647)
        Dim MD5String As String = getMD5Hash(RndNumber.ToString)
        TempRegistryKey = MD5String
        My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "SecureKey", MD5String)
        Me.TopLevel = True
    End Sub

    Private Sub WaitForfrmWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WaitForfrmWait.Tick
        If frmWait.Visible = False Then
            Dim client As New WebClient()
            Dim state As String
            Try
                state = client.DownloadString("http://www.gamefilesopen.com/global_tools/login.php?ID=" &
                                                UsernameTextBox.Text & "&PSW=" & getMD5Hash(PasswordTextBox.Text) &
                                                "&TOKEN=" & TempRegistryKey & "&SECURETOKEN=" & (RndNumber - 256).ToString & "&PUBLICTOKEN=" &
                                                getMD5Hash(UsernameTextBox.Text & (RndNumber - 1024).ToString))
            Catch ex As Exception
                ErrorText.Text = "Login failed! Please contact the Game Files Open Staff."
                Exit Sub
            End Try

            If state = "success" Then
                frmSharing.UserName = UsernameTextBox.Text
                frmSharing.Password = getMD5Hash(PasswordTextBox.Text)
                frmSharing.Show(frmMain)
                Me.Close()
            ElseIf state = "failed" Then
                If My.Computer.Registry.GetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "IncomingPackets", String.Empty) = Nothing Then
                    My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "IncomingPackets", "1", Microsoft.Win32.RegistryValueKind.String)
                End If

                Dim baseDate As New DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second)

                If My.Computer.Registry.GetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "LastPacket", String.Empty) = Nothing Then
                    My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "LastPacket", GetTimeStamp(baseDate), Microsoft.Win32.RegistryValueKind.String)
                End If

                Dim TimeToWait As Integer = 300 - (GetTimeStamp(baseDate) - Convert.ToInt32(My.Computer.Registry.GetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "LastPacket", String.Empty)))
                If TimeToWait < 0 Then
                    My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "IncomingPackets", "0", Microsoft.Win32.RegistryValueKind.String)
                    My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "LastPacket", GetTimeStamp(baseDate), Microsoft.Win32.RegistryValueKind.String)
                End If

                Dim Tryed As Integer = My.Computer.Registry.GetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "IncomingPackets", String.Empty)
                If Tryed < 5 Then
                    My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "IncomingPackets", Tryed + 1, Microsoft.Win32.RegistryValueKind.String)
                    ErrorText.Text = "Invalid username or password!"
                Else
                    ErrorText.Text = "Login failed 5 times, please wait 5 minutes and retry!"
                    My.Computer.Registry.SetValue(level1 & level2 & level3 & "WindowsXMLSecureConnection", "LimitPackets", GetTimeStamp(baseDate).ToString, Microsoft.Win32.RegistryValueKind.String)
                End If
                End If
                WaitForfrmWait.Stop()
        End If
    End Sub

    Private Function GetTimeStamp(ByVal inputDate As DateTime) As Int32
        Dim baseDate As New DateTime(1970, 1, 1, 0, 0, 0)
        If baseDate > inputDate Then Return 0
        Return Convert.ToInt32((inputDate - baseDate).TotalSeconds)
    End Function

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://www.gamefilesopen.com/member.php?action=register")
    End Sub
End Class
