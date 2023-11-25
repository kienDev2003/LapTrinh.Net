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
    public partial class NhapHang : Form
    {
        DBConnection DbConn = new DBConnection();

        public NhapHang()
        {
            InitializeComponent();
        }

        private DataTable LoadDataToCbo(string columns,string table)
        {
            try
            {
                DbConn.GetConn();
                string query = $"SELECT {columns} FROM {table}";
                DataTable data = DbConn.DataTable(query);
                DbConn.CloseConn();
                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void ResetTextBox()
        {
            txtMaDN.Text = "";
            cboTenNV.Text = "--Chọn NV--";
            cboLoaiSP.Text = "--Chọn SP--";
            cboTenNCC.Text = "--Chọn NCC--";
            txtSLSP.Text = "";
        }

        private void LoadList()
        {
            try
            {
                lsvDanhSach.Items.Clear();

                DbConn.GetConn();

                string query = @"SELECT * FROM tbl_NhapHang";
                SqlDataReader reader = DbConn.Reader(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string MaDN = reader.GetString(0);
                        string TenNV = reader.GetString(1);
                        string TenNCC = reader.GetString(2);
                        string LoaiSP = reader.GetString(3);
                        string SoLuongSP = reader.GetString(4);
                        string NgayCapNhat = reader.GetString(5);

                        ListViewItem lvi = new ListViewItem(MaDN);
                        lvi.SubItems.Add(TenNV);
                        lvi.SubItems.Add(TenNCC);
                        lvi.SubItems.Add(LoaiSP);
                        lvi.SubItems.Add(SoLuongSP);
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

        private void NhapHang_Load(object sender, EventArgs e)
        {
            Home homeForm = Application.OpenForms["Home"] as Home;
            if (homeForm != null && homeForm.lb_ChucVu.Text != "Quản Lý")
            {
                cboLoaiSP.DropDownStyle = ComboBoxStyle.DropDownList;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
            DataTable dataCboLoaiSP = LoadDataToCbo("tensanpham", "tbl_Products");
            DataTable dataCboNhaCC = LoadDataToCbo("tennhacungcap", "tbl_NCC");
            DataTable dataCboTkTenNV = LoadDataToCbo("tennhanvien", "tbl_Accounts");
            DataTable dataCboTenNV = LoadDataToCbo("tennhanvien", "tbl_Accounts");

            cboLoaiSP.DataSource = dataCboLoaiSP;
            cboLoaiSP.DisplayMember = "tensanpham";
            
            cboTenNCC.DataSource = dataCboNhaCC;
            cboTenNCC.DisplayMember = "tennhacungcap";

            cboTkTenNV.DataSource = dataCboTkTenNV;
            cboTkTenNV.DisplayMember = "tennhanvien";
            
            cboTenNV.DataSource = dataCboTenNV;
            cboTenNV.DisplayMember = "tennhanvien";

            cboTkTenNV.Text = "--Chọn Tên NV--";
            cboTenNV.Text = "--Chọn Tên NV--";
            cboTenNCC.Text = "--Chọn Tên NV--";
            cboLoaiSP.Text = "--Chọn SP--";

            LoadList();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                lsvDanhSach.Items.Clear();

                string maDN = txtTkMaDN.Text.Trim();
                string tenNV = cboTkTenNV.Text.Trim();

                if (maDN == "" && tenNV == "--Chọn Tên NV--")
                {
                    MessageBox.Show("Mã ĐN hoặc Tên KH không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                    return;
                }
                if (maDN != "" && tenNV != "--Chọn Tên NV--")
                {
                    MessageBox.Show("Vui lòng tìm theo Mã ĐX hoặc Tên KH !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                    return;
                }
                if (maDN != "")
                {
                    DbConn.GetConn();

                    string query = $"SELECT * FROM tbl_NhapHang WHERE madonhang = N'{maDN}'";
                    SqlDataReader reader = DbConn.Reader(query);
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            string MaDN = maDN.Trim();
                            string TenNV = reader.GetString(1);
                            string TenNCC = reader.GetString(2);
                            string LoaiSP = reader.GetString(3);
                            string SoLuongSP = reader.GetString(4);
                            string NgayCapNhat = reader.GetString(5);

                            ListViewItem lvi = new ListViewItem(MaDN);
                            lvi.SubItems.Add(TenNV);
                            lvi.SubItems.Add(TenNCC);
                            lvi.SubItems.Add(LoaiSP);
                            lvi.SubItems.Add(SoLuongSP);
                            lvi.SubItems.Add(NgayCapNhat);

                            lsvDanhSach.Items.Add(lvi);
                            txtTkMaDN.Text = "";
                        }
                        reader.Close();
                        DbConn.CloseConn();
                    }
                    else
                    {
                        reader.Close();
                        DbConn.CloseConn();
                        MessageBox.Show("Không tìm thấy đơn nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtTkMaDN.Text = "";
                        LoadList();
                    }
                }
                else
                {
                    DbConn.GetConn();

                    string query = $"SELECT * FROM tbl_NhapHang WHERE tennhanvien LIKE N'%{tenNV}%'";

                    SqlDataReader reader = DbConn.Reader(query);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string MaDN = reader.GetString(0);
                            string TenNV = reader.GetString(1);
                            string TenNCC = reader.GetString(2);
                            string LoaiSP = reader.GetString(3);
                            string SoLuongSP = reader.GetString(4);
                            string NgayCapNhat = reader.GetString(5);

                            ListViewItem lvi = new ListViewItem(MaDN);
                            lvi.SubItems.Add(TenNV);
                            lvi.SubItems.Add(TenNCC);
                            lvi.SubItems.Add(LoaiSP);
                            lvi.SubItems.Add(SoLuongSP);
                            lvi.SubItems.Add(NgayCapNhat);

                            lsvDanhSach.Items.Add(lvi);
                            cboTkTenNV.Text = "--Chọn Tên NV--";
                        }
                        reader.Close();
                        DbConn.CloseConn();
                    }
                    else
                    {
                        reader.Close();
                        DbConn.CloseConn();
                        MessageBox.Show("Không tìm thấy đơn xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cboTkTenNV.Text = "--Chọn Tên NV--";
                        LoadList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
