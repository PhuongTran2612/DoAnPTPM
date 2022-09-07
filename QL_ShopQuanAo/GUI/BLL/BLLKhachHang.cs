using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLKhachHang
    {
        DALKhachHang dalkh = new DALKhachHang();
        public List<KHACHHANG> LoadKH()
        {
            return dalkh.LoadKH();
        }
        public void ThemKH(string makh, int dtl)
        {
            dalkh.ThemKH(makh, dtl);
        }
        public void XoaKH(string makh)
        {
            dalkh.XoaKH(makh);
        }
        public void SuaKH(string makh, int dtl)
        {
            dalkh.SuaKH(makh, dtl);
        }
    }
}
