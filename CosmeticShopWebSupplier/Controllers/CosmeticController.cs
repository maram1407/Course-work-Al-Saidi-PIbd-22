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

        public IActionResult ListCosmetics(int WerehouseId)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            ViewBag.WerehouseId = WerehouseId;
            return View(cosmeticLogic.Read(null));
        }

        public IActionResult AddCosmetic(int? CosmeticId, int? WerehouseId=3)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (CosmeticId == null && WerehouseId == null)
            {
                return NotFound();
            }
            if (TempData["ErrorLackInWerehouse"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLackInWerehouse"].ToString());
            }
            var Cosmetic = cosmeticLogic.Read(new CosmeticBindingModel
            {
                Id = CosmeticId
            })?[0];
            if (Cosmetic == null)
            {
                return NotFound();
            }
            ViewBag.CosmeticName = Cosmetic.CosmeticName;
            ViewBag.WerehouseId = WerehouseId;
            return View(new RequestCosmeticBindingModel
            {
                CosmeticId = CosmeticId.Value,
                WerehouseId = WerehouseId.Value,
            });
        }
    }
}