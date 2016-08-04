using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using TeraLibrary;

namespace MoveCk
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("start !!");

			var wk = new Work_Data(
				@"C:\Users\x45991\Documents\Projects",
				@"C:\Users\x45991\Documents\Program Files");

			wk.Copy();

			Console.WriteLine("end !!");

			foreach (var ab in wk.dic_AB)
			{
				var key = ab.Key;
				var value = ab.Value;

				Console.WriteLine(value.a_FullName + " -> " + value.b_FullName);
			}

			Console.ReadKey();
		}
	}
}
