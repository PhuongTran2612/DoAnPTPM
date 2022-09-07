﻿using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmTienKhach : DevExpress.XtraEditors.XtraForm
    {
        private static string soTien;
        float tt1 = float.Parse(FrmBanHang.Bh_thanhtoan);
        public static string SoTien
        {
            get { return FrmTienKhach.soTien; }
            set { FrmTienKhach.soTien = value; }
        }
        private static float val;

        public static float Val
        {
            get { return FrmTienKhach.val; }
            set { FrmTienKhach.val = value; }
        }
        public FrmTienKhach()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                textBox1.Text = float.Parse(soTien).ToString("#,##0");
                textBox1.Select(textBox1.Text.Length, 0);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // kiểm tra nhập số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                MessageBox.Show("Vui lòng nhập số", "Thông Báo");
                e.Handled = true;
            }
            if (char.IsDigit(e.KeyChar))
                soTien += e.KeyChar;
            else if (e.KeyChar == '\b' && soTien != string.Empty)
                soTien = soTien.Remove(soTien.Length - 1);
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập giá trị");
                this.textBox1.Focus();
                return;
            }
            if (tt1 > float.Parse(this.textBox1.Text))
            {
                MessageBox.Show("Không đủ giá trị thanh toán");
                this.textBox1.Focus();
                return;
            }
            else
            {
                Val = float.Parse(soTien);
                FrmTienThua tt = new FrmTienThua();
                tt.ShowDialog();
                this.Hide();
            }
        }
    }
}