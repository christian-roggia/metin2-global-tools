Imports System.Runtime.InteropServices

Public Class frmLZO
    <DllImport("Metin2 Pack DLL.dll", CharSet:=CharSet.Unicode)> _
    Public Shared Function DumpArchive() As Boolean
    End Function

    Private Sub frmLZO_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DumpArchive()
    End Sub
End Class