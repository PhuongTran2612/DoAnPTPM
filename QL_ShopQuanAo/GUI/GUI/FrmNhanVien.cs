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
using System.Text.RegularExpressions;

namespace GUI
{
    public partial class FrmNhanVien : DevExpress.XtraEditors.XtraUserControl
    {
        public FrmNhanVien()
        {
            InitializeComponent();
        }
        BUSNhanVien nv = new BUSNhanVien();
        public static bool isEmail(string inputEmail)
        {
            inputEmail = inputEmail ?? string.Empty;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        public bool IsValidVietNamPhoneNumber(string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
                return false;
            string sMailPattern = @"^((09(\d){8})|(03(\d){8})|(08(\d){8})|(07(\d){8})|(05(\d){8}))$";
            return Regex.IsMatch(phoneNum.Trim(), sMailPattern);
        }
        
        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            dtv_nhanvien.DataSource = nv.getNV();
        }






        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void dtv_nhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //var senderGrid = (DataGridView)sender;

            //if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
            //    e.RowIndex >= 0)
            //{
            //    //TODO - Button Clicked - Execute Code Here
            //    DataGridViewRow row = this.dtv_nhanvien.Rows[e.RowIndex];
            //    //Đưa dữ liệu vào 
            //    nv.updateMATKHAU(row.Cells[9].Value.ToString());

            //}
        }

        private void dtv_nhanvien_SelectionChanged(object sender, EventArgs e)
        {
            //Hiển thị thông tin tƣơng ứng lên các textbox
            txt_mnv.Text = dtv_nhanvien.CurrentRow.Cells[0].Value.ToString();
            txt_tennv.Text = dtv_nhanvien.CurrentRow.Cells[1].Value.ToString();
            cb_gioitinh.Text = dtv_nhanvien.CurrentRow.Cells[3].Value.ToString();
            txt_sdt.Text = dtv_nhanvien.CurrentRow.Cells[6].Value.ToString();
            txt_diachi.Text = dtv_nhanvien.CurrentRow.Cells[7].Value.ToString();
            EMAIL.Text = dtv_nhanvien.CurrentRow.Cells[5].Value.ToString();
            MATK.Text = dtv_nhanvien.CurrentRow.Cells[8].Value.ToString();
            TENTK.Text = dtv_nhanvien.CurrentRow.Cells[9].Value.ToString();
            txtMK.Text = dtv_nhanvien.CurrentRow.Cells[10].Value.ToString();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txt_mnv.Text.Length == 0 || txt_tennv.Text.Length == 0 || txt_sdt.Text.Length == 0 || txt_diachi.Text.Length == 0 || cb_gioitinh.Text.Length == 0 || EMAIL.Text.Length == 0 || MATK.Text.Length == 0 || TENTK.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi thêm!");
                return;
            }
            if (isEmail(EMAIL.Text) == false)
            {
                MessageBox.Show("Email sai định dạng");
                return;
            }
            if (IsValidVietNamPhoneNumber(txt_sdt.Text) == false)
            {
                MessageBox.Show("Số điện thoại sai định dạng!");
                return;
            }
            if (nv.kiemtraKCNV(txt_mnv.Text) == false)
            {
                MessageBox.Show("Mã nhân viên này đã tồn tại!");
                return;
            }
            if (nv.kiemtraKCTK(MATK.Text) == false)
            {
                MessageBox.Show("Mã tài khoản này đã tồn tại!");
                return;
            }
            if (nv.kiemtraTrungemail(EMAIL.Text, "THONGTINTAIKHOAN") == false)
            {
                MessageBox.Show("Email này đã tồn tại!");
                return;
            }
            if (nv.kiemtraTrungSDT(txt_sdt.Text, "THONGTINTAIKHOAN") == false)
            {
                MessageBox.Show("Số điện thoại này đã tồn tại!");
                return;
            }
            if (nv.kiemtraTrungTENTK(TENTK.Text, "TAIKHOAN") == false)
            {
                MessageBox.Show("Tên tài khoản này đã tồn tại!");
                return;
            }
            else
            {
                nv.insertNV(MATK.Text, TENTK.Text, txtMK.Text, txt_tennv.Text, dt_ngaysinh.Value.ToString(), EMAIL.Text, txt_diachi.Text, cb_gioitinh.Text, txt_sdt.Text, txt_mnv.Text);
                dtv_nhanvien.DataSource = nv.getNV();
            }

        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn xóa?", "Thông báo",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                             MessageBoxDefaultButton.Button2) ==

                             System.Windows.Forms.DialogResult.Yes)
            {
                if (MATK.Text == string.Empty)
                {
                    MessageBox.Show("Mã tài khoản không được rỗng");
                    return;
                }
                else
                {
                    nv.UpdateNghiViec(MATK.Text);
                    dtv_nhanvien.DataSource = nv.getNV();
                }
            }
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txt_mnv.Text.Length == 0 || txt_tennv.Text.Length == 0 || txt_sdt.Text.Length == 0 || txt_diachi.Text.Length == 0 || cb_gioitinh.Text.Length == 0 || EMAIL.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi thêm!");
                return;
            }
            if (isEmail(EMAIL.Text) == false)
            {
                MessageBox.Show("Email sai định dạng");
                return;
            }
            if (IsValidVietNamPhoneNumber(txt_sdt.Text) == false)
            {
                MessageBox.Show("Số điện thoại sai định dạng!");
                return;
            }
            if (nv.kiemtraKCNV(txt_mnv.Text) == true)
            {
                MessageBox.Show("Mã nhân viên này ko tồn tại!");
                return;
            }
            if (nv.kiemtraKCTK(MATK.Text) == true)
            {
                MessageBox.Show("Mã tài khoản này ko tồn tại!");
                return;
            }
            else
            {
                nv.updateNV(txt_tennv.Text, dt_ngaysinh.Value.ToString("dd/MM/yyyy"), EMAIL.Text, txt_diachi.Text, cb_gioitinh.Text, txt_sdt.Text, txt_mnv.Text);
                dtv_nhanvien.DataSource = nv.getNV();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dtv_nhanvien.DataSource = nv.Search(txtTimKiem.Text);
        }
    }
}
