using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DTO;

namespace DAL
{
    public class DALSanPham
    {
        QLQADataContext qlqa = new QLQADataContext();
        public DALSanPham()
        { }

        public List<View_SanPham> LoadSanPham()
        {
            List<SANPHAM> kq = qlqa.SANPHAMs.Select(t => t).ToList<SANPHAM>();
            var kqnew = kq.ConvertAll(m => new View_SanPham()
            {
                MASP = m.MASP,
                TENSP = m.TENSP,
                DONGIA = m.DONGIA,
                HINHANH=m.HINHANH,
                NSX = m.NSX,
                MALSP = m.MALSP
            });
            return kqnew.ToList<View_SanPham>();
        }
        public List<SANPHAM> LoadSanPham(int masp)
        {
            return qlqa.SANPHAMs.Where(t => t.MASP==masp).ToList();
        }
        public void ThemSanPham(string tenSP, float dongia, string hinhanh, string nsx, string malsp)
        {
            SANPHAM sp = new SANPHAM();
            sp.TENSP = tenSP;
            sp.DONGIA = dongia;
            sp.HINHANH = hinhanh;
            sp.NSX = nsx;
            sp.MALSP = malsp;
            qlqa.SANPHAMs.InsertOnSubmit(sp);
            qlqa.SubmitChanges();
        }
        public void XoaSanPham(int maSP)
        {
            var xoa = qlqa.SANPHAMs.Single(t => t.MASP == maSP);
            qlqa.SANPHAMs.DeleteOnSubmit(xoa);
            qlqa.SubmitChanges();
        }
        public void SuaSanPham(int maSP,string tenSP, float dongia, string hinhanh, string nsx, string malsp)
        {
            SANPHAM sp = new SANPHAM();
            var sua = qlqa.SANPHAMs.Single(t => t.MASP == maSP);
            sua.TENSP = tenSP;
            sua.DONGIA = dongia;
            sua.HINHANH = hinhanh;
            sua.NSX = nsx;
            sua.MALSP = malsp;
            qlqa.SubmitChanges();
        }
        public List<View_SanPham> TimKiem(string timkiem)
        {
            List<View_SanPham> kq=new List<View_SanPham>();
            List<View_SanPham> kq_masp = qlqa.View_SanPhams.Where(t => t.MASP.ToString() == timkiem).ToList();
            if (kq_masp.Count()==0)
            {
                kq = qlqa.View_SanPhams.Where(t => t.TENSP.Contains(timkiem) == true).ToList();
            }
            else
            {
                kq = qlqa.View_SanPhams.Where(t => t.MASP == int.Parse(timkiem) || t.TENSP.Contains(timkiem) == true).ToList();
            }
            return kq;
        }

        //public List<View_SanPham> TimKiemTheoTenSP(string tenSP)
        //{
        //    return qlqa.View_SanPhams.Where(t => t.TENSP.Contains(tenSP)==true).ToList();
        //}
        public int ktMaSP(int maSP)
        {
            return qlqa.SANPHAMs.Where(t => t.MASP == maSP).Count();
        }
        public int ktTenSP(string tenSP)
        {
            return qlqa.SANPHAMs.Where(t => t.TENSP.Contains(tenSP) == true).Count();
        }
        public int ktKho(int maSP)
        {
            return qlqa.KHOs.Where(t => t.MASP == maSP).Count();
        }
    }

}
