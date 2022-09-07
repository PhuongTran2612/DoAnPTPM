using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLSanPham
    {
        DALSanPham dalsp = new DALSanPham();
        public List<View_SanPham> LoadSanPham()
        {
            return dalsp.LoadSanPham();
        }
        //public List<SANPHAM> LoadSanPham()
        //{
        //    return dalsp.LoadSanPham();
        //}
        public List<SANPHAM> LoadSanPham(int masp)
        {
            return dalsp.LoadSanPham(masp);
        }
        public void ThemSanPham(string tensp, float dongia, string hinhanh, string nsx, string malsp)
        {
            dalsp.ThemSanPham(tensp,dongia, hinhanh, nsx, malsp);
        }
        public void XoaSP(int maSP)
        {
            dalsp.XoaSanPham(maSP);
        }
        public void SuaSanPham(int maSP, string tensp, float dongia, string hinhanh, string nsx, string malsp)
        {
            dalsp.SuaSanPham(maSP, tensp, dongia,hinhanh, nsx, malsp);
        }
        public int ktTenSP(string tenSP)
        {
            return dalsp.ktTenSP(tenSP);
        }
        public List<View_SanPham> TimKiem(string timkiem)
        {
            return dalsp.TimKiem(timkiem);
        }

        //public List<View_SanPham> TimKiemTheoTenSP(string tenSP)
        //{
        //    return dalsp.TimKiemTheoTenSP(tenSP);
        //}
        public int ktMaSP(int maSP)
        {
            return dalsp.ktMaSP(maSP);
        }
        public int ktKho(int maSP)
        {
            return dalsp.ktKho(maSP);
        }
    }
}
