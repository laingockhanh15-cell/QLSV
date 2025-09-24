Public Class frmQLSV
    'Khai bao bien de truy xuat DB tu lop DataBaseAccess
    Private _DBAccess As New DataBaseAccess

    'Khai bao bien trang thai kiem tra du lieu dang Load
    Private _isLoading As Boolean = False

    'Dinh nghia thu tuc load du lieu tu bang Lop vao ComBobox
    Private Sub LoadDataOnComBobox()
        Dim sqlQuery As String = "SELECT ClassID, ClassName FROM dbo.Class"
        Dim dTable As DataTable = _DBAccess.GetDataTable(sqlQuery)
        Me.cmbClass.DataSource = dTable
        Me.cmbClass.ValueMember = "ClassID"
        Me.cmbClass.DisplayMember = "ClassName"
    End Sub

    'Dinh nghia thu tuc load du lieu tu bang Sinh vien theo tung Lop vao Gridview
    Private Sub LoadDataOnGridView(ClassID As String)
        Dim sqlQuery As String = _
            String.Format("SELECT StudentID, StudentName, Phone, Address FROM dbo.Students WHERE ClassID = '{0}'", ClassID)
        Dim dTable As DataTable = _DBAccess.GetDataTable(sqlQuery)
        Me.dgvStudents.DataSource = dTable
        With Me.dgvStudents
            .Columns(0).HeaderText = "Student ID"
            .Columns(1).HeaderText = "Student Name"
            .Columns(2).HeaderText = "Phone"
            .Columns(3).HeaderText = "Address"
            .Columns(3).Width = 200
        End With
    End Sub

    Private Sub frmQLSV_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim d = Date.Now()
        _isLoading = True   'True khi du lieu bat dau load

        LoadDataOnComBobox()
        LoadDataOnGridView(Me.cmbClass.SelectedValue)

        _isLoading = False  'False khi load du lieu xong
    End Sub

    Private Sub cmbClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClass.SelectedIndexChanged
        If Not _isLoading Then  'Neu load du lieu xong
            LoadDataOnGridView(Me.cmbClass.SelectedValue)
        End If
    End Sub

    'Dinh nghia thu tuc hien thi ket qua Search: theo phuong phap tuong tu - Tim kiem tuong tu
    Private Sub SearchStudent(ClassID As String, value As String)
        Dim sqlQuery As String = _
            String.Format("SELECT StudentID, StudentName, Phone, Address FROM dbo.Students WHERE ClassID = '{0}'", ClassID)
        If Me.cmbSearch.SelectedIndex = 0 Then      'Tim theo Student ID
            sqlQuery += String.Format(" AND StudentID LIKE '{0}%'", value)
        ElseIf Me.cmbSearch.SelectedIndex = 1 Then  'Tim theo Student Name
            sqlQuery += String.Format(" AND StudentName LIKE '{0}%'", value)
        End If
        Dim dTable As DataTable = _DBAccess.GetDataTable(sqlQuery)
        Me.dgvStudents.DataSource = dTable
        With Me.dgvStudents
            .Columns(0).HeaderText = "Student ID"
            .Columns(1).HeaderText = "Student Name"
            .Columns(2).HeaderText = "Phone"
            .Columns(3).HeaderText = "Address"
            .Columns(3).Width = 200
        End With
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        SearchStudent(Me.cmbClass.SelectedValue, Me.txtSearch.Text)
    End Sub


    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim frm As New frmStudent(False)
        frm.txtClassID.Text = Me.cmbClass.SelectedValue
        frm.ShowDialog()
        If frm.DialogResult = Windows.Forms.DialogResult.OK Then
            'Load du lieu
            LoadDataOnGridView(Me.cmbClass.SelectedValue)
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim frm As New frmStudent(True)
        frm.txtClassID.Text = Me.cmbClass.SelectedValue
        frm.txtStudentID.ReadOnly = True  'Chi cho doc, truong nay khong cho phep thay doi khi sua du lieu
        With Me.dgvStudents
            frm.txtStudentID.Text = .Rows(.CurrentCell.RowIndex).Cells("StudentID").Value
            frm.txtStudentName.Text = .Rows(.CurrentCell.RowIndex).Cells("StudentName").Value
            frm.txtPhone.Text = .Rows(.CurrentCell.RowIndex).Cells("Phone").Value
            frm.txtAddress.Text = .Rows(.CurrentCell.RowIndex).Cells("Address").Value
        End With
        frm.ShowDialog()
        If frm.DialogResult = Windows.Forms.DialogResult.OK Then 'Sua du lieu thanh cong thi load lai du lieu vao gridview
            LoadDataOnGridView(Me.cmbClass.SelectedValue)
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        'Khai bao bien lay StudentID ma dong can xoa da duoc chon tren gridview
        Dim StudentID As String = Me.dgvStudents.Rows(Me.dgvStudents.CurrentCell.RowIndex).Cells("StudentID").Value

        'Khai bao cau lenh Query de xoa
        Dim sqlQuery As String = String.Format("DELETE Students WHERE StudentID = '{0}'", StudentID)

        'Thuc hien xoa
        If _DBAccess.ExecuteNoneQuery(sqlQuery) Then    'Xoa thanh cong thi thong bao
            MessageBox.Show("Da xoa du lieu thanh cong!")
            'Load lai du lieu tren Gridview
            LoadDataOnGridView(Me.cmbClass.SelectedValue)
        Else
            MessageBox.Show("Loi xoa du lieu!")
        End If

    End Sub
End Class
