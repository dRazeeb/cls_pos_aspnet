﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace GCTL.Data.Models
{
    public partial class CoreUserInfo
    {
        public CoreUserInfo()
        {
            ProductCategoryCreatedByNavigation = new HashSet<ProductCategory>();
            ProductCategoryUpdatedByNavigation = new HashSet<ProductCategory>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string AccessCode { get; set; }
        public string EmployeeId { get; set; }
        public string CompanyCode { get; set; }
        public string Role { get; set; }
        public DateTime? EntryDate { get; set; }
        public string Luser { get; set; }
        public DateTime? Ldate { get; set; }
        public string Lip { get; set; }
        public string Lmac { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual ICollection<ProductCategory> ProductCategoryCreatedByNavigation { get; set; }
        public virtual ICollection<ProductCategory> ProductCategoryUpdatedByNavigation { get; set; }
    }
}