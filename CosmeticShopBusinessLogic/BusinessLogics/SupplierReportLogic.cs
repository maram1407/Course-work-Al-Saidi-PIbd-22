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
using System.Text;

namespace CosmeticShopBusinessLogic.BusinessLogics
{
    public class SupplierReportLogic
    {
        private readonly IRequestLogic requestLogic;
        private readonly ICosmeticLogic cosmeticLogic;
        public SupplierReportLogic(IRequestLogic requestLogic, ICosmeticLogic cosmeticLogic)
        {
            this.requestLogic = requestLogic;
            this.cosmeticLogic = cosmeticLogic;
        }

        public Dictionary<int, (string, int, bool)> GetRequestCosmetics(int requestId)
        {
            var requestFoods = requestLogic.Read(new RequestBindingModel
            {
                Id = requestId
            })?[0].Cosmetics;
            return requestFoods;
        }

        public List<ReportCosmeticViewModel> GetCosmetics(RequestBindingModel model)
        {
            var cosmetics = cosmeticLogic.Read(null);
            var requests = requestLogic.Read(model);
            var list = new List<ReportCosmeticViewModel>();
            foreach (var request in requests)
            {
                foreach (var requestCosmetic in request.Cosmetics)
                {
                    foreach (var cosmetic in cosmetics)
                    {
                        if (cosmetic.CosmeticName == requestCosmetic.Value.Item1)
                        {
                            var record = new ReportCosmeticViewModel
                            {
                                RequestId = request.Id,
                                SupplierFIO = request.SupplierFIO,
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
            return list;
        }

        public void SaveNeedCosmeticToWordFile(WordInfo wordInfo, string email)
        {
            string title = "Список требуемой косметики по заявке №" + wordInfo.RequestId;
            wordInfo.Title = title;
            wordInfo.FileName = wordInfo.FileName;
            wordInfo.RequestCosmetics = GetRequestCosmetics(wordInfo.RequestId);
            SupplierSaveToWord.CreateDoc(wordInfo);
            SendMail(email, wordInfo.FileName, title);
        }

        public void SaveNeedCosmeticToExcelFile(ExcelInfo excelInfo, string email)
        {
            string title = "Список требуемой косметики по заявке №" + excelInfo.RequestId;
            excelInfo.Title = title;
            excelInfo.FileName = excelInfo.FileName;
            excelInfo.RequestCosmetics = GetRequestCosmetics(excelInfo.RequestId);
            SupplierSaveToExcel.CreateDoc(excelInfo);
            SendMail(email, excelInfo.FileName, title);
        }

        public void SaveCosmeticsToPdfFile(string fileName, RequestBindingModel model, string email)
        {
            string title = "Список косметики в период с " + model.DateFrom.ToString() + " по " + model.DateTo.ToString();
            SupplierSaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = fileName,
                Title = title,
                Cosmetics = GetCosmetics(model)
            });
            SendMail(email, fileName, title);
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

        public void SendMailBackup(string email, string fileName, string subject, string type)
        {
            MailAddress from = new MailAddress("dggfddg6@gmail.com", "Салон красоты");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Attachments.Add(new Attachment(fileName + "\\Request." + type));
            m.Attachments.Add(new Attachment(fileName + "\\RequestCosmetic." + type));
            m.Attachments.Add(new Attachment(fileName + "\\Werehouse." + type));
            m.Attachments.Add(new Attachment(fileName + "\\WerehouseCOsmetic." + type));
            m.Attachments.Add(new Attachment(fileName + "\\Supplier." + type));
            m.Attachments.Add(new Attachment(fileName + "\\Cosmetic." + type));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("dggfddg6@gmail.com", "alu050600!");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}