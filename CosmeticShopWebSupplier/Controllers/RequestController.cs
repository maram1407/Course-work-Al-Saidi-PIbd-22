using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.BusinessLogics;
using CosmeticShopBusinessLogic.HelperModels;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.ViewModels;
using CosmeticShopWebSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmeticShopWebSupplier.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestLogic requestLogic;
        private readonly IWerehouseLogic werehousesLogic;
        private readonly ICosmeticLogic foodLogic;
        private readonly SupplierBusinessLogic supplierLogic;
        private readonly SupplierReportLogic reportLogic;
        public RequestController(IRequestLogic requestLogic, IWerehouseLogic werehousesLogic, SupplierBusinessLogic supplierLogic, SupplierReportLogic reportLogic, ICosmeticLogic foodLogic)
        {
            this.requestLogic = requestLogic;
            this.werehousesLogic = werehousesLogic;
            this.supplierLogic = supplierLogic;
            this.reportLogic = reportLogic;
            this.foodLogic = foodLogic;
        }

        public IActionResult Request()
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            var requests = requestLogic.Read(new RequestBindingModel
            {
                SupplierId = Program.Supplier.Id
            });
            return View(requests);
        }

        public IActionResult Report(ReportModel model)
        {
            var requests = new List<RequestViewModel>();
            requests = requestLogic.Read(new RequestBindingModel
            {
                SupplierId = Program.Supplier.Id,
                DateFrom = model.From,
                DateTo = model.To
            });
            ViewBag.Requests = requests;
            string fileName = "E:\\report-kursovaa\\Reportpdf.pdf";
            if (model.SendMail)
            {
                reportLogic.SaveCosmeticsToPdfFile(fileName, new RequestBindingModel
                {
                    SupplierId = Program.Supplier.Id,
                    DateFrom = model.From,
                    DateTo = model.To
                }, Program.Supplier.Login);
            }
            return View();
        }

        public IActionResult RequestView(int ID)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            if (TempData["ErrorCosmeticReserve"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorCosmeticReserve"].ToString());
            }
            ViewBag.RequestID = ID;
            var cosmetics = requestLogic.Read(new RequestBindingModel
            {
                Id = ID
            })?[0].Cosmetics;
            return View(cosmetics);
        }

        public IActionResult Reserve(int requestId, int cosmeticId)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            supplierLogic.ReserveCosmetics(new ReserveCosmeticsBindingModel
            {
                RequestId = requestId,
                CosmeticId = cosmeticId
            });
            return RedirectToAction("RequestView", new { id = requestId });
        }

        public IActionResult AcceptRequest(int id)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            supplierLogic.AcceptRequest(new ChangeRequestStatusBindingModel
            {
                RequestId = id
            });
            return RedirectToAction("Request");
        }

        public IActionResult CompleteRequest(int id)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            supplierLogic.CompleteRequest(new ChangeRequestStatusBindingModel
            {
                RequestId = id
            });
            return RedirectToAction("Request");
        }

        public IActionResult ListCosmeticAvailable(int id, int count, string name, int requestId)
        {
            if (Program.Supplier == null)
            {
                return new UnauthorizedResult();
            }
            ViewBag.CosmeticName = name;
            ViewBag.Count = count;
            ViewBag.CosmeticId = id;
            ViewBag.RequestId = requestId;
            var werehouses = werehousesLogic.GetWerehouseAvailable(new RequestCosmeticBindingModel
            {
                CosmeticId = id,
                 Count = count
            });
            return View(werehouses);
        }

        public IActionResult SendWordReport(int id)
        {
            string fileName = "E:\\report-kursovaa\\" + id + ".docx";
            reportLogic.SaveNeedCosmeticToWordFile(new WordInfo
            {
                FileName = fileName,
                RequestId = id,
                SupplierFIO = Program.Supplier.SupplierFIO
            }, Program.Supplier.Login);
            return RedirectToAction("Request");
        }
        public IActionResult SendExcelReport(int id)
        {
            string fileName = "E:\\report-kursovaa\\" + id + ".xlsx";
            reportLogic.SaveNeedCosmeticToExcelFile(new ExcelInfo
            {
                FileName = fileName,
                RequestId = id,
                SupplierFIO = Program.Supplier.SupplierFIO
            }, Program.Supplier.Login);
            return RedirectToAction("Request");
        }
    }
}