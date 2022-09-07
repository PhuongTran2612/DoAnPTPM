using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using BUS;

namespace BLL
{
    public class BUSKho
    {
        Data da = new Data();
        public DataTable getKho(int MASP)
        {
            DataTable dt = null;
            String sql = "SELECT MASP,SIZE,SOLUONG FROM KHO WHERE MASP='" + MASP + "'";
            dt = da.getTable(sql);
            return dt;
        }
        public DataTable getKho()
        {
            DataTable dt = null;
            String sql = "SET DATEFORMAT dmy; SELECT * FROM KHO";
            dt = da.getTable(sql);
            return dt;
        }
        public DataTable UpdateKho(int masp, string size, int sl)
        {
            DataTable dt = null;
            String sql = "UPDATE KHO SET SOLUONG = SOLUONG -'" + sl + "' WHERE MASP = '" + masp + "' AND SIZE = '" + size + "'";
            dt = da.getTable(sql);
        return dt;
        }
        public DataTable UpdateKhoSP(int masp, string size, int sl)
        {
            DataTable dt = null;
            String sql = "UPDATE KHO SET SOLUONG = SOLUONG +'" + sl + "' WHERE MASP = '" + masp + "' AND SIZE = '" + size + "'";
            dt = da.getTable(sql);
            return dt;
        }
        public DataTable getKhoSP(int MASP)
        {
            DataTable dt = null;
            String sql = "SELECT MASP,SIZE,SOLUONG FROM KHO WHERE MASP='" + MASP + "'";
            dt = da.getTable(sql);
            return dt;
        }
        public DataTable getKhoSLNH10()
        {
            DataTable dt = null;
            String sql = "SET DATEFORMAT dmy; SELECT K.MASP,TENSP,SIZE,SOLUONG,DONGIA,HINHANH,NSX,MALSP FROM KHO K,SANPHAM SP WHERE K.MASP=SP.MASP AND SOLUONG < '" + 10 +"'";
            dt = da.getTable(sql);
            return dt;
        }

    }
   
}
