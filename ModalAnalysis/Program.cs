using System;

namespace ModalAnalysis
{
	class MainClass
	{
		public static double rL = 0; //Room length
		public static double rW = 0; //Room width
		public static double rH = 0; //Room height
		public static double v = 344; //Speed of sound (default 344m/s)

		public static void Main (string[] args)
		{
			CommonLib.Util.WriteLine ("Modal Analysis\n");

			ParseArgs (args);

			CommonLib.Util.WriteLine ("Room Properties:");
			CommonLib.Util.Write ("\t\t Length: " + rL.ToString() + "m\n");
			CommonLib.Util.Write ("\t\t Width:  " + rW.ToString() + "m\n");
			CommonLib.Util.Write ("\t\t Height: " + rH.ToString() + "m\n");
			CommonLib.Util.WriteLine ("Speed of sound: " + v.ToString() + "m/s");
		}

		public static void ParseArgs (string[] args)
		{
			for (int i = 0; i < args.Length; i = i + 2) {
				switch (args [i]) {
				case ("-l"):
					rL = double.Parse (args [i + 1]);
					break;
				case ("-w"):
					rW = double.Parse (args [i + 1]);
					break;
				case ("-h"):
					rH = double.Parse (args [i + 1]);
					break;
				case ("-v"):
					v = double.Parse (args [i + 1]);
					break;
				default:
					CommonLib.Util.WriteLine ("Invalid argument: " + args [i], ConsoleColor.White, ConsoleColor.Red);
					break;
				}
			}
		}
	}
}
