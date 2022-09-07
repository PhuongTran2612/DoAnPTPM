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
using DevExpress.XtraCharts;

namespace GUI
{
    public partial class FrmThongKe : DevExpress.XtraEditors.XtraUserControl
    {
        QLQADataContext qlqa = new QLQADataContext();
        BLLThongKe blltk = new BLLThongKe();
        public FrmThongKe()
        {
            InitializeComponent();
        }

        private void FrmThongKe_Load(object sender, EventArgs e)
        {
            cbbNam.SelectedIndexChanged -= cbbNam_SelectedIndexChanged;

            var CBO_Nam = (from n in qlqa.THONGKEs select n.NAM).Distinct();
            cbbNam.DataSource = CBO_Nam;

            int namtk = int.Parse(cbbNam.SelectedValue.ToString());
            dataGridView1.DataSource = blltk.LoadThongKe(namtk);
            chartDoanhThu.DataSource = blltk.LoadThongKe(namtk);
            chartDoanhThu.Series["DoanhThu"].XValueMember = "Thang";
            chartDoanhThu.Series["DoanhThu"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            chartDoanhThu.Series["DoanhThu"].YValueMembers = "DoanhThu";
            chartDoanhThu.Series["DoanhThu"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            cbbNam.SelectedIndexChanged += cbbNam_SelectedIndexChanged;
        }

        private void cbbNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            int namtk = int.Parse(cbbNam.SelectedValue.ToString());
            chartDoanhThu.DataSource = blltk.LoadThongKe(namtk);
            chartDoanhThu.Series["DoanhThu"].XValueMember = "Thang";
            chartDoanhThu.Series["DoanhThu"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            chartDoanhThu.Series["DoanhThu"].YValueMembers = "DoanhThu";
            chartDoanhThu.Series["DoanhThu"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            dataGridView1.DataSource = blltk.LoadThongKe(namtk);
        }
    }
}
