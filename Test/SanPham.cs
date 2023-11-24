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
    public partial class SanPham : Form
    {
        DBConnection DbConn = new DBConnection();
        public SanPham()
        {
            InitializeComponent();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                lsvDanhSach.Items.Clear();

                string maSP = txtTkMaSP.Text.Trim();
                string tenSP = txtTkTenSP.Text.Trim();

                if (maSP.Trim() == "" && tenSP.Trim() == "")
                {
                    MessageBox.Show("Mã SP hoặc Tên SP không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                    return;
                }
                if (maSP.Trim() != "" && tenSP.Trim() != "")
                {
                    MessageBox.Show("Vui lòng tìm theo Mã SP hoặc Tên SP !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                    return;
                }
                if (maSP != "")
                {
                    DbConn.GetConn();

                    string query = $"SELECT * FROM tbl_Products WHERE masanpham = '{maSP}'";
                    SqlDataReader reader = DbConn.Reader(query);
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            string MaSP = maSP.Trim();
                            string TenSP = reader.GetString(1);
                            string Hang = reader.GetString(2);
                            string MauSac = reader.GetString(3);
                            string DungLuong = reader.GetString(4);
                            string SoLuong = reader.GetString(5);
                            string GiaBan = reader.GetString(6);
                            string NgayCapNhat = reader.GetString(7);

                            ListViewItem lvi = new ListViewItem(MaSP);
                            lvi.SubItems.Add(TenSP);
                            lvi.SubItems.Add(Hang);
                            lvi.SubItems.Add(MauSac);
                            lvi.SubItems.Add(DungLuong);
                            lvi.SubItems.Add(SoLuong);
                            lvi.SubItems.Add(GiaBan);
                            lvi.SubItems.Add(NgayCapNhat);

                            lsvDanhSach.Items.Add(lvi);
                        }
                        reader.Close();
                        DbConn.CloseConn();
                    }
                    else
                    {
                        reader.Close();
                        DbConn.CloseConn();
                        MessageBox.Show("Không tìm thấy sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadList();
                    }
                }
                else
                {
                    DbConn.GetConn();

                    string query = $"SELECT * FROM tbl_Products WHERE tensanpham LIKE N'%{tenSP}%'";

                    SqlDataReader reader = DbConn.Reader(query);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string MaSP = reader.GetString(0);
                            string TenSP = reader.GetString(1);
                            string Hang = reader.GetString(2);
                            string MauSac = reader.GetString(3);
                            string DungLuong = reader.GetString(4);
                            string SoLuong = reader.GetString(5);
                            string GiaBan = reader.GetString(6);
                            string NgayCapNhat = reader.GetString(7);

                            ListViewItem lvi = new ListViewItem(MaSP);
                            lvi.SubItems.Add(TenSP);
                            lvi.SubItems.Add(Hang);
                            lvi.SubItems.Add(MauSac);
                            lvi.SubItems.Add(DungLuong);
                            lvi.SubItems.Add(SoLuong);
                            lvi.SubItems.Add(GiaBan);
                            lvi.SubItems.Add(NgayCapNhat);

                            lsvDanhSach.Items.Add(lvi);
                        }
                        reader.Close();
                        DbConn.CloseConn();
                    }
                    else
                    {
                        reader.Close();
                        DbConn.CloseConn();
                        MessageBox.Show("Không tìm thấy sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SanPham_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            try
            {
                lsvDanhSach.Items.Clear();

                DbConn.GetConn();

                string query = @"SELECT * FROM tbl_Products";
                SqlDataReader reader = DbConn.Reader(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string MaSP = reader.GetString(0);
                        string TenSP = reader.GetString(1);
                        string Hang = reader.GetString(2);
                        string MauSac = reader.GetString(3);
                        string DungLuong = reader.GetString(4);
                        string SoLuong = reader.GetString(5);
                        string GiaBan = reader.GetString(6);
                        string NgayCapNhat = reader.GetString(7);

                        ListViewItem lvi = new ListViewItem(MaSP);
                        lvi.SubItems.Add(TenSP);
                        lvi.SubItems.Add(Hang);
                        lvi.SubItems.Add(MauSac);
                        lvi.SubItems.Add(DungLuong);
                        lvi.SubItems.Add(SoLuong);
                        lvi.SubItems.Add(GiaBan);
                        lvi.SubItems.Add(NgayCapNhat);

                        lsvDanhSach.Items.Add(lvi);
                    }
                    reader.Close();
                    DbConn.CloseConn();
                }
                else
                {
                    DbConn.CloseConn();
                    MessageBox.Show("Danh sách sản phẩm không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.ToString(),"Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }          
        }

        private bool KiemTraTrungMa(string MaSP)
        {
            DbConn.GetConn();
            string query = $"SELECT * FROM tbl_Products WHERE masanpham = '{MaSP}'";
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
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            txtHang.Text = "";
            txtMauSac.Text = "";
            txtDungLuong.Text = "";
            txtSoLuong.Text = "";
            txtGiaBan.Text = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string MaSP = txtMaSP.Text.Trim();
                string TenSP = txtTenSP.Text.Trim();
                string Hang = txtHang.Text.Trim();
                string MauSac = txtMauSac.Text.Trim();
                string DungLuong = txtDungLuong.Text.Trim();
                string SoLuong = txtSoLuong.Text.Trim();
                string GiaBan = txtGiaBan.Text.Trim();
                string NgayCapNhat = DateTime.Now.ToString("hh:mm-dd/MM/yyyy");

                if (KiemTraTrungMa(MaSP) == true){
                    MessageBox.Show("Mã sản phẩm bị trùng!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DbConn.GetConn();
                string query = $"INSERT INTO tbl_Products (masanpham,tensanpham,thuonghieu,mausac,dungluong,soluong,giaban,ngaycapnhat)" +
                               $" VALUES ('{MaSP}','{TenSP}','{Hang}','{MauSac}','{DungLuong}','{SoLuong}','{GiaBan}','{NgayCapNhat}')";
                int check = DbConn.Command(query);
                if (check > 0)
                {
                    MessageBox.Show("Thêm sản phẩm thành công!","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetTextBox();
                    LoadList();
                    DbConn.CloseConn();
                }
                else
                {
                    MessageBox.Show("Thêm không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadList();
                    DbConn.CloseConn();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.ToString(),"Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetTextBox();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string MaSP = txtMaSP.Text.Trim();

            try
            {
                if(KiemTraTrungMa(MaSP) == true)
                {
                    DialogResult result = MessageBox.Show($"Bạn thật sự muốn xóa sản phẩm có mã {MaSP}", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DbConn.GetConn();
                        string query = $"DELETE FROM tbl_Products WHERE masanpham = '{MaSP}'";
                        int check = DbConn.Command(query);
                        if (check > 0)
                        {
                            MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadList();
                            ResetTextBox();
                            DbConn.CloseConn();
                        }
                        else
                        {
                            MessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadList();
                            DbConn.CloseConn();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mã sản phẩm không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetTextBox();
                    LoadList();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
