// Copyright (c) 2010 Mike Kestner 
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.Xml;

namespace Gir2Gapi {

	public static class SymbolTable {

		static Dictionary<string, string> symbols;

		static SymbolTable ()
		{
			symbols = new Dictionary<string, string> ();
			symbols ["utf8"] = "gchar*";
		}

		public static void AddTypes (XmlElement ns)
		{
			AddNamespace (ns, false);
		}

		public static void AddImport (XmlElement import)
		{
			AddNamespace (import, true);
		}

		public static string Lookup (string name)
		{
			string ctype;
			if (symbols.TryGetValue (name, out ctype))
				return ctype;
			Console.WriteLine ("Lookup failed for type " + name);
			return null;
		}

		static void AddNamespace (XmlElement ns, bool qual)
		{
			string prefix = qual ? ns.GetAttribute ("name") + "." : String.Empty;

			foreach (XmlNode node in ns.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "bitfield":
				case "callback":
				case "class":
				case "enumeration":
				case "interface":
				case "record":
					symbols [prefix + child.GetAttribute ("name")] = child.GetAttribute ("c:type");
					break;
				default:
					break;
				}
			}
		}
	}
}

