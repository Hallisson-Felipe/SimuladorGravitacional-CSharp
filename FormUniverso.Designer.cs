namespace SimuladorGavitacional
{
	partial class FormUniverso
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
            components = new System.ComponentModel.Container();
            panelUniverso = new PanelUniverso();
            panelController = new Panel();
            labelVelocidade = new Label();
            trackBar1 = new TrackBar();
            panel1 = new Panel();
            btnParar = new Button();
            btnReiniciar = new Button();
            btnIniciar = new Button();
            timerUniverso = new System.Windows.Forms.Timer(components);
            panelUniverso.SuspendLayout();
            panelController.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelUniverso
            // 
            panelUniverso.AutoSize = true;
            panelUniverso.BackColor = Color.DimGray;
            panelUniverso.BackgroundImageLayout = ImageLayout.Stretch;
            panelUniverso.Controls.Add(panelController);
            panelUniverso.Dock = DockStyle.Fill;
            panelUniverso.Location = new Point(0, 0);
            panelUniverso.Margin = new Padding(3, 4, 3, 4);
            panelUniverso.Name = "panelUniverso";
            panelUniverso.Size = new Size(945, 603);
            panelUniverso.TabIndex = 0;
            panelUniverso.Paint += panelUniverso_Paint;
            // 
            // panelController
            // 
            panelController.BackColor = Color.Transparent;
            panelController.Controls.Add(labelVelocidade);
            panelController.Controls.Add(trackBar1);
            panelController.Controls.Add(panel1);
            panelController.Dock = DockStyle.Bottom;
            panelController.Location = new Point(0, 509);
            panelController.Name = "panelController";
            panelController.Size = new Size(945, 94);
            panelController.TabIndex = 0;
            // 
            // labelVelocidade
            // 
            labelVelocidade.AutoSize = true;
            labelVelocidade.BackColor = SystemColors.Control;
            labelVelocidade.Font = new Font("Segoe UI", 20F);
            labelVelocidade.Location = new Point(860, 36);
            labelVelocidade.Name = "labelVelocidade";
            labelVelocidade.Size = new Size(54, 46);
            labelVelocidade.TabIndex = 4;
            labelVelocidade.Text = "1x";
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(557, 26);
            trackBar1.Maximum = 100;
            trackBar1.Minimum = 1;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(298, 56);
            trackBar1.TabIndex = 3;
            trackBar1.TickFrequency = 10;
            trackBar1.Value = 50;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(btnParar);
            panel1.Controls.Add(btnReiniciar);
            panel1.Controls.Add(btnIniciar);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(470, 94);
            panel1.TabIndex = 3;
            // 
            // btnParar
            // 
            btnParar.BackColor = Color.IndianRed;
            btnParar.Dock = DockStyle.Left;
            btnParar.Location = new Point(0, 0);
            btnParar.Name = "btnParar";
            btnParar.Size = new Size(156, 94);
            btnParar.TabIndex = 1;
            btnParar.Text = "Parar";
            btnParar.UseVisualStyleBackColor = false;
            btnParar.Click += btnParar_Click;
            // 
            // btnReiniciar
            // 
            btnReiniciar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnReiniciar.BackColor = Color.White;
            btnReiniciar.Location = new Point(162, 12);
            btnReiniciar.Name = "btnReiniciar";
            btnReiniciar.Size = new Size(140, 70);
            btnReiniciar.TabIndex = 2;
            btnReiniciar.Text = "Reiniciar";
            btnReiniciar.UseVisualStyleBackColor = false;
            btnReiniciar.Click += btnReiniciar_Click;
            // 
            // btnIniciar
            // 
            btnIniciar.BackColor = Color.YellowGreen;
            btnIniciar.Dock = DockStyle.Right;
            btnIniciar.Location = new Point(308, 0);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(162, 94);
            btnIniciar.TabIndex = 0;
            btnIniciar.Text = "Iniciar";
            btnIniciar.UseVisualStyleBackColor = false;
            btnIniciar.Click += btnIniciar_Click;
            // 
            // timerUniverso
            // 
            timerUniverso.Interval = 16;
            timerUniverso.Tick += timerUniverso_Tick;
            // 
            // FormUniverso
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(945, 603);
            Controls.Add(panelUniverso);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormUniverso";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Universo";
            Load += FormUniverso_Load;
            panelUniverso.ResumeLayout(false);
            panelController.ResumeLayout(false);
            panelController.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timerUniverso;
        private Panel panelController;
        private Button btnReiniciar;
        private Button btnParar;
        private Button btnIniciar;
        private PanelUniverso panelUniverso;
        private TrackBar trackBar1;
        private Panel panel1;
        private Label labelVelocidade;
    }
}