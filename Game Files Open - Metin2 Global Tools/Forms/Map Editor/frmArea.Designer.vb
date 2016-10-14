<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmArea
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmArea))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateAreaDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportAreaDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportAreaDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataBasesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportFromDirectoyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewDataBaseFromDirectoryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddItemToCurrentDataBaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddItemToNewDataBaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AreaDataDataBaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToDataBaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportDataBaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ComboQuadrants = New System.Windows.Forms.ToolStripComboBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.DataGridView3 = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column17 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FilesToolStripMenuItem, Me.DataBasesToolStripMenuItem, Me.AreaDataDataBaseToolStripMenuItem, Me.ComboQuadrants})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1366, 27)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FilesToolStripMenuItem
        '
        Me.FilesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CreateAreaDataToolStripMenuItem, Me.ImportAreaDataToolStripMenuItem, Me.ExportAreaDataToolStripMenuItem, Me.ToolStripSeparator1, Me.CloseToolStripMenuItem})
        Me.FilesToolStripMenuItem.Name = "FilesToolStripMenuItem"
        Me.FilesToolStripMenuItem.Size = New System.Drawing.Size(42, 23)
        Me.FilesToolStripMenuItem.Text = "Files"
        '
        'CreateAreaDataToolStripMenuItem
        '
        Me.CreateAreaDataToolStripMenuItem.Enabled = False
        Me.CreateAreaDataToolStripMenuItem.Name = "CreateAreaDataToolStripMenuItem"
        Me.CreateAreaDataToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.CreateAreaDataToolStripMenuItem.Text = "Create Area Data"
        '
        'ImportAreaDataToolStripMenuItem
        '
        Me.ImportAreaDataToolStripMenuItem.Name = "ImportAreaDataToolStripMenuItem"
        Me.ImportAreaDataToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ImportAreaDataToolStripMenuItem.Text = "Import Area Data"
        '
        'ExportAreaDataToolStripMenuItem
        '
        Me.ExportAreaDataToolStripMenuItem.Enabled = False
        Me.ExportAreaDataToolStripMenuItem.Name = "ExportAreaDataToolStripMenuItem"
        Me.ExportAreaDataToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ExportAreaDataToolStripMenuItem.Text = "Export Area Data"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(161, 6)
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.CloseToolStripMenuItem.Text = "Close"
        '
        'DataBasesToolStripMenuItem
        '
        Me.DataBasesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImportFromDirectoyToolStripMenuItem, Me.NewDatabaseToolStripMenuItem, Me.NewDataBaseFromDirectoryToolStripMenuItem, Me.ToolStripSeparator2, Me.AddItemToCurrentDataBaseToolStripMenuItem, Me.AddItemToNewDataBaseToolStripMenuItem})
        Me.DataBasesToolStripMenuItem.Name = "DataBasesToolStripMenuItem"
        Me.DataBasesToolStripMenuItem.Size = New System.Drawing.Size(110, 23)
        Me.DataBasesToolStripMenuItem.Text = "Objects DataBase"
        '
        'ImportFromDirectoyToolStripMenuItem
        '
        Me.ImportFromDirectoyToolStripMenuItem.Name = "ImportFromDirectoyToolStripMenuItem"
        Me.ImportFromDirectoyToolStripMenuItem.Size = New System.Drawing.Size(234, 22)
        Me.ImportFromDirectoyToolStripMenuItem.Text = "Import DataBase"
        '
        'NewDatabaseToolStripMenuItem
        '
        Me.NewDatabaseToolStripMenuItem.Enabled = False
        Me.NewDatabaseToolStripMenuItem.Name = "NewDatabaseToolStripMenuItem"
        Me.NewDatabaseToolStripMenuItem.Size = New System.Drawing.Size(234, 22)
        Me.NewDatabaseToolStripMenuItem.Text = "New DataBase"
        '
        'NewDataBaseFromDirectoryToolStripMenuItem
        '
        Me.NewDataBaseFromDirectoryToolStripMenuItem.Name = "NewDataBaseFromDirectoryToolStripMenuItem"
        Me.NewDataBaseFromDirectoryToolStripMenuItem.Size = New System.Drawing.Size(234, 22)
        Me.NewDataBaseFromDirectoryToolStripMenuItem.Text = "New DataBases from Directory"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(231, 6)
        '
        'AddItemToCurrentDataBaseToolStripMenuItem
        '
        Me.AddItemToCurrentDataBaseToolStripMenuItem.Enabled = False
        Me.AddItemToCurrentDataBaseToolStripMenuItem.Name = "AddItemToCurrentDataBaseToolStripMenuItem"
        Me.AddItemToCurrentDataBaseToolStripMenuItem.Size = New System.Drawing.Size(234, 22)
        Me.AddItemToCurrentDataBaseToolStripMenuItem.Text = "Add Item to Current DataBase"
        '
        'AddItemToNewDataBaseToolStripMenuItem
        '
        Me.AddItemToNewDataBaseToolStripMenuItem.Enabled = False
        Me.AddItemToNewDataBaseToolStripMenuItem.Name = "AddItemToNewDataBaseToolStripMenuItem"
        Me.AddItemToNewDataBaseToolStripMenuItem.Size = New System.Drawing.Size(234, 22)
        Me.AddItemToNewDataBaseToolStripMenuItem.Text = "Add Item to New DataBase"
        '
        'AreaDataDataBaseToolStripMenuItem
        '
        Me.AreaDataDataBaseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportToDataBaseToolStripMenuItem, Me.ImportDataBaseToolStripMenuItem})
        Me.AreaDataDataBaseToolStripMenuItem.Enabled = False
        Me.AreaDataDataBaseToolStripMenuItem.Name = "AreaDataDataBaseToolStripMenuItem"
        Me.AreaDataDataBaseToolStripMenuItem.Size = New System.Drawing.Size(121, 23)
        Me.AreaDataDataBaseToolStripMenuItem.Text = "Area Data DataBase"
        '
        'ExportToDataBaseToolStripMenuItem
        '
        Me.ExportToDataBaseToolStripMenuItem.Enabled = False
        Me.ExportToDataBaseToolStripMenuItem.Name = "ExportToDataBaseToolStripMenuItem"
        Me.ExportToDataBaseToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ExportToDataBaseToolStripMenuItem.Text = "Export DataBase"
        '
        'ImportDataBaseToolStripMenuItem
        '
        Me.ImportDataBaseToolStripMenuItem.Enabled = False
        Me.ImportDataBaseToolStripMenuItem.Name = "ImportDataBaseToolStripMenuItem"
        Me.ImportDataBaseToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ImportDataBaseToolStripMenuItem.Text = "Import DataBase"
        '
        'ComboQuadrants
        '
        Me.ComboQuadrants.Name = "ComboQuadrants"
        Me.ComboQuadrants.Size = New System.Drawing.Size(121, 23)
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToOrderColumns = True
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5})
        Me.DataGridView1.Location = New System.Drawing.Point(530, 30)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(821, 261)
        Me.DataGridView1.TabIndex = 1
        '
        'Column1
        '
        Me.Column1.HeaderText = "Object Pointer"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        Me.Column2.HeaderText = "Building File"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 320
        '
        'Column3
        '
        Me.Column3.HeaderText = "Property Name"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 180
        '
        'Column4
        '
        Me.Column4.HeaderText = "Property Type"
        Me.Column4.Name = "Column4"
        '
        'Column5
        '
        Me.Column5.HeaderText = "Shadow Flag"
        Me.Column5.Name = "Column5"
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToOrderColumns = True
        Me.DataGridView2.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column14, Me.Column10, Me.Column11, Me.Column12, Me.Column13})
        Me.DataGridView2.Location = New System.Drawing.Point(12, 548)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(863, 186)
        Me.DataGridView2.TabIndex = 2
        '
        'Column6
        '
        Me.Column6.HeaderText = "ID"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Width = 40
        '
        'Column7
        '
        Me.Column7.HeaderText = "X"
        Me.Column7.Name = "Column7"
        '
        'Column8
        '
        Me.Column8.HeaderText = "Y"
        Me.Column8.Name = "Column8"
        '
        'Column9
        '
        Me.Column9.HeaderText = "Z"
        Me.Column9.Name = "Column9"
        '
        'Column14
        '
        Me.Column14.HeaderText = "Object Pointer"
        Me.Column14.Name = "Column14"
        Me.Column14.Width = 120
        '
        'Column10
        '
        Me.Column10.HeaderText = "Rotate X"
        Me.Column10.Name = "Column10"
        '
        'Column11
        '
        Me.Column11.HeaderText = "Rotate Y"
        Me.Column11.Name = "Column11"
        '
        'Column12
        '
        Me.Column12.HeaderText = "Rotate Z"
        Me.Column12.Name = "Column12"
        '
        'Column13
        '
        Me.Column13.HeaderText = "Unknown"
        Me.Column13.Name = "Column13"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(12, 30)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(512, 512)
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'BackgroundWorker1
        '
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'DataGridView3
        '
        Me.DataGridView3.AllowUserToAddRows = False
        Me.DataGridView3.AllowUserToDeleteRows = False
        Me.DataGridView3.AllowUserToOrderColumns = True
        Me.DataGridView3.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView3.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.Column15, Me.Column16, Me.Column17})
        Me.DataGridView3.Location = New System.Drawing.Point(530, 297)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.RowHeadersVisible = False
        Me.DataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView3.Size = New System.Drawing.Size(821, 245)
        Me.DataGridView3.TabIndex = 4
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Object Pointer"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Property Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 180
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "Property Type"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        '
        'Column15
        '
        Me.Column15.HeaderText = "Tree File"
        Me.Column15.Name = "Column15"
        Me.Column15.Width = 220
        '
        'Column16
        '
        Me.Column16.HeaderText = "Tree Size"
        Me.Column16.Name = "Column16"
        '
        'Column17
        '
        Me.Column17.HeaderText = "TreeVariance"
        Me.Column17.Name = "Column17"
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Palatino Linotype", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 16
        Me.ListBox1.Location = New System.Drawing.Point(881, 570)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(470, 164)
        Me.ListBox1.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Palatino Linotype", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(878, 551)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "List of logs and errors"
        '
        'frmArea
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1366, 746)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.DataGridView3)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmArea"
        Me.Text = "AreaData Editor v1.0.0.0 Beta"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreateAreaDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportAreaDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportAreaDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataBasesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportFromDirectoyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewDatabaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewDataBaseFromDirectoryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AddItemToCurrentDataBaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddItemToNewDataBaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents AreaDataDataBaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToDataBaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportDataBaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComboQuadrants As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column15 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column16 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column17 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
