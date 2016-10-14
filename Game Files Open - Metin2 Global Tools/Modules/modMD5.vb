Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Module modMD5
    Public Function ChFile_md5(ByVal filepath As String) As Object
        If Not My.Computer.FileSystem.FileExists(filepath) Then
            Return "File inesistente"
        End If
        Dim hash As Byte() = Nothing
        Dim inputStream As New FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, &H2000)
        Dim provider As New MD5CryptoServiceProvider
        provider.ComputeHash(inputStream)
        hash = provider.Hash
        inputStream.Close()
        Dim builder As New StringBuilder(hash.Length)
        Dim num2 As Integer = (hash.Length - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            builder.Append(hash(i).ToString("X2"))
            i += 1
        Loop
        Return builder.ToString.ToUpper
    End Function
End Module
