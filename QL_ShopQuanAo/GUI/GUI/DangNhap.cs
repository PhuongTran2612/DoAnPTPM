using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;
using DTO;
using BLL;

namespace GUI
{
    public partial class DangNhap : DevExpress.XtraEditors.XtraForm
    {
        QLQADataContext ql = new QLQADataContext();
        BLLTaiKhoan bllTaiKhoan = new BLLTaiKhoan();
        public TAIKHOAN taikhoan;
        public TAIKHOAN taikhoanNV;
        public THONGTINTAIKHOAN tttk;
        public NHANVIEN nv;
        public DangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Select();
                return;
            }
            TAIKHOAN tk = bllTaiKhoan.DangNhap(txtUsername.Text, txtPassword.Text);
            if (tk != null)
            {
                taikhoan = tk;
                tttk = ql.THONGTINTAIKHOANs.SingleOrDefault(t => t.MATK == taikhoan.MATK);
                nv = ql.NHANVIENs.SingleOrDefault(t => t.MATK == taikhoan.MATK);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại tên đăng nhập hoặc mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Select();
                return;
            }
        }
    }
}