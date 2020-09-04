using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Unity;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.ViewModels;
using CosmeticShopBusinessLogic.BusinessLogics;

namespace CosmeticShopView
{
    public partial class FormReportSetXls : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly ReportLogic logic;
        public FormReportSetXls(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            dataGridViewFoodToDish.Columns.Add("Набор", "Набор");
            dataGridViewFoodToDish.Columns.Add("Косметика", "Косметика");
            dataGridViewFoodToDish.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void FormReportFoodsToDishes_Load(object sender, EventArgs e)
        {
            try
            {
                var dict = logic.GetSetCosmetics();
                if (dict != null)
                {
                    Dictionary<string, List<ReportSetCosmeticViewModel>> dishFoods = new Dictionary<string, List<ReportSetCosmeticViewModel>>();
                    dataGridViewFoodToDish.Rows.Clear();
                    foreach (var elem in dict)
                    {
                        if (!dishFoods.ContainsKey(elem.SetName))
                            dishFoods.Add(elem.SetName, new List<ReportSetCosmeticViewModel>() { elem });
                        else
                            dishFoods[elem.SetName].Add(elem);
                    }
                    foreach (var order in dishFoods)
                    {
                        dataGridViewFoodToDish.Rows.Add(order.Key, "", "");
                        foreach (var dish in order.Value)
                        {
                            dataGridViewFoodToDish.Rows.Add("", dish.CosmeticName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ButtonSaveToExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.SaveOrdersToExcelFile(new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        });
                        MessageBox.Show("Отчет сохранен и отправлен на почту", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}