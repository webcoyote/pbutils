using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices; 

namespace BringToFront {
    class Program {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        const int SW_RESTORE = 9;

        public static void BringToFront(string windowClass) {
            IntPtr handle = FindWindow(windowClass, null);
            if (handle == IntPtr.Zero)
                return;

            if (IsIconic(handle))
                ShowWindow(handle, SW_RESTORE);
            SetForegroundWindow(handle);
        }

        static void Usage () {
            Console.WriteLine(
                "USAGE: BringToFront WINDOWCLASS"
            );
        }

        static void Main(string[] args) {
            if (args.Length != 1) {
                Usage();
                return;
            }

            BringToFront(args[0]);
        }
    }
}