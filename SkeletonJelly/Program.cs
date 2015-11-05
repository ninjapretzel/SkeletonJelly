using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkeletonJelly {
	static class Program {

		public static List<string> log = new List<string>();
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		
		[STAThread]
		static void Main(string[] args) {
			if (args.Length == 0) {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
			if (args.Length == 1) {
				string srcPath = RelativeToAbsolute(args[0]);
				Convert(srcPath, srcPath);
			}
			if (args.Length == 2) {
				string srcPath = RelativeToAbsolute(args[0]);
				string dstPath = RelativeToAbsolute(args[1]);

				Convert(srcPath, dstPath);
			}

		}

		public static string RelativeToAbsolute(string path) {
			if (!path.Contains(":")) {
				return Directory.GetCurrentDirectory() + "/" + path;
			}
			return path;
		}

		public static void Convert(string srcPath, string dstPath) {
			if (dstPath.Length < 1 || !Directory.Exists(dstPath)) {
				dstPath = srcPath;
			}

			List<string> sjFiles = new List<string>();

			string[] files = Directory.GetFiles(srcPath);
			foreach (string s in files) {
				string file = s;
				if (file.EndsWith(".sj")) {
					file = file.ForwardSlashPath();
					sjFiles.Add(file);
				}
			}

			log = new List<string>();
			if (sjFiles != null) {
				foreach (string file in sjFiles) {
					string text = File.ReadAllText(file);

					SkeletonGenerator gen = new SkeletonGenerator(text);
					string header = gen.header;

					foreach (SJClass c in gen.classes) {
						StringBuilder generated = new StringBuilder(header);
						generated += c.GeneratedString();

						string path = dstPath + "/" + c.name + ".cs";
						File.WriteAllText(path, generated.ToString());
						log.Add("Created File:" + path);

					}

				}

			}

			
		}


	}



}
