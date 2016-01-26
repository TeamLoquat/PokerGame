namespace Poker.Core
{
    using System;
    using System.Windows.Forms;
    using Interfaces;

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
