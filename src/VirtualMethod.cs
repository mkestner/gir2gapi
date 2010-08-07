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

	public class VirtualMethod {

		XmlElement elem;
		bool throws;

		public VirtualMethod (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = doc.CreateElement ("virtual_method");
			SetAttributes (gapi_elem);
			AddChildren (gapi_elem);
			return gapi_elem;
		}

		void SetAttributes (XmlElement gapi_elem)
		{
			foreach (XmlAttribute attr in elem.Attributes) {
				switch (attr.Name) {
				case "name":
					gapi_elem.SetAttribute ("name", Mangler.StudlyCase (attr.Value));
					gapi_elem.SetAttribute ("cname", attr.Value);
					break;
				case "throws":
					throws = attr.Value == "1";
					break;
				case "doc":
				case "introspectable":
				case "invoker":
				case "version":
					// Ignore
					break;
				default:
					Console.WriteLine ("Unexpected attribute on virtual-method element: " + attr.Name);
					break;
				}
			}
		}

		void AddChildren (XmlElement gapi_child)
		{
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "return-value":
					ReturnValue retval = new ReturnValue (child);
					gapi_child.AppendChild (retval.CreateGapiElement (gapi_child.OwnerDocument));
					break;
				case "parameters":
					Parameters parms = new Parameters (child, throws);
					gapi_child.AppendChild (parms.CreateGapiElement (gapi_child.OwnerDocument));
					break;
				default:
					Console.WriteLine ("Unexpected child on callback element: " + node.Name);
					break;
				}
			}
		}
	}
}

