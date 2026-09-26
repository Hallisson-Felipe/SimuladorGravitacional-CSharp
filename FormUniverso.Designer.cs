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
            labelIteracoes = new Label();
            dataGridView1 = new DataGridView();
            ColNome = new DataGridViewTextBoxColumn();
            ColMassa = new DataGridViewTextBoxColumn();
            ColDensidade = new DataGridViewTextBoxColumn();
            ColPosX = new DataGridViewTextBoxColumn();
            ColPosY = new DataGridViewTextBoxColumn();
            ColVelX = new DataGridViewTextBoxColumn();
            ColVelY = new DataGridViewTextBoxColumn();
            panelController = new Panel();
            btnDados = new Button();
            trackBar1 = new TrackBar();
            panel1 = new PanelUniverso();
            btnParar = new Button();
            btnIniciar = new Button();
            timerUniverso = new System.Windows.Forms.Timer(components);
            panelUniverso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panelController.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelUniverso
            // 
            panelUniverso.BackColor = Color.DimGray;
            panelUniverso.BackgroundImageLayout = ImageLayout.Stretch;
            panelUniverso.Controls.Add(labelIteracoes);
            panelUniverso.Controls.Add(dataGridView1);
            panelUniverso.Controls.Add(panelController);
            panelUniverso.Dock = DockStyle.Fill;
            panelUniverso.Location = new Point(0, 0);
            panelUniverso.Margin = new Padding(3, 4, 3, 4);
            panelUniverso.Name = "panelUniverso";
            panelUniverso.Size = new Size(945, 603);
            panelUniverso.TabIndex = 0;
            panelUniverso.Paint += panelUniverso_Paint;
            // 
            // labelIteracoes
            // 
            labelIteracoes.AutoSize = true;
            labelIteracoes.Font = new Font("Segoe UI", 15F);
            labelIteracoes.ForeColor = SystemColors.Control;
            labelIteracoes.Location = new Point(0, 0);
            labelIteracoes.Name = "labelIteracoes";
            labelIteracoes.Size = new Size(137, 35);
            labelIteracoes.TabIndex = 5;
            labelIteracoes.Text = "X iterações";
            labelIteracoes.Visible = false;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.ControlDarkDark;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColNome, ColMassa, ColDensidade, ColPosX, ColPosY, ColVelX, ColVelY });
            dataGridView1.Location = new Point(534, 0);
            dataGridView1.MaximumSize = new Size(411, 509);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(411, 509);
            dataGridView1.TabIndex = 1;
            dataGridView1.Visible = false;
            // 
            // ColNome
            // 
            ColNome.HeaderText = "Nome";
            ColNome.MinimumWidth = 6;
            ColNome.Name = "ColNome";
            ColNome.ReadOnly = true;
            // 
            // ColMassa
            // 
            ColMassa.HeaderText = "Massa";
            ColMassa.MinimumWidth = 6;
            ColMassa.Name = "ColMassa";
            ColMassa.ReadOnly = true;
            // 
            // ColDensidade
            // 
            ColDensidade.HeaderText = "Densidade";
            ColDensidade.MinimumWidth = 6;
            ColDensidade.Name = "ColDensidade";
            ColDensidade.ReadOnly = true;
            // 
            // ColPosX
            // 
            ColPosX.HeaderText = "PosX";
            ColPosX.MinimumWidth = 6;
            ColPosX.Name = "ColPosX";
            ColPosX.ReadOnly = true;
            // 
            // ColPosY
            // 
            ColPosY.HeaderText = "PosY";
            ColPosY.MinimumWidth = 6;
            ColPosY.Name = "ColPosY";
            ColPosY.ReadOnly = true;
            // 
            // ColVelX
            // 
            ColVelX.HeaderText = "VelX";
            ColVelX.MinimumWidth = 6;
            ColVelX.Name = "ColVelX";
            ColVelX.ReadOnly = true;
            // 
            // ColVelY
            // 
            ColVelY.HeaderText = "VelY";
            ColVelY.MinimumWidth = 6;
            ColVelY.Name = "ColVelY";
            ColVelY.ReadOnly = true;
            // 
            // panelController
            // 
            panelController.BackColor = Color.Transparent;
            panelController.Controls.Add(btnDados);
            panelController.Controls.Add(trackBar1);
            panelController.Controls.Add(panel1);
            panelController.Dock = DockStyle.Bottom;
            panelController.Location = new Point(0, 509);
            panelController.Name = "panelController";
            panelController.Size = new Size(945, 94);
            panelController.TabIndex = 0;
            panelController.Paint += panelController_Paint;
            // 
            // btnDados
            // 
            btnDados.Anchor = AnchorStyles.Right;
            btnDados.Location = new Point(791, 26);
            btnDados.Name = "btnDados";
            btnDados.Size = new Size(132, 43);
            btnDados.TabIndex = 4;
            btnDados.Text = "Mostrar dados";
            btnDados.UseVisualStyleBackColor = true;
            btnDados.Click += btnDados_Click;
            // 
            // trackBar1
            // 
            trackBar1.BackColor = Color.DimGray;
            trackBar1.Location = new Point(324, 26);
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
            panel1.Controls.Add(btnIniciar);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(318, 94);
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
            // btnIniciar
            // 
            btnIniciar.BackColor = Color.YellowGreen;
            btnIniciar.Dock = DockStyle.Right;
            btnIniciar.Location = new Point(156, 0);
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
            panelUniverso.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panelController.ResumeLayout(false);
            panelController.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timerUniverso;
        private Panel panelController;
        private Button btnParar;
        private Button btnIniciar;
        private PanelUniverso panelUniverso;
        private TrackBar trackBar1;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn ColNome;
        private DataGridViewTextBoxColumn ColMassa;
        private DataGridViewTextBoxColumn ColDensidade;
        private DataGridViewTextBoxColumn ColPosX;
        private DataGridViewTextBoxColumn ColPosY;
        private DataGridViewTextBoxColumn ColVelX;
        private DataGridViewTextBoxColumn ColVelY;
        private Button btnDados;
        private PanelUniverso panel1;
        private Label labelIteracoes;
    }
}