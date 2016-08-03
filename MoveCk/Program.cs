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

			Console.WriteLine("end !!");

			Console.ReadKey();
		}
	}
}
