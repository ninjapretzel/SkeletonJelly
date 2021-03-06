using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkeletonJelly {
	public partial class Form1 : Form {
		string lastValidDir = null;
		List<string> sjFiles = null;
		List<string> sjFileNames = null;

		public Form1() {
			InitializeComponent();
		}

		public void LoadFiles(object sender, EventArgs e) {
			string dir = sourceDir.Text;
			if (Directory.Exists(dir) && lastValidDir != dir) {
				lastValidDir = dir;
				sjFiles = new List<string>();
				sjFileNames = new List<string>();

				string[] files = Directory.GetFiles(dir);
				foreach (string s in files) {
					string file = s;
					if (file.EndsWith(".sj")) {
						file = file.ForwardSlashPath();
						sjFiles.Add(file);
						sjFileNames.Add(file.FromLast("/"));
					}
				}

				fileBox.DataSource = sjFileNames;
				//filesLabel.Text = fileList;
			}


		}

		private void doItButton_Click(object sender, EventArgs e) {
			string srcPath = sourceDir.Text;
			string dstPath = destDir.Text;

			Program.Convert(srcPath, dstPath);

			logBox.Lines = Program.log.ToArray();
		}



	}
}
