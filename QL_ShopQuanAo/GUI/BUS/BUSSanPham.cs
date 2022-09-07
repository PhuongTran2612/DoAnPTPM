using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BUS
{
    public class BUSSanPham
    {
        Data da = new Data();
        public DataTable getSP()
        {
            DataTable dt = null;
            String sql = "SELECT MASP, TENSP,DONGIA FROM SANPHAM";
            dt = da.getTable(sql);
            return dt;
        }
        public DataTable Search(String condi)
        {
            DataTable dt = null;
            String sql = "Select MASP, TENSP,DONGIA from SANPHAM where TENSP like N'%" + condi + "%'";
            dt = da.getTable(sql);
            return dt;
        }
        public DataTable UpdateSP(int masp,int sl)
        {
            DataTable dt = null;
            String sql = "UPDATE SANPHAM SET SOLUONG = SOLUONG -'"+ sl +"' WHERE MASP = '"+ masp +"'";
            dt = da.getTable(sql);
            return dt;
        }
    }
}
