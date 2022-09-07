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
using BUS;
using DTO;

namespace GUI
{
    public partial class FrmCTHD : DevExpress.XtraEditors.XtraForm
    {
        public FrmCTHD()
        {
            InitializeComponent();
        }
        int MAHD = FrmHoaDon.MAHD;
        BUSCTHD CT = new BUSCTHD();
        BUSSanPham sp = new BUSSanPham();
        QLQADataContext qlqa = new QLQADataContext();

        private void FrmCTHD_Load(object sender, EventArgs e)
        {
            barStaticItem3.Caption = MAHD.ToString();
            txt_msp.DisplayMember = "MASP";
            txt_msp.ValueMember = "MASP";
            txt_msp.DataSource = sp.getSP();
            var size = (from k in qlqa.KHOs select k.SIZE).Distinct();
            cbo_Size.DataSource = size;
            dataGridView1.DataSource = sp.getSP();
            dtv_CTHD.DataSource = CT.getCTHD(MAHD);
        }

        private void txt_msp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_msp_TextChanged(object sender, EventArgs e)
        {
            if (txt_msp.Text.Length > 0)
            {
                char phantucuoi = txt_msp.Text[txt_msp.Text.Length - 1];
                if (char.IsDigit(phantucuoi) == false)
                {
                    MessageBox.Show("Bạn nhập không phải số");
                    txt_msp.Text = "";
                    txt_msp.Focus();
                }
            }
        }

        private void txt_sl_MaskChanged(object sender, EventArgs e)
        {
            if (txt_sl.Text.Length > 0)
            {
                char phantucuoi = txt_sl.Text[txt_sl.Text.Length - 1];
                if (char.IsDigit(phantucuoi) == false)
                {
                    MessageBox.Show("Bạn nhập không phải số");
                    txt_sl.Text = "";
                    txt_sl.Focus();
                }
            }
        }

        //private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (txt_msp.Text.Length == 0 || txt_sl.Text.Length == 0)
        //    {
        //        MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi thêm!");
        //        return;
        //    }
        //    if (int.Parse(txt_sl.Text) <= 0)
        //    {
        //        MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0");
        //        return;
        //    }
        //    if (CT.kiemtraKC(MAHD, int.Parse(txt_msp.Text)) == false)
        //    {
        //        MessageBox.Show("Sản phẩm đã có trong hóa đơn!");
        //        return;
        //    }
        //    else
        //    {
        //        CT.insertCTHD(MAHD, int.Parse(txt_msp.Text),cbo_Size.SelectedValue.ToString() ,int.Parse(txt_sl.Text));
        //        dtv_CTHD.DataSource = CT.getCTHD(MAHD);
        //    }
        //}

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn xóa?", "Thông báo",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) ==

                            System.Windows.Forms.DialogResult.Yes)
            {
                if (txt_msp.Text == string.Empty)
                {
                    MessageBox.Show("Mã sản phẩm không được rỗng");
                    return;
                }
                else
                {
                    CT.deletectHD(int.Parse(txt_msp.Text));
                    dtv_CTHD.DataSource = CT.getCTHD(MAHD);
                }
            }
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txt_msp.Text.Length == 0 || txt_sl.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            if (CT.kiemtraKC(MAHD, cbo_Size.SelectedValue.ToString(),int.Parse(txt_msp.Text)) == true)
            {
                MessageBox.Show("Sản phẩm không có trong hóa đơn!");
                return;
            }
            else
            {
                CT.updateHD(MAHD, cbo_Size.SelectedValue.ToString(),int.Parse(txt_msp.Text), int.Parse(txt_sl.Text));
                dtv_CTHD.DataSource = CT.getCTHD(MAHD);
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnXoaTC_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn xóa tất cả?", "Thông báo",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                          MessageBoxDefaultButton.Button2) ==

                          System.Windows.Forms.DialogResult.Yes)
            {

                CT.deleteHD(MAHD);
                dtv_CTHD.DataSource = CT.getCTHD(MAHD);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = sp.Search(txtSearch.Text);
        }

        private void dtv_CTHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_msp.Text = dtv_CTHD.CurrentRow.Cells[0].Value.ToString();
            cbo_Size.Text = dtv_CTHD.CurrentRow.Cells[2].Value.ToString();
            txt_sl.Text = dtv_CTHD.CurrentRow.Cells[3].Value.ToString();
        }
    }
}