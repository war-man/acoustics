using System;
using System.Linq;
using System.Collections.Generic;

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

			Console.Title = "Modal Analysis - L: " + rL + ", W: " + rW + ", H: " + rH;

			CommonLib.Util.WriteLine ("Room Properties:");
			CommonLib.Util.Write ("\t\t Length: " + rL.ToString() + "m\n");
			CommonLib.Util.Write ("\t\t Width:  " + rW.ToString() + "m\n");
			CommonLib.Util.Write ("\t\t Height: " + rH.ToString() + "m\n");
			CommonLib.Util.WriteLine ("Speed of sound: " + v.ToString() + "m/s");
			CommonLib.Util.Write ("--------------------------------------------------\n\n");

			Calculate ();

			Console.ReadKey ();
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

			//Check all required arguments are present
			if (rL == 0 || rW == 0 || rH == 0) {
				CommonLib.Util.WriteLine ("Insufficient arguments", ConsoleColor.White, ConsoleColor.Red);
				Console.ReadKey ();
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
			CommonLib.Util.WriteLine ("Critical Frequency:");
			CommonLib.Util.Write ("\t\t ");
			CommonLib.Util.Write (rCF.ToString() + " Hz\n", ConsoleColor.White, ConsoleColor.DarkGreen);

			//Fundamental axial modes
			double[] rFAM = CalculateAxialModes ();
			CommonLib.Util.WriteLine ("");
			CommonLib.Util.WriteLine ("Fundamental Axial Modes:");
			CommonLib.Util.Write ("\t\t ");
			CommonLib.Util.Write ("Length: " + rFAM [0].ToString () + " Hz\n", ConsoleColor.White, ConsoleColor.DarkGreen);
			CommonLib.Util.Write ("\t\t ");
			CommonLib.Util.Write ("Width:  " + rFAM [1].ToString () + " Hz\n", ConsoleColor.White, ConsoleColor.DarkGreen);
			CommonLib.Util.Write ("\t\t ");
			CommonLib.Util.Write ("Height: " + rFAM [2].ToString () + " Hz\n", ConsoleColor.White, ConsoleColor.DarkGreen);

			//Axial harmonics
			List<KeyValuePair<string, double>> rLamH = CalculateAxialHarmonics (rFAM [0], rCF, "L"); //Calculates axial harmonics for Length
			List<KeyValuePair<string, double>> rWamH = CalculateAxialHarmonics (rFAM [1], rCF, "W"); //Calculates axial harmonics for Width
			List<KeyValuePair<string, double>> rHamH = CalculateAxialHarmonics (rFAM [2], rCF, "H"); //Calculates axial harmonics for Height
			CommonLib.Util.WriteLine ("");
			CommonLib.Util.WriteLine ("Axial Mode Harmonics:");
			foreach (var harm in rLamH){
				CommonLib.Util.Write ("\t\t ");
				CommonLib.Util.Write(harm.Key + ": " + harm.Value.ToString() + " Hz", ConsoleColor.White, ConsoleColor.DarkGreen);
				CommonLib.Util.Write("\n");
			}
			CommonLib.Util.WriteLine ("");
			foreach (var harm in rWamH){
				CommonLib.Util.Write ("\t\t ");
				CommonLib.Util.Write(harm.Key + ": " + harm.Value.ToString() + " Hz", ConsoleColor.White, ConsoleColor.DarkGreen);
				CommonLib.Util.Write("\n");
			}
			CommonLib.Util.WriteLine ("");
			foreach (var harm in rHamH){
				CommonLib.Util.Write ("\t\t ");
				CommonLib.Util.Write(harm.Key + ": " + harm.Value.ToString() + " Hz", ConsoleColor.White, ConsoleColor.DarkGreen);
				CommonLib.Util.Write("\n");
			}

			//Sort harmonics
			List<KeyValuePair<string, double>> ramH = new List<KeyValuePair<string, double>>(rLamH.Count + rWamH.Count + rHamH.Count + 3); // Plus 3 for fundamentals
			ramH.AddRange (rLamH);
			ramH.AddRange (rWamH);
			ramH.AddRange (rHamH);
			List<KeyValuePair<string, double>> ramHS = SortHarmonics (ramH);
			CommonLib.Util.WriteLine ("");
			CommonLib.Util.WriteLine ("Sorted Axial Mode Harmonics:");
			foreach (var harm in ramHS){
				CommonLib.Util.Write ("\t\t ");
				CommonLib.Util.Write(harm.Key + ": " + harm.Value.ToString() + " Hz", ConsoleColor.White, ConsoleColor.DarkGreen);
				CommonLib.Util.Write("\n");
			}

			//Find coincidences
			CommonLib.Util.WriteLine ("");
			CommonLib.Util.WriteLine ("Coincidences:");
			FindCoincidences(ramHS);

			//Find Spacings
			CommonLib.Util.WriteLine ("");
			CommonLib.Util.WriteLine ("Spacings:");
			FindSpacings(ramHS);
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
			CommonLib.Util.WriteLine("Room volume: " + rVol + " m^3", ConsoleColor.White, ConsoleColor.DarkGreen);

			//Calculate room surface area
			double rS = 2 * ((rL * rW) + (rL * rH) + (rW * rH));
			CommonLib.Util.WriteLine("Room surface area: " + rS + " m^2", ConsoleColor.White, ConsoleColor.DarkGreen);

			//Calculate MFP
			double rMFP = 4*(rVol/rS);
			CommonLib.Util.WriteLine("Mean Free Path (MPF): " + rMFP + " m", ConsoleColor.White, ConsoleColor.DarkGreen);
			CommonLib.Util.WriteLine ("");

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

		/// <summary>
		/// Calculates the harmonics of the fundamental axial modes, up to the critical frequency
		/// </summary>
		/// <returns>Axial Harmonics (as string/double KeyValuePair, in Hz)</returns>
		static List<KeyValuePair<string, double>> CalculateAxialHarmonics (double rFAM, double rCF, string axis)
		{
			List<KeyValuePair<string, double>> amH = new List<KeyValuePair<string, double>>(); //List instead of array because the number of harmonics is unknown

			amH.Add (new KeyValuePair<string, double> (axis + "1", rFAM));

			int i = 2;
			double lastHarmonic = 0;
			while (lastHarmonic <= rCF)
			{
				double currentHarmonic = i * rFAM;

				amH.Add(new KeyValuePair<string, double>(axis + i.ToString(), currentHarmonic));

				lastHarmonic = currentHarmonic;
				i++;
			}

			amH.RemoveAt (i - 2); //Removes the final list item, as it will always be higher than the critical frequency

			return amH;
		}

		/// <summary>
		/// Sorts the harmonics.
		/// </summary>
		/// <returns>Sorted harmonics (as string/double KeyValuePair, in Hz)</returns>
		static List<KeyValuePair<string, double>> SortHarmonics(List<KeyValuePair<string, double>> ramH)
		{
			return ramH.OrderBy(a=>a.Value).ToList();
		}

		/// <summary>
		/// Finds the coincidences.
		/// </summary>
		/// <returns>Coincidences (as string/double KeyValuePair, in Hz)</returns>
		static void FindCoincidences (List<KeyValuePair<string, double>> ramHS)
		{
			for (int i = 0; i < ramHS.Count - 1; i++)
			{
				KeyValuePair<string, double> freqA = ramHS [i];
				KeyValuePair<string, double> freqB = ramHS [i + 1];
				double diff = Math.Round(freqB.Value) - Math.Round(freqA.Value);

				if (((diff/freqB.Value) * 100) <= 0.05) {
					CommonLib.Util.Write ("\t\t ");
					CommonLib.Util.Write (freqA.Key + ", " + freqB.Key + ": " + diff + " Hz (" + Math.Round((diff/freqB.Value) * 100).ToString() + "%) @ " +
						"" + freqA.Value + " Hz", ConsoleColor.White, ConsoleColor.DarkGreen);
					CommonLib.Util.Write ("\n");
				}
			}
		}

		static void FindSpacings (List<KeyValuePair<string, double>> ramHS)
		{
			int spacingCount = 0;
			
			for (int i = 0; i < ramHS.Count - 1; i++)
			{
				KeyValuePair<string, double> freqA = ramHS [i];
				KeyValuePair<string, double> freqB = ramHS [i + 1];
				double diff = Math.Round(freqB.Value) - Math.Round(freqA.Value);

				if (diff >= 25) {
					CommonLib.Util.Write ("\t\t ");
					CommonLib.Util.Write (freqA.Key + " <--> " + freqB.Key, ConsoleColor.White, ConsoleColor.DarkGreen);
					CommonLib.Util.Write ("\n");
					spacingCount++;
				}
			}

			if (spacingCount == 0) {
				CommonLib.Util.Write ("\t\t ");
				CommonLib.Util.Write ("No spacings found!", ConsoleColor.Black, ConsoleColor.White);
				CommonLib.Util.Write ("\n");
			}
		}
	}
}
