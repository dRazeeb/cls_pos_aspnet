﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace GCTL.Data.Models
{
    public partial class AccVoucherEntryDetails
    {
        public decimal AutoId { get; set; }
        public decimal VoucherEntryAutoId { get; set; }
        public string VoucherEntryDetailsCodeNo { get; set; }
        public string AccCode { get; set; }
        public string TrType { get; set; }
        public string Description { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Luser { get; set; }
        public DateTime? Ldate { get; set; }
        public string Lip { get; set; }
        public string Lmac { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual AccVoucherEntry VoucherEntryAuto { get; set; }
    }
}