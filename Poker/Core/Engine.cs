namespace Poker.Core
{
    using System;
    using System.Windows.Forms;
    using Interfaces;

    /// <summary>
    /// Main engine of the program
    /// </summary>
    public class Engine : IEngine
    {
        /// <summary>
        /// instance of type Idatabase (holds entities used by program)
        /// </summary>
        private IDatabase database;

        /// <summary>
        /// creates an instance of Engine
        /// </summary>
        /// <param name="database">Database to hold entities</param>
        public Engine(IDatabase database)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.Database = database;
            Form = new Form1(this.Database);
        }

        /// <summary>
        /// gets or sets the form used to display things
        /// </summary>
        public Form1 Form
        {
            get;
            set;
        }

        /// <summary>
        /// gets or sets the database
        /// </summary>
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

        /// <summary>
        /// initializes the main loop of the engine
        /// </summary>
        public void Run()
        {
            Application.Run(Form);
        }
    }
}
