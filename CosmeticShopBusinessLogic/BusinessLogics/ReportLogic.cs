using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.BusinessLogic;
using CosmeticShopBusinessLogic.Enums;
using CosmeticShopBusinessLogic.HelperModels;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace CosmeticShopBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly ISetLogic setLogic;
        private readonly IOrderLogic orderLogic;
        private readonly ICosmeticLogic cosmeticLogic;
        private readonly IRequestLogic requestLogic;

        public ReportLogic(ISetLogic setLogic, IOrderLogic orderLogic, 
            ICosmeticLogic cosmeticLogic, IRequestLogic requestLogic)
        {
            this.setLogic = setLogic;
            this.orderLogic = orderLogic;
            this.cosmeticLogic = cosmeticLogic;
            this.requestLogic = requestLogic;
        }

        public List<ReportSetCosmeticViewModel> GetSetCosmetics()
        {
            var sets = setLogic.Read(null);
            var list = new List<ReportSetCosmeticViewModel>();
            foreach (var set in sets)
            {
                foreach (var sc in set.SetCosmetics)
                {
                    var record = new ReportSetCosmeticViewModel
                    {
                        SetName = set.SetName,
                        CosmeticName = sc.Value.Item1,
                        Count = sc.Value.Item2
                    };
                    list.Add(record);
                }
            }
            return list;
        }

        public List<ReportCosmeticViewModel> GetCosmetics(DateTime from, DateTime to)
        {
            var cosmetics = cosmeticLogic.Read(null);
            var requests = requestLogic.Read(null);
            var list = new List<ReportCosmeticViewModel>();
            foreach (var request in requests)
            {
                if (request.CreationDate >= from && request.CreationDate.Value.AddDays(-1) <= to)
                {
                    foreach (var requestCosmetic in request.Cosmetics)
                    {
                        foreach (var cosmetic in cosmetics)
                        {
                            if (cosmetic.CosmeticName == requestCosmetic.Value.Item1)
                            {
                                var record = new ReportCosmeticViewModel
                                {
                                    CosmeticName = requestCosmetic.Value.Item1,
                                    Count = requestCosmetic.Value.Item2,
                                    Status = StatusCosmetic(request.Status),
                                    CreationDate = request.CreationDate,
                                    CompletionDate = request.CompletionDate,
                                    Price = cosmetic.Price * requestCosmetic.Value.Item2
                                };
                                list.Add(record);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public string StatusCosmetic(RequestStatus requestStatus)
        {
            if (requestStatus == RequestStatus.Создана)
                return "Ждут отправки";
            if (requestStatus == RequestStatus.Выполняется)
                return "В пути";
            if (requestStatus == RequestStatus.Готова)
                return "Поставлено";
            if (requestStatus == RequestStatus.Обработана)
                return "Использовано";
            return "";
        }

        public List<ReportOrdersViewModel> GetReportOrder(ReportBindingModel model)
        {
            var sets = orderLogic.Read(null);
            var list = new List<ReportOrdersViewModel>();
            foreach (var set in sets)
            {
                var record = new ReportOrdersViewModel
                {
                    SetName = set.SetName,
                    Amount = set.Sum,
                    Count = set.Count,
                    CreationDate = set.CreationDate,
                    Status = set.Status
                };
                list.Add(record);
            }
            return list;
        }

        public void SaveOrdersToWordFile(ReportBindingModel model)
        {
            try
            {
                SaveToWord.CreateDoc(new WordInfo
                {
                    FileName = model.FileName,
                    Title = "Список наборов",
                    Orders = GetReportOrder(model),
                    SetCosmetics = GetSetCosmetics()
                });
            } catch(Exception)
            {
                throw;
            }
            SendMail("dggfddg6@gmail.com", model.FileName, "Список ноборов косметики");
        }

        public void SaveOrdersToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список наборов",
                Orders = GetReportOrder(model),
                SetCosmetics = GetSetCosmetics()
            });
            SendMail("kristina.zolotareva.14@gmail.com", model.FileName, "Список наборов с косметикой");
        }

        public void SaveCosmeticsToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Отчет по движению косметики",
                Cosmetics = GetCosmetics(model.DateFrom, model.DateTo),
                DateTo = model.DateTo,
                DateFrom = model.DateFrom
            });
            SendMail("dggfddg6@gmail.com", model.FileName, "Отчет по движению косметики");
        }

        public void SendMail(string email, string fileName, string subject)
        {
            MailAddress from = new MailAddress("dggfddg6@gmail.com", "Салон красоты");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Attachments.Add(new Attachment(fileName));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("dggfddg6@gmail.com", "alu050600!");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

        public void SendMailReport(string email, string fileName, string subject, string type)
        {
            MailAddress from = new MailAddress("dggfddg6@gmail.com", "Салон красоты");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Attachments.Add(new Attachment(fileName + "\\Order." + type));
            m.Attachments.Add(new Attachment(fileName + "\\Request." + type));
            m.Attachments.Add(new Attachment(fileName + "\\RequestCosmetic." + type));
            m.Attachments.Add(new Attachment(fileName + "\\Set." + type));
            m.Attachments.Add(new Attachment(fileName + "\\SetCosmetic." + type));
            m.Attachments.Add(new Attachment(fileName + "\\Cosmetic." + type));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("dggfddg6@gmail.com", "alu050600!");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}