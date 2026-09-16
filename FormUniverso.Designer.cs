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
			panelUniverso.Dock = DockStyle.Fill;
			panelUniverso.Location = new Point(0, 0);
			panelUniverso.MaximumSize = new Size(1920, 1080);
			panelUniverso.MinimumSize = new Size(1920, 1080);
			panelUniverso.Name = "panelUniverso";
			panelUniverso.Size = new Size(1920, 1080);
			panelUniverso.TabIndex = 0;
			panelUniverso.Paint += panelUniverso_Paint;
			// 
			// FormUniverso
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(panelUniverso);
			Name = "FormUniverso";
			Text = "FormUniverso";
			ResumeLayout(false);
		}

		#endregion

		private Panel panelUniverso;
	}
}