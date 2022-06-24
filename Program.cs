using System;
using System.IO;
using System.Windows.Forms;

namespace JSONReader
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string file = "";

            foreach(string i in args)
            {
                if(File.Exists(i))
                {
                    file = i;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(file));
        }
    }
}
