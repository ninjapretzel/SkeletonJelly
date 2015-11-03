/* 
 * 
**/

//Compile flags:
//#define RETURN_VALUES_BY_DEFAULT
//When enabled, changes the default behaviour of generated methods with no provided information to return a 'default' value of whatever type of method it is.
//When disabled, default behaviour of 'throw new NotImplementedException();' is used for all methods that do not provide any default information

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Reflection;

namespace SkeletonJelly {
	public class SkeletonGenerator {
		const string CLASSDEF_REGEX = @"(\w+\ )+\{(.|\n)*?\}";

		const string USING_REGEX = @"using.*\;";

		public List<SJClass> classes;
		public List<string> usings;

		public string header { 
			get {
				StringBuilder str = new StringBuilder();
				foreach (string use in usings) { str += use + "\n"; }
				return str.ToString();
			}
		}

		public SkeletonGenerator(string input) {
			classes = new List<SJClass>();
			usings = new List<string>();
			Regex classReg = new Regex(CLASSDEF_REGEX);
			Regex usingReg = new Regex(USING_REGEX);
			var match = classReg.Match(input);

			int i = 0;
			while (match.Success) {
				//Console.WriteLine("Match " + i + ":\n" + match.Value);
				SJClass made = new SJClass(match.Value);

				classes.Add(made);
				//Console.WriteLine(made.GeneratedString());


				match = match.NextMatch();
				i++;
			}
			
			match = usingReg.Match(input);
			Console.WriteLine("match: " + match.Value);
			while (match.Success) {
				usings.Add(match.Value);
				Console.WriteLine("Using: [" + match.Value + "]");

				match = match.NextMatch();
			}



		}



	}






	public class SJClass {
		public string name { get; private set; }
		private string type;
		private string[] bindings;
		private List<SJMethod> methods;

		public SJClass(string text) {
			methods = new List<SJMethod>();
			string[] lines = text.Split('\n');
			string defLine = lines[0].UpToLast("{").Trim();
			string[] defSplits = defLine.Split(' ');
			bindings = new string[defSplits.Length - 2];
			for (int i = 0; i < bindings.Length; i++) {
				bindings[i] = defSplits[i];
			}
			name = defSplits[defSplits.Length - 1];
			type = defSplits[defSplits.Length - 2];
			for (int i = 1; i < lines.Length - 1; i++) {
				string line = lines[i].Trim().UpToFirst("//");
				try {
					if (line.Length > 4) {
						SJMethod method = new SJMethod(line);
						methods.Add(method);
					}
				} catch (Exception e) {
					Console.WriteLine("Failed to parse function from line [" + line + "]");
					Console.WriteLine(e);
				}
			}
		}

		public override string ToString() {
			StringBuilder str = new StringBuilder();
			foreach (string s in bindings) { str += s + " "; }
			str += type + " " + name + " {";
			foreach (var method in methods) { str += "\n\t" + method.ToString(); }
			str += "\n}";

			return str.ToString();
		}

		public string GeneratedString() {
			StringBuilder str = new StringBuilder();
			foreach (string s in bindings) { str += s + " "; }
			str += type + " " + name + " {";
			foreach (var method in methods) { str += "\n\t" + method.GeneratedString().IncreaseIndent(); }
			str += "\n}";


			return str.ToString();
		}
	}

	public class SJMethod {
		public string type { get; private set; }
		public string name { get; private set; }
		public object retVal { get; private set; }
		public bool exception { get; private set; }
		public string[] bindings;
		public SJSignature[] signature;

		const string FUNCNAME_REGEX = @"(\w[\w\d]*|\S+)\(";
		const string SIGNATURE_REGEX = @"\(.*\)";

