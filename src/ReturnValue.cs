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

	public class ReturnValue {

		XmlElement elem;

		public ReturnValue (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = doc.CreateElement ("return-type");
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
				case "doc":
					// Ignore
					break;
				default:
					Console.WriteLine ("Unexpected attribute on return-value element: " + attr.Name);
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
					gapi_child.SetAttribute ("type", child.GetAttribute ("name"));
					gapi_child.SetAttribute ("array", "true");
					break;
				case "type":
					AddTypeElementInfo (child, gapi_child);
					break;
				default:
					Console.WriteLine ("Unexpected child on return-value element: " + node.Name);
					break;
				}
			}
		}

		void AddTypeElementInfo (XmlElement type, XmlElement gapi_child)
		{
			foreach (XmlAttribute attr in type.Attributes) {
				switch (attr.Name) {
				case "name":
					gapi_child.SetAttribute ("type", attr.Value);
					break;
				case "c:type":
					// Ignore
					break;
				default:
					Console.WriteLine ("Unexpected attribute on return-value/type element: " + attr.Name);
					break;
				}
			}

			foreach (XmlNode node in type.ChildNodes) {
				XmlElement type_child = node as XmlElement;
				if (type_child == null)
					continue;
				switch (node.Name) {
				case "type":
					gapi_child.SetAttribute ("element_type", type_child.GetAttribute ("name"));
					break;
				default:
					Console.WriteLine ("Unexpected child on return-value/type element: " + node.Name);
					break;
				}
			}
		}
	}
}

