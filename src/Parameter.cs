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

	public class Parameter {

		XmlElement elem;

		public Parameter (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = doc.CreateElement ("parameter");
			SetAttributeInfo (gapi_elem);
			AddChildInfo (gapi_elem);
			return gapi_elem;
		}

		void SetAttributeInfo (XmlElement gapi_elem)
		{
			foreach (XmlAttribute attr in elem.Attributes) {
				switch (attr.Name) {
				case "allow-none":
					gapi_elem.SetAttribute ("allow-null", "true");
					break;
				case "caller-allocates":
					if (attr.Value == "1")
						gapi_elem.SetAttribute ("pass_as", "ref");
					break;
				case "destroy":
					gapi_elem.SetAttribute ("destroy", attr.Value);
					break;
				case "direction":
					gapi_elem.SetAttribute ("pass_as", attr.Value);
					break;
				case "name":
					gapi_elem.SetAttribute ("name", attr.Value);
					break;
				case "scope":
					gapi_elem.SetAttribute ("scope", attr.Value);
					break;
				case "transfer-ownership":
					switch (attr.Value) {
					case "full":
						gapi_elem.SetAttribute ("owned", "true");
						break;
					case "none":
						break;
					default:
						Console.WriteLine ("Unexpected ownership transfer value: " + attr.Value);
						break;
					}
					break;
				case "closure":
				case "doc":
					// Ignore
					break;
				default:
					Console.WriteLine ("Unexpected attribute on parameter element: " + attr.Name);
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
				case "array":
					new Array (child).UpdateGapiElement (gapi_child);
					break;
				case "doc":
					// Ignore
					break;
				case "type":
					new Type (child).UpdateGapiElement (gapi_child, false);
					break;
				case "varargs":
					gapi_child.SetAttribute ("ellipsis", "true");
					break;
				default:
					Console.WriteLine ("Unexpected child on parameter element: " + node.Name);
					break;
				}
			}
		}
	}
}

