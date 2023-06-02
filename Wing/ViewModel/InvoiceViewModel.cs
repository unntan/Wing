﻿using System;
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

        public bool UpdateFlag { get; set; }
        public int No { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int ToCompany { get; set; }
        public int Manager { get; set; }
        public string GenbaMei { get; set; }
        public Double Suryo { get; set; }
        public string Tani { get; set; }
        public Double Tanka { get; set; }
        public bool InTax { get; set; }
        public int Kingaku { get; set; }
        public string Biko { get; set; }
    }
}
