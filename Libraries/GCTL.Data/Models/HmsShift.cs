﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace GCTL.Data.Models
{
    public partial class HmsShift
    {
        public decimal AutoId { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string ShiftShortName { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string Luser { get; set; }
        public DateTime? Ldate { get; set; }
        public string Lip { get; set; }
        public string Lmac { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string BanglaShift { get; set; }
        public string BanglaShortName { get; set; }
        public string EmployeeId { get; set; }
        public string CompanyCode { get; set; }
    }
}