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

	public class Array {

		XmlElement elem;

		public Array (XmlElement elem)
		{
			this.elem = elem;
		}

		public void UpdateGapiElement (XmlElement gapi_elem)
		{
			HandleAttributes (gapi_elem);
			AddChildren (gapi_elem);
		}

		void HandleAttributes (XmlElement gapi_elem)
		{
			bool needs_array = true;
			foreach (XmlAttribute attr in elem.Attributes) {
				switch (attr.Name) {
				case "c:type":
					gapi_elem.SetAttribute ("type", attr.Value);
					break;
				case "fixed-size":
					gapi_elem.SetAttribute ("array_len", attr.Value);
					break;
				case "length":
					gapi_elem.SetAttribute ("length_param", attr.Value);
					break;
				case "name":
					if (!elem.HasAttribute ("c:type"))
						gapi_elem.SetAttribute ("type", SymbolTable.Lookup (attr.Value));
					needs_array = false;
					break;
				default:
					Console.WriteLine ("Unexpected attribute on array element: " + attr.Name);
					break;
				}
			}
			if (needs_array)
				gapi_elem.SetAttribute ("array", "1");
		}

		void AddChildren (XmlElement gapi_elem)
		{
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "type":
					new Type (child).UpdateGapiElement (gapi_elem, true);
					break;
				default:
					Console.WriteLine ("Unexpected child on property element: " + node.Name);
					break;
				}
			}
		}
	}
}
