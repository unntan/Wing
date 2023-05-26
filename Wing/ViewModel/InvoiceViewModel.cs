using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wing.ViewModel
{
    class InvoiceViewModel
    {
        public InvoiceViewModel()
        {

        }

        public int No { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string ToCompany { get; set; }
        public string Manager { get; set; }
        public string GenbaMei { get; set; }
        public Double Suryo { get; set; }
        public string Tani { get; set; }
        public Double Tanka { get; set; }
        public bool InTax { get; set; }
        public int Kingaku { get; set; }
        public string Biko { get; set; }
    }
}
