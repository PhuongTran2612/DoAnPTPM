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
using DTO;
using BLL;
using System.IO;
using System.Drawing.Imaging;

namespace GUI
{
    public partial class FrmSanPham : DevExpress.XtraEditors.XtraUserControl
    {
        public FrmSanPham()
        {
            InitializeComponent();
        }
        BLLSanPham bllSP = new BLLSanPham();
        BUSKho kho = new BUSKho();
        QLQADataContext qlqa = new QLQADataContext();
        private static int mASP; //phương thức lấy mã sp

        public static int MASP 
        {
            get { return FrmSanPham.mASP; }
            set { FrmSanPham.mASP = value; }
        }

        private void FrmSanPham_Load(object sender, EventArgs e)
        {
            //bảng sản phẩm
            dataGridView1.DataSource = bllSP.LoadSanPham();
            var CBO_MaLSP = from sp in qlqa.LOAISPs
                            select new
                            {
                                sp.MALSP
                            };
            cbo_MALSP.DataSource = CBO_MaLSP;
            cbo_MALSP.ValueMember = "MALSP";
            cbo_MALSP.DisplayMember = "MALSP";
        }
        public void nhapHinhAnh()
        {
            //BMP, GIF, JPEG, EXIF, PNG và TIFF, ICO...
            openFileDialog1.Filter = "All Image (*.jpg)|*.jpg|All Image (*.png)|*.png|All Image (*.gif)|*.gif|All Image (*.jpn)|*.jpn";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                picHA.Image = Image.FromFile(openFileDialog1.FileName.ToString());

                string[] name = openFileDialog1.FileName.Split('\\');
                string tenFile = name[name.Length - 1].ToString().Trim();

                txtAnh.Text = tenFile;
            }
        }

        public void luuHinhAnh(string tenFile)
        {
            bool kt = File.Exists(System.Windows.Forms.Application.StartupPath + "\\img\\" + tenFile);
            if (kt == false)
                picHA.Image.Save(System.Windows.Forms.Application.StartupPath + "\\img\\" + tenFile, ImageFormat.Png);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMSP.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtTenSP.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtDG.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtAnh.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtNSX.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            cbo_MALSP.SelectedItem = dataGridView1.CurrentRow.Cells[5].Value.ToString();

            bool ktFileTonTai = File.Exists(System.Windows.Forms.Application.StartupPath + "\\img\\" + dataGridView1.Rows[e.RowIndex].Cells["Column5"].FormattedValue.ToString());
            if (ktFileTonTai == true)
            {
                picHA.Image = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\img\\" + dataGridView1.Rows[e.RowIndex].Cells["Column5"].FormattedValue.ToString() + "");
            }
            else
                anhDefault("empty.jpg");

            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                //Đưa dữ liệu vào
                MASP = int.Parse(row.Cells[1].Value.ToString());
                FrmKho childForm = new FrmKho();
                childForm.Show();

            }
        }
        public void anhDefault(string tenFile)
        {
            picHA.Image = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\img\\" + tenFile);
            txtAnh.Text = tenFile;
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (txtTimKiem.TextLength == 0)
            {
                MessageBox.Show("Vui lòng nhập thông tin cần tìm!");
                txtTimKiem.Focus();
            }
            dataGridView1.DataSource = bllSP.TimKiem(txtTimKiem.Text);
        }

        private void butonQuanAo1_Click(object sender, EventArgs e)
        {
            nhapHinhAnh();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FrmSanPham sp = new FrmSanPham();
            string url = txtAnh.Text;
            bllSP.ThemSanPham(txtTenSP.Text, float.Parse(txtDG.Text), txtAnh.Text, txtNSX.Text, cbo_MALSP.SelectedValue.ToString());
            MessageBox.Show("Thêm Sản Phẩm Thành Công");
            luuHinhAnh(url);
            FrmSanPham_Load(sender, e);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (bllSP.ktKho(int.Parse(txtMSP.Text)) > 0)
            {
                MessageBox.Show("Không Thể Xoá Được Sản Phẩm Này");
                return;
            }
            else
            {
                bllSP.XoaSP(int.Parse(txtMSP.Text));
                MessageBox.Show("Xoá Sản Phẩm Thành Công");
                FrmSanPham_Load(sender, e);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            bllSP.SuaSanPham(int.Parse(txtMSP.Text), txtTenSP.Text, float.Parse(txtDG.Text), txtAnh.Text, txtNSX.Text, cbo_MALSP.SelectedValue.ToString());
            MessageBox.Show("Sửa Sản Phẩm Thành Công");
            FrmSanPham_Load(sender, e);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
           
        }
    }
}