		public SJMethod(string line) {
			Regex func = new Regex(FUNCNAME_REGEX);
			Regex sig = new Regex(SIGNATURE_REGEX);
			var funcNameMatch = func.Match(line);
			var signatureMatch = sig.Match(line);

			string preName = line.Substring(0, funcNameMatch.Index - 1);

			name = funcNameMatch.Value.Substring(0, funcNameMatch.Length - 1);
			string[] preSplit = preName.Split(' ');
			bindings = preSplit.Take(preSplit.Length - 1).ToArray();
			type = preSplit[preSplit.Length - 1];

			string innerSig = signatureMatch.Value.Substring(1, signatureMatch.Length - 2);
			if (innerSig.Length > 0) {
				string[] sigSplits = innerSig.Split(',');
				signature = new SJSignature[sigSplits.Length];
				for (int i = 0; i < signature.Length; i++) {
					signature[i] = new SJSignature(sigSplits[i].Trim());

				}
			} else {
				signature = new SJSignature[0];
			}

			exception = false;
			if (line.Contains(":")) {
				string rest = line.FromLast(':').Trim();
				decimal parsed;

				if (rest.StartsWith("throw")) {
					exception = true;
					string ex = rest.FromFirst("throw").Trim();
					if (ex.Length == 0) { ex = "NotImplementedException"; }
					retVal = ex;
					//TBD, Set exception as return value...
				} else if (rest == "null") {
					retVal = null;
				} else if (rest.StartsWith("\"") && rest.EndsWith("\"")) {
					retVal = rest.Substring(1, rest.Length - 2);
				} else if (decimal.TryParse(rest, out parsed)) {
					retVal = parsed;
				}
			} else {//No value is provided. What to do?
#if RETURN_VALUES_BY_DEFAULT
				//This block of code is if the default behaviour is to return a value
				Type t = Helpers.GetType(type);
				//Console.WriteLine("" + type + ":[" + t + "]:");
				if (t == null) { 
				retVal = null; 
				} else if (t.IsNumeric()) {
				retVal = 0;
				} else if (t == typeof(bool)) {
				retVal = false;
				} else if (t == typeof(char)) { 
				retVal = '!';
				} else if (t == typeof(string)) {
				retVal = "";
				} else {
				retVal = null;
				}
				//*/
#else
				//Otherwise this block of code that defaults to 'throw new NotImplementedException();' is used
				exception = true;
				retVal = "NotImplementedException";
#endif
			}

		}

		public override string ToString() {
			StringBuilder b = new StringBuilder();
			foreach (string bind in bindings) { b += bind + " "; }
			b += type + " " + name + "(";
			for (int i = 0; i < signature.Length; i++) { b += signature[i] + ((i + 1 < signature.Length) ? ", " : ""); }
			b += ")";

			return b.ToString();
		}

		public string GeneratedString() {
			StringBuilder str = new StringBuilder(ToString());
			str += " {\n\t";
			if (exception) {
				str += "throw new " + retVal + "(\"Not Implemented Yet.\");";
			} else if (type == "void") {
				str += "//TBD: Write method contents";
			} else {
				str += "return ";
				if (retVal == null) { str += "null"; } else if (type == "string" || type == "System.String") { str += "\"" + retVal + "\""; } else { str += retVal; }
				str += ";";
			}

			str += "\n}";
			return str.ToString();
		}


	}

	public class SJSignature {
		public string name { get; private set; }
		public string type { get; private set; }
		public string mod { get; private set; }

		public SJSignature(string sig) {

			string[] stuff = sig.Split(' ');
			if (stuff.Length == 2) {
				mod = "";
				type = stuff[0];
				name = stuff[1];
			}
			if (stuff.Length == 3) {
				mod = stuff[0];
				type = stuff[1];
				name = stuff[2];
			}

		}

		public override string ToString() {
			if (mod == "") {
				return type + " " + name;
			}
			return mod + " " + type + " " + name;
		}
	}





	public static class Helpers {
		static Dictionary<string, Type> types = new Dictionary<string, Type>() {
			{"bool", typeof(bool)},
			{"byte", typeof(byte)},
			{"sbyte", typeof(sbyte)},
			{"char", typeof(char)},
			{"decimal", typeof(decimal)},
			{"double", typeof(double)},
			{"float", typeof(float)},
			{"int", typeof(int)},
			{"uint", typeof(uint)},
			{"long", typeof(long)},
			{"ulong", typeof(ulong)},
			{"object", typeof(object)},
			{"short", typeof(short)},
			{"ushort", typeof(ushort)},
			{"string", typeof(string)},
		};
		/// <summary> Array of numeric types </summary>
		static Type[] numericTypes = new Type[] { 
			typeof(byte),
			typeof(sbyte),
			typeof(decimal), 
			typeof(double), 
			typeof(float), 
			typeof(int), 
			typeof(uint), 
			typeof(long),
			typeof(ulong),
			typeof(short), 
			typeof(ushort), 
		};

