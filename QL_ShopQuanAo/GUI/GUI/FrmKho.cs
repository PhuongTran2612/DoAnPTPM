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
using BLL;

namespace GUI
{
    public partial class FrmKho : DevExpress.XtraEditors.XtraForm
    {
        
        public FrmKho()
        {
            InitializeComponent();
        }
        QLQADataContext qlqa = new QLQADataContext();
        int MASP = FrmSanPham.MASP;
        BUSSanPham sp = new BUSSanPham();
        BUSKho kho = new BUSKho();
        BLLKho bllkho = new BLLKho();
        private void FrmKho_Load(object sender, EventArgs e)
        { 
            //bảng kho
            txtMSP.Text = MASP.ToString();
            var CBO_SIZE = (from s in qlqa.KHOs select s.SIZE).Distinct();
            cbo_Size.DataSource = CBO_SIZE;
            dataGridView1.DataSource = kho.getKhoSP(MASP);
            kho.getKho();
            kho.getKhoSLNH10();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMSP.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            cbo_Size.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtSl.Text= dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtSl.TextLength == 0)
            {
                MessageBox.Show("Vui lòng nhâp đầy đủ thông tin");
                return;
            }
            else
            {

                kho.UpdateKhoSP(int.Parse(txtMSP.Text), cbo_Size.SelectedValue.ToString(), int.Parse(txtSl.Text));
                MessageBox.Show("Update Thành Công");
                FrmKho_Load(sender, e);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            bllkho.Xoa(int.Parse(txtMSP.Text), cbo_Size.SelectedValue.ToString());
            MessageBox.Show("Xoá Thành Công");
            FrmKho_Load(sender, e);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtSl.TextLength == 0)
            {
                MessageBox.Show("Vui lòng nhâp đầy đủ thông tin");
                return;
            }
            else
            {
                bllkho.Sua(int.Parse(txtMSP.Text), cbo_Size.SelectedValue.ToString(), int.Parse(txtSl.Text));
                MessageBox.Show("Sửa Thành Công");
                FrmKho_Load(sender, e);
            }
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            if (txtSl.TextLength == 0)
            {
                MessageBox.Show("Vui lòng nhâp đầy đủ thông tin");
                return;
            }
            else
            {
                bllkho.Them(int.Parse(txtMSP.Text), cbo_Size.SelectedValue.ToString(), int.Parse(txtSl.Text));
                MessageBox.Show("Thêm Thành Công");
                FrmKho_Load(sender, e);
            }
        }
    }
}