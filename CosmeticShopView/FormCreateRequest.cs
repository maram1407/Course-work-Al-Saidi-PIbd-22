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
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.ViewModels;
using CosmeticShopBusinessLogic.BusinessLogics;

namespace CosmeticShopView
{
    public partial class FormCreateRequest : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly IRequestLogic requestLogic;
        private readonly ISupplierLogic supplierLogic;
        private readonly MainLogic mainLogic;
        public int ID { set { Id = value; } }
        private int? Id;
        private Dictionary<int, (string, int, bool)> requestCosmetics;

        public FormCreateRequest(MainLogic mainLogic,
            IRequestLogic requestLogic, ISupplierLogic supplierLogic)
        {
            InitializeComponent();
            this.requestLogic = requestLogic;
            this.supplierLogic = supplierLogic;
            this.mainLogic = mainLogic;
        }

        private void RequestCreationForm_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            if (Id.HasValue)
            {
                try
                {
                    RequestViewModel request = requestLogic.Read(new RequestBindingModel
                    {
                        Id = Id.Value
                    })?[0];
                    if (request != null)
                    {
                        comboBoxSupplier.SelectedIndex =
                            comboBoxSupplier.FindStringExact(request.SupplierFIO);
                        requestCosmetics = request.Cosmetics;
                        LoadCosmetics();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка загрузки данных заявки",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                requestCosmetics = new Dictionary<int, (string, int, bool)>();
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                List<SupplierViewModel> suppliersList = supplierLogic.Read(null);
                if (suppliersList != null)
                {
                    comboBoxSupplier.DisplayMember = "Login";
                    comboBoxSupplier.ValueMember = "Id";
                    comboBoxSupplier.DataSource = suppliersList;
                    comboBoxSupplier.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка загрузки списка поставщиков",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadCosmetics()
        {
            try
            {
                if (requestCosmetics != null)
                {
                    cosmeticsGridView.Rows.Clear();
                    foreach (var requestFood in requestCosmetics)
                    {
                        cosmeticsGridView.Rows.Add(new object[] {
                            requestFood.Key,
                            requestFood.Value.Item1,
                            requestFood.Value.Item2
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка загрузки",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AddCosmeticButton_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormAddCosmetics>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (requestCosmetics.ContainsKey(form.Id))
                {
                    requestCosmetics[form.Id] = (form.CosmeticName, form.Count, false);
                }
                else
                {
                    requestCosmetics.Add(form.Id, (form.CosmeticName, form.Count, false));
                }
                LoadCosmetics();
            }
        }

        private void UpdateCosmeticButton_Click(object sender, EventArgs e)
        {
            if (cosmeticsGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormAddCosmetics>();
                int Id = Convert.ToInt32(cosmeticsGridView.SelectedRows[0].Cells[0].Value);
                form.Id = Id;
                form.Count = requestCosmetics[Id].Item2;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    requestCosmetics[form.Id] = (form.CosmeticName, form.Count, false);
                    LoadCosmetics();
                }
            }
        }

        private void DeleteCosmeticButton_Click(object sender, EventArgs e)
        {
            if (cosmeticsGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show(
                    "Действительно хотите удалить продукт?",
                    "Требуется подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        requestCosmetics.Remove(Convert.ToInt32(cosmeticsGridView.SelectedRows[0].Cells[0].Value));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            ex.Message,
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    LoadCosmetics();
                }
            }
        }

        private void RefreshCosmeticsButton_Click(object sender, EventArgs e)
        {
            LoadCosmetics();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (comboBoxSupplier.SelectedValue == null)
            {
                MessageBox.Show(
                    "Поставщик не выбран",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (requestCosmetics == null || requestCosmetics.Count == 0)
            {
                MessageBox.Show(
                    "Не выбрано ни одного продукта",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            try
            {
                mainLogic.CreateOrUpdateRequest(new RequestBindingModel
                {
                    Id = Id,
                    SupplierId = Convert.ToInt32(comboBoxSupplier.SelectedValue),
                    Cosmetics = requestCosmetics
                });
                MessageBox.Show(
                    "Сохранение заявки прошло успешно",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void СancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}