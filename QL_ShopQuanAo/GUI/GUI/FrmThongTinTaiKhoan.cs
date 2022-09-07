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

namespace GUI
{
    public partial class FrmThongTinTaiKhoan : DevExpress.XtraEditors.XtraUserControl
    {
        public FrmThongTinTaiKhoan()
        {
            InitializeComponent();
        }
        BLLTaiKhoan bbTK = new BLLTaiKhoan();
        QLQADataContext qlqa = new QLQADataContext();
        public static string matk;

        private void FrmThongTinTaiKhoan_Load(object sender, EventArgs e)
        {
            var CBO_GT = (from t in qlqa.THONGTINTAIKHOANs select t.GTINH).Distinct();
            txtGT.DataSource = CBO_GT;

            THONGTINTAIKHOAN tttk = bbTK.ThongTinTK(matk);
            FrmMain mh = new FrmMain();
            label6.Text = matk;
            txtHoTen.Text = tttk.HOTEN;
            txtDiaChi.Text = tttk.DCHI;
            txtGT.Text = tttk.GTINH;
            DateTime datens = DateTime.Parse(tttk.NGSINH.ToString());
            dtpNS.Value = DateTime.Parse(datens.ToShortDateString());
        }
        private void btnCapNhat_Click_1(object sender, EventArgs e)
        {
            string ns = dtpNS.Value.ToString();
            bbTK.SuaTaiKhoan(label6.Text, txtHoTen.Text, txtDiaChi.Text, ns, txtGT.Text);
            MessageBox.Show("Sửa Thông Tin Thành Công");
            FrmThongTinTaiKhoan_Load(sender, e);
        }
    }
}
