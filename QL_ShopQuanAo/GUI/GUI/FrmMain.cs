using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DTO;
using GUI;

namespace GUI
{
    public partial class FrmMain : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        BLLTaiKhoan bllTK = new BLLTaiKhoan();
        QLQADataContext qlqa = new QLQADataContext();
        public static string matk;
        public static string tentk;
        public static string tenhienthi;
        public static string manv;
        public static string tennv;
        

        //Gọi các user controls
        FrmKhachHang frmKhachHang;
        FrmBanHang frmBanHang;
        FrmHoaDon frmHoaDon;
        FrmThongTinTaiKhoan frmThongTinTaiKhoan;
        FrmDMK frmDoiMatKhau;
        FrmGioiThieu frmGioiThieu;
        FrmThongKe frmThongKe;
        FrmSanPham frmSanPham;
        FrmNhanVien frmNhanVien;
        FrmSanPhamSapHet frmSanPhamSapHet;
        public FrmMain()
        {
            InitializeComponent();
        }
        //void OpenFrom(Type typeFrom)
        //{
        //    foreach (Form f in MdiChildren)
        //    {
        //        if (f.GetType() == typeFrom)
        //        {
        //            f.Activate();
        //            return;
        //        }
        //    }
        //    Form frm = (Form)Activator.CreateInstance(typeFrom);
        //    frm.MdiParent = this;
        //    //frm.Dock = DockStyle.Fill;
        //    frm.Show();
        //}

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.Hide();
            var f = new DangNhap();
            f.ShowDialog();
            TAIKHOAN taikhoan = f.taikhoan;
            THONGTINTAIKHOAN tttk = f.tttk;
            NHANVIEN nv = f.nv;
            if (f.taikhoan.MAQUYEN == 1)
            {
                MessageBox.Show("Xin Chào: " + taikhoan.TENTK);
                label1.Caption = String.Format("{0}", tttk.HOTEN);
                tennv = tttk.HOTEN;
                tenhienthi = taikhoan.TENTK;
                btnNhanVien.Visible = false;
                FrmThongTinTaiKhoan.matk = taikhoan.MATK;
                this.Show();
            }
            else
            {
                MessageBox.Show("Xin Chào: " + taikhoan.TENTK);
                label1.Caption = String.Format("{0}", tttk.HOTEN);
                tennv = tttk.HOTEN;
                manv = nv.MANV;
                tenhienthi = taikhoan.TENTK;
                btnQL.Visible = false;
                FrmThongTinTaiKhoan.matk = taikhoan.MATK;
                this.Show();
            }
        }

        private void BtnKH_Click(object sender, EventArgs e)
        {
            if (frmKhachHang == null)
            {
                frmKhachHang = new FrmKhachHang();
                frmKhachHang.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmKhachHang);
                frmKhachHang.BringToFront();

            }
            else frmKhachHang.BringToFront();
            lblTieuDe.Caption = BtnKH.Text;
        }

        private void BtnBanHang_Click(object sender, EventArgs e)
        {
            if (frmBanHang == null)
            {
                frmBanHang = new FrmBanHang();
                frmBanHang.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmBanHang);
                frmBanHang.BringToFront();

            }
            else frmBanHang.BringToFront();
            lblTieuDe.Caption = BtnBanHang.Text;
        }
        private void BtnTTCN_Click(object sender, EventArgs e)
        {
            if (frmThongTinTaiKhoan == null)
            {
                frmThongTinTaiKhoan = new FrmThongTinTaiKhoan();
                frmThongTinTaiKhoan.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmThongTinTaiKhoan);
                frmThongTinTaiKhoan.BringToFront();

            }
            else frmThongTinTaiKhoan.BringToFront();
            lblTieuDe.Caption = BtnTTCN.Text;
        }

        private void BtnDMK_Click(object sender, EventArgs e)
        {
            if (frmDoiMatKhau == null)
            {
                frmDoiMatKhau = new FrmDMK();
                frmDoiMatKhau.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmDoiMatKhau);
                frmDoiMatKhau.BringToFront();

            }
            else frmDoiMatKhau.BringToFront();
            lblTieuDe.Caption = BtnDMK.Text;
        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {
            if (frmGioiThieu == null)
            {
                frmGioiThieu = new FrmGioiThieu();
                frmGioiThieu.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmGioiThieu);
                frmGioiThieu.BringToFront();

            }
            else frmGioiThieu.BringToFront();
            lblTieuDe.Caption = accordionControlElement2.Text;
        }

        private void BtnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnDX_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                //this.Close();
                FrmMain f = new FrmMain();
                f.ShowDialog();
            }
        }

        private void BtnQLTK_Click(object sender, EventArgs e)
        {
            if (frmThongKe == null)
            {
                frmThongKe = new FrmThongKe();
                frmThongKe.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmThongKe);
                frmThongKe.BringToFront();

            }
            else frmThongKe.BringToFront();
            lblTieuDe.Caption = BtnQLTK.Text;
        }

        private void btnQLSP_Click(object sender, EventArgs e)
        {
            if (frmSanPham == null)
            {
                frmSanPham = new FrmSanPham();
                frmSanPham.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmSanPham);
                frmSanPham.BringToFront();

            }
            else frmSanPham.BringToFront();
            lblTieuDe.Caption = btnQLSP.Text;
        }

        private void BtnQLNV_Click(object sender, EventArgs e)
        {
            if (frmNhanVien == null)
            {
                frmNhanVien = new FrmNhanVien();
                frmNhanVien.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmNhanVien);
                frmNhanVien.BringToFront();

            }
            else frmNhanVien.BringToFront();
            lblTieuDe.Caption = BtnQLNV.Text;
        }

        //private void accordionControlElement3_Click(object sender, EventArgs e)
        //{
        //    if (frmHoaDon == null)
        //    {
        //        frmHoaDon = new FrmHoaDon();
        //        frmHoaDon.Dock = DockStyle.Fill;
        //        Controlsmain.Controls.Add(frmHoaDon);
        //        frmHoaDon.BringToFront();

        //    }
        //    else frmHoaDon.BringToFront();
        //    lblTieuDe.Caption = accordionControlElement3.Text;
        //}

        private void btnHD_Click(object sender, EventArgs e)
        {
            if (frmHoaDon == null)
            {
                frmHoaDon = new FrmHoaDon();
                frmHoaDon.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmHoaDon);
                frmHoaDon.BringToFront();

            }
            else frmHoaDon.BringToFront();
            lblTieuDe.Caption = btnHD.Text;
        }

        private void btnSPSH_Click(object sender, EventArgs e)
        {
            if (frmSanPhamSapHet == null)
            {
                frmSanPhamSapHet = new FrmSanPhamSapHet();
                frmSanPhamSapHet.Dock = DockStyle.Fill;
                Controlsmain.Controls.Add(frmSanPhamSapHet);
                frmSanPhamSapHet.BringToFront();

            }
            else frmSanPhamSapHet.BringToFront();
            lblTieuDe.Caption = btnSPSH.Text;
        }
    }
}
