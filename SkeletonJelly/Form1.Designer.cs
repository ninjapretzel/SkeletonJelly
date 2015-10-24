namespace SkeletonJelly {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.label3 = new System.Windows.Forms.Label();
			this.sourceDir = new System.Windows.Forms.TextBox();
			this.destDir = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.fileBox = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.logBox = new System.Windows.Forms.TextBox();
			this.doItButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Source Directory";
			// 
			// sourceDir
			// 
			this.sourceDir.Location = new System.Drawing.Point(123, 10);
			this.sourceDir.Name = "sourceDir";
			this.sourceDir.Size = new System.Drawing.Size(354, 20);
			this.sourceDir.TabIndex = 2;
			this.sourceDir.TextChanged += new System.EventHandler(this.LoadFiles);
			// 
			// destDir
			// 
			this.destDir.Location = new System.Drawing.Point(123, 37);
			this.destDir.Name = "destDir";
			this.destDir.Size = new System.Drawing.Size(354, 20);
			this.destDir.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(105, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Destination Directory";
			// 
			// fileBox
			// 
			this.fileBox.FormattingEnabled = true;
			this.fileBox.Location = new System.Drawing.Point(15, 89);
			this.fileBox.Name = "fileBox";
			this.fileBox.Size = new System.Drawing.Size(298, 160);
			this.fileBox.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(15, 70);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(85, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Files Discovered";
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(332, 70);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(25, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "Log";
			// 
			// logBox
			// 
			this.logBox.AcceptsReturn = true;
			this.logBox.AcceptsTab = true;
			this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.logBox.Location = new System.Drawing.Point(335, 89);
			this.logBox.Multiline = true;
			this.logBox.Name = "logBox";
			this.logBox.ReadOnly = true;
			this.logBox.Size = new System.Drawing.Size(351, 162);
			this.logBox.TabIndex = 9;
			// 
			// doItButton
			// 
			this.doItButton.Location = new System.Drawing.Point(487, 10);
			this.doItButton.Name = "doItButton";
			this.doItButton.Size = new System.Drawing.Size(208, 47);
			this.doItButton.TabIndex = 10;
			this.doItButton.Text = "SkeletonJellify";
			this.doItButton.UseVisualStyleBackColor = true;
			this.doItButton.Click += new System.EventHandler(this.doItButton_Click);
			// 
			// Form1
			// 
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(698, 259);
			this.Controls.Add(this.doItButton);
			this.Controls.Add(this.logBox);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.fileBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.destDir);
			this.Controls.Add(this.sourceDir);
			this.Controls.Add(this.label3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(706, 286);
			this.MinimumSize = new System.Drawing.Size(706, 286);
			this.Name = "Form1";
			this.Text = "Skeleton Jelly";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox sourceDir;
		private System.Windows.Forms.TextBox destDir;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox fileBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox logBox;
		private System.Windows.Forms.Button doItButton;
	}
}

