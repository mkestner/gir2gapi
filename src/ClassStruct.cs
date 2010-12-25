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

	public class ClassStruct {

		XmlElement elem;

		public ClassStruct (XmlElement elem)
		{
			this.elem = elem;
		}

		public string Name {
			get { return elem.GetAttribute ("name"); }
		}

		public string Parent {
			get { return elem.GetAttribute ("glib:is-gtype-struct-for"); }
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = doc.CreateElement ("class_struct");
			SetAttributes (gapi_elem);
			AddChildren (gapi_elem);
			return gapi_elem;
		}

		void SetAttributes (XmlElement gapi_elem)
		{
			foreach (XmlAttribute attr in elem.Attributes) {
				switch (attr.Name) {
				case "c:type":
					gapi_elem.SetAttribute ("cname", attr.Value);
					break;
				case "disguised":
				case "doc":
				case "glib:is-gtype-struct-for":
				case "name":
				case "version":
					// Ignore
					break;
				default:
					Console.WriteLine ("Unexpected attribute on class-struct record element: " + attr.Name);
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
				case "field":
					gapi_child.AppendChild (new Field (child).CreateGapiElement (gapi_child.OwnerDocument));
					break;
				default:
					Console.WriteLine ("Unexpected child on class-struct record element: " + node.Name);
					break;
				}
			}
		}
	}
}

