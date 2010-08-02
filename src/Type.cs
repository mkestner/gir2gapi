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
using System.Xml;

namespace Gir2Gapi {

	public class Type {

		XmlElement elem;

		public Type (XmlElement elem)
		{
			this.elem = elem;
		}

		string GetCType (XmlElement type_elem)
		{
			return type_elem.HasAttribute ("c:type") ? type_elem.GetAttribute ("c:type") : SymbolTable.Lookup (type_elem.GetAttribute ("name"));
		}

		public void UpdateGapiElement (XmlElement gapi_elem)
		{
			if (Converter.Default.Verbose)
				Validate ();
			gapi_elem.SetAttribute ("type", GetCType (elem));
			if (elem ["type"] != null)
				gapi_elem.SetAttribute ("element_type", GetCType (elem ["type"]));
		}

		void Validate ()
		{
			foreach (XmlAttribute attr in elem.Attributes) {
				switch (attr.Name) {
				case "name":
				case "c:type":
					// known
					break;
				default:
					Console.WriteLine ("Unexpected attribute on type element: " + attr.Name);
					break;
				}
			}

			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "type":
					// known
					break;
				default:
					Console.WriteLine ("Unexpected child on type element: " + node.Name);
					break;
				}
			}
		}
	}
}

