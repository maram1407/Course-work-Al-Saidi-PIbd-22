using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.BusinessLogics;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using CosmeticShopBusinessLogic.ViewModels;
using System.Runtime.Serialization.Json;

namespace CosmeticShopView
{
    public partial class FormMain : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly MainLogic logic;
        private readonly IOrderLogic orderLogic;
        private readonly IRequestLogic requestLogic;
        private readonly ISetLogic setLogic;
        private readonly ICosmeticLogic cosmeticLogic;
        private readonly ReportLogic report;

        public FormMain(MainLogic logic, IOrderLogic orderLogic, ReportLogic report,
            ISetLogic setLogic, IRequestLogic requestLogic, ICosmeticLogic cosmeticLogic)
        {
            InitializeComponent();
            this.logic = logic;
            this.orderLogic = orderLogic;
            this.report = report;
            this.setLogic = setLogic;
            this.requestLogic = requestLogic;
            this.cosmeticLogic = cosmeticLogic;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var listOrders = orderLogic.Read(null);
            if (listOrders != null)
            {
                dataGridView.DataSource = listOrders;
                dataGridView.Columns[0].Visible = false;
                dataGridView.Columns[1].Visible = false;
                dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView.Columns[6].Visible = false;
            }
            dataGridView.Update();
        }

        private void CosmeticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCosmetics>();
            form.ShowDialog();
        }

        private void SetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormSets>();
            form.ShowDialog();
        }

        private void ButtonCreateOrder_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateOrder>();
            form.ShowDialog();
            LoadData();
        }

        private void ButtonTakeOrderInWork_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.TakeOrderInWork(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonOrderReady_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.FinishOrder(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonPayOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.PayOrder(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void косметикаPdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportMovingPdf>();
            form.ShowDialog();
        }

        private void наборыDocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "docx|*.docx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        report.SaveOrdersToWordFile(new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        });
                        MessageBox.Show("Отчет сохранен и отправлен на почту", "Успех", MessageBoxButtons.OK,
                       MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void наборыXlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportSetXls>();
            form.ShowDialog();
        }

        private void заказатьКосметикуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateRequest>();
            form.ShowDialog();
        }

        private void посмотретьДоступнуюКосметикуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormRequest>();
            form.ShowDialog();
        }

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    requestLogic.SaveXmlRequest(fbd.SelectedPath);
                    requestLogic.SaveXmlRequestCosmetic(fbd.SelectedPath);
                    setLogic.SaveXmlSet(fbd.SelectedPath);
                    setLogic.SaveXmlSetCosmetic(fbd.SelectedPath);
                    orderLogic.SaveXmlOrder(fbd.SelectedPath);
                    cosmeticLogic.SaveXmlCosmetic(fbd.SelectedPath);
                    report.SendMailReport("dggfddg6@gmail.com", fbd.SelectedPath, "XML бекап", "xml");
                    MessageBox.Show("Бекап сохранен и отправлен на почту", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void jSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    requestLogic.SaveJsonRequest(fbd.SelectedPath);
                    requestLogic.SaveJsonRequestCosmetic(fbd.SelectedPath);
                    setLogic.SaveJsonSet(fbd.SelectedPath);
                    setLogic.SaveJsonSetCosmetic(fbd.SelectedPath);
                    orderLogic.SaveJsonOrder(fbd.SelectedPath);
                    cosmeticLogic.SaveJsonCosmetic(fbd.SelectedPath);
                    report.SendMailReport("dggfddg6@gmail.com", fbd.SelectedPath, "JSON бекап", "json");
                    MessageBox.Show("Бекап сохранен и отправлен на почту", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }
    }
}