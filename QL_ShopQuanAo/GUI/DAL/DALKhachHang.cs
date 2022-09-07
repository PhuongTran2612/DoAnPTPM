using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class DALKhachHang
    {
        QLQADataContext qlqa = new QLQADataContext();
        public List<KHACHHANG> LoadKH()
        {
            return qlqa.KHACHHANGs.Select(t => t).ToList();
        }
        public void ThemKH(string makh, int dtl)
        {
            KHACHHANG kh = new KHACHHANG();
            kh.MAKH = makh;
            kh.DIEMTICHLUY = dtl;
            qlqa.KHACHHANGs.InsertOnSubmit(kh);
            qlqa.SubmitChanges();
        }
        public void XoaKH(string makh)
        {
            var xoa = qlqa.KHACHHANGs.Single(t => t.MAKH == makh);
            qlqa.KHACHHANGs.DeleteOnSubmit(xoa);
            qlqa.SubmitChanges();
        }
        public void SuaKH(string makh,int dtl)
        {
            var sua = qlqa.KHACHHANGs.Single(t => t.MAKH == makh);
            sua.DIEMTICHLUY = dtl;
            qlqa.SubmitChanges();
        }
    }
}
