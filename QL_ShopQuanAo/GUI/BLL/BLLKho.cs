using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class BLLKho
    {
        DALKho dalkho = new DALKho();
        public int ktSL(int ma, int sl, string size)
        {
            return dalkho.ktSL(ma, sl, size);
        }
        public List<KHO> LoadKho(int ma)
        {
            return dalkho.LoadKho(ma);
        }
        public List<View_Kho> LoadViewKho(int ma)
        {
            return dalkho.LoadViewKho(ma);
        }
        public void Them(int ma, string size, int sl)
        {
            dalkho.Them(ma, size, sl);
        }
        public void Xoa(int ma, string size)
        {
            dalkho.Xoa(ma, size);
        }
        public void Sua(int ma, string size, int sl)
        {
            dalkho.Sua(ma, size, sl);
        }
        public int ktTonTaiSize(int MASP,string size)
        {
            return dalkho.ktTonTaiSize(MASP, size);
        }
    }
}
