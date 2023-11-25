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
    public partial class XuatHang : Form
    {
        DBConnection DbConn = new DBConnection();

        public XuatHang()
        {
            InitializeComponent();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                lsvDanhSach.Items.Clear();

                string maDX = txtTkMaDX.Text.Trim();
                string tenKh = txtTkTenKH.Text.Trim();

                if (maDX.Trim() == "" && tenKh.Trim() == "")
                {
                    MessageBox.Show("Mã ĐX hoặc Tên KH không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                    return;
                }
                if (maDX.Trim() != "" && tenKh.Trim() != "")
                {
                    MessageBox.Show("Vui lòng tìm theo Mã ĐX hoặc Tên KH !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                    return;
                }
                if (maDX != "")
                {
                    DbConn.GetConn();

                    string query = $"SELECT * FROM tbl_DonXuat WHERE madonhang = N'{maDX}'";
                    SqlDataReader reader = DbConn.Reader(query);
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            string MaDX = maDX.Trim();
                            string TenKH = reader.GetString(1);
                            string SĐT = reader.GetString(2);
                            string DiaChi = reader.GetString(3);
                            string LoaiSP = reader.GetString(5);
                            string SoLuongSP = reader.GetString(6);
                            string NgayCapNhat = reader.GetString(4);

                            ListViewItem lvi = new ListViewItem(MaDX);
                            lvi.SubItems.Add(TenKH);
                            lvi.SubItems.Add(LoaiSP);
                            lvi.SubItems.Add(SĐT);
                            lvi.SubItems.Add(DiaChi);
                            lvi.SubItems.Add(SoLuongSP);
                            lvi.SubItems.Add(NgayCapNhat);

                            lsvDanhSach.Items.Add(lvi);
                            txtTkMaDX.Text = "";
                        }
                        reader.Close();
                        DbConn.CloseConn();
                    }
                    else
                    {
                        reader.Close();
                        DbConn.CloseConn();
                        MessageBox.Show("Không tìm thấy đơn xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtTkMaDX.Text = "";
                        LoadList();
                    }
                }
                else
                {
                    DbConn.GetConn();

                    string query = $"SELECT * FROM tbl_DonXuat WHERE tenkhachhang LIKE N'%{tenKh}%'";

                    SqlDataReader reader = DbConn.Reader(query);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string MaDX = reader.GetString(0);
                            string TenKH = reader.GetString(1);
                            string SĐT = reader.GetString(2);
                            string DiaChi = reader.GetString(3);
                            string LoaiSP = reader.GetString(5);
                            string SoLuongSP = reader.GetString(6);
                            string NgayCapNhat = reader.GetString(4);

                            ListViewItem lvi = new ListViewItem(MaDX);
                            lvi.SubItems.Add(TenKH);
                            lvi.SubItems.Add(LoaiSP);
                            lvi.SubItems.Add(SĐT);
                            lvi.SubItems.Add(DiaChi);
                            lvi.SubItems.Add(SoLuongSP);
                            lvi.SubItems.Add(NgayCapNhat);

                            lsvDanhSach.Items.Add(lvi);
                            txtTkTenKH.Text = "";
                        }
                        reader.Close();
                        DbConn.CloseConn();
                    }
                    else
                    {
                        reader.Close();
                        DbConn.CloseConn();
                        MessageBox.Show("Không tìm thấy đơn xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtTkTenKH.Text = "";
                        LoadList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadList()
        {
            try
            {
                lsvDanhSach.Items.Clear();

                DbConn.GetConn();

                string query = @"SELECT * FROM tbl_DonXuat";
                SqlDataReader reader = DbConn.Reader(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string MaDX = reader.GetString(0);
                        string TenKH = reader.GetString(1);
                        string SĐT = reader.GetString(2);
                        string DiaChi = reader.GetString(3);
                        string LoaiSP = reader.GetString(5);
                        string SoLuongSP = reader.GetString(6);
                        string NgayCapNhat = reader.GetString(4);

                        ListViewItem lvi = new ListViewItem(MaDX);
                        lvi.SubItems.Add(TenKH);
                        lvi.SubItems.Add(LoaiSP);
                        lvi.SubItems.Add(SĐT);
                        lvi.SubItems.Add(DiaChi);
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
                    MessageBox.Show("Danh sách đơn xuất không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataToCbo()
        {
            try
            {
                DbConn.GetConn();
                string query = @"SELECT tensanpham FROM tbl_Products";
                DataTable data = DbConn.DataTable(query);
                cboLoaiSP.DataSource = data;
                cboLoaiSP.DisplayMember = "tensanpham";
                DbConn.CloseConn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetTextBox()
        {
            txtMaDX.Text = "";
            txtTenKH.Text = "";
            cboLoaiSP.SelectedIndex = 0;
            txtSDT.Text = "";
            txtAddr.Text = "";
            txtSLSP.Text = "";
        }

        private bool KiemTraTrungMa(string MaDX)
        {
            DbConn.GetConn();
            string query = $"SELECT * FROM tbl_DonXuat WHERE madonhang = N'{MaDX}'";
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

        private void XuatHang_Load(object sender, EventArgs e)
        {
            LoadList();
            LoadDataToCbo();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetTextBox();
        }

        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lsvDanhSach.SelectedItems[0];

                string MaDX = lvi.SubItems[0].Text;
                string TenKH = lvi.SubItems[1].Text;
                string LoaiSP = lvi.SubItems[2].Text;
                string SĐT = lvi.SubItems[3].Text;
                string DiaCHi = lvi.SubItems[4].Text;
                string SLSP = lvi.SubItems[5].Text;
                string NgayCapNhat = DateTime.Now.ToString("hh:mm-dd/MM/yyyy");

                txtMaDX.Text = MaDX;
                txtTenKH.Text = TenKH;
                cboLoaiSP.Text = LoaiSP;
                txtSDT.Text = SĐT;
                txtAddr.Text = DiaCHi;
                txtSLSP.Text = SLSP;

            }
        }

        private void btnXuatDon_Click(object sender, EventArgs e)
        {
            try
            {
                string MaDX = txtMaDX.Text.Trim();
                string TenKH = txtTenKH.Text.Trim();
                string LoaiSP = cboLoaiSP.Text.Trim();
                string SĐT = txtSDT.Text.Trim();
                string DiaChi = txtAddr.Text.Trim();
                string SLSP = txtSLSP.Text.Trim();
                string NgayCapNhat = DateTime.Now.ToString("hh:mm-dd/MM/yyyy");


                if (KiemTraTrungMa(MaDX) == false)
                {
                    DbConn.GetConn();
                    string query = $"INSERT INTO tbl_DonXuat (madonhang,tenkhachhang,sodienthoai,diachi,soluongsanpham,loaisanpham,ngaycapnhat)" +
                                    $" VALUES (N'{MaDX}', N'{TenKH}', N'{SĐT}', N'{DiaChi}', N'{SLSP}', N'{LoaiSP}', N'{NgayCapNhat}') ";
                    int check = DbConn.Command(query);
                    if (check > 0)
                    {
                        MessageBox.Show("Tạo đơn xuất hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DbConn.CloseConn();
                        LoadList();
                    }
                    else
                    {
                        MessageBox.Show("Tạo đơn xuất hàng KHÔNG thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DbConn.CloseConn();
                    }
                }
                else
                {
                    MessageBox.Show("Mã đơn xuất bị trùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
