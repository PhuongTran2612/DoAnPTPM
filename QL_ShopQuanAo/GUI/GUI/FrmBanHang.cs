using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using BUS;
using BLL;
using DTO;

namespace GUI
{
    public partial class FrmBanHang : DevExpress.XtraEditors.XtraUserControl
    {
        public FrmBanHang()
        {
            InitializeComponent();
        }
        BUSKhachHang kh = new BUSKhachHang();
        BUSHoaDon hd = new BUSHoaDon();
        BUSSanPham sp = new BUSSanPham();
        BUSCTHD ct = new BUSCTHD();
        BLLSanPham bllsp = new BLLSanPham();
        BLLHoaDon bllhd = new BLLHoaDon();
        BLLKho bllkho = new BLLKho();
        BUSKho kho = new BUSKho();
        QLQADataContext qlqa = new QLQADataContext();

        private static string bh_tongtien, bh_giamgia, bh_thanhtoan;

        public static string Bh_thanhtoan
        {
            get { return FrmBanHang.bh_thanhtoan; }
            set { FrmBanHang.bh_thanhtoan = value; }
        }

        public static string Bh_giamgia
        {
            get { return FrmBanHang.bh_giamgia; }
            set { FrmBanHang.bh_giamgia = value; }
        }

        public static string Bh_tongtien
        {
            get { return FrmBanHang.bh_tongtien; }
            set { FrmBanHang.bh_tongtien = value; }
        }



        private static int mAHDBH;//phương thức mã hóa đơn vừa tạo
        string tennv = FrmMain.tennv;//phương thức lấy tên nv

        public static int MAHDBH
        {
            get { return FrmBanHang.mAHDBH; }
            set { FrmBanHang.mAHDBH = value; }
        }
        private static string time;//phương thức ngày giờ tạo hóa đơn

        public static string Time
        {
            get { return FrmBanHang.time; }
            set { FrmBanHang.time = value; }
        }

        string manv = FrmMain.manv;//phương thức lấy mã nv đăng nhập

        private void FrmBanHang_Load(object sender, EventArgs e)
        {
            cb_makh.DisplayMember = "MAKH";
            cb_makh.ValueMember = "MAKH";
            cb_makh.DataSource = kh.getKH();
        }

        float tongtien()
        {
            float sum = 0;
            for (int i = 0; i < dtv_hd.Rows.Count; i++)
            {
                sum += float.Parse(dtv_hd.Rows[i].Cells["ThanhTIEN"].Value.ToString());
            }
            return sum;
        }
        public bool IsValidVietNamPhoneNumber(string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
                return false;
            string sMailPattern = @"^((09(\d){8})|(03(\d){8})|(08(\d){8})|(07(\d){8})|(05(\d){8}))$";
            return Regex.IsMatch(phoneNum.Trim(), sMailPattern);
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            btnXoaHDTao.Enabled = true;
            if (cb_makh.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi thêm!");
                return;
            }
            else
            {
                //lấy điểm tích lũy theo mã kh
                System.Data.DataTable dt1 = new System.Data.DataTable();
                try
                {
                    dt1 = kh.getdtl(cb_makh.Text);

                    if (dt1.Rows.Count > 0)
                    {
                        lblDTL.Text = dt1.Rows[0][0].ToString();
                        giamgia = float.Parse(lblDTL.Text) * 20000;
                        txtGiamGia.Text = giamgia.ToString();
                        Bh_giamgia = giamgia.ToString();
                    }
                }

                catch (FormatException)
                {
                    MessageBox.Show("Bạn đã nhập sai cú pháp");
                }

                //lấy thời gian hiện tại 
                //time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000");
                time = DateTime.Now.ToString("yyyy-MM-dd");
                //tạo mã hóa đơn
                hd.insertHDBH(Time, manv, cb_makh.Text);
                //lấy dữ liệu cho cb_masp
                cb_mahang.DisplayMember = "MASP";
                cb_mahang.ValueMember = "MASP";
                cb_mahang.DataSource = sp.getSP();

                txt_soluong.Enabled = true;
                cb_mahang.Enabled = true;
                btnThem.Enabled = true;
                btnXoa.Enabled = true;
                btnSua.Enabled = true;

                btnTao.Enabled = false;
                cb_makh.Enabled = false;
                txt_sdt.Enabled = false;
                btnTaoKH.Enabled = false;

                System.Data.DataTable dt = new System.Data.DataTable();
                try
                {
                    //lấy mã hóa đơn vừa tạo
                    dt = hd.getMaHD(Time, manv, cb_makh.Text);

                    if (dt.Rows.Count > 0)
                    {
                        //truyền mã hóa đơn vừa tạo vào mAHDBH
                        mAHDBH = int.Parse(dt.Rows[0][0].ToString());

                    }
                }

                catch (FormatException)
                {
                    MessageBox.Show("Bạn đã nhập sai cú pháp");
                }
            }
        }

