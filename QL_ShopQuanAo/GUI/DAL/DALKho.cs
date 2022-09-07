using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DTO;

namespace DAL
{
    public class DALKho
    {
        QLQADataContext qlqa = new QLQADataContext();
        public int ktSL(int ma, int sl, string size)
        {
            var getma = qlqa.KHOs.SingleOrDefault(t => t.MASP == ma && t.SIZE==size);
            if (getma.SOLUONG < sl)
            {
                return 1;
            }
            else return 0;
        }
        public List<KHO> LoadKho(int ma)
        {
            return qlqa.KHOs.Where(t => t.MASP == ma).ToList();
        }
        public List<View_Kho> LoadViewKho(int ma)
        {
            List<KHO> kq = qlqa.KHOs.Where(t => t.MASP==ma).ToList();
            var kqnew = kq.ConvertAll(m => new View_Kho()
            {
                MASP = m.MASP,
                SIZE=m.SIZE,
                SOLUONG=m.SOLUONG,
            });
            return kqnew.ToList<View_Kho>();
        }
        public void Them(int ma, string size, int sl)
        {
            KHO k = new KHO();
            k.MASP = ma;
            k.SIZE = size;
            k.SOLUONG = sl;
            qlqa.KHOs.InsertOnSubmit(k);
            qlqa.SubmitChanges();
        }
        public void Xoa(int ma, string size)
        {
            var xoa = qlqa.KHOs.Single(t => t.MASP == ma && t.SIZE==size);
            qlqa.KHOs.DeleteOnSubmit(xoa);
            qlqa.SubmitChanges();
        }
        public void Sua(int ma, string size, int sl)
        {
            var sua = qlqa.KHOs.SingleOrDefault(t => t.MASP == ma && t.SIZE==size);
            sua.SOLUONG = sl;
            qlqa.SubmitChanges();
        }
        public int ktTonTaiSize(int MASP, string size)
        {
            var getma = qlqa.KHOs.Single(t => t.MASP == MASP);
            if (getma.SIZE == size)
            {
                return 1;
            }
            else return 0;
        }
    }
}
