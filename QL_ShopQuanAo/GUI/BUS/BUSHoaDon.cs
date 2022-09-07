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
    public class BUSHoaDon
    {
        Data da = new Data();
        public DataTable getHD()
        {
            DataTable dt = null;
            String sql = "SET DATEFORMAT dmy;SELECT * FROM HOADON";
            dt = da.getTable(sql);
            return dt;
        }

        public bool kiemtraKC(int MAHD)
        {
            string sql = "SELECT  count (*) from  HOADON WHERE MAHD = '" + MAHD + "'";

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
        public bool kiemtraKN(int MAHD)
        {
            string sql = "SELECT  count (*) from  CHITIETHD WHERE MAHD = '" + MAHD + "'";

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
        public void insertHD(string NGTAO, double tt, string TINHTRANG, string MANV, string MAKH)
        {
            string sql = "INSERT INTO HOADON VALUES ('" + NGTAO + "','1','" + TINHTRANG + "','" + MAKH + "','" + MANV + "')";
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
        public void updateHD(string NGTAO, string TINHTRANG, string MANV, string MAKH, int MAHD)
        {
            String sql = "SET DATEFORMAT dmy; UPDATE HOADON set NGTAO='" + NGTAO + "' ,THANHTOAN='0',TINHTRANG='" + TINHTRANG + "',MANV='" + MANV + "',MAKH='" + MAKH + "'  where MAHD='" + MAHD + "'";
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
            String sql = "delete HOADON where MAHD='" + MAHD + "'";
            try
            {
                da.ExcuteNonQuery(sql);
                //MessageBox.Show("Xóa thành công !");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi CSDL !" + ex.Message);

            }
        }
        //BAN HANG
        public DataTable getHDBH(int MAHD)
        {
            DataTable dt = null;
            String sql = "SET DATEFORMAT dmy; SELECT CHITIETHD.MAHD,MANV,NGTAO,MAKH,CHITIETHD.MASP,TENSP,CHITIETHD.SIZE,CHITIETHD.SOLUONG,DONGIA,(CHITIETHD.SOLUONG*DONGIA) AS 'TONG' FROM HOADON,CHITIETHD,SANPHAM,KHO WHERE HOADON.MAHD=CHITIETHD.MAHD AND CHITIETHD.SIZE=KHO.SIZE AND CHITIETHD.MASP=SANPHAM.MASP AND KHO.MASP=SANPHAM.MASP AND  CHITIETHD.MAHD='" + MAHD + "'";
            dt = da.getTable(sql);
            return dt;
        }
        public DataTable getHDBill(int MAHD)
        {
            DataTable dt = null;
            String sql = "SELECT CHITIETHD.MAHD,MANV,NGTAO,MAKH,CHITIETHD.MASP,TENSP,CHITIETHD.SOLUONG,DONGIA,THANHTOAN FROM HOADON,CHITIETHD,SANPHAM WHERE HOADON.MAHD=CHITIETHD.MAHD AND CHITIETHD.MASP=SANPHAM.MASP AND  HOADON.MAHD='" + MAHD + "'";
            dt = da.getTable(sql);
            return dt;
        }
        public void insertHDBH(string NGTAO, string MANV, string MAKH)
        {
            string sql = "INSERT INTO HOADON VALUES ('" + NGTAO + "','1',N'Chưa thanh toán','" + MAKH + "','" + MANV + "')";
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
        public void updateHDBH(int mahd)
        {
            string sql = "UPDATE HOADON SET TINHTRANG = N'Đã thanh toán' where MAHD ='" + mahd + "'";
            try
            {
                da.ExcuteNonQuery(sql);
                MessageBox.Show("thanh toán thành công !");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("thanh toán thất bại !");
                MessageBox.Show(ex.Message);
            }
        }
        public void themvaothongke(int tien, int thang, int nam)
        {
            //DataTable themdv = new DataTable();
            
            string sql = "Update ThongKe set Doanhthu = Doanhthu + " + tien + " where thang = " + thang + " and nam = " + nam + "";
            try
            {
                da.ExcuteNonQueryS(sql);
                return;
            }
            catch
            {
                return;
            }

        }
        public DataTable getMaHD(string NGTAO, string MANV, string MAKH)
        {
            DataTable dt = null;
            String sql = "SELECT MAHD FROM HOADON WHERE NGTAO = '" + NGTAO + "' AND THANHTOAN = '1' AND MAKH = '" + MAKH + "' AND MANV = '" + MANV + "'";
            dt = da.getTable(sql);
            return dt;
        }
    }
}
