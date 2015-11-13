using System;

namespace ModalAnalysis
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			CommonLib.Util.WriteLine ("Modal Analysis");
			CommonLib.Util.WriteLine ("");

			ParseArgs (args);
		}

		public static void ParseArgs (string[] args)
		{
			for (int i = 0; i < args.Length; i = i + 2) {
				switch (args [i]) {
				default:
					CommonLib.Util.WriteLine ("Invalid argument: " + args [i], ConsoleColor.White, ConsoleColor.Red);
					break;
				}
			}
		}
	}
}
