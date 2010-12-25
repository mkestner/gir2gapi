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

	public class Interface {

		XmlElement elem;
		ClassStruct class_struct;

		public Interface (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc, List<ClassStruct> cstructs)
		{
			XmlElement gapi_elem = doc.CreateElement ("interface");
			HandleAttributes (gapi_elem);
			string name = elem.GetAttribute ("name");
			foreach (ClassStruct cs in cstructs) {
				if (cs.Parent == name) {
					class_struct = cs;
					break;
				}
			}
			cstructs.Remove (class_struct);
			if (class_struct != null)
				gapi_elem.AppendChild (class_struct.CreateGapiElement (gapi_elem.OwnerDocument));
			AddChildren (gapi_elem);
			return gapi_elem;
		}

		void HandleAttributes (XmlElement gapi_elem)
		{
			foreach (XmlAttribute attr in elem.Attributes) {
				switch (attr.Name) {
				case "c:type":
					gapi_elem.SetAttribute ("cname", attr.Value);
					break;
				case "name":
					gapi_elem.SetAttribute ("name", attr.Value);
					break;
				case "glib:get-type":
					gapi_elem.SetAttribute ("gtype", attr.Value);
					break;
				case "c:symbol-prefix":
				case "doc":
				case "glib:type-name":
				case "glib:type-struct":
				case "version":
					// Ignore
					break;
				default:
					Console.WriteLine ("Unexpected attribute on interface element: " + attr.Name);
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
				case "doc":
					// Ignore
					break;
				case "glib:signal":
					gapi_child.AppendChild (new Signal (child).CreateGapiElement (gapi_child.OwnerDocument));
					break;
				case "method":
					gapi_child.AppendChild (new Method (child).CreateGapiElement (gapi_child.OwnerDocument));
					break;
				case "prerequisite":
					gapi_child.SetAttribute ("prerequisite", child.GetAttribute ("name"));
					break;
				case "property":
					gapi_child.AppendChild (new Property (child).CreateGapiElement (gapi_child.OwnerDocument));
					break;
				case "virtual-method":
					gapi_child.AppendChild (new VirtualMethod (child).CreateGapiElement (gapi_child.OwnerDocument));
					break;
				default:
					Console.WriteLine ("Unexpected child on interface element: " + node.Name);
					break;
				}
			}
		}
	}
}

