namespace Coinbase.Exchange.FrontEnd
{
    partial class SettingsForm
    {
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
        private void InitializeComponent()
        {
            panel1 = new Panel();
            lbl_title = new Label();
            lstbox_instrument = new ListBox();
            lstbox_subcribedInstrument = new ListBox();
            btn_add = new Button();
            btn_remove = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btn_remove);
            panel1.Controls.Add(btn_add);
            panel1.Controls.Add(lstbox_subcribedInstrument);
            panel1.Controls.Add(lstbox_instrument);
            panel1.Controls.Add(lbl_title);
            panel1.Location = new Point(2, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(797, 449);
            panel1.TabIndex = 0;
            // 
            // lbl_title
            // 
            lbl_title.AutoSize = true;
            lbl_title.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_title.Location = new Point(256, 22);
            lbl_title.Name = "lbl_title";
            lbl_title.Size = new Size(263, 28);
            lbl_title.TabIndex = 0;
            lbl_title.Text = "Add Instrument Subscription";
            // 
            // lstbox_instrument
            // 
            lstbox_instrument.FormattingEnabled = true;
            lstbox_instrument.ItemHeight = 15;
            lstbox_instrument.Location = new Point(40, 67);
            lstbox_instrument.Name = "lstbox_instrument";
            lstbox_instrument.Size = new Size(275, 349);
            lstbox_instrument.TabIndex = 1;
            // 
            // lstbox_subcribedInstrument
            // 
            lstbox_subcribedInstrument.FormattingEnabled = true;
            lstbox_subcribedInstrument.ItemHeight = 15;
            lstbox_subcribedInstrument.Location = new Point(469, 67);
            lstbox_subcribedInstrument.Name = "lstbox_subcribedInstrument";
            lstbox_subcribedInstrument.Size = new Size(275, 349);
            lstbox_subcribedInstrument.TabIndex = 2;
            // 
            // btn_add
            // 
            btn_add.Location = new Point(357, 195);
            btn_add.Name = "btn_add";
            btn_add.Size = new Size(57, 23);
            btn_add.TabIndex = 3;
            btn_add.Text = ">>";
            btn_add.UseVisualStyleBackColor = true;
            btn_add.Click += btn_add_Click;
            // 
            // btn_remove
            // 
            btn_remove.Location = new Point(357, 262);
            btn_remove.Name = "btn_remove";
            btn_remove.Size = new Size(57, 23);
            btn_remove.TabIndex = 4;
            btn_remove.Text = "<<";
            btn_remove.UseVisualStyleBackColor = true;
            btn_remove.Click += btn_remove_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Name = "SettingsForm";
            Text = "SettingsForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lbl_title;
        private Button btn_remove;
        private Button btn_add;
        private ListBox lstbox_subcribedInstrument;
        private ListBox lstbox_instrument;
    }
}