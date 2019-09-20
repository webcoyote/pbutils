using System;
using System.Text;
using System.Windows.Forms;

namespace PbCopy
{
    class Program {
        [STAThread]
        static void Main(string[] args) {
            string s;
            StringBuilder output = new StringBuilder( string.Join(" ", args) );
            while ((s = Console.ReadLine()) != null)
                output.AppendLine(s);

            Clipboard.SetText(output.ToString());
        }
    }
}

