using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class NhaCungCap : Form
    {
        DBConnection DbConn = new DBConnection();
        
        public NhaCungCap()
        {
            InitializeComponent();
        }

        private void LoadList()
        {
            try
            {
                lsvDanhSach.Items.Clear();

                DbConn.GetConn();

                string query = @"SELECT * FROM tbl_NCC";
                SqlDataReader reader = DbConn.Reader(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string MaNCC = reader.GetString(0);
                        string TenNCC = reader.GetString(1);
                        string SĐT = reader.GetString(2);
                        string Email = reader.GetString(3);
                        string NgayCapNhat = reader.GetString(4);

                        ListViewItem lvi = new ListViewItem(MaNCC);
                        lvi.SubItems.Add(TenNCC);
                        lvi.SubItems.Add(SĐT);
                        lvi.SubItems.Add(Email);
                        lvi.SubItems.Add(NgayCapNhat);

                        lsvDanhSach.Items.Add(lvi);
                    }
                    reader.Close();
                    DbConn.CloseConn();
                }
                else
                {
                    DbConn.CloseConn();
                    MessageBox.Show("Danh sách đơn nhập không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraTrungMa(string MaNCC)
        {
            DbConn.GetConn();
            string query = $"SELECT * FROM tbl_NCC WHERE manhacungcap = N'{MaNCC}'";
            SqlDataReader reader = DbConn.Reader(query);
            if (reader.HasRows)
            {
                reader.Close();
                DbConn.CloseConn();
                return true;
            }
            DbConn.CloseConn();
            return false;
        }

        private void ResetTextBox()
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                lsvDanhSach.Items.Clear();

                string maNCC = txtTkMaNCC.Text.Trim();
                string tenNCC = txtTkTenNCC.Text.Trim();
                if (maNCC.Trim() == "" && tenNCC.Trim() == "")
                {
                    MessageBox.Show("Mã NCC hoặc Tên NCC không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                    return;
                }
                if (maNCC.Trim() != "" && tenNCC.Trim() != "")
                {
                    MessageBox.Show("Vui lòng tìm theo Mã SP hoặc Tên SP !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                    return;
                }

                if (maNCC != "")
                {
                    DbConn.GetConn();

                    string query = $"SELECT * FROM tbl_NCC WHERE manhacungcap = N'{maNCC}'";
                    SqlDataReader reader = DbConn.Reader(query);
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            string MaNCC = reader.GetString(0);
                            string TenNCC = reader.GetString(1);
                            string SĐT = reader.GetString(2);
                            string Email = reader.GetString(3);
                            string NgayCapNhat = reader.GetString(4);

                            ListViewItem lvi = new ListViewItem(MaNCC);
                            lvi.SubItems.Add(TenNCC);
                            lvi.SubItems.Add(SĐT);
                            lvi.SubItems.Add(Email);
                            lvi.SubItems.Add(NgayCapNhat);

                            lsvDanhSach.Items.Add(lvi);
                            txtTkMaNCC.Text = "";
                        }
                        reader.Close();
                        DbConn.CloseConn();
                    }
                    else
                    {
                        reader.Close();
                        DbConn.CloseConn();
                        MessageBox.Show($"Không tìm thấy Nhà cung cấp có mã {maNCC} !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtTkMaNCC.Text = "";
                        LoadList();
                    }
                }
                else
                {
                    DbConn.GetConn();

                    string query = $"SELECT * FROM tbl_NCC WHERE tennhacungcap LIKE N'%{tenNCC}%'";

                    SqlDataReader reader = DbConn.Reader(query);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string MaNCC = reader.GetString(0);
                            string TenNCC = reader.GetString(1);
                            string SĐT = reader.GetString(2);
                            string Email = reader.GetString(3);
                            string NgayCapNhat = reader.GetString(4);

                            ListViewItem lvi = new ListViewItem(MaNCC);
                            lvi.SubItems.Add(TenNCC);
                            lvi.SubItems.Add(SĐT);
                            lvi.SubItems.Add(Email);
                            lvi.SubItems.Add(NgayCapNhat);

                            lsvDanhSach.Items.Add(lvi);
                        }
                        reader.Close();
                        DbConn.CloseConn();
                        txtTkTenNCC.Text = "";
                    }
                    else
                    {
                        reader.Close();
                        DbConn.CloseConn();
                        MessageBox.Show("Không tìm thấy Nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NhaCungCap_Load(object sender, EventArgs e)
        {
            LoadList();
            Home homeForm = Application.OpenForms["Home"] as Home;
            if (homeForm != null && homeForm.lb_ChucVu.Text != "Quản Lý")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string MaNCC = txtMaNCC.Text.Trim();
                string TenNCC = txtTenNCC.Text.Trim();
                string SĐT = txtSDT.Text.Trim();
                string Email = txtEmail.Text.Trim();
                string NgayCapNhat = DateTime.Now.ToString("hh:mm-dd/MM/yyyy");

                if (MaNCC == "" || TenNCC == "" || SĐT == "" || Email == "" )
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (KiemTraTrungMa(MaNCC) == true)
                {
                    MessageBox.Show("Mã Nhà cung cấp bị trùng!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DbConn.GetConn();
                string query = $"INSERT INTO tbl_NCC (manhacungcap,tennhacungcap,sodienthoai,email,ngaycapnhat)" +
                               $" VALUES (N'{MaNCC}',N'{TenNCC}',N'{SĐT}',N'{Email}',N'{NgayCapNhat}')";
                int check = DbConn.Command(query);
                if (check > 0)
                {
                    MessageBox.Show("Thêm Nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetTextBox();
                    LoadList();
                    DbConn.CloseConn();
                }
                else
                {
                    MessageBox.Show("Thêm Nhà cung cấp KHÔNG thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadList();
                    DbConn.CloseConn();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