        private void btnTaoKH_Click(object sender, EventArgs e)
        {
            if (txt_sdt.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi thêm!");
                return;
            }
            if (IsValidVietNamPhoneNumber(txt_sdt.Text) == false)
            {
                MessageBox.Show("Số điện thoại sai định dạng!");
                return;
            }
            if (kh.kiemtraKC(txt_sdt.Text) == false)
            {
                MessageBox.Show("Khách hàng này đã tồn tại!");
                return;
            }
            else
            {
                kh.insertKHMacDinh(txt_sdt.Text);
                cb_makh.DataSource = kh.getKH();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnXuatHD.Enabled = false;
            btnHDNew.Enabled = false;
            if (cb_mahang.Text.Length == 0 || txt_soluong.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi thêm!");
                return;
            }
            if (int.Parse(txt_soluong.Text) <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0");
                return;
            }
            if (bllkho.ktSL(int.Parse(cb_mahang.SelectedValue.ToString()), int.Parse(txt_soluong.Text),cbo_Size.SelectedValue.ToString()) > 0)
            {
                MessageBox.Show("Quá số lượng trong kho");
                return;
            }
            if (ct.kiemtraKC(MAHDBH, cbo_Size.SelectedValue.ToString(),int.Parse(cb_mahang.Text)) == false)
            {
                MessageBox.Show("Sản phẩm đã có trong hóa đơn!\r\n Vui lòng cập nhật lại số lượng");
                return;
            }
            else
            {
                ct.insertCTHD(MAHDBH, int.Parse(cb_mahang.Text),cbo_Size.SelectedValue.ToString(),int.Parse(txt_soluong.Text));
                dtv_hd.DataSource = hd.getHDBH(MAHDBH);

                txtTongTien.Text = tongtien().ToString();
                thanhtoan = tongtien() - giamgia;
                txtTT.Text = thanhtoan.ToString();

                btnTT.Enabled = true;

                Bh_tongtien = tongtien().ToString();
                Bh_thanhtoan = thanhtoan.ToString();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnXuatHD.Enabled = false;
            btnHDNew.Enabled = false;
            if (cb_mahang.Text.Length == 0 || txt_soluong.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            if (ct.kiemtraKC(MAHDBH, cbo_Size.SelectedValue.ToString(), int.Parse(cb_mahang.Text)) == true)
            {
                MessageBox.Show("Sản phẩm không có trong hóa đơn!");
                return;
            }
            if (bllkho.ktSL(int.Parse(cb_mahang.SelectedValue.ToString()), int.Parse(txt_soluong.Text), cbo_Size.SelectedValue.ToString()) > 0)
            {
                MessageBox.Show("Quá số lượng trong kho");
                return;
            }
            else
            {
                ct.updateHD(MAHDBH, cbo_Size.SelectedValue.ToString(),int.Parse(cb_mahang.Text), int.Parse(txt_soluong.Text));
                dtv_hd.DataSource = hd.getHDBH(MAHDBH);

                txtTongTien.Text = tongtien().ToString();
                thanhtoan = tongtien() - giamgia;
                txtTT.Text = thanhtoan.ToString();

                bh_tongtien = tongtien().ToString();
                bh_thanhtoan = thanhtoan.ToString();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            btnXuatHD.Enabled = false;
            btnHDNew.Enabled = false;
            if (MessageBox.Show("Bạn muốn xóa?", "Thông báo",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                             MessageBoxDefaultButton.Button2) ==
                             System.Windows.Forms.DialogResult.Yes)
            {
                if (cb_mahang.Text.Length == 0)
                {
                    MessageBox.Show("Vui lòng nhập mã sản phẩm!");
                    return;
                }
                else
                {
                    ct.deletectHD(int.Parse(cb_mahang.Text));
                    dtv_hd.DataSource = hd.getHDBH(MAHDBH);
                    txtTongTien.Text = tongtien().ToString();
                    if (int.Parse(txtTongTien.Text)==0)
                    {
                        MessageBox.Show("Giỏ hàng trống");
                        txtTT.Text = txtTongTien.Text;
                        return;
                    }
                    else
                    {
                        thanhtoan = tongtien() - giamgia;
                        txtTT.Text = thanhtoan.ToString();
                        bh_tongtien = tongtien().ToString();
                        bh_thanhtoan = thanhtoan.ToString();
                        //if (bllhd.ktMaHD(MAHDBH) == 0)
                        //{
                        //    hd.deleteHD(MAHDBH);
                        //    //MessageBox.Show("Xoá HĐ thành công");
                        //}
                        btnHDNew.Enabled = true;
                    }
                }
            }
        }

        private void btnTT_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            int year = now.Year;
            int month = now.Month;

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            double value = double.Parse(txtTT.Text, System.Globalization.NumberStyles.AllowThousands);
            int tienhang = int.Parse(String.Format(culture, "{0:000}", value));
            if (this.dtv_hd.RowCount == 0)
            {
                MessageBox.Show("Vui lòng lập hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                return;
            }
            FrmTienKhach tKhach = new FrmTienKhach();
            tKhach.ShowDialog(); // lấy được số tiền khách đã đưa
            btnXuatHD.Enabled = true;
            btnHDNew.Enabled = true;
            hd.updateHDBH(MAHDBH);
            //sp.UpdateSP(int.Parse(cb_mahang.SelectedValue.ToString()), int.Parse(txt_soluong.Text));
            kho.UpdateKho(int.Parse(cb_mahang.SelectedValue.ToString()),cbo_Size.SelectedValue.ToString(), int.Parse(txt_soluong.Text));
            hd.themvaothongke(tienhang, month, year);
            hd.getHD();
        }

        private void cb_mahang_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbo_tenhang.DataSource = bllsp.LoadSanPham(int.Parse(cb_mahang.SelectedValue.ToString()));
            cbo_tenhang.ValueMember = "MaSP";
            cbo_tenhang.DisplayMember = "TenSP";

            cbo_Size.DataSource = bllkho.LoadKho(int.Parse(cb_mahang.SelectedValue.ToString()));
            cbo_Size.ValueMember = "Size";
            cbo_Size.DisplayMember = "Size";
        }

        private void txt_soluong_TextChanged(object sender, EventArgs e)
        {
            if (txt_soluong.Text.Length > 0)
            {
                char phantucuoi = txt_soluong.Text[txt_soluong.Text.Length - 1];
                if (char.IsDigit(phantucuoi) == false)
                {
                    MessageBox.Show("Bạn nhập không phải số");
                    txt_soluong.Text = "";
                    txt_soluong.Focus();
                }
            }
        }

        private void txtTongTien_TextChanged(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            //double value = double.Parse(txtTongTien.Text, System.Globalization.NumberStyles.AllowThousands);
            decimal value = decimal.Parse(txtTongTien.Text, System.Globalization.NumberStyles.AllowThousands);
            txtTongTien.Text = String.Format(String.Format(culture, "{0:#,##0}", value));
            //txtTongTien.Text = String.Format(String.Format(culture, "{0:###,###,##0}", value));
            txtTongTien.Select(txtTongTien.Text.Length, 0);
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            //double value = double.Parse(txtGiamGia.Text, System.Globalization.NumberStyles.AllowThousands);
            decimal value = decimal.Parse(txtGiamGia.Text, System.Globalization.NumberStyles.AllowThousands);
            txtGiamGia.Text = String.Format(culture, "{0:#,##0}", value);
            //txtGiamGia.Text = String.Format(culture, "{0:###,###,##0}", value);
            txtGiamGia.Select(txtGiamGia.Text.Length, 0);
        }

        private void txtTT_TextChanged(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            //double value = double.Parse(txtTT.Text, System.Globalization.NumberStyles.AllowThousands);
            decimal value = decimal.Parse(txtTT.Text, System.Globalization.NumberStyles.AllowThousands);
            txtTT.Text = String.Format(culture, "{0:#,##0}", value);
            //txtTT.Text = String.Format(culture, "{0:###,###,##0}", value);
            txtTT.Select(txtTT.Text.Length, 0);
        }

        private void dtv_hd_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void btnHDNew_Click(object sender, EventArgs e)
        {
            txt_sdt.Enabled = true;
            cb_makh.Enabled = true;
            btnTao.Enabled = true;
            btnTaoKH.Enabled = true;

            txt_soluong.Enabled = false;
            cb_mahang.Enabled = false;
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;

            btnHDNew.Enabled = false;
            btnTT.Enabled = false;
            btnXuatHD.Enabled = false;
        }

        private void dtv_hd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cb_mahang.Text = dtv_hd.CurrentRow.Cells[4].Value.ToString();
            cbo_tenhang.Text = dtv_hd.CurrentRow.Cells[5].Value.ToString();
            txt_soluong.Text = dtv_hd.CurrentRow.Cells[7].Value.ToString();
            cbo_Size.Text = dtv_hd.CurrentRow.Cells[6].Value.ToString();
        }

        float giamgia = 0;
        float thanhtoan = 0;

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            List<object> _lstSP = new List<object>();
            string MaNV = string.Empty;
            string TenNV = tennv;
            int tongSoluong = 0;

            foreach (DataGridViewRow r in dtv_hd.Rows)
            {
                _lstSP.Add(new 
                { 
                    TenSP = r.Cells[5].Value.ToString(), // cột sp
                    SoLuong = r.Cells[7].Value.ToString(), // cột sô lượng
                    Size = r.Cells[6].Value.ToString(), // cốt size
                    Gia = r.Cells[8].Value.ToString() // cột giá
                });

                //MaNV = r.Cells[1].Value.ToString(); // cột nhân viên 

                tongSoluong += int.Parse(r.Cells[7].Value.ToString());
            }

            List<string[]> data = new List<string[]>();
            data.Add(new string[] { "TenNhanVien", "TongSoLuong", "TongGia", "GiamGia", "ThanhTien" });
            data.Add(new string[] { TenNV, tongSoluong.ToString(), txtTongTien.Text, txtGiamGia.Text, txtTT.Text });


            // Show thông tin phiếu nhập
            new Reports<object>().export_Word("reportHoaDon.docx", "ListSP", _lstSP, data);
        }

        private void btnXoaHDTao_Click(object sender, EventArgs e)
        {
            hd.deleteHD(MAHDBH);
            MessageBox.Show("Xoá Hoá Đơn Vừa Tạo Thành Công");
            btnTT.Enabled = false;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnXuatHD.Enabled = false;
        }
    }
}
