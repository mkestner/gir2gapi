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

	public class Enumeration {

		XmlElement elem;

		public Enumeration (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement result = doc.CreateElement ("enum");
			result.SetAttribute ("name", elem.GetAttribute ("name"));
			result.SetAttribute ("cname", elem.GetAttribute ("c:type"));
			if (elem.HasAttribute ("glib:get-type"))
				result.SetAttribute ("gtype", elem.GetAttribute ("glib:get-type"));
			result.SetAttribute ("type", elem.Name == "enumeration" ? "enum" : "flags");
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "member":
					XmlElement gapi_child = doc.CreateElement ("member");
					gapi_child.SetAttribute ("name", Mangler.StudlyCase (child.GetAttribute ("name")));
					gapi_child.SetAttribute ("cname", child.GetAttribute ("c:identifier"));
					gapi_child.SetAttribute ("value", child.GetAttribute ("value"));
					result.AppendChild (gapi_child);
					break;
				default:
					Console.WriteLine ("Unexpected enumeration/bitfield child: " + node.Name);
					break;
				}
			}
					
			return result;
		}
	}
}

