using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.Enums;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BusinessLogics
{
    public class MainLogic
    {
        private readonly IOrderLogic orderLogic;
        private readonly IRequestLogic requestLogic;
        private readonly ISetLogic setLogic;

        public MainLogic(IOrderLogic orderLogic, IRequestLogic requestLogic, ISetLogic setLogic)
        {
            this.orderLogic = orderLogic;
            this.requestLogic = requestLogic;
            this.setLogic = setLogic;
        }

        public void CreateOrder(OrderBindingModel order)
        {
            orderLogic.CreateOrUpdate(new OrderBindingModel
            {
                SetId = order.SetId,
                Count = order.Count,
                CreationDate = DateTime.Now,
                Status = Status.Принят,
                Sum = order.Sum
            });
        }

        public void TakeOrderInWork(ChangeStatusBindingModel model)
        {
            var order = orderLogic.Read(new OrderBindingModel { Id = model.OrderId })?[0];
            var request = requestLogic.Read(new RequestBindingModel { Id = model.OrderId })?[0];
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }

            if (order.Status != Status.Принят)
            {
                throw new Exception("Заказ не в статусе \"Принят\"");
            }

            if (request.Status != RequestStatus.Готова)
            {
                throw new Exception("Продукты ещё не доставлены");
            }

            requestLogic.CreateOrUpdate(new RequestBindingModel
            {
                Id = request.Id,
                SupplierId = request.SupplierId,
                Cosmetics = request.Cosmetics,
                CreationDate = request.CreationDate,
                Status = RequestStatus.Обработана,
                CompletionDate = request.CompletionDate
            });

            orderLogic.CreateOrUpdate(new OrderBindingModel
            {
                Id = order.Id,
                SetId = order.SetId,
                Count = order.Count,
                Sum = order.Sum,
                CreationDate = order.CreationDate,
                Status = Status.Выполняется
            });
        }

        public void FinishOrder(ChangeStatusBindingModel model)
        {
            var order = orderLogic.Read(new OrderBindingModel { Id = model.OrderId })?[0];
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != Status.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            orderLogic.CreateOrUpdate(new OrderBindingModel
            {
                Id = order.Id,
                SetId = order.SetId,
                Count = order.Count,
                Sum = order.Sum,
                CreationDate = order.CreationDate,
                CompletionDate = DateTime.Now,
                Status = Status.Готов
            });
        }

        public void PayOrder(ChangeStatusBindingModel model)
        {
            var order = orderLogic.Read(new OrderBindingModel { Id = model.OrderId })?[0];
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != Status.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            orderLogic.CreateOrUpdate(new OrderBindingModel
            {
                Id = order.Id,
                SetId = order.SetId,
                Count = order.Count,
                Sum = order.Sum,
                CreationDate = order.CreationDate,
                CompletionDate = order.CompletionDate,
                Status = Status.Оплачен
            });
        }

        public void CreateOrUpdateRequest(RequestBindingModel model)
        {
            requestLogic.CreateOrUpdate(new RequestBindingModel
            {
                Id = model.Id,
                SupplierId = model.SupplierId,
                Status = RequestStatus.Создана,
                Cosmetics = model.Cosmetics,                
                CreationDate = DateTime.Now
            });
        }

        public List<ReportSetCosmeticViewModel> GetDishFoodsOrder()
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
    }
}