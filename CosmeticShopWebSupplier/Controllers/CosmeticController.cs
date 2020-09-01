using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CosmeticShopBusinessLogic.BusinessLogics;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopDatabaseImplement.Implements;
using CosmeticShopBusinessLogic.BindingModels;

namespace CosmeticShopWebSupplier.Controllers
{
    public class CosmeticController : Controller
    {
        private readonly ICosmeticLogic cosmeticLogic;

        public CosmeticController(ICosmeticLogic cosmeticLogic)
        {
            this.cosmeticLogic = cosmeticLogic;
        }

        public IActionResult ListFoods(int werehouseeId)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            ViewBag.werehouseeId = werehouseeId;
            return View(cosmeticLogic.Read(null));
        }

        public IActionResult AddFood(int? CosmeticId, int? WerehouseId)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (CosmeticId == null && WerehouseId == null)
            {
                return NotFound();
            }
            if (TempData["ErrorLackInFridge"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLackInFridge"].ToString());
            }
            var food = cosmeticLogic.Read(new CosmeticBindingModel
            {
                Id = CosmeticId
            })?[0];
            if (food == null)
            {
                return NotFound();
            }
            ViewBag.CosmeticName = food.CosmeticName;
            ViewBag.WerehouseId = WerehouseId;
            return View(new RequestCosmeticBindingModel
            {
                CosmeticId = CosmeticId.Value,
                WerehouseId = WerehouseId.Value,
            });
        }
    }
}