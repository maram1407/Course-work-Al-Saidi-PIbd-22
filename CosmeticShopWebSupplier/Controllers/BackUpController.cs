using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CosmeticShopBusinessLogic.BusinessLogics;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopDatabaseImplement.Implements;

namespace CosmeticShopWebSupplier.Controllers
{
    public class BackUpController : Controller
    {
        private readonly IRequestLogic _request;
        private readonly IWerehouseLogic _werehoume;
        private readonly ISupplierLogic _supplier;
        private readonly ICosmeticLogic _cosmetic;
        private readonly SupplierReportLogic _supplierReport;
        public BackUpController(IRequestLogic request, IWerehouseLogic w, ISupplierLogic supplier, ICosmeticLogic c, SupplierReportLogic supplierReport)
        {
            _request = request;
            _werehoume = w;
            _supplier = supplier;
            _cosmetic = c;
            _supplierReport = supplierReport;
        }
        public IActionResult BackUp()
        {
            return View();
        }
        public IActionResult BackUpToJson()
        {
            string fileName = "C:\\Users\\Lyays\\Desktop\\Backup\\BackupJson";
            if (Directory.Exists(fileName))
            {
                _request.SaveJsonRequest(fileName);
                _request.SaveJsonRequestCosmetic(fileName);
                _werehoume.SaveJsonWerehouse(fileName);
                _werehoume.SaveJsonWerehouseCosmetic(fileName);
                _supplier.SaveJsonSupplier(fileName);
                _cosmetic.SaveJsonCosmetic(fileName);
                _supplierReport.SendMailBackup("dggfddg6@gmail.com", fileName, "Бэкап Json", "json");
                return RedirectToAction("BackUp");
            }
            else
            {
                return RedirectToAction("BackUp");
            }
        }
        public IActionResult BackUpToXml()
        {
            string fileName = "C:\\Users\\Lyays\\Desktop\\Backup\\BackupXml";
            if (Directory.Exists(fileName))
            {
                _request.SaveXmlRequest(fileName);
                _request.SaveXmlRequestCosmetic(fileName);
                _werehoume.SaveXmlWerehouse(fileName);
                _werehoume.SaveXmlWerehouseCosmetic(fileName);
                _supplier.SaveXmlSupplier(fileName);
                _cosmetic.SaveXmlCosmetic(fileName);
                _supplierReport.SendMailBackup("dggfddg6@gmail.com", fileName, "Бэкап Xml", "xml");
                return RedirectToAction("BackUp");
            }
            else
            {
                return RedirectToAction("BackUp");
            }
        }
    }
}