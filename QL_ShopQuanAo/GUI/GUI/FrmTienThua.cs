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

namespace GUI
{
    public partial class FrmTienThua : DevExpress.XtraEditors.XtraForm
    {
        public FrmTienThua()
        {
            InitializeComponent();
        }
        float giaTien = float.Parse(FrmBanHang.Bh_thanhtoan);
        string giamgia = FrmBanHang.Bh_giamgia;
        string tongtien = FrmBanHang.Bh_tongtien;
        float tienkhach = FrmTienKhach.Val;
        private void FrmTienThua_Load(object sender, EventArgs e)
        {
            float a = tienkhach - giaTien;
            lblTienThua.Text = string.Format("{0:#,##0} VNĐ", a);
        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}