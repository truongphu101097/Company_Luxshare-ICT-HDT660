using System;
using System.Collections.Generic;

namespace MerryDll.Debug {
    class Program {
		static void Main(string[] args) {
			var merryDll = new MerryDllFramework.MerryDll();
			while (merryDll.Start(new List<string> { "4058990007F3", null, "4058990007F3" }, IntPtr.Zero)) {
				var cmd = Console.ReadLine();
				Console.WriteLine(merryDll.Run(cmd));
			}
		}
	}
}
