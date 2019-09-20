using System;
using System.Text;
using System.Windows.Forms;

namespace PbPaste {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            Console.Write(Clipboard.GetText());
        }
    }
}
