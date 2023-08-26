namespace Coinbase.Exchange.FrontEnd
{
    partial class HomeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            panel1 = new Panel();
            label_instruments = new Label();
            comboBox_instruments = new ComboBox();
            label_bestoffer_value = new Label();
            label_bestbid_value = new Label();
            label_bids = new Label();
            label_bestoffer = new Label();
            listBox_offers = new ListBox();
            listBox_bids = new ListBox();
            pageSetupDialog1 = new PageSetupDialog();
            label_price = new Label();
            label_price_value = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Dock = DockStyle.Left;
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(30, 509);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // panel1
            // 
            panel1.Controls.Add(label_price_value);
            panel1.Controls.Add(label_price);
            panel1.Controls.Add(label_instruments);
            panel1.Controls.Add(comboBox_instruments);
            panel1.Controls.Add(label_bestoffer_value);
            panel1.Controls.Add(label_bestbid_value);
            panel1.Controls.Add(label_bids);
            panel1.Controls.Add(label_bestoffer);
            panel1.Controls.Add(listBox_offers);
            panel1.Controls.Add(listBox_bids);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(804, 509);
            panel1.TabIndex = 1;
            // 
            // label_instruments
            // 
            label_instruments.AutoSize = true;
            label_instruments.Location = new Point(39, 35);
            label_instruments.Name = "label_instruments";
            label_instruments.Size = new Size(70, 15);
            label_instruments.TabIndex = 7;
            label_instruments.Text = "Instruments";
            // 
            // comboBox_instruments
            // 
            comboBox_instruments.FormattingEnabled = true;
            comboBox_instruments.Location = new Point(115, 32);
            comboBox_instruments.Name = "comboBox_instruments";
            comboBox_instruments.Size = new Size(121, 23);
            comboBox_instruments.TabIndex = 6;
            comboBox_instruments.SelectedIndexChanged += comboBox_instruments_SelectedIndexChanged;
            // 
            // label_bestoffer_value
            // 
            label_bestoffer_value.AutoSize = true;
            label_bestoffer_value.Location = new Point(477, 264);
            label_bestoffer_value.Name = "label_bestoffer_value";
            label_bestoffer_value.Size = new Size(0, 15);
            label_bestoffer_value.TabIndex = 5;
            // 
            // label_bestbid_value
            // 
            label_bestbid_value.AutoSize = true;
            label_bestbid_value.Location = new Point(477, 35);
            label_bestbid_value.Name = "label_bestbid_value";
            label_bestbid_value.Size = new Size(0, 15);
            label_bestbid_value.TabIndex = 4;
            // 
            // label_bids
            // 
            label_bids.AutoSize = true;
            label_bids.Location = new Point(414, 35);
            label_bids.Name = "label_bids";
            label_bids.Size = new Size(57, 15);
            label_bids.TabIndex = 3;
            label_bids.Text = "Best Bids:";
            // 
            // label_bestoffer
            // 
            label_bestoffer.AutoSize = true;
            label_bestoffer.Location = new Point(414, 264);
            label_bestoffer.Name = "label_bestoffer";
            label_bestoffer.Size = new Size(62, 15);
            label_bestoffer.TabIndex = 2;
            label_bestoffer.Text = "Best Offer:";
            // 
            // listBox_offers
            // 
            listBox_offers.FormattingEnabled = true;
            listBox_offers.ItemHeight = 15;
            listBox_offers.Location = new Point(414, 291);
            listBox_offers.Name = "listBox_offers";
            listBox_offers.Size = new Size(137, 169);
            listBox_offers.TabIndex = 1;
            // 
            // listBox_bids
            // 
            listBox_bids.FormattingEnabled = true;
            listBox_bids.ItemHeight = 15;
            listBox_bids.Location = new Point(411, 63);
            listBox_bids.Name = "listBox_bids";
            listBox_bids.Size = new Size(140, 169);
            listBox_bids.TabIndex = 0;
            // 
            // label_price
            // 
            label_price.AutoSize = true;
            label_price.Location = new Point(262, 36);
            label_price.Name = "label_price";
            label_price.Size = new Size(39, 15);
            label_price.TabIndex = 8;
            label_price.Text = "Price :";
            // 
            // label_price_value
            // 
            label_price_value.AutoSize = true;
            label_price_value.Location = new Point(306, 37);
            label_price_value.Name = "label_price_value";
            label_price_value.Size = new Size(0, 15);
            label_price_value.TabIndex = 9;
            // 
            // HomeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(805, 509);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "HomeForm";
            Text = "Form1";
            Load += HomeForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private Panel panel1;
        private PageSetupDialog pageSetupDialog1;
        private Label label_bestoffer;
        private ListBox listBox_offers;
        private ListBox listBox_bids;
        private Label label_bids;
        private Label label_bestoffer_value;
        private Label label_bestbid_value;
        private Label label_instruments;
        private ComboBox comboBox_instruments;
        private Label label_price_value;
        private Label label_price;
    }
}