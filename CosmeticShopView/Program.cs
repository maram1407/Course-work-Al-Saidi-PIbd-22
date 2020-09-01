using CosmeticShopBusinessLogic.BusinessLogics;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopDatabaseImplement.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace CosmeticShopView
{
    static class Program
    {
        public static bool Cheak;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var login = new FormLogin();
            login.ShowDialog();
            if (Cheak)
            {
                Application.Run(container.Resolve<FormMain>());
            }
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<ICosmeticLogic, CosmeticLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRequestLogic, RequestLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<MainLogic>
                (new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ReportLogic>
                (new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOrderLogic, OrderLogic>
                (new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWerehouseLogic, WerehouseLogic>
                (new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ISetLogic, SetLogic>
                (new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ISupplierLogic, SupplierLogic>
                (new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}