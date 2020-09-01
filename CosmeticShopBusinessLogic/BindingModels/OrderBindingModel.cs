﻿using CosmeticShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BindingModels
{
    public class OrderBindingModel
    {
        public int? Id { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public Status Status { get; set; }
        public int SetId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}