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
       private IDatabase database;
       public Form1 Form { get; set; }

       public IDatabase Database
       {
           get
           {
               return this.database;
           }
           set
           {
               if (value == null)
               {
                   throw new ArgumentNullException("Database cannot be null");
               }
               this.database = value;
           }
       }

        

        public Engine(IDatabase database)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.Database = database;
            Form = new Form1(this.Database);
        }

        public void Run()
        {
            Application.Run(Form);
        }

    }
}
