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
            pageSetupDialog1 = new PageSetupDialog();
            btn_dashboard = new Button();
            btn_setup = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Dock = DockStyle.Left;
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(30, 450);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // panel1
            // 
            panel1.Controls.Add(btn_setup);
            panel1.Controls.Add(btn_dashboard);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(804, 450);
            panel1.TabIndex = 1;
            // 
            // btn_dashboard
            // 
            btn_dashboard.Location = new Point(200, 163);
            btn_dashboard.Name = "btn_dashboard";
            btn_dashboard.Size = new Size(151, 68);
            btn_dashboard.TabIndex = 0;
            btn_dashboard.Text = "DASHBOARD";
            btn_dashboard.UseVisualStyleBackColor = true;
            btn_dashboard.Click += btn_dashboard_Click;
            // 
            // btn_setup
            // 
            btn_setup.Location = new Point(455, 163);
            btn_setup.Name = "btn_setup";
            btn_setup.Size = new Size(151, 68);
            btn_setup.TabIndex = 1;
            btn_setup.Text = "SETUP";
            btn_setup.UseVisualStyleBackColor = true;
            btn_setup.Click += btn_setup_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private Panel panel1;
        private PageSetupDialog pageSetupDialog1;
        private Button btn_setup;
        private Button btn_dashboard;
    }
}