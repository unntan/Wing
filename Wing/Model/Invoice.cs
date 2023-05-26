using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wing.Model
{
    class Invoice
    {
        // 請求番号
        public int No { get; set; }

        // 発行年
        public int Year { get; set; }

        // 発行月
        public int Month { get; set; }

        // 対象企業
        public string ToCompany { get; set; }

        // 対象担当者
        public string Manager { get; set; }

        // 現場名
        public string SiteName { get; set; }

        // 金額
        public int Quantity { get; set; }

        // 単位
        public string Unit { get; set; }

        // 単価
        public int UnitPrice { get; set; }

        // 金額
        public int Amount { get; set; }

        // 備考
        public string Remarks { get; set; }

        // 作成日
        public string CreateDateTime { get; set; }

        // 作成ユーザ
        public string CreateUser { get; set; }

        // 更新日
        public string UpdateDateTime { get; set; }

        // 更新ユーザ
        public string UpdateUser { get; set; }
    }
}
