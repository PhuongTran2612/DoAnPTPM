using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class DALThongKe
    {
        QLQADataContext qlqa = new QLQADataContext();
        public List<THONGKE> LoadThongKe(int nam)
        {
            return qlqa.THONGKEs.Where(t => t.NAM==nam).ToList();
        }
    }
}
