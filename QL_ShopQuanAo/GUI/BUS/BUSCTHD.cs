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
    public class BUSCTHD
    {
        Data da = new Data();
        public DataTable getCTHD(int MAHD)
        {
            DataTable dt = null;
            String sql = "SELECT CHITIETHD.MASP,TENSP,CHITIETHD.SIZE,CHITIETHD.SOLUONG FROM CHITIETHD ,SANPHAM WHERE SANPHAM.MASP=CHITIETHD.MASP AND MAHD='" + MAHD + "'";
            dt = da.getTable(sql);
            return dt;
        }

        public bool kiemtraKC(int MAHD, string SIZE,int MASP)
        {
            string sql = "SELECT  count (*) from  CHITIETHD WHERE MAHD = '" + MAHD + "' AND SIZE ='" + SIZE + "' AND MASP ='" + MASP + "'";

            try
            {
                int kq = (int)da.ExcuteScalar(sql);//lay gia tri do mang ve chuuong trinh
                if (kq > 0)
                {
                    return false;
                }
                else return true;

            }
            catch (SqlException ex)
            {
                return false;
                MessageBox.Show(ex.Message);
            }
        }
        public bool ktraSL(int sl, int masp)
        {
            String soluongSP = "SELECT SOLUONG FROM SANPHAM WHERE MASP = '" + masp + "'";
            if (sl > int.Parse(soluongSP))
            {
                return false;
            }
            else return true;
        }
        public void insertCTHD(int MAHD, int MASP,string SIZE, int SOLUONG)
        {
            string sql = " insert into CHITIETHD values ('" + MAHD + "','" + MASP + "','" + SIZE + "','" + SOLUONG + "')";
            try
            {
                da.ExcuteNonQuery(sql);
                MessageBox.Show("Thêm thành công !");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Thêm thất bại !");
                MessageBox.Show(ex.Message);
            }
        }
        public void updateHD(int MAHD, string SIZE,int MASP, int SOLUONG)
        {
            //String sql = "UPDATE CHITIETHD set MASP='" + MASP + "' , SOLUONG='" + SOLUONG + "' where MAHD='" + MAHD + "'";
            //String sl = "select SOLUONG FROM SANPHAM SP WHERE MASP = '" + MASP + "'";
            //if (SOLUONG > int.Parse(sl))
            //{
            //    MessageBox.Show("Quá số lượng trong kho!");
            //    return;
            //}
            String sql = "UPDATE CHITIETHD set SOLUONG='" + SOLUONG + "' where MAHD='" + MAHD + "' AND SIZE= '" + SIZE + "' AND MASP= '" + MASP + "'";
            try
            {
                da.ExcuteNonQuery(sql);
                MessageBox.Show("Sửa thành công !");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Sửa thất bại !");
                MessageBox.Show(ex.Message);
            }
        }
        public void deleteHD(int MAHD)
        {
            String sql = "delete CHITIETHD where MAHD='" + MAHD + "'";
            try
            {
                da.ExcuteNonQuery(sql);
                MessageBox.Show("Xóa thành công !");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi CSDL !" + ex.Message);

            }
        }
        public void deletectHD(int MASP)
        {
            String sql = "delete CHITIETHD where MASP='" + MASP + "'";
            try
            {
                da.ExcuteNonQuery(sql);
                MessageBox.Show("Xóa thành công !");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi CSDL !" + ex.Message);

            }
        }
    }
}
