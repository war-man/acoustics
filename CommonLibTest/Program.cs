using System;

namespace CommonLibTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.Title = "CommonLibTest";

			CommonLib.Util.WriteLine ("CommonLib.Util.WriteLine test", ConsoleColor.Blue, ConsoleColor.Gray);

			Console.In.ReadLine(); // Stops console from closing on Windows
		}
	}
}
