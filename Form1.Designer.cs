namespace SimuladorGavitacional
{
    partial class Form1
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
            Universo = new Panel();
            Cabecalho = new Panel();
            Titulo = new Label();
            Lateral = new Panel();
            Cabecalho.SuspendLayout();
            SuspendLayout();
            // 
            // Universo
            // 
            Universo.BackColor = SystemColors.Control;
            Universo.Dock = DockStyle.Fill;
            Universo.Location = new Point(0, 0);
            Universo.Name = "Universo";
            Universo.Size = new Size(1367, 740);
            Universo.TabIndex = 0;
            // 
            // Cabecalho
            // 
            Cabecalho.BackColor = SystemColors.ControlDark;
            Cabecalho.Controls.Add(Titulo);
            Cabecalho.Dock = DockStyle.Top;
            Cabecalho.Location = new Point(0, 0);
            Cabecalho.Name = "Cabecalho";
            Cabecalho.Size = new Size(1367, 163);
            Cabecalho.TabIndex = 1;
            // 
            // Titulo
            // 
            Titulo.AutoSize = true;
            Titulo.Font = new Font("Segoe UI", 22F);
            Titulo.Location = new Point(193, 31);
            Titulo.MaximumSize = new Size(1000, 100);
            Titulo.MinimumSize = new Size(1000, 100);
            Titulo.Name = "Titulo";
            Titulo.Size = new Size(1000, 100);
            Titulo.TabIndex = 0;
            Titulo.Text = "Simulador Gravitacional";
            Titulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Lateral
            // 
            Lateral.BackColor = SystemColors.ControlDarkDark;
            Lateral.Dock = DockStyle.Right;
            Lateral.Location = new Point(1027, 163);
            Lateral.Name = "Lateral";
            Lateral.Size = new Size(340, 577);
            Lateral.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1367, 740);
            Controls.Add(Lateral);
            Controls.Add(Cabecalho);
            Controls.Add(Universo);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            Cabecalho.ResumeLayout(false);
            Cabecalho.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel Universo;
        private Panel Cabecalho;
        private Label Titulo;
        private Panel Lateral;
    }
}
