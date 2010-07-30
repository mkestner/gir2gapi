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

	public class Field {

		XmlElement elem;

		public Field (XmlElement elem)
		{
			this.elem = elem;
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = null;
			if (elem ["type"] != null) {
				gapi_elem = doc.CreateElement ("field");
				gapi_elem.SetAttribute ("name", Mangler.StudlyCase (elem.GetAttribute ("name")));
				gapi_elem.SetAttribute ("cname", elem.GetAttribute ("name"));
				gapi_elem.SetAttribute ("type", elem ["type"].GetAttribute ("c:type"));
			} else if (elem ["callback"] != null) {
				gapi_elem = doc.CreateElement ("method");
				gapi_elem.SetAttribute ("vm", elem ["callback"].GetAttribute ("c:type"));
			} else {
				Console.WriteLine ("Unexpected field element: " + elem.OuterXml);
			}
			return gapi_elem;
		}
	}
}

