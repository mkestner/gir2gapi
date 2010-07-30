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

	public class Class {

		XmlElement elem;
		ClassStruct class_struct;

		public Class (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc, List<ClassStruct> cstructs, List<Function> funcs)
		{
			XmlElement gapi_elem = doc.CreateElement ("object");
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
				case "abstract":
					gapi_elem.SetAttribute ("abstract", attr.Value);
					break;
				case "name":
					gapi_elem.SetAttribute ("name", attr.Value);
					break;
				case "c:type":
					gapi_elem.SetAttribute ("cname", attr.Value);
					break;
				case "parent":
					gapi_elem.SetAttribute ("parent", GetParentType (attr.Value));
					break;
				case "glib:get-type":
					gapi_elem.SetAttribute ("gtype", attr.Value);
					break;
				case "glib:type-struct":
				case "doc":
				case "glib:type-name":
				case "version":
					// Ignore
					break;
				default:
					Console.WriteLine ("Unexpected attribute on class element: " + attr.Name);
					break;
				}
			}
		}

		string GetParentType (string name)
		{
			if (name.StartsWith ("GObject."))
				return "G" + name.Substring (8);

			if (name.IndexOf ('.') < 0) {
				foreach (XmlNode node in elem.ParentNode.ChildNodes) {
					XmlElement sibling = node as XmlElement;
					if (sibling != null && sibling.Name == "class" && sibling.GetAttribute ("name") == name)
						return sibling.GetAttribute ("c:type");
				}
				Console.WriteLine ("Unable to find parent type:" + name);
				return String.Empty;
			} else {
				Console.WriteLine ("Unsupported parent type from external namespace: " + name);
				return String.Empty;
			}
		}

		void AddChildren (XmlElement gapi_child)
		{
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "field":
					gapi_child.AppendChild (new Field (child).CreateGapiElement (gapi_child.OwnerDocument));
					break;
				case "virtual-method":
					gapi_child.AppendChild (new VirtualMethod (child).CreateGapiElement (gapi_child.OwnerDocument));
					break;
				case "constructor":
				case "function":
				case "glib:signal":
				case "implements":
				case "method":
				case "property":
					//FIXME: handle these
					break;
				default:
					Console.WriteLine ("Unexpected child on class element: " + node.Name);
					break;
				}
			}
		}
	}
}

