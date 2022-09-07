using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALHoaDon
    {
        QLQADataContext qlqa = new QLQADataContext();
        public DALHoaDon()
        {

        }
        public List<View_HoaDon> LoadHoaDon()
        {
            List<HOADON> kq = qlqa.HOADONs.Select(t => t).ToList<HOADON>();
            var kqnew = kq.ConvertAll(m => new View_HoaDon()
            {
                MAHD = m.MAHD,
                NGTAO = m.NGTAO,
                THANHTOAN=m.THANHTOAN,
                TINHTRANG=m.TINHTRANG,
                MAKH=m.MAKH,
                MANV=m.MANV
            });
            return kqnew.ToList<View_HoaDon>();
        }
        public void ThemHoaDon(string ngTao,float thanhToan, string tinhTrang, string maKH, string maNV)
        {
            HOADON hd = new HOADON();
            //hd.MAHD = mahd;
            hd.NGTAO =DateTime.Parse(ngTao);
            hd.THANHTOAN=thanhToan;
            hd.TINHTRANG=tinhTrang;
            hd.MAKH=maKH;
            hd.MANV=maNV;
            qlqa.HOADONs.InsertOnSubmit(hd);
            qlqa.SubmitChanges();
        }
        public void XoaHoaDon(int maHD)
        {
            var xoa = qlqa.HOADONs.Single(t => t.MAHD == maHD);
            qlqa.HOADONs.DeleteOnSubmit(xoa);
            qlqa.SubmitChanges();
        }
        public void SuaHoaDon(int maHD,string ngTao,float thanhToan,string tinhTrang,string maKH,string maNV )
        {
            HOADON hd = new HOADON();
            var sua = qlqa.HOADONs.Single(t => t.MAHD == maHD);
            sua.NGTAO = DateTime.Parse(ngTao);
            sua.THANHTOAN=thanhToan;
            sua.TINHTRANG=tinhTrang;
            sua.MAKH=maKH;
            sua.MANV=maNV;
            qlqa.SubmitChanges();
        }
        public int ktMaHD(int maHD)
        {
            return qlqa.CHITIETHDs.Where(t => t.MAHD == maHD).Count();
        }
        public List<View_HoaDon> TimKiem(string timkiem)
        {
            List<View_HoaDon> kq = new List<View_HoaDon>();
            List<View_HoaDon> kq_mahd = qlqa.View_HoaDons.Where(t => t.MAHD.ToString() == timkiem).ToList();
            if (kq_mahd.Count() == 0)
            {
                kq = qlqa.View_HoaDons.Where(t => t.MANV.Contains(timkiem) == true).ToList();
            }
            else
            {
                kq = qlqa.View_HoaDons.Where(t => t.MAHD == int.Parse(timkiem) || t.MANV.Contains(timkiem) == true).ToList();
            }
            return kq;
        }
    }
}
