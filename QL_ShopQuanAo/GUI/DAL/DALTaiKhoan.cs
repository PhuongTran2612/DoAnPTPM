using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALTaiKhoan
    {
        QLQADataContext qlqa = new QLQADataContext();
        
        public int LoadTaiKhoan(int quyen)
        {
            return qlqa.TAIKHOANs.Where(t => t.MAQUYEN == quyen).Count();
        }
        public TAIKHOAN DangNhap(string tenTK,string matkhau)
        {
            return qlqa.TAIKHOANs.SingleOrDefault(t => t.TENTK == tenTK && t.MATKHAU == matkhau);
        }
        public THONGTINTAIKHOAN ThongTinTK(string maTK)
        {
            return qlqa.THONGTINTAIKHOANs.SingleOrDefault(t => t.MATK == maTK);
        }
        public void SuaTaiKhoan(string maTK, string hoten, string diachi, string ngaysinh, string gioitinh)
        {
            THONGTINTAIKHOAN tk = new THONGTINTAIKHOAN();
            var sua = qlqa.THONGTINTAIKHOANs.Single(t => t.MATK == maTK);
            sua.HOTEN = hoten;
            sua.DCHI = diachi;
            sua.NGSINH = DateTime.Parse(ngaysinh);
            sua.GTINH = gioitinh;
            qlqa.SubmitChanges();
        }
        public void DoiMatKhau(string tenTK, string passMoi)
        {
            var sua = qlqa.TAIKHOANs.Single(t => t.TENTK == tenTK);
            sua.MATKHAU = passMoi;
            qlqa.SubmitChanges();
        }
        public TAIKHOAN ktPass(string tenTK, string pass)
        {
            return qlqa.TAIKHOANs.SingleOrDefault(t =>t.TENTK==tenTK && t.MATKHAU == pass);
        }
    }
}
