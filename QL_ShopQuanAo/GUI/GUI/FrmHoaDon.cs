using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;
using DTO;
using BLL;
using BUS;
using System.Data.Common;

namespace GUI
{
    public partial class FrmHoaDon : DevExpress.XtraEditors.XtraUserControl
    {
        BLLHoaDon bllhoadon = new BLLHoaDon();
        QLQADataContext qlqa = new QLQADataContext();
        private static int mAHD;
        BUSKhachHang kh = new BUSKhachHang();
        BUSHoaDon hd = new BUSHoaDon();
        private static string time;
        public static string dtp
        {
            get { return FrmHoaDon.time; }
            set { FrmHoaDon.time = value; }
        }

        public static int MAHD
        {
            get { return FrmHoaDon.mAHD; }
            set { FrmHoaDon.mAHD = value; }
        }
        public FrmHoaDon()
        {
            InitializeComponent();
        }

        private void FrmHoaDon_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = hd.getHD();

            var CBO_MaNV = from hd in qlqa.NHANVIENs
                           select new
                           {
                               hd.MANV
                           };
            cb_manv.DataSource = CBO_MaNV;
            cb_manv.ValueMember = "MaNV";
            cb_manv.DisplayMember = "MaNV";

            var CBO_MaKH = from hd in qlqa.KHACHHANGs
                           select new
                           {
                               hd.MAKH
                           };
            cbo_MaKH.DataSource = CBO_MaKH;
            cbo_MaKH.ValueMember = "MaKH";
            cbo_MaKH.DisplayMember = "MaKH";

            var CBO_TT = (from hd in qlqa.HOADONs select hd.TINHTRANG).Distinct();
            CBo_TT.DataSource = CBO_TT;
        }

        //private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    dtp = dtpNT.Value.ToString();
        //    //string dtp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000");
        //    if (txtmaHD.Text.Trim() == "")
        //    {
        //        MessageBox.Show("Không được bỏ trống Mã Hoá Đơn");
        //    }
        //    else
        //    {
        //        bllhoadon.ThemHoaDon(dtp, float.Parse(txt_THHTOAN.Text), CBo_TT.SelectedValue.ToString(), cbo_MaKH.SelectedValue.ToString(), cb_manv.SelectedValue.ToString());
        //        MessageBox.Show("Thêm Hoá Đơn Thành Công");
        //        FrmHoaDon_Load(sender, e);
        //    }
        //}

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bllhoadon.ktMaHD(int.Parse(txtmaHD.Text)) == 0)
            {
                MessageBox.Show("Trùng khoá không thể xoá");
                return;
            }
            bllhoadon.XoaHoaDon(int.Parse(txtmaHD.Text));
            MessageBox.Show("Xoá Hoá Đơn Thành Công");
            FrmHoaDon_Load(sender, e);
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string dtp = dtpNT.Value.ToString();
            bllhoadon.SuaHoaDon(int.Parse(txtmaHD.Text), dtp, float.Parse(txt_THHTOAN.Text), CBo_TT.SelectedValue.ToString(), cbo_MaKH.SelectedValue.ToString(), cb_manv.SelectedValue.ToString());
            MessageBox.Show("Sửa Hoá Đơn Thành Công");
            FrmHoaDon_Load(sender, e);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                //Đưa dữ liệu vào 
                MAHD = int.Parse(row.Cells[1].Value.ToString());
                //this.Hide();
                FrmCTHD childForm = new FrmCTHD();
                //childForm.MdiParent = this.ParentForm;
                //childForm.Dock = DockStyle.Fill;
                //childForm.BringToFront();
                childForm.Show();
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (txtTimKiem.TextLength == 0)
            {
                MessageBox.Show("Vui lòng nhập thông tin cần tìm!");
                txtTimKiem.Focus();
            }
            dataGridView1.DataSource = bllhoadon.TimKiem(txtTimKiem.Text);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            txtmaHD.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            //dtpNT.Value.ToString("dd/MM/yyyy")= dataGridView1.CurrentRow.Cells[2].Value.ToString();
            DateTime datens = DateTime.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());
            dtpNT.Value = DateTime.Parse(datens.ToShortDateString());
            txt_THHTOAN.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            CBo_TT.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            cbo_MaKH.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            cb_manv.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }
    }
}
