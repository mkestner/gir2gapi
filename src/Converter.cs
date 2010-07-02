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

namespace Gir2Gapi {

	public class Converter {

		public static int Main (string[] args)
		{
			try {
				new Converter (args);
				Console.WriteLine ("Doesn't do anything yet!");
				return 0;
			} catch (Exception) {
				return 1;
			}
		}

		string gir_path;
		string output_path;

		public Converter (string[] args)
		{
			foreach (string arg in args) {
				if (arg.StartsWith ("--gir="))
					gir_path = arg.Substring (6);
				else if (arg.StartsWith ("--out="))
					output_path = arg.Substring (6);
				else {
					Usage ();
					throw new ArgumentException ("args");
				}
			}

			if (String.IsNullOrEmpty (gir_path) || String.IsNullOrEmpty (output_path)) {
				Usage ();
				throw new ArgumentException ("args");
			}
		}

		void Usage ()
		{
			Console.Error.WriteLine ("Usage: gir2gapi <args>");
			Console.Error.WriteLine ("   --gir=<file> : GIR repository file to convert : required");
			Console.Error.WriteLine ("   --out=<file> : path to place converted document : required");
		}
	}
}

