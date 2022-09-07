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
using BLL;
using DTO;
using GUI;

namespace GUI
{
    public partial class FrmDMK : DevExpress.XtraEditors.XtraUserControl
    {
        public FrmDMK()
        {
            InitializeComponent();
        }
        BLLTaiKhoan bllTK = new BLLTaiKhoan();

        private void FrmDMK_Load(object sender, EventArgs e)
        {
            txtPassCu.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtPassCu.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassCu.UseSystemPasswordChar = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                txtPassMoi.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassMoi.UseSystemPasswordChar = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                txtRePassMoi.UseSystemPasswordChar = false;
            }
            else
            {
                txtRePassMoi.UseSystemPasswordChar = true;
            }
        }

        private void btnDMK_Click(object sender, EventArgs e)
        {
            if (txtPassCu.TextLength == 0 || txtPassMoi.TextLength == 0 || txtRePassMoi.TextLength == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            else if (bllTK.kiemTraPass(FrmMain.tenhienthi, txtPassCu.Text) != null)
            {
                if (!txtRePassMoi.Text.Equals(txtPassMoi.Text))
                {
                    errorProvider1.SetError(txtRePassMoi, "Mật khẩu nhập lại không khớp");
                }
                else
                {
                    bllTK.DoiPass(FrmMain.tenhienthi, txtPassMoi.Text);
                    MessageBox.Show("Đổi Mật Khẩu Thành Công");
                    FrmDMK_Load(sender, e);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
