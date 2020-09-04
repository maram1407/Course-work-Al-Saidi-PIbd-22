﻿using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace CosmeticShopView
{
    public partial class FormRequestCosmetic : Form
    {
        public new IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly IRequestLogic logic;
        private int? id;
        private Dictionary<int, (string, int, bool)> cosmetics;

        public FormRequestCosmetic(IRequestLogic service)
        {
            InitializeComponent();
            dataGridView.Columns.Add("Id", "Id");
            dataGridView.Columns.Add("CosmeticName", "Продукты");
            dataGridView.Columns.Add("Count", "Количество");
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.logic = service;
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            var listRequest = logic.Read(new RequestBindingModel { Id = id.Value })?[0];
            if (listRequest != null)
            {
                cosmetics = listRequest.Cosmetics;
                LoadData();
            }
            dataGridView.Update();
        }

        private void LoadData()
        {
            try
            {
                if (cosmetics != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var pc in cosmetics)
                    {
                        dataGridView.Rows.Add(new object[] { pc.Key, pc.Value.Item1, pc.Value.Item2 });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
