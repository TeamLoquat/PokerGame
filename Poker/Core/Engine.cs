using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Poker.Interfaces;

namespace Poker.Core
{
   public class Engine : IEngine
    {
        public Form1 Form { get; set; }

        public Engine()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form = new Form1();
        }

        public void Run()
        {
            Application.Run(Form);
        }

    }
}
