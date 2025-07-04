﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginRegistrationForm.Forms
{
    public partial class FormThemGiaoDich : Form
    {
        //id ví tiền chưa chọn gì trong bảng là -1
        int id = -1;
        public FormThemGiaoDich()
        {
            InitializeComponent();
        }

        private void LoadGiaoDichTheoNguoiDung(int maTaiKhoan)
        {
            using (var db = new DataClassesDBDataContext())
            {
                string loaiLoc = null;
                if (rbLocThu.Checked) loaiLoc = "Thu";
                else if (rbLocChi.Checked) loaiLoc = "Chi";
                // nếu rbLocTatCa thì giữ null

                var danhSachGiaoDich = from gd in db.GIAO_DICHes
                                       join vi in db.VI_TIENs on gd.Ma_Vi equals vi.Ma_Vi
                                       join dm in db.DANH_MUCs on gd.Ma_Danh_Muc equals dm.Ma_Danh_Muc
                                       where vi.Ma_Tai_Khoan == maTaiKhoan
                                       && (loaiLoc == null || dm.Loai_Thu_Chi == loaiLoc)
                                       select new
                                       {
                                           MaGiaoDich = gd.Ma_Giao_Dich,
                                           TenVi = vi.Ten_Vi,
                                           DanhMuc = dm.Ten_Danh_Muc,
                                           Loai = dm.Loai_Thu_Chi,
                                           SoTien = gd.So_Tien,
                                           NgayGiaoDich = gd.Ngay_Giao_Dich,
                                           TinhTrang = gd.Tinh_Trang == true ? "Hoàn thành" : "Chưa hoàn thành"
                                       };

                var dt = new DataTable();
                dt.Columns.Add("MaGiaoDich", typeof(int));
                dt.Columns.Add("TenVi", typeof(string));
                dt.Columns.Add("DanhMuc", typeof(string));
                dt.Columns.Add("SoTien", typeof(decimal));
                dt.Columns.Add("NgayGiaoDich", typeof(DateTime));
                dt.Columns.Add("TinhTrang", typeof(string));

                foreach (var gd in danhSachGiaoDich)
                {
                    dt.Rows.Add(gd.MaGiaoDich, gd.TenVi, gd.DanhMuc, gd.SoTien, gd.NgayGiaoDich, gd.TinhTrang);
                }

                dgvGiaoDich.DataSource = dt.DefaultView; // <-- DefaultView cho phép sắp xếp
                // Tắt dòng * ở cuối
                dgvGiaoDich.AllowUserToAddRows = false;
                foreach (DataGridViewColumn column in dgvGiaoDich.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
        }

        private void TimKiem(int maTaiKhoan, string timkiem)
        {
            using (var db = new DataClassesDBDataContext())
            {
                string loaiLoc = null;
                if (rbLocThu.Checked) loaiLoc = "Thu";
                else if (rbLocChi.Checked) loaiLoc = "Chi";
                // nếu rbLocTatCa thì giữ null

                var danhSachGiaoDich = from gd in db.GIAO_DICHes
                                       join vi in db.VI_TIENs on gd.Ma_Vi equals vi.Ma_Vi
                                       join dm in db.DANH_MUCs on gd.Ma_Danh_Muc equals dm.Ma_Danh_Muc
                                       where vi.Ma_Tai_Khoan == maTaiKhoan && (vi.Ten_Vi.Contains(timkiem) || gd.DANH_MUC.Ten_Danh_Muc.Contains(timkiem))
                                       && (loaiLoc == null || dm.Loai_Thu_Chi == loaiLoc)
                                       select new
                                       {
                                           MaGiaoDich = gd.Ma_Giao_Dich,
                                           TenVi = vi.Ten_Vi,
                                           DanhMuc = dm.Ten_Danh_Muc,
                                           Loai = dm.Loai_Thu_Chi,
                                           SoTien = gd.So_Tien,
                                           NgayGiaoDich = gd.Ngay_Giao_Dich,
                                           TinhTrang = gd.Tinh_Trang == true ? "Hoàn thành" : "Chưa hoàn thành"
                                       };

                var dt = new DataTable();
                dt.Columns.Add("MaGiaoDich", typeof(int));
                dt.Columns.Add("TenVi", typeof(string));
                dt.Columns.Add("DanhMuc", typeof(string));
                dt.Columns.Add("SoTien", typeof(decimal));
                dt.Columns.Add("NgayGiaoDich", typeof(DateTime));
                dt.Columns.Add("TinhTrang", typeof(string));

                foreach (var gd in danhSachGiaoDich)
                {
                    dt.Rows.Add(gd.MaGiaoDich, gd.TenVi, gd.DanhMuc, gd.SoTien, gd.NgayGiaoDich, gd.TinhTrang);
                }

                dgvGiaoDich.DataSource = dt.DefaultView; // <-- DefaultView cho phép sắp xếp
                // Tắt dòng * ở cuối
                dgvGiaoDich.AllowUserToAddRows = false;
                foreach (DataGridViewColumn column in dgvGiaoDich.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
        }

        private void login_username_TextChanged(object sender, EventArgs e)
        {

        }
        private void LoadComboBoxTheoTaiKhoan(int maTaiKhoan)
        {
            using (var db = new DataClassesDBDataContext())
            {
                // Load ví
                var danhSachVi = db.VI_TIENs
                                   .Where(vi => vi.Ma_Tai_Khoan == maTaiKhoan)
                                   .Select(vi => new { vi.Ma_Vi, vi.Ten_Vi })
                                   .ToList();

                comboBoxChonVi.DataSource = danhSachVi;
                comboBoxChonVi.DisplayMember = "Ten_Vi";
                comboBoxChonVi.ValueMember = "Ma_Vi";

                // Load danh mục
                var danhSachDanhMuc = db.DANH_MUCs
                                        .Where(dm => dm.Ma_Tai_Khoan == maTaiKhoan)
                                        .Select(dm => new { dm.Ma_Danh_Muc, dm.Ten_Danh_Muc })
                                        .ToList();

                comboBoxChonDanhMuc.DataSource = danhSachDanhMuc;
                comboBoxChonDanhMuc.DisplayMember = "Ten_Danh_Muc";
                comboBoxChonDanhMuc.ValueMember = "Ma_Danh_Muc";
            }
        }
        private void FormThemGiaoDich_Load(object sender, EventArgs e)
        {
            int maNguoiDung = Session.MaTaiKhoan;
            LoadGiaoDichTheoNguoiDung(maNguoiDung);
            LoadComboBoxTheoTaiKhoan(maNguoiDung);
            // Thiết lập trạng thái ban đầu cho radio button
            rbThanhToan.Checked = true;
            rbLocTatCa.Checked = true;
        }

        private void rbThanhToan_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void dgvGiaoDich_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGiaoDich.CurrentRow.Index != -1)
            {
                
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvGiaoDich.Rows[e.RowIndex];

                    // Lấy Ma_Giao_Dich từ cột ẩn hoặc cấu hình
                    id = Convert.ToInt32(dgvGiaoDich.CurrentRow.Cells[0].Value);

                    // Lấy lại ID tài khoản nếu cần (hoặc giữ từ trước)
                    int maNguoiDung = Session.MaTaiKhoan;

                    using (var db = new DataClassesDBDataContext())
                    {
                        var giaoDich = db.GIAO_DICHes.FirstOrDefault(gd => gd.Ma_Giao_Dich == id);
                        if (giaoDich != null)
                        {
                            // Load danh sách Ví và Danh mục
                            LoadComboBoxTheoTaiKhoan(maNguoiDung);

                            // Gán giá trị ComboBox
                            comboBoxChonVi.SelectedValue = giaoDich.Ma_Vi;
                            comboBoxChonDanhMuc.SelectedValue = giaoDich.Ma_Danh_Muc;
                            tbSoTien.Text = giaoDich.So_Tien.ToString("0.##");
                            if (giaoDich.Tinh_Trang == true)
                            {
                                rbThanhToan.Checked = true;
                            }

                            else if (giaoDich.Tinh_Trang == false)
                            {
                                rbChuaThanhToan.Checked = true;
                            }
                        }
                    }
                }

            }
        }

        //thêm giao dịch
        private void buttonAddGiaoDich_Click(object sender, EventArgs e)
        {   
            //lấy tình trạng
            bool tinhTrang = rbThanhToan.Checked ? true : (rbChuaThanhToan.Checked ? false : true);
            if (!decimal.TryParse(tbSoTien.Text, out decimal soTien))
            {
                MessageBox.Show("Vui lòng nhập số tiền hợp lệ.");
                return;
            }

            //lấy giá trị từ combobox
            int maVi = (int)comboBoxChonVi.SelectedValue;
            int maDanhMuc = (int)comboBoxChonDanhMuc.SelectedValue;
            //đưa dữ liệu vào modal GIAO_DICH
            using (var db = new DataClassesDBDataContext())
            {
                GIAO_DICH giaoDichMoi = new GIAO_DICH
                {
                    Ma_Vi = maVi,
                    Ma_Danh_Muc = maDanhMuc,
                    So_Tien = soTien,
                    Ngay_Giao_Dich = DateTime.Now,
                    Tinh_Trang = tinhTrang,
                };

                //thêm giao dịch
                db.GIAO_DICHes.InsertOnSubmit(giaoDichMoi);
                db.SubmitChanges();

                MessageBox.Show("Thêm giao dịch thành công!");
                tbSoTien.Clear();
                //load lại dữ liệu
                int maNguoiDung = Session.MaTaiKhoan;
                LoadGiaoDichTheoNguoiDung(maNguoiDung);
                LoadComboBoxTheoTaiKhoan(maNguoiDung);
            }
        }

        private void buttonSuaGiaoDich_Click(object sender, EventArgs e)
        {
            if (id == -1)
            {
                MessageBox.Show("Vui lòng chọn một giao dịch để sửa.");
                return;
            }

            if (!decimal.TryParse(tbSoTien.Text, out decimal soTien))
            {
                MessageBox.Show("Số tiền không hợp lệ.");
                return;
            }

            using (var db = new DataClassesDBDataContext())
            {
                var giaoDich = db.GIAO_DICHes.FirstOrDefault(gd => gd.Ma_Giao_Dich == id);
                if (giaoDich != null)
                {
                    bool tinhTrang = rbThanhToan.Checked ? true : (rbChuaThanhToan.Checked ? false : true);
                    giaoDich.Ma_Vi = (int)comboBoxChonVi.SelectedValue;
                    giaoDich.Ma_Danh_Muc = (int)comboBoxChonDanhMuc.SelectedValue;
                    giaoDich.So_Tien = soTien;
                    giaoDich.Tinh_Trang = tinhTrang;
                    db.SubmitChanges();

                    MessageBox.Show("Cập nhật giao dịch thành công!");
                    tbSoTien.Clear();
                    int maNguoiDung = Session.MaTaiKhoan;
                    id = -1;
                    LoadGiaoDichTheoNguoiDung(maNguoiDung);
                }
            }
        }

        private void buttonXoaDanhMuc_Click(object sender, EventArgs e)
        {
            if (id == -1)
            {
                MessageBox.Show("Vui lòng chọn giao dịch để xóa.");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa giao dịch này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var db = new DataClassesDBDataContext())
                {
                    var giaoDich = db.GIAO_DICHes.FirstOrDefault(gd => gd.Ma_Giao_Dich == id);
                    if (giaoDich != null)
                    {
                        db.GIAO_DICHes.DeleteOnSubmit(giaoDich);
                        db.SubmitChanges();

                        MessageBox.Show("Xóa giao dịch thành công!");
                        tbSoTien.Clear();
                        int maNguoiDung = Session.MaTaiKhoan;                  
                        LoadGiaoDichTheoNguoiDung(maNguoiDung);
                        LoadComboBoxTheoTaiKhoan(maNguoiDung);
                        id = -1;
                    }
                }
            }
        }

        private void rbLocTatCa_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocTatCa.Checked)
                LoadGiaoDichTheoNguoiDung(Session.MaTaiKhoan);
        }

        private void rbLocThu_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocThu.Checked)
                LoadGiaoDichTheoNguoiDung(Session.MaTaiKhoan);
        }

        private void rbLocChi_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocChi.Checked)
                LoadGiaoDichTheoNguoiDung(Session.MaTaiKhoan);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TimKiem(Session.MaTaiKhoan, txt_TimKiem.Text.Trim());
        }
    }
}
