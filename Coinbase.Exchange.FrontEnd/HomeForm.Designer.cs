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
            button_remove_instrument = new Button();
            button_add_instrument = new Button();
            label_add_subscription = new Label();
            listBox_subscribed_instruments = new ListBox();
            listBox_instruments = new ListBox();
            dataGridView_offers = new DataGridView();
            dataGridView_bids = new DataGridView();
            label_price_value = new Label();
            label_price = new Label();
            label_instruments = new Label();
            comboBox_instruments = new ComboBox();
            label_bestoffer_value = new Label();
            label_bestbid_value = new Label();
            label_bids = new Label();
            label_bestoffer = new Label();
            pageSetupDialog1 = new PageSetupDialog();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_offers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView_bids).BeginInit();
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
            panel1.Controls.Add(button_remove_instrument);
            panel1.Controls.Add(button_add_instrument);
            panel1.Controls.Add(label_add_subscription);
            panel1.Controls.Add(listBox_subscribed_instruments);
            panel1.Controls.Add(listBox_instruments);
            panel1.Controls.Add(dataGridView_offers);
            panel1.Controls.Add(dataGridView_bids);
            panel1.Controls.Add(label_price_value);
            panel1.Controls.Add(label_price);
            panel1.Controls.Add(label_instruments);
            panel1.Controls.Add(comboBox_instruments);
            panel1.Controls.Add(label_bestoffer_value);
            panel1.Controls.Add(label_bestbid_value);
            panel1.Controls.Add(label_bids);
            panel1.Controls.Add(label_bestoffer);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1072, 509);
            panel1.TabIndex = 1;
            // 
            // button_remove_instrument
            // 
            button_remove_instrument.Location = new Point(869, 309);
            button_remove_instrument.Name = "button_remove_instrument";
            button_remove_instrument.Size = new Size(53, 23);
            button_remove_instrument.TabIndex = 16;
            button_remove_instrument.Text = "<<";
            button_remove_instrument.UseVisualStyleBackColor = true;
            // 
            // button_add_instrument
            // 
            button_add_instrument.Location = new Point(867, 251);
            button_add_instrument.Name = "button_add_instrument";
            button_add_instrument.Size = new Size(55, 28);
            button_add_instrument.TabIndex = 15;
            button_add_instrument.Text = ">>";
            button_add_instrument.UseVisualStyleBackColor = true;
            // 
            // label_add_subscription
            // 
            label_add_subscription.AutoSize = true;
            label_add_subscription.Location = new Point(792, 32);
            label_add_subscription.Name = "label_add_subscription";
            label_add_subscription.Size = new Size(187, 15);
            label_add_subscription.TabIndex = 14;
            label_add_subscription.Text = "ADD INSTRUMENT SUBSCRIPTION";
            // 
            // listBox_subscribed_instruments
            // 
            listBox_subscribed_instruments.FormattingEnabled = true;
            listBox_subscribed_instruments.ItemHeight = 15;
            listBox_subscribed_instruments.Location = new Point(928, 79);
            listBox_subscribed_instruments.Name = "listBox_subscribed_instruments";
            listBox_subscribed_instruments.Size = new Size(120, 394);
            listBox_subscribed_instruments.TabIndex = 13;
            // 
            // listBox_instruments
            // 
            listBox_instruments.FormattingEnabled = true;
            listBox_instruments.ItemHeight = 15;
            listBox_instruments.Location = new Point(741, 78);
            listBox_instruments.Name = "listBox_instruments";
            listBox_instruments.Size = new Size(120, 394);
            listBox_instruments.TabIndex = 12;
            // 
            // dataGridView_offers
            // 
            dataGridView_offers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_offers.Location = new Point(414, 300);
            dataGridView_offers.Name = "dataGridView_offers";
            dataGridView_offers.RowTemplate.Height = 25;
            dataGridView_offers.Size = new Size(289, 173);
            dataGridView_offers.TabIndex = 11;
            // 
            // dataGridView_bids
            // 
            dataGridView_bids.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_bids.Location = new Point(414, 78);
            dataGridView_bids.Name = "dataGridView_bids";
            dataGridView_bids.RowTemplate.Height = 25;
            dataGridView_bids.Size = new Size(289, 163);
            dataGridView_bids.TabIndex = 10;
            // 
            // label_price_value
            // 
            label_price_value.AutoSize = true;
            label_price_value.Location = new Point(306, 37);
            label_price_value.Name = "label_price_value";
            label_price_value.Size = new Size(0, 15);
            label_price_value.TabIndex = 9;
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
            // HomeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1073, 509);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "HomeForm";
            Text = "Form1";
            Load += HomeForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_offers).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView_bids).EndInit();
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
        private DataGridView dataGridView_bids;
        private DataGridView dataGridView_offers;
        private Label label_add_subscription;
        private ListBox listBox_subscribed_instruments;
        private ListBox listBox_instruments;
        private Button button_remove_instrument;
        private Button button_add_instrument;
    }
}