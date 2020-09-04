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
using CosmeticShopBusinessLogic.BusinessLogics;
using CosmeticShopBusinessLogic.ViewModels;
using CosmeticShopBusinessLogic.BindingModels;

namespace CosmeticShopView
{
    public partial class FormAddCosmetics : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id
        {
            get { return Convert.ToInt32(comboBoxCosmetic.SelectedValue); }
            set { comboBoxCosmetic.SelectedValue = value; }
        }
        public string CosmeticName { get { return comboBoxCosmetic.Text; } }
        public int Count
        {
            get { return Convert.ToInt32(textBoxCount.Text); }
            set { textBoxCount.Text = value.ToString(); }
        }

        public FormAddCosmetics(ICosmeticLogic logic)
        {
            InitializeComponent();
            List<CosmeticViewModel> list = logic.Read(null);
            if (list != null)
            {
                comboBoxCosmetic.DisplayMember = "CosmeticName";
                comboBoxCosmetic.ValueMember = "Id";
                comboBoxCosmetic.DataSource = list;
                comboBoxCosmetic.SelectedItem = null;
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show(
                    "Поле \"Количество\" не заполнено",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (comboBoxCosmetic.SelectedValue == null)
            {
                MessageBox.Show(
                    "Комплектующее не выбрано",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}