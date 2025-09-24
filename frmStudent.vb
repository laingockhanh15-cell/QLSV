Public Class frmStudent
    'Khai bao bien truy xuat DB tu lop DBAcccess
    Private _DBAccess As New DataBaseAccess

    'Khai bao bien de biet trang thai dang la Edit hay Insert
    Private _isEdit As Boolean = False
    Public Sub New(IsEdit As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _isEdit = IsEdit
    End Sub

    'Dinh nghia Ham them ban ghi Sinh vien vao database
    Private Function InsertStudent() As Boolean
        Dim sqlQuery As String = "INSERT INTO Students (StudentID, StudentName, Phone, Address, ClassID) "
        sqlQuery += String.Format("VALUES ('{0}','{1}','{2}','{3}','{4}')", _
                                  txtStudentID.Text, txtStudentName.Text, txtPhone.Text, txtAddress.Text, txtClassID.Text)
        Return _DBAccess.ExecuteNoneQuery(sqlQuery)
    End Function

    'Dinh nghia Ham Update 
    Private Function UpdateStudent() As Boolean
        Dim sqlQuery As String = String.Format("UPDATE Students SET StudentName = '{0}', Phone ='{1}', Address ='{2}' WHERE StudentID = '{3}'", _
                                               Me.txtStudentName.Text, Me.txtPhone.Text, Me.txtAddress.Text, Me.txtStudentID.Text)
        Return _DBAccess.ExecuteNoneQuery(sqlQuery)
    End Function

    'Dinh nghia ham kiem tra gia tri truoc khi insert du lieu vao database
    Private Function IsEmpty() As Boolean
        Return (String.IsNullOrEmpty(txtStudentID.Text) OrElse String.IsNullOrEmpty(txtStudentName.Text) OrElse _
                String.IsNullOrEmpty(txtPhone.Text) OrElse String.IsNullOrEmpty(txtAddress.Text) OrElse _
                String.IsNullOrEmpty(txtClassID.Text))
    End Function

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If IsEmpty() Then   'Kiem tra truong du lieu truoc khi thuc hien THEM, SUA
            MessageBox.Show("Hay nhap gia tri vao truoc khi ghi vao database", "Error", MessageBoxButtons.OK)
        Else
            If _isEdit Then     'Neu la Edit thi goi ham Update
                If UpdateStudent() Then 'Neu Update thanh cong thi thong bao
                    MessageBox.Show("Sua du lieu thanh cong!", "Infomation", MessageBoxButtons.OK)
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                Else        'Neu co loi khi sua thi thong bao loi
                    MessageBox.Show("Loi sua du lieu", "Error", MessageBoxButtons.OK)
                    Me.DialogResult = Windows.Forms.DialogResult.No
                End If
            Else                'Neu khong phai Edit thi goi ham Insert
                If InsertStudent() Then 'Neu insert thanh cong thi thong bao
                    MessageBox.Show("Them du lieu thanh cong!", "Infomation", MessageBoxButtons.OK)
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                Else
                    MessageBox.Show("Loi Them du lieu!", "Error", MessageBoxButtons.OK)
                    Me.DialogResult = Windows.Forms.DialogResult.No
                End If
            End If

            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub


 
End Class