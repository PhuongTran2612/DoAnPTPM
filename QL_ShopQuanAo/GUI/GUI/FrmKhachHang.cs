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
using BLL;
using DTO;
using BUS;

namespace GUI
{
    public partial class FrmKhachHang : DevExpress.XtraEditors.XtraUserControl
    {
        public FrmKhachHang()
        {
            InitializeComponent();
        }
        BLLKhachHang bllkh = new BLLKhachHang();
        BUSKhachHang kh = new BUSKhachHang();
        public bool IsValidVietNamPhoneNumber(string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
                return false;
            string sMailPattern = @"^((09(\d){8})|(03(\d){8})|(08(\d){8})|(07(\d){8})|(05(\d){8}))$";
            return Regex.IsMatch(phoneNum.Trim(), sMailPattern);
        }
        //public void Load_Text()
        //{
        //    DevExpress.XtraGrid.Views.Base.ColumnView bw = (DevExpress.XtraGrid.Views.Base.ColumnView)dtv_khachhang.FocusedView;
        //    if (bw.IsDetailView)
        //    {
        //        int detailrowhandle = bw.FocusedRowHandle;
        //        DataRow row = bw.GetDataRow(detailrowhandle);
        //        txt_sdt.Text = row[0].ToString();
        //        txt_diem.Text = row[1].ToString();
        //    }
        //}

        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            dtv_khachhang.DataSource = bllkh.LoadKH();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txt_sdt.Text.Length == 0 || txt_diem.Text.Length == 0)
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
                bllkh.ThemKH(txt_sdt.Text, int.Parse(txt_diem.Text));
                FrmKhachHang_Load(sender, e);
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn xóa?", "Thông báo",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                               MessageBoxDefaultButton.Button2) ==

                               System.Windows.Forms.DialogResult.Yes)
            {
                if (txt_sdt.Text == string.Empty)
                {
                    MessageBox.Show("Mã không được rỗng");
                    return;
                }
                if (kh.kiemtraKN(txt_sdt.Text) == false)
                {
                    MessageBox.Show("Khách hàng không thể xóa vì có dữ liệu trên hóa đơn");
                    return;
                }
                else
                {
                    bllkh.XoaKH(txt_sdt.Text);
                    FrmKhachHang_Load(sender, e);
                }
            }
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txt_sdt.Text.Length == 0 || txt_diem.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            //if (IsValidVietNamPhoneNumber(txt_sdt.Text))
            //{
            //    MessageBox.Show("Số điện thoại sai định dạng!");
            //    return;
            //}
            if (kh.kiemtraKC(txt_sdt.Text) == true)
            {
                MessageBox.Show("Khách hàng này ko tồn tại!");
                return;
            }
            else
            {
                bllkh.SuaKH(txt_sdt.Text, int.Parse(txt_diem.Text));
                FrmKhachHang_Load(sender, e);
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void txt_diem_TextChanged(object sender, EventArgs e)
        {
            if (txt_diem.Text.Length > 0)
            {
                char phantucuoi = txt_diem.Text[txt_diem.Text.Length - 1];
                if (char.IsDigit(phantucuoi) == false)
                {
                    MessageBox.Show("Bạn nhập không phải số");
                    txt_diem.Text = "";
                    txt_diem.Focus();
                }
            }
        }
        //Hiển thị GridControls lên textbox tương ứng
        private void DSKH_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
                txt_sdt.Text = DSKH.GetRowCellValue(e.FocusedRowHandle, "MAKH").ToString();
                txt_diem.Text = DSKH.GetRowCellValue(e.FocusedRowHandle, "DIEMTICHLUY").ToString();
        }

        //Hiển thị STT
        private void DSKH_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if(e.Info.IsRowIndicator&& e.RowHandle>=0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }    
        }
    }
}
