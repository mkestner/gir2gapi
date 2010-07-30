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

	public class Parameters {

		XmlElement elem;
		bool throws;

		public Parameters (XmlElement elem, bool throws)
		{
			this.elem = elem;
			this.throws = throws;
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = doc.CreateElement ("parameters");
			if (elem.Attributes.Count > 0) 
				Console.WriteLine ("Unexpected attributes on parameters element");
			AddChildren (gapi_elem);
			if (throws)
				AddGErrorParameter (gapi_elem);
			return gapi_elem;
		}

		void AddChildren (XmlElement gapi_child)
		{
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "parameter":
					Parameter p = new Parameter (child);
					gapi_child.AppendChild (p.CreateGapiElement (gapi_child.OwnerDocument));
					break;
				default:
					Console.WriteLine ("Unexpected child on parameters element: " + node.Name);
					break;
				}
			}
		}

		void AddGErrorParameter (XmlElement gapi_child)
		{
			XmlElement gerr = gapi_child.OwnerDocument.CreateElement ("parameter");
			gerr.SetAttribute ("type", "GError**");
			gerr.SetAttribute ("name", "error");
			gapi_child.AppendChild (gerr);
		}
	}
}

