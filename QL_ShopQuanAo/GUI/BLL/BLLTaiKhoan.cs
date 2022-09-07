using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DTO;
using DAL;

namespace BLL
{
    public class BLLTaiKhoan
    {
        DALTaiKhoan daltk = new DALTaiKhoan();
        public int LoadTaiKhoan(int quyen)
        {
            return daltk.LoadTaiKhoan(quyen);
        }
        public THONGTINTAIKHOAN ThongTinTK(string maTK)
        {
            return daltk.ThongTinTK(maTK);
        }
        public TAIKHOAN DangNhap(string tenTK,string matkhau)
        {
            return daltk.DangNhap(tenTK,matkhau);
        }
        public void SuaTaiKhoan(string maTK, string hoten, string diachi, string ngaysinh, string gioitinh)
        {
            daltk.SuaTaiKhoan(maTK, hoten, diachi, ngaysinh, gioitinh);
        }
        public void DoiPass(string tenTK,string passMoi)
        {
            daltk.DoiMatKhau(tenTK, passMoi);
        }
        public TAIKHOAN kiemTraPass(string tenTK, string pass)
        {
            return daltk.ktPass(tenTK,pass);
        }
    }
}
