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
			Cabecalho = new Panel();
			Titulo = new Label();
			Lateral = new Panel();
			btnGerar = new Button();
			numericVelMax = new NumericUpDown();
			numericVelMin = new NumericUpDown();
			numericMassaMax = new NumericUpDown();
			numericQuant = new NumericUpDown();
			numericMassaMin = new NumericUpDown();
			labelVelMax = new Label();
			labelVelMin = new Label();
			labelMassaMax = new Label();
			labelMassaMin = new Label();
			labelQuant = new Label();
			Cabecalho.SuspendLayout();
			Lateral.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numericVelMax).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericVelMin).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericMassaMax).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericQuant).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericMassaMin).BeginInit();
			SuspendLayout();
			// 
			// Cabecalho
			// 
			Cabecalho.BackColor = SystemColors.ControlDark;
			Cabecalho.Controls.Add(Titulo);
			Cabecalho.Dock = DockStyle.Top;
			Cabecalho.Location = new Point(0, 0);
			Cabecalho.Margin = new Padding(3, 2, 3, 2);
			Cabecalho.Name = "Cabecalho";
			Cabecalho.Size = new Size(526, 122);
			Cabecalho.TabIndex = 1;
			// 
			// Titulo
			// 
			Titulo.AutoSize = true;
			Titulo.Font = new Font("Segoe UI", 22F);
			Titulo.Location = new Point(95, 25);
			Titulo.MinimumSize = new Size(262, 75);
			Titulo.Name = "Titulo";
			Titulo.Size = new Size(330, 75);
			Titulo.TabIndex = 0;
			Titulo.Text = "Simulador Gravitacional";
			Titulo.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// Lateral
			// 
			Lateral.BackColor = SystemColors.Control;
			Lateral.Controls.Add(btnGerar);
			Lateral.Controls.Add(numericVelMax);
			Lateral.Controls.Add(numericVelMin);
			Lateral.Controls.Add(numericMassaMax);
			Lateral.Controls.Add(numericQuant);
			Lateral.Controls.Add(numericMassaMin);
			Lateral.Controls.Add(labelVelMax);
			Lateral.Controls.Add(labelVelMin);
			Lateral.Controls.Add(labelMassaMax);
			Lateral.Controls.Add(labelMassaMin);
			Lateral.Controls.Add(labelQuant);
			Lateral.Dock = DockStyle.Fill;
			Lateral.Location = new Point(0, 122);
			Lateral.Margin = new Padding(3, 2, 3, 2);
			Lateral.Name = "Lateral";
			Lateral.Size = new Size(526, 343);
			Lateral.TabIndex = 2;
			// 
			// btnGerar
			// 
			btnGerar.Font = new Font("Segoe UI", 15F);
			btnGerar.Location = new Point(173, 259);
			btnGerar.Margin = new Padding(3, 2, 3, 2);
			btnGerar.Name = "btnGerar";
			btnGerar.Size = new Size(158, 46);
			btnGerar.TabIndex = 10;
			btnGerar.Text = "GERAR";
			btnGerar.UseVisualStyleBackColor = true;
			btnGerar.Click += btnGerar_Click;
			// 
			// numericVelMax
			// 
			numericVelMax.Font = new Font("Segoe UI", 13F);
			numericVelMax.Location = new Point(348, 204);
			numericVelMax.Margin = new Padding(3, 2, 3, 2);
			numericVelMax.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
			numericVelMax.Minimum = new decimal(new int[] { 10, 0, 0, int.MinValue });
			numericVelMax.Name = "numericVelMax";
			numericVelMax.Size = new Size(77, 31);
			numericVelMax.TabIndex = 9;
			numericVelMax.Value = new decimal(new int[] { 10, 0, 0, 0 });
			// 
			// numericVelMin
			// 
			numericVelMin.Font = new Font("Segoe UI", 13F);
			numericVelMin.Location = new Point(348, 173);
			numericVelMin.Margin = new Padding(3, 2, 3, 2);
			numericVelMin.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
			numericVelMin.Minimum = new decimal(new int[] { 10, 0, 0, int.MinValue });
			numericVelMin.Name = "numericVelMin";
			numericVelMin.Size = new Size(77, 31);
			numericVelMin.TabIndex = 8;
			numericVelMin.Value = new decimal(new int[] { 10, 0, 0, int.MinValue });
			// 
			// numericMassaMax
			// 
			numericMassaMax.Font = new Font("Segoe UI", 13F);
			numericMassaMax.Location = new Point(348, 123);
			numericMassaMax.Margin = new Padding(3, 2, 3, 2);
			numericMassaMax.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			numericMassaMax.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numericMassaMax.Name = "numericMassaMax";
			numericMassaMax.Size = new Size(77, 31);
			numericMassaMax.TabIndex = 7;
			numericMassaMax.Value = new decimal(new int[] { 1000, 0, 0, 0 });
			// 
			// numericQuant
			// 
			numericQuant.Font = new Font("Segoe UI", 13F);
			numericQuant.Location = new Point(348, 44);
			numericQuant.Margin = new Padding(3, 2, 3, 2);
			numericQuant.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			numericQuant.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numericQuant.Name = "numericQuant";
			numericQuant.Size = new Size(77, 31);
			numericQuant.TabIndex = 6;
			numericQuant.Value = new decimal(new int[] { 1, 0, 0, 0 });
			// 
			// numericMassaMin
			// 
			numericMassaMin.Font = new Font("Segoe UI", 13F);
			numericMassaMin.Location = new Point(348, 91);
			numericMassaMin.Margin = new Padding(3, 2, 3, 2);
			numericMassaMin.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			numericMassaMin.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numericMassaMin.Name = "numericMassaMin";
			numericMassaMin.Size = new Size(77, 31);
			numericMassaMin.TabIndex = 5;
			numericMassaMin.Value = new decimal(new int[] { 1, 0, 0, 0 });
			// 
			// labelVelMax
			// 
			labelVelMax.AutoSize = true;
			labelVelMax.Font = new Font("Segoe UI", 13F);
			labelVelMax.Location = new Point(119, 208);
			labelVelMax.Name = "labelVelMax";
			labelVelMax.Size = new Size(213, 25);
			labelVelMax.TabIndex = 4;
			labelVelMax.Text = "Velocidade inicial máxima\r\n";
			// 
			// labelVelMin
			// 
			labelVelMin.AutoSize = true;
			labelVelMin.Font = new Font("Segoe UI", 13F);
			labelVelMin.Location = new Point(119, 178);
			labelVelMin.Name = "labelVelMin";
			labelVelMin.Size = new Size(210, 25);
			labelVelMin.TabIndex = 3;
			labelVelMin.Text = "Velocidade inicial Mínima";
			// 
			// labelMassaMax
			// 
			labelMassaMax.AutoSize = true;
			labelMassaMax.Font = new Font("Segoe UI", 13F);
			labelMassaMax.Location = new Point(119, 129);
			labelMassaMax.Name = "labelMassaMax";
			labelMassaMax.Size = new Size(129, 25);
			labelMassaMax.TabIndex = 2;
			labelMassaMax.Text = "Massa máxima";
			// 
			// labelMassaMin
			// 
			labelMassaMin.AutoSize = true;
			labelMassaMin.Font = new Font("Segoe UI", 13F);
			labelMassaMin.Location = new Point(119, 91);
			labelMassaMin.Name = "labelMassaMin";
			labelMassaMin.Size = new Size(126, 25);
			labelMassaMin.TabIndex = 1;
			labelMassaMin.Text = "Massa mínima";
			// 
			// labelQuant
			// 
			labelQuant.AutoSize = true;
			labelQuant.Font = new Font("Segoe UI", 13F);
			labelQuant.Location = new Point(119, 46);
			labelQuant.Name = "labelQuant";
			labelQuant.Size = new Size(105, 25);
			labelQuant.TabIndex = 0;
			labelQuant.Text = "Quantidade";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(526, 465);
			Controls.Add(Lateral);
			Controls.Add(Cabecalho);
			Margin = new Padding(3, 2, 3, 2);
			MaximumSize = new Size(542, 504);
			MinimumSize = new Size(542, 504);
			Name = "Form1";
			Text = "Form1";
			Load += Form1_Load;
			Cabecalho.ResumeLayout(false);
			Cabecalho.PerformLayout();
			Lateral.ResumeLayout(false);
			Lateral.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numericVelMax).EndInit();
			((System.ComponentModel.ISupportInitialize)numericVelMin).EndInit();
			((System.ComponentModel.ISupportInitialize)numericMassaMax).EndInit();
			((System.ComponentModel.ISupportInitialize)numericQuant).EndInit();
			((System.ComponentModel.ISupportInitialize)numericMassaMin).EndInit();
			ResumeLayout(false);
		}

		#endregion
		private Panel Cabecalho;
        private Label Titulo;
        private Panel Lateral;
        private Label labelVelMax;
        private Label labelVelMin;
        private Label labelMassaMax;
        private Label labelMassaMin;
        private Label labelQuant;
        private NumericUpDown numericVelMax;
        private NumericUpDown numericVelMin;
        private NumericUpDown numericMassaMax;
        private NumericUpDown numericQuant;
        private NumericUpDown numericMassaMin;
        private Button btnGerar;
    }
}
