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
using System.IO;
using System.Xml;

namespace Gir2Gapi {

	public class GirDocument : XmlDocument {

		public GirDocument (string path) : base ()
		{
			Load (path);
			if (DocumentElement.Name != "repository")
				throw new Exception ("Invalid file");
			foreach (XmlNode node in DocumentElement.ChildNodes) {
				XmlElement elem = node as XmlElement;
				if (elem == null)
					continue;
				switch (node.Name) {
				case "include":
					string inc_path = Path.Combine (Path.GetDirectoryName (path), elem.GetAttribute ("name") + "-" + elem.GetAttribute ("version") + ".gir");
					if (File.Exists (inc_path)) {
						XmlDocument inc = new XmlDocument ();
						inc.Load (inc_path);
						SymbolTable.AddImport (inc.DocumentElement ["namespace"]);
					} else
						Console.WriteLine ("unable to find included gir " + inc_path);
					break;
				case "package":
				case "c:include":
					break;
				case "namespace":
					SymbolTable.AddTypes (elem);
					namespaces.Add (new Namespace (elem));
					break;
				default:
					Console.WriteLine ("Unexpected node in repository doc: " + node.Name);
					break;
				}
			}
		}

		List<Namespace> namespaces = new List<Namespace> ();
		public List<Namespace> Namespaces {
			get { return namespaces; }
		}
	}
}

