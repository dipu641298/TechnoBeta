﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnoBeta.Areas.Admin.Models.ViewModels.Shop
{
    public class OrdersForAdminVM
    {
        public int OrderNumber { get; set; }
        public string Username { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }
        public string Delivery_address { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}