		/// <summary> is a type a numeric type? </summary>
		public static bool IsNumeric(this Type type) {
			return numericTypes.Contains(type);
		}

		public static Type GetType(string name, int expectedPos = 0) {
			if (types.ContainsKey(name)) { return types[name]; }

			var found = GetTypeByName(name);
			if (found.Length == 0) { return null; }
			if (found.Length > 1) {
				Console.WriteLine("Potential problem- multiple classes named " + name + " found");
			}
			return found[expectedPos];
		}

		public static Type[] GetTypeByName(string className) {
			List<Type> returnVal = new List<Type>();

			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
				Type[] assemblyTypes = a.GetTypes();
				for (int j = 0; j < assemblyTypes.Length; j++) {
					if (assemblyTypes[j].Name == className) {
						returnVal.Add(assemblyTypes[j]);
					}
				}
			}

			return returnVal.ToArray();
		}
		public static string IncreaseIndent(this string s) { return s.Replace("\n", "\n\t"); }
		public static string DecreaseIndent(this string s) { return s.Replace("\n\t", "\n"); }

		///Converts all backslashes in a path to forward slashes.
		public static string ForwardSlashPath(this string path) {
			return path.Replace('\\', '/');
		}

		///Gets the last folder's name of a given string
		public static string DirectoryName(this string path) {
			return path.Substring(path.LastIndexOf("/") + 1);
		}

		///Moves a string representing a path to point one directory above.
		public static string PreviousDirectory(this string path) {
			return path.Substring(0, path.LastIndexOf("/"));
		}

		///<summary>Returns the substring of a string up to the first instance of a character.</summary>
		public static string UpToFirst(this string s, char c) {
			int index = s.IndexOf(c);
			if (index == -1) { return s; }
			return s.Substring(0, index);
		}
		///<summary>Returns the substring of a string up to the first instance of a string.</summary>
		public static string UpToFirst(this string s, string c) {
			int index = s.IndexOf(c);
			if (index == -1) { return s; }
			return s.Substring(0, index);
		}

		///<summary>Returns the substring of a string up to the last instance of a character.</summary>
		public static string UpToLast(this string s, char c) {
			int lastIndex = s.LastIndexOf(c);
			if (lastIndex == -1) { return s; }
			return s.Substring(0, lastIndex);
		}
		///<summary>Returns the substring of a string up to the last instance of a string.</summary>
		public static string UpToLast(this string s, string c) {
			int lastIndex = s.LastIndexOf(c);
			if (lastIndex == -1) { return s; }
			return s.Substring(0, lastIndex);
		}

		///<summary>Returns the substring of a string from the first instance of a character. (Not Inclusive)</summary>
		public static string FromFirst(this string s, char c) {
			int index = s.IndexOf(c);
			if (index == -1) { return s; }
			return s.Substring(index + 1);
		}
		///<summary>Returns the substring of a string from the first instance of a string. (Not Inclusive)</summary>
		public static string FromFirst(this string s, string c) {
			int index = s.IndexOf(c);
			if (index == -1) { return s; }
			return s.Substring(index + c.Length);
		}

		///<summary>Returns the substring of a string from the last instance of a character. (Not Inclusive)</summary>
		public static string FromLast(this string s, char c) {
			int lastIndex = s.LastIndexOf(c);
			if (lastIndex == -1) { return s; }
			return s.Substring(lastIndex + 1);
		}
		///<summary>Returns the substring of a string from the last instance of a string. (Not Inclusive)</summary>
		public static string FromLast(this string s, string c) {
			int lastIndex = s.LastIndexOf(c);
			if (lastIndex == -1) { return s; }
			return s.Substring(lastIndex + c.Length);
		}
	}


}

