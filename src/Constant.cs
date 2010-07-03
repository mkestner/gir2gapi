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

	public class Constant {

		XmlElement elem;

		public Constant (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = doc.CreateElement ("constant");
			SetAttributeInfo (gapi_elem);
			AddChildInfo (gapi_elem);
			return gapi_elem;
		}

		void SetAttributeInfo (XmlElement gapi_elem)
		{
			foreach (XmlAttribute attr in elem.Attributes) {
				switch (attr.Name) {
				case "name":
					gapi_elem.SetAttribute ("name", attr.Value);
					break;
				case "value":
					gapi_elem.SetAttribute ("value", attr.Value);
					break;
				default:
					Console.WriteLine ("Unexpected attribute on constant element: " + attr.Name);
					break;
				}
			}
		}

		void AddChildInfo (XmlElement gapi_child)
		{
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "type":
					AddTypeElementInfo (child, gapi_child);
					break;
				default:
					Console.WriteLine ("Unexpected child on constant element: " + node.Name);
					break;
				}
			}
		}

		void AddTypeElementInfo (XmlElement child, XmlElement gapi_child)
		{
			foreach (XmlAttribute attr in child.Attributes) {
				switch (attr.Name) {
				case "c:type":
					gapi_child.SetAttribute ("type", attr.Value);
					break;
				case "name":
					if (child.HasAttribute ("c:type"))
						continue;
					switch (attr.Value) {
					case "utf8":
						gapi_child.SetAttribute ("type", "gchar*");
						break;
					default:
						Console.WriteLine ("Unexpected name in c:type-less constant/type element: " + attr.Value);
						break;
					}
					break;
				default:
					Console.WriteLine ("Unexpected attribute on constant/type element: " + attr.Name);
					break;
				}
			}
		}
	}
}

