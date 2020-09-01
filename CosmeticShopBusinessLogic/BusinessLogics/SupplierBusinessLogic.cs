using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.Enums;
using CosmeticShopBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BusinessLogics
{
    public class SupplierBusinessLogic
    {
        private readonly IRequestLogic requestLogic;
        public SupplierBusinessLogic(IRequestLogic requestLogic)
        {
            this.requestLogic = requestLogic;
        }

        public void AcceptRequest(ChangeRequestStatusBindingModel model)
        {
            var request = requestLogic.Read(new RequestBindingModel
            {
                Id = model.RequestId
            })?[0];
            if (request == null)
            {
                throw new Exception("Заявка не найдена");
            }
            if (request.Status != RequestStatus.Создана)
            {
                throw new Exception("Заявка не в статусе \"Создана\"");
            }
            requestLogic.CreateOrUpdate(new RequestBindingModel
            {
                Id = request.Id,
                SupplierId = request.SupplierId,
                Status = RequestStatus.Выполняется,
                Cosmetics = request.Cosmetics,
                CreationDate = request.CreationDate
            });
        }

        public void CompleteRequest(ChangeRequestStatusBindingModel model)
        {
            var request = requestLogic.Read(new RequestBindingModel
            {
                Id = model.RequestId
            })?[0];
            if (request == null)
            {
                throw new Exception("Заявка не найдена");
            }
            if (request.Status != RequestStatus.Выполняется)
            {
                throw new Exception("Заявка не в статусе \"Выполняется\"");
            }
            requestLogic.CreateOrUpdate(new RequestBindingModel
            {
                Id = request.Id,
                SupplierId = request.SupplierId,
                CreationDate = request.CreationDate,
                CompletionDate = DateTime.Now,
                Status = RequestStatus.Готова,
                Cosmetics = request.Cosmetics
            });
        }

        public void ReserveCosmetics(ReserveCosmeticsBindingModel model)
        {
            var request = requestLogic.Read(new RequestBindingModel
            {
                Id = model.RequestId
            })?[0];
            if (request == null)
            {
                throw new Exception("Заявка не найдена");
            }
            if (request.Status != RequestStatus.Выполняется)
            {
                throw new Exception("Заявка не в статусе \"Выполняется\"");
            }
            requestLogic.Reserve(model);
        }
    }
}