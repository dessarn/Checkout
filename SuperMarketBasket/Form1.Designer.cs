using SuperMarketBasket.Client.Interfaces;
using SuperMarketBasket.DataObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SuperMarketBasket
{
    partial class Form1
    {

        private Panel buttonPanel = new Panel();
        private DataGridView productsDataGridView = new DataGridView();
        private DataGridView basketDataGridView = new DataGridView();
        private Button calculateTotalButton = new Button();
        private Button changeSpecialOfferButton = new Button();
        private Button updateBasketButton = new Button();
        //private Button deleteRowButton = new Button();

        IBasketClient client;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(IBasketClient client)
        {
            this.client = client;
            client.GetItems();
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";
            this.Load += new EventHandler(Form1_Load);
        }

        #endregion

        private void Form1_Load(System.Object sender, System.EventArgs e)
        {
            SetupLayout();
            SetupDataGridView();
            PopulateDataGridView(productsDataGridView, client.ProductList);
            PopulateDataGridView(basketDataGridView, client.Basket);
        }

        private void SetupLayout()
        {
            this.Size = new Size(600, 600);

            calculateTotalButton.Text = "Calculate Total";
            calculateTotalButton.Size = new Size(150, 25);
            calculateTotalButton.Location = new Point(10, 10);
            calculateTotalButton.Click += new EventHandler(calculateTotalButtonButton_Click);

            changeSpecialOfferButton.Text = "Change Special Offer";
            changeSpecialOfferButton.Size = new Size(150, 25);
            changeSpecialOfferButton.Location = new Point(160, 10);
            changeSpecialOfferButton.Click += new EventHandler(changeSpecialOfferButton_Click);

            updateBasketButton.Text = "Update Basket";
            updateBasketButton.Size = new Size(150, 25);
            updateBasketButton.Location = new Point(310, 10);
            updateBasketButton.Click += new EventHandler(updateBasketButton_Click);

            buttonPanel.Controls.Add(calculateTotalButton);
            buttonPanel.Controls.Add(changeSpecialOfferButton);
            buttonPanel.Controls.Add(updateBasketButton);
            buttonPanel.Height = 50;
            buttonPanel.Dock = DockStyle.Bottom;

            this.Controls.Add(this.buttonPanel);
        }

        private void SetupDataGridView()
        {
            Label productLabel = new Label() { Left = 10, Top = 0, Text = "Products Avalible" };

            this.Controls.Add(productLabel);

            this.Controls.Add(productsDataGridView);

            productsDataGridView.ColumnCount = 4;

            productsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            productsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            productsDataGridView.ColumnHeadersDefaultCellStyle.Font =
                new Font(productsDataGridView.Font, FontStyle.Bold);

            productsDataGridView.Name = "productsDataGridView";
            productsDataGridView.Location = new Point(8, 30);
            productsDataGridView.Size = new Size(500, 230);
            productsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            productsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            productsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            productsDataGridView.GridColor = Color.Black;
            productsDataGridView.RowHeadersVisible = false;

            productsDataGridView.Columns[0].Name = "SKU";
            productsDataGridView.Columns[1].Name = "Name";
            productsDataGridView.Columns[2].Name = "Unit Price";
            productsDataGridView.Columns[3].Name = "Special Offer";

            productsDataGridView.SelectionMode =
                DataGridViewSelectionMode.CellSelect;
            productsDataGridView.MultiSelect = false;
            //productsDataGridView.Dock = DockStyle.Top;

            Label basketLabel = new Label() { Left = 10, Top = 270, Text = "Basket" };

            this.Controls.Add(basketLabel);

            this.Controls.Add(basketDataGridView);

            basketDataGridView.ColumnCount = 4;

            basketDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            basketDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            basketDataGridView.ColumnHeadersDefaultCellStyle.Font =
                new Font(productsDataGridView.Font, FontStyle.Bold);

            basketDataGridView.Name = "basketDataGridView";
            basketDataGridView.Location = new Point(8, 300);
            //basketDataGridView.Left = 20;
            //basketDataGridView.Top = 100;
            basketDataGridView.Size = new Size(500, 230);
            basketDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            basketDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            basketDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            basketDataGridView.GridColor = Color.Black;
            basketDataGridView.RowHeadersVisible = false;

            basketDataGridView.Columns[0].Name = "SKU";
            basketDataGridView.Columns[1].Name = "Name";
            basketDataGridView.Columns[2].Name = "Unit Price";
            basketDataGridView.Columns[3].Name = "Special Offer";

            basketDataGridView.SelectionMode =
                DataGridViewSelectionMode.CellSelect;
            basketDataGridView.MultiSelect = false;
            //basketDataGridView.Dock = DockStyle.Fill;


        }

        private void PopulateDataGridView(DataGridView dgv, List<Product> list)
        {
            dgv.Rows.Clear();

            foreach (var product in list)
            {
                string[] row = { product.SKU , product.Name, product.UnitPrice.ToString(),
                                product.Offer != null ? $"{product.Offer.Number} for {product.Offer.OfferPrice}" : ""};

                dgv.Rows.Add(row);
            }
        }

        private void calculateTotalButtonButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Total amount: {client.Checkout().ToString()}");
        }

        private void changeSpecialOfferButton_Click(object sender, EventArgs e)
        {
            var result = ShowChangeSpecialOfferDialog();
            
            if (String.IsNullOrWhiteSpace(result.Item1.ToString()) ||
                result.Item2.Number <= 0 || result.Item2.OfferPrice <= 0m)
            {
                MessageBox.Show("data entered not valid");             
            }
            else
            {
                if(client.ChangeSpecialOffer(result.Item1, result.Item2))
                {
                    PopulateDataGridView(productsDataGridView, client.ProductList);
                }
                else
                {
                    MessageBox.Show("unable to change special offer");
                }
                
            }
            
        }

        private void updateBasketButton_Click(object sender, EventArgs e)
        {
            PopulateDataGridView(basketDataGridView, client.Basket);
        }

        private Tuple<string, SpecialOffer> ShowChangeSpecialOfferDialog()
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 300;
            prompt.Text = "Change special Offer";

            Label textLabel = new Label() { Left = 50, Top = 10, Text = "Select Item by SKU" };
            ComboBox combo = new ComboBox() { Left = 150, Top = 10 } ;
            foreach (var product in client.ProductList)
                combo.Items.Add(product.SKU); 
            Label textLabel2 = new Label() { Left = 50, Top = 40, Text = "Number of Items" };
            NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 70, Width = 400 };
            Label textLabel3 = new Label() { Left = 50, Top = 100, Text = "For what price" };
            NumericUpDown inputBox2 = new NumericUpDown() { Left = 50, Top = 130, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 150, Width = 100, Top = 150 };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(combo);
            prompt.Controls.Add(textLabel2);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(textLabel3);
            prompt.Controls.Add(inputBox2);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();

            return new Tuple<string, SpecialOffer>(combo.Text.ToString(), new SpecialOffer((int)inputBox.Value, inputBox2.Value));        
            
        }
    }
}

