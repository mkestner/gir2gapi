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

	public class Namespace {

		XmlElement elem;

		public Namespace (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = doc.CreateElement ("namespace");
			SetAttributes (gapi_elem);
			AddChildren (gapi_elem);
			return gapi_elem;
		}

		void AddChildren (XmlElement gapi_elem)
		{
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "bitfield":
				case "enumeration":
					Enumeration enum_bf = new Enumeration (child);
					gapi_elem.AppendChild (enum_bf.CreateGapiElement (gapi_elem.OwnerDocument));
					break;
				case "callback":
				case "class":
				case "constant":
				case "function":
				case "interface":
				case "record":
					break;
				default:
					Console.WriteLine ("Unexpected namespace child: " + node.Name);
					break;
				}
			}
		}

		void SetAttributes (XmlElement gapi_elem)
		{
			foreach (XmlAttribute attr in elem.Attributes) {
				switch (attr.Name) {
				case "name":
					gapi_elem.SetAttribute ("name", attr.Value);
					break;
				case "shared-library":
					gapi_elem.SetAttribute ("library", attr.Value);
					break;
				case "c:prefix":
				case "version":
					break;
				default:
					Console.WriteLine ("Unexpected namespace attribute: " + attr.Name);
					break;
				}
			}
		}
	}
}

