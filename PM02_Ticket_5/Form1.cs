﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace PM02_Ticket_5
{
    public partial class Form1 : Form
    {
        Dictionary<decimal, string> ServiceType = new Dictionary<decimal, string>()
        {
            { 5400, "Окна" },
            { 7400, "Балконы" },
            { 6750, "Двери" }
        };
        string typeService;
        public Form1()
        {
            InitializeComponent();

            comboBoxService.DataSource = new BindingSource(ServiceType, null);
            comboBoxService.ValueMember = "Key";
            comboBoxService.DisplayMember = "Value";
        }

        private void buttonLoadPicture_Click(object sender, EventArgs e)
        {
            string path = Environment.CurrentDirectory;

            pictureBox1.ImageLocation = Path.Combine(path, @"..\..\ресурсы\логотипы\Окна2.jpg");

            buttonLoadPicture.Enabled= false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.Select();
            buttonGetReceipt.Enabled = false;
        }

        private void buttonGetReceipt_Click(object sender, EventArgs e)
        {
            string startupPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\");
            Word.Document doc = null;
            try
            {
                Word.Application app = new Word.Application();
                // Путь до шаблона документа
                string source = startupPath + @"чек.docx";
                // Открываем
                doc = app.Documents.Open(source);
                doc.Activate();
                // Делаем копию 
                doc.SaveAs2(startupPath + @"чекКопия.docx");

                doc.Close();
                doc = null;
            }
            catch
            {
                doc = null;
                return;
            }
            try
            {
                Word.Application app = new Word.Application();
                // Путь до шаблона документа
                string source = startupPath + @"чекКопия.docx";
                // Открываем
                doc = app.Documents.Open(source);
                doc.Activate();
                Word.Bookmarks wBookmarks = doc.Bookmarks;
                Word.Range wRange;
                
                Word.Range range = app.Selection.Range;
                

                doc.Close();
                doc = null;

            }
            catch (Exception ex)
            {
                doc.Close();
                doc = null;
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonCalculation_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxWidth.Text) || string.IsNullOrWhiteSpace(textBoxHeight.Text))
            {
                MessageBox.Show("Заполните поля", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(textBoxWidth.Text, out int sad) || !int.TryParse(textBoxHeight.Text, out sad))
            {
                MessageBox.Show("Заполните поля целыми числами", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int width = Convert.ToInt32(textBoxWidth.Text);
            int height = Convert.ToInt32(textBoxHeight.Text);
            decimal service = (decimal)comboBoxService.SelectedValue;


            try
            {
                decimal sumResult = Calculation(width, height, service, typeService);
                labelResult.Text = "Вывод расчетов:\n" + sumResult;
                buttonGetReceipt.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public decimal Calculation(int width, int height, decimal service, string typeService)
        {
            decimal sumResult;
            decimal priceTypeService = 0;
            if (width < 30 || height < 30 || width > 30000 || height > 30000)
            {
                MessageBox.Show("Введите правдоподобные размеры", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            switch (typeService)
            {
                case "Глухое":
                    priceTypeService = 1000;
                    break;
                case "Поворотное":
                    priceTypeService = 3400.5m;
                    break;
                case "Откидное":
                    priceTypeService = 2560;
                    break;
                case "Фрамужное":
                    priceTypeService = 7900.9m;
                    break;
                case "Раздвижное":
                    priceTypeService = 6210.5m;
                    break;
                default:
                    break;
            }
            sumResult = (decimal)((decimal)width * (decimal)height / 10000);
            sumResult = sumResult * service;
            sumResult += (decimal)priceTypeService;
            sumResult = Math.Round(sumResult, 2);
            return sumResult;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            buttonGetReceipt.Enabled = false;
            if (radioButton1.Checked)
            {
                typeService = "Глухое";
                return;
            }
            if (radioButton2.Checked)
            {
                typeService = "Поворотное";
                return;
            }
            if (radioButton3.Checked)
            {
                typeService = "Откидное";
                return;
            }
            if (radioButton4.Checked)
            {
                typeService = "Фрамужное";
                return;
            }
            if (radioButton5.Checked)
            {
                typeService = "Раздвижное";
                return;
            }
        }
    }
}
