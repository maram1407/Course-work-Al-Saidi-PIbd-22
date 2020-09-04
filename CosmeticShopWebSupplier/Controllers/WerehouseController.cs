using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;

using CosmeticShopWebSupplier.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopDatabaseImplement.Implements;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopDatabaseImplement.Models;

namespace CosmeticShopWebSupplier.Controllers
{
    public class WerehouseController : Controller
    {
        private readonly IWerehouseLogic werehouseLogic;
        private readonly ICosmeticLogic cosmeticLogic;

        public WerehouseController(IWerehouseLogic werehouseLogic, ICosmeticLogic cosmeticLogic)
        {
            this.werehouseLogic = werehouseLogic;
            this.cosmeticLogic = cosmeticLogic;
        }

        public IActionResult ListWerehouses()
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            var fridge = werehouseLogic.Read(new WerehouseBindingModel
            {
                SupplierId = Program.Supplier.Id
            });
            return View(fridge);
        }

        public IActionResult Details(int? id)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.WerehouseId = id;
            var foods = werehouseLogic.Read(new WerehouseBindingModel
            {
                Id = id
            })?[0].Cosmetics;
            return View(foods);
        }

        public IActionResult CreateWerehouse()
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateWerehouse([Bind("WerehouseName,Capacity,Type")] Werehouse fridge)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    werehouseLogic.CreateOrUpdate(new WerehouseBindingModel
                    {
                        WerehouseName = fridge.WerehouseName,
                        Capacity = fridge.Capacity,
                        Type = fridge.Type,
                        SupplierId = Program.Supplier.Id
                    });
                    return RedirectToAction(nameof(ListWerehouses));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(fridge);
        }

        public IActionResult ChangeWerehouse(int? id)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (id == null)
            {
                return NotFound();
            }
            var fridge = werehouseLogic.Read(new WerehouseBindingModel
            {
                Id = id
            })?[0];
            if (fridge == null)
            {
                return NotFound();
            }
            return View(new Werehouse
            {
                Id = id.Value,
                WerehouseName = fridge.WerehouseName,
                Capacity = fridge.Capacity,
                Type = fridge.Type
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeWerehouse(int id, [Bind("Id,WerehouseName,Capacity,Type")] Werehouse fridge)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (id != fridge.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    werehouseLogic.CreateOrUpdate(new WerehouseBindingModel
                    {
                        Id = id,
                        WerehouseName = fridge.WerehouseName,
                        Capacity = fridge.Capacity,
                        Type = fridge.Type,
                        SupplierId = Program.Supplier.Id
                    });
                }
                catch (Exception exception)
                {
                    if (!WerehouseExists(fridge.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", exception.Message);
                        return View(fridge);
                    }
                }
                return RedirectToAction(nameof(ListWerehouses));
            }
            return View(fridge);
        }

        public IActionResult DeleteWerehouse(int? id)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (id == null)
            {
                return NotFound();
            }
            var fridge = werehouseLogic.Read(new WerehouseBindingModel
            {
                Id = id
            })?[0];
            if (fridge == null)
            {
                return NotFound();
            }
            return View(new Werehouse
            {
                Id = id.Value,
                WerehouseName = fridge.WerehouseName,
                Capacity = fridge.Capacity,
                Type = fridge.Type
            });
        }

        [HttpPost, ActionName("DeleteWerehouse")]
        [ValidateAntiForgeryToken]
        public IActionResult Completion(int id)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            werehouseLogic.Delete(new WerehouseBindingModel
            {
                Id = id
            });
            return RedirectToAction(nameof(ListWerehouses));
        }

        private bool WerehouseExists(int id)
        {
            return werehouseLogic.Read(new WerehouseBindingModel
            {
                Id = id
            }).Count == 1;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCosmetic([Bind("WerehouseId, CosmeticId, Count, Price")] RequestCosmeticBindingModel model)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    werehouseLogic.AddCosmetic(model);
                }
                catch (Exception exception)
                {
                    TempData["ErrorLackInWerehouse"] = exception.Message;
                    return RedirectToAction("AddCosmetic", "Cosmetic", new
                    {
                        cosmeticId = model.CosmeticId,
                        werehouseId = model.WerehouseId
                    });
                }
            }
            return RedirectToAction("Details", new { id = model.WerehouseId });
        }

        public IActionResult ReserveCosmetics(int werehouseId, int cosmeticId, int count, int requestId)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            try
            {
                werehouseLogic.ReserveCosmetics(new RequestCosmeticBindingModel
                {
                    WerehouseId = werehouseId,
                    CosmeticId = cosmeticId,
                    Count = count
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorCosmeticReserve"] = ex.Message;
                return RedirectToAction("RequestView", "Request", new { id = requestId });
            }
            return RedirectToAction("Reserve", "Request", new
            {
                RequestId = requestId,
                CosmeticId = cosmeticId
            });
        }
    }
}