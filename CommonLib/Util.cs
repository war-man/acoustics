using System;

namespace CommonLib
{
	public static class Util
	{
		static Util ()
		{
		}

		public static void WriteLine(string message)
		{
			var time = DateTime.Now.ToString ("h:mm:ss tt");
			Console.WriteLine("[" + time + "] " + message);
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
