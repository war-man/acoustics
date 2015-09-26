using System;

namespace CommonLibTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.Title = "CommonLibTest";

			CommonLib.Util.WriteLine ("CommonLib.Util.WriteLine test", ConsoleColor.Blue, ConsoleColor.Gray);

			CommonLib.Util.Write ("Write test", ConsoleColor.Red, ConsoleColor.Yellow);
			CommonLib.Util.Write (" WriteLine\n");

			Console.In.ReadLine(); // Stops console from closing on Windows
		}
	}
}
