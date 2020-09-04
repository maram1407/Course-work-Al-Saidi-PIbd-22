﻿using CosmeticShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BindingModels
{
    public class RequestBindingModel
    {
        public int? Id { get; set; }
        public string SupplierFIO { get; set; }
        public int SupplierId { get; set; }
        public RequestStatus Status { get; set; }
        public Dictionary<int, (string, int, bool)> Cosmetics { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal Sum { get; set; }
    }
}