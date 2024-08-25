<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Button1 = New Button()
        TextBox1 = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        TextBox2 = New TextBox()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(122, 90)
        Button1.Name = "Button1"
        Button1.RightToLeft = RightToLeft.Yes
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 0
        Button1.Text = "submit"
        Button1.UseVisualStyleBackColor = True
        AddHandler Button1.Click, AddressOf Button1_Click
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(12, 38)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(100, 23)
        TextBox1.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(38, 20)
        Label1.Name = "Label1"
        Label1.Size = New Size(37, 15)
        Label1.TabIndex = 2
        Label1.Text = "name"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(244, 20)
        Label2.Name = "Label2"
        Label2.Size = New Size(33, 15)
        Label2.TabIndex = 3
        Label2.Text = "price"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(204, 38)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(100, 23)
        TextBox2.TabIndex = 4
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(326, 125)
        Controls.Add(TextBox2)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(TextBox1)
        Controls.Add(Button1)
        Name = "Form1"
        Text = "SONIC"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ' Get the text from the textboxes
        Dim name As String = TextBox1.Text
        Dim price As String = TextBox2.Text

        ' Specify the folder and file path
        Dim folderPath As String = "C:\SONIC-OUT"
        Dim filePath As String = System.IO.Path.Combine(folderPath, "OUTPUT.txt")

        ' Check if the folder exists, if not, create it
        If Not System.IO.Directory.Exists(folderPath) Then
            System.IO.Directory.CreateDirectory(folderPath)
        End If

        ' Combine the name and price into one line
        Dim line As String = name & " - " & price

        ' Write the line to the file
        ' If the file doesn't exist, it will be created. If it exists, the line will be appended.
        System.IO.File.AppendAllText(filePath, line & Environment.NewLine)

        ' Optional: Clear the textboxes after submitting
        TextBox1.Clear()
        TextBox2.Clear()
    End Sub

End Class
