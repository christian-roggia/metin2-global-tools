Imports Game_Files_Open___Metin2_Global_Tools.frmMapEditor

Public Class frmSettings
    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextGCPath.Text = GCPath
        TextBox1.Text = CellScale
        TextBox2.Text = HeightScale
        TextBox3.Text = ViewRadius
        TextBox4.Text = SMapSize
        TextBox5.Text = BasePosition
        TextBox6.Text = TextureSet
        TextBox7.Text = Environment
    End Sub

    Private Sub CursorCross_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CursorCross.CheckedChanged
        If CursorCross.Checked = True Then
            My.Settings.Cursor = Cursors.Cross
        Else
            My.Settings.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub ShowCoordintes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCoordintes.CheckedChanged
        If ShowCoordintes.Checked = False Then
            My.Settings.Coordinates = False
        Else
            My.Settings.Coordinates = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        CellScale = TextBox1.Text
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        HeightScale = TextBox2.Text
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        ViewRadius = TextBox3.Text
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        SMapSize = TextBox4.Text
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        BasePosition = TextBox5.Text
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged
        TextureSet = TextBox6.Text
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged
        Environment = TextBox7.Text
    End Sub
End Class