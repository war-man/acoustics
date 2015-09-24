using System;

namespace CommonLib
{
	public static class Util
	{
		static Util ()
		{
		}

		public static void WriteLine(string message, ConsoleColor textColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
		{
			var time = DateTime.Now.ToString ("h:mm:ss tt");

			Console.ForegroundColor = textColor;
			Console.BackgroundColor = bgColor;

			Console.WriteLine("[" + time + "] " + message);

			Console.ResetColor ();
		}

		public static void Write(string message, ConsoleColor textColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
		{
			Console.ForegroundColor = textColor;
			Console.BackgroundColor = bgColor;

			Console.Write(message);

			Console.ResetColor ();
		}

		public static string libInfo()
		{
			return "CommonLib.Acoustics\nVersion: 0.1";
		}

		public static string classInfo()
		{
			return "CommonLib.Acoustics.Util\nUtility class";
		}
	}
}
