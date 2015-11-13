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
			CommonLib.Util.Write ("--------------------------------------------------\n\n");

			Calculate ();
		}

		static void ParseArgs (string[] args)
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

		/// <summary>
		/// Performs modal analysis of a given room.
		/// 1. Calculate critical frequency
		/// 2. Calculate fundamental axial modes
		/// 3. Calculate modal harmonics up to critical frequency
		/// 4. Sort harmonics of all axes
		/// 5. Find coincidences
		/// 6. Find spacings
		/// </summary>
		static void Calculate ()
		{
			//Critical frequency
			double rCF = CriticalFrequency ();
			CommonLib.Util.WriteLine ("Critical Frequency: ");
			CommonLib.Util.Write ("\t\t ");
			CommonLib.Util.Write (rCF.ToString() + "Hz\n", ConsoleColor.White, ConsoleColor.DarkGreen);

			//Fundamental axial modes
			double[] rFAM = CalculateAxialModes ();
			CommonLib.Util.WriteLine ("Fundamental Axial Modes:");
			CommonLib.Util.Write ("\t\t ");
			CommonLib.Util.Write ("Length: " + rFAM [0].ToString () + "Hz\n", ConsoleColor.White, ConsoleColor.DarkGreen);
			CommonLib.Util.Write ("\t\t ");
			CommonLib.Util.Write ("Width:  " + rFAM [1].ToString () + "Hz\n", ConsoleColor.White, ConsoleColor.DarkGreen);
			CommonLib.Util.Write ("\t\t ");
			CommonLib.Util.Write ("Height: " + rFAM [2].ToString () + "Hz\n", ConsoleColor.White, ConsoleColor.DarkGreen);
		}

		/// <summary>
		/// Calculates the critical frequency of a given room.
		/// 
		/// 1. Calculate the volume of the room
		/// 2. Calculate the surface area of the room (4 walls + roof + floor, flat planes)
		/// 3. Calculate the Mean Free Path (MFP)
		/// 4. Calculate the critical frequency
		/// </summary>
		/// <returns>Critical Frequency (as double, in Hz)</returns>
		static double CriticalFrequency ()
		{
			//Calculate room volume
			double rVol = rL * rW * rH;

			//Calculate room surface area
			double rS = 2 * ((rL * rW) + (rL * rH) + (rW * rH));

			//Calculate MFP
			double rMFP = 4*(rVol/rS);

			//Calculate critical frequency
			double rCF = 1.5 * (v/rMFP);

			return rCF;
		}

		/// <summary>
		/// Calculates the axial modes.
		/// </summary>
		/// <returns>Axial Modes (as double array, in Hz)</returns>
		static double[] CalculateAxialModes ()
		{
			double[] am = new double[3];
			am[0] = v/(2 * rL); //Length fundamental axial mode
			am[1] = v/(2 * rW); //Width fundamental axial mode
			am[2] = v/(2 * rH); //Height fundamental axial mode

			return am;
		}
	}
}
