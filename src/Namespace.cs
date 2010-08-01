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

	public class Namespace {

		XmlElement elem;
		List<Callback> callbacks = new List<Callback> ();
		List<Class> classes = new List<Class> ();
		List<ClassStruct> cstructs = new List<ClassStruct> ();
		List<Constant> constants = new List<Constant> ();
		List<Enumeration> enums = new List<Enumeration> ();
		List<Method> functions = new List<Method> ();
		List<Interface> ifaces = new List<Interface> ();
		List<Record> records = new List<Record> ();

		public Namespace (XmlElement elem)
		{
			this.elem = elem;
			AddChildren ();
		}

		void AddChildren ()
		{
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement child = node as XmlElement;
				if (child == null)
					continue;
				switch (node.Name) {
				case "bitfield":
				case "enumeration":
					enums.Add (new Enumeration (child));
					break;
				case "callback":
					callbacks.Add (new Callback (child));
					break;
				case "class":
					classes.Add (new Class (child));
					break;
				case "constant":
					constants.Add (new Constant (child));
					break;
				case "function":
					functions.Add (new Method (child));
					break;
				case "interface":
					ifaces.Add (new Interface (child));
					break;
				case "record":
					if (child.HasAttribute ("glib:is-gtype-struct-for"))
						cstructs.Add (new ClassStruct (child));
					else
						records.Add (new Record (child));
					break;
				default:
					Console.WriteLine ("Unexpected namespace child: " + node.Name);
					break;
				}
			}
		}

		public XmlElement CreateGapiElement (XmlDocument doc)
		{
			XmlElement gapi_elem = doc.CreateElement ("namespace");
			SetAttributes (gapi_elem);

			CreateEnumElements (gapi_elem);
			CreateCallbackElements (gapi_elem);
			CreateConstantsElement (gapi_elem);
			CreateObjectElements (gapi_elem);
			CreateInterfaceElements (gapi_elem);
			CreateStructElements (gapi_elem);
			CreateGlobalClassElement (gapi_elem);

			if (cstructs.Count > 0) {
				Console.Write ("ClassStructs leftover after namespace creation:\n   ");
				foreach (ClassStruct cs in cstructs)
					Console.Write (cs.Name + " ");
				Console.WriteLine ();
			}

			return gapi_elem;
		}

		void CreateCallbackElements (XmlElement gapi_elem)
		{
			foreach (Callback cb in callbacks)
				gapi_elem.AppendChild (cb.CreateGapiElement (gapi_elem.OwnerDocument));
		}

		void CreateConstantsElement (XmlElement gapi_elem)
		{
			if (constants.Count == 0)
				return;

			XmlElement constants_elem = gapi_elem.OwnerDocument.CreateElement ("constants");
			foreach (Constant constant in constants)
				constants_elem.AppendChild (constant.CreateGapiElement (gapi_elem.OwnerDocument));
			gapi_elem.AppendChild (constants_elem);
		}

		void CreateEnumElements (XmlElement gapi_elem)
		{
			foreach (Enumeration e in enums)
				gapi_elem.AppendChild (e.CreateGapiElement (gapi_elem.OwnerDocument));
		}

		void CreateGlobalClassElement (XmlElement gapi_elem)
		{
			if (functions.Count == 0)
				return;
			XmlElement elem = gapi_elem.OwnerDocument.CreateElement ("class");
			elem.SetAttribute ("name", "Global");
			foreach (Method m in functions)
				elem.AppendChild (m.CreateGapiElement (gapi_elem.OwnerDocument));
			gapi_elem.AppendChild (elem);
		}

		void CreateInterfaceElements (XmlElement gapi_elem)
		{
			foreach (Interface iface in ifaces)
				gapi_elem.AppendChild (iface.CreateGapiElement (gapi_elem.OwnerDocument, cstructs));
		}

		void CreateObjectElements (XmlElement gapi_elem)
		{
			foreach (Class cls in classes)
				gapi_elem.AppendChild (cls.CreateGapiElement (gapi_elem.OwnerDocument, cstructs));
		}

		void CreateStructElements (XmlElement gapi_elem)
		{
			foreach (Record record in records)
				gapi_elem.AppendChild (record.CreateGapiElement (gapi_elem.OwnerDocument));
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

