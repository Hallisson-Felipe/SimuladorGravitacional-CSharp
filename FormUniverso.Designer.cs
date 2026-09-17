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
            panelUniverso = new Panel();
            SuspendLayout();
            // 
            // panelUniverso
            // 
            panelUniverso.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelUniverso.BackColor = SystemColors.ActiveCaption;
            panelUniverso.Location = new Point(83, 13);
            panelUniverso.Margin = new Padding(3, 4, 3, 4);
            panelUniverso.Name = "panelUniverso";
            panelUniverso.Size = new Size(744, 559);
            panelUniverso.TabIndex = 0;
            panelUniverso.Paint += panelUniverso_Paint;
            // 
            // FormUniverso
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(panelUniverso);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormUniverso";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormUniverso";
            Load += FormUniverso_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel panelUniverso;
	}
}