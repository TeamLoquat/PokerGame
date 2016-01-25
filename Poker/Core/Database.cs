namespace Poker.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using Poker.Interfaces;


    public class Database : IDatabase
    {
        private static readonly List<string> ButtonNames = new List<string>()
                                                       {
                                                           "Fold",
                                                           "Check",
                                                           "Call",
                                                           "Raise",
                                                           "AddChips",
                                                           "Options",
                                                           "BB",
                                                           "SB"
                                                       };
        private IList<IPlayer> players;
        private IDictionary<string, Button> buttons;
        private IBotFactory botFactory;
        private IHumanFactory humanFactory;

        public Database(IBotFactory botFactory, IHumanFactory humanFactory )
        {
            this.BotFactory = botFactory;
            this.HumanFactory = humanFactory;
        }


        public IList<IPlayer> Players
        {
            get { return this.players; }
            set { this.players = value; }
        }

        public IDictionary<string, Button> Buttons
        {
            get { return this.buttons; }
            set { this.buttons = value; }
        }

        private IBotFactory BotFactory
        {
            get
            {
                return this.botFactory;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Bot Factory cannot be null");
                }
                this.botFactory = value;
            }
        }

        private IHumanFactory HumanFactory
        {
            get
            {
                return this.humanFactory;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Human Factory cannot be null");
                }
                this.humanFactory = value;
            }
        }

        public void Initialize()
        {
            this.InitializePlayers();
            this.InitializeButtons();
        }

        private void InitializePlayers()
        {
            this.Players = new List<IPlayer>();
            this.players.Add(this.HumanFactory.Create());
            for (int j = 0; j < 5; j++)
            {
                this.players.Add(this.BotFactory.Create(j + 1));
            }

            var locationPointsForPlayers = new Dictionary<int, List<Point>>()
                                               {
                                                   {
                                                       0, new List<Point>()
                                                              {
                                                                  new Point(755, 563),
                                                                  new Point(755, 589)
                                                              }
                                                   },
                                                   {
                                                       1, new List<Point>()
                                                              {
                                                                  new Point(181, 563),
                                                                  new Point(181, 589)
                                                              }
                                                   },
                                                   {
                                                       2, new List<Point>()
                                                              {
                                                                  new Point(276, 81),
                                                                  new Point(276, 107)
                                                              }
                                                   },
                                                   {
                                                       3, new List<Point>()
                                                              {
                                                                  new Point(755, 81),
                                                                  new Point(755, 107)
                                                              }
                                                   },
                                                   {
                                                       4, new List<Point>()
                                                              {
                                                                  new Point(950, 81),
                                                                  new Point(950, 107)
                                                              }
                                                   },
                                                   {
                                                       5, new List<Point>()
                                                              {
                                                                  new Point(1012, 563),
                                                                  new Point(1012, 589)
                                                              }
                                                   },
                                               };

            // vars needeed for automatic IPlayer initialization, they are used by all players
            var fontForEachPlayer = new System.Drawing.Font(
                "Microsoft Sans Serif",
                10F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(204)));
            var chipsTextBoxNameTemplate = "textBox{0}Chips";
            var statusLabelNameTemplate = "{0}Status";
            var chipsTextBoxTextTemplate = "Chips : 0";
            var sizeTemplate = new Size(163, 23);
            for (int j = 0; j < this.Players.Count; j++)
            {
                this.Players[j].ChipsTextBox.Font = fontForEachPlayer;
                this.Players[j].ChipsTextBox.Name = string.Format(
                    chipsTextBoxNameTemplate,
                    this.Players[j].Name);
                this.Players[j].StatusLabel.Name = string.Format(
                    statusLabelNameTemplate,
                    this.Players[j].Name);
                this.Players[j].ChipsTextBox.Text = chipsTextBoxTextTemplate;
                this.Players[j].ChipsTextBox.Size = sizeTemplate;
                this.Players[j].StatusLabel.Size = sizeTemplate;
                this.Players[j].ChipsTextBox.TabIndex = j;
                this.Players[j].StatusLabel.TabIndex = j + 1;
                this.Players[j].ChipsTextBox.Location = locationPointsForPlayers[j][0];
                this.Players[j].StatusLabel.Location = locationPointsForPlayers[j][1];
            }
        }

        private void InitializeButtons()
        {
            this.Buttons = new Dictionary<string, Button>();
            ButtonNames.ForEach(b => this.Buttons[b] = new Button());

            //BUTTONS INITIALIZATION----------------
            var locationPointsForButtons = new Dictionary<string, Point>()
                                     {
                                        {"Fold",new Point(335, 670)},
                                        {"Check",new Point(494, 670)},
                                        {"Call",new Point(667, 671)},
                                        {"Raise",new Point(835, 671)},
                                        {"AddChips",new Point(12, 707)},
                                        {"Options",new Point(12, 12)},
                                        {"BB",new Point(12, 254)},
                                        {"SB",new Point(12, 199)},
                                     };
            //vars needeed for automatic Buttons initialization, they are used by all Buttons
            var fontTemplate = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            var backColor = true;
            var tabIndex = 0;
            var sizeTemplate = new Size(130, 25);

            foreach (var keyValuePair in this.Buttons)
            {
                keyValuePair.Value.Font = fontTemplate;
                keyValuePair.Value.UseVisualStyleBackColor = backColor;
                keyValuePair.Value.Name = keyValuePair.Key.ToString();
                keyValuePair.Value.Text = keyValuePair.Key.ToString();
                keyValuePair.Value.TabIndex = tabIndex++;
                keyValuePair.Value.Size = sizeTemplate;
                keyValuePair.Value.Location = locationPointsForButtons[keyValuePair.Key.ToString()];
            }
        }

        public void DisplayPlayerChips()
        {
            foreach (var player in this.players)
            {
                player.ChipsTextBox.Enabled = false;
                player.ChipsTextBox.Text = $"Chips : {player.Chips}";
            }
        }


    }
}