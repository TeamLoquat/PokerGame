using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Poker.Core;
    using Poker.Interfaces;
    using Poker.Models;

    public partial class Form1 : Form
    {
        

        private const int NumberOfCardsInADeck = 52;

        #region Variables

        private IDatabase database;
        public IDatabase Database
        {
            get
            {
                return this.database;
            }

            set
            {
                this.database = value;
            }
        }

        public int call;
        private int foldedPlayers;
        private int Flop;
        private int Turn;
        private int River;
        private int End;
        private int maxLeft;
        private int raisedTurn;
        private int time;
        private int bigBlind;
        private int smallBlind;
        private int up;
        private int height;
        private int width;
        private int winners;
        private int last;   // original value 123; - never used
        private int i;
        private int turnCount;

        private double Raise;
        private double type;
        private double rounds;

        private IList<bool?> bools;
        private IList<Type> win;
        private IList<string> checkWinners;
        private IList<int> ints;

        private string[] imagesPathsFromDirectory;

        private bool restart;
        private bool raising;
        private bool intsadded;
        private bool changed;

        private int[] reserve;

        private Image[] deckImages;
        private PictureBox[] cardsHolder;

        private Timer timer;
        private Timer updates;

        private Poker.Type sorted;

        public TextBox textBoxPot;



        private ProgressBar progressBarTimer;

        private TextBox textBoxRaise;
        private TextBox textBoxAdd;
        private TextBox textBoxBB;
        private TextBox textBoxSB;


        private Label label1;

        #endregion

        public Form1(IDatabase database)
        {
            this.Database = database;

            this.call = 500;
            this.foldedPlayers = 5;
            this.Flop = 1;
            this.Turn = 2;
            this.River = 3;
            this.End = 4;
            this.maxLeft = 6;
            this.raisedTurn = 1;

            this.bools = new List<bool?>();
            this.win = new List<Type>();
            this.checkWinners = new List<string>();
            this.ints = new List<int>();

            this.imagesPathsFromDirectory = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);

            this.restart = false;
            this.raising = false;

            this.reserve = new int[17];

            this.deckImages = new Image[NumberOfCardsInADeck];
            this.cardsHolder = new PictureBox[52];
            this.timer = new Timer();
            this.updates = new Timer();
            this.time = 60;
            this.bigBlind = 500;
            this.smallBlind = 250;
            this.up = 10000000;

            //bools.Add(PFturn); bools.Add(B1Fturn); bools.Add(B2Fturn); bools.Add(B3Fturn); bools.Add(B4Fturn); bools.Add(B5Fturn);
            //call = bigBlind;
            MaximizeBox = false;
            MinimizeBox = false;

            

            

            updates.Start();

            InitializeComponent();

            this.width = this.Width;
            this.height = this.Height;



            Shuffle(this.Database.Players, this.Database.Buttons);
            this.Database.DisplayPlayerChips();


            timer.Interval = 1000;   // 1 * 1 * 1000;
            timer.Tick += TimerTick;
            updates.Interval = 100; // 1 * 1 * 100;
            updates.Tick += UpdateChipsAmountOnUI;

            textBoxBB.Visible = false;
            textBoxSB.Visible = false;
            this.Database.Buttons["BB"].Visible = false;
            this.Database.Buttons["SB"].Visible = false;
            textBoxRaise.Text = (bigBlind * 2).ToString();


        }

        public async Task Shuffle(IList<IPlayer> playerList, IDictionary<string,Button> Buttons)
        {
            foreach (var player in playerList)
            {
                bools.Add(player.FoldedTurn);
            }

            Buttons["Call"].Enabled = false;
            Buttons["Raise"].Enabled = false;
            Buttons["Fold"].Enabled = false;
            Buttons["Check"].Enabled = false;

            MaximizeBox = false;
            MinimizeBox = false;

            bool check = false;

            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");

            int horizontal = 580;
            int vertical = 480;
            Random random = new Random();


             for (i = imagesPathsFromDirectory.Length; i > 0; i--)  // razmesva testeto
             {
                 int j = random.Next(i);
                 var k = imagesPathsFromDirectory[j];
                 imagesPathsFromDirectory[j] = imagesPathsFromDirectory[i - 1];
                 imagesPathsFromDirectory[i - 1] = k;
             }


            #region Throwing Cards

            for (i = 0; i < 17; i++)
            {
                deckImages[i] = Image.FromFile(imagesPathsFromDirectory[i]);
                string[] charsToRemove = new string[] { "Assets\\Cards\\", ".png" };

                foreach (string str in charsToRemove)
                {
                    imagesPathsFromDirectory[i] = imagesPathsFromDirectory[i].Replace(str, string.Empty);
                }

                reserve[i] = int.Parse(imagesPathsFromDirectory[i]) - 1;
                cardsHolder[i] = new PictureBox();
                cardsHolder[i].SizeMode = PictureBoxSizeMode.StretchImage;
                cardsHolder[i].Height = 130;
                cardsHolder[i].Width = 80;
                this.Controls.Add(cardsHolder[i]);
                cardsHolder[i].Name = "pb" + i.ToString();
                await Task.Delay(200);

              

                if (i < 2)
                {
                    if (cardsHolder[0].Tag != null)
                    {
                        cardsHolder[1].Tag = reserve[1];
                    }

                    cardsHolder[0].Tag = reserve[0];
                    cardsHolder[i].Image = deckImages[i];
                    cardsHolder[i].Anchor = AnchorStyles.Bottom;

                    //Holder[i].Dock = DockStyle.Top;
                    cardsHolder[i].Location = new Point(horizontal, vertical);
                    horizontal += cardsHolder[i].Width;
                    playerList[0].Panel.Visible = false;
                    this.Controls.Add(playerList[0].Panel);
                    playerList[0].Panel.Location = new Point(cardsHolder[0].Left - 10, cardsHolder[0].Top - 10);
                    playerList[0].Panel.BackColor = Color.DarkBlue;
                    playerList[0].Panel.Height = 150;
                    playerList[0].Panel.Width = 180;
                    
                }

                if (playerList[1].Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 2 && i < 4)
                    {
                        if (cardsHolder[2].Tag != null)
                        {
                            cardsHolder[3].Tag = reserve[3];
                        }

                        cardsHolder[2].Tag = reserve[2];

                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        cardsHolder[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                        cardsHolder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        cardsHolder[i].Location = new Point(horizontal, vertical);
                        horizontal += cardsHolder[i].Width;
                        cardsHolder[i].Visible = true;
                        playerList[1].Panel.Visible = false;

                        this.Controls.Add(playerList[1].Panel);

                        playerList[1].Panel.Location = new Point(cardsHolder[2].Left - 10, cardsHolder[2].Top - 10);
                        playerList[1].Panel.BackColor = Color.DarkBlue;
                        playerList[1].Panel.Height = 150;
                        playerList[1].Panel.Width = 180;
                      
                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (playerList[2].Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 4 && i < 6)
                    {
                        if (cardsHolder[4].Tag != null)
                        {
                            cardsHolder[5].Tag = reserve[5];
                        }

                        cardsHolder[4].Tag = reserve[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }

                        check = true;
                        cardsHolder[i].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        cardsHolder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        cardsHolder[i].Location = new Point(horizontal, vertical);
                        horizontal += cardsHolder[i].Width;
                        cardsHolder[i].Visible = true;
                        playerList[2].Panel.Visible = false;
                        this.Controls.Add(playerList[2].Panel);
                        playerList[2].Panel.Location = new Point(cardsHolder[4].Left - 10, cardsHolder[4].Top - 10);
                        playerList[2].Panel.BackColor = Color.DarkBlue;
                        playerList[2].Panel.Height = 150;
                        playerList[2].Panel.Width = 180;

                        
                        

                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (playerList[3].Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 6 && i < 8)
                    {
                        if (cardsHolder[6].Tag != null)
                        {
                            cardsHolder[7].Tag = reserve[7];
                        }

                        cardsHolder[6].Tag = reserve[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;
                        cardsHolder[i].Anchor = AnchorStyles.Top;
                        cardsHolder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        cardsHolder[i].Location = new Point(horizontal, vertical);
                        horizontal += cardsHolder[i].Width;
                        cardsHolder[i].Visible = true;
                        playerList[3].Panel.Visible = false;
                        this.Controls.Add(playerList[3].Panel);
                        playerList[3].Panel.Location = new Point(cardsHolder[6].Left - 10, cardsHolder[6].Top - 10);
                        playerList[3].Panel.BackColor = Color.DarkBlue;
                        playerList[3].Panel.Height = 150;
                        playerList[3].Panel.Width = 180;
                        
                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (playerList[4].Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 8 && i < 10)
                    {
                        if (cardsHolder[8].Tag != null)
                        {
                            cardsHolder[9].Tag = reserve[9];
                        }

                        cardsHolder[8].Tag = reserve[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }

                        check = true;
                        cardsHolder[i].Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        cardsHolder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        cardsHolder[i].Location = new Point(horizontal, vertical);
                        horizontal += cardsHolder[i].Width;
                        cardsHolder[i].Visible = true;
                        playerList[4].Panel.Visible = false;
                        this.Controls.Add(playerList[4].Panel);
                        playerList[4].Panel.Location = new Point(cardsHolder[8].Left - 10, cardsHolder[8].Top - 10);
                        playerList[4].Panel.BackColor = Color.DarkBlue;
                        playerList[4].Panel.Height = 150;
                        playerList[4].Panel.Width = 180;
                       
                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (playerList[5].Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 10 && i < 12)
                    {
                        if (cardsHolder[10].Tag != null)
                        {
                            cardsHolder[11].Tag = reserve[11];
                        }

                        cardsHolder[10].Tag = reserve[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }

                        check = true;
                        cardsHolder[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        cardsHolder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        cardsHolder[i].Location = new Point(horizontal, vertical);
                        horizontal += cardsHolder[i].Width;
                        cardsHolder[i].Visible = true;
                        playerList[5].Panel.Visible = false;
                        this.Controls.Add(playerList[5].Panel);
                        playerList[5].Panel.Location = new Point(cardsHolder[10].Left - 10, cardsHolder[10].Top - 10);
                        playerList[5].Panel.BackColor = Color.DarkBlue;
                        playerList[5].Panel.Height = 150;
                        playerList[5].Panel.Width = 180;
                       
                        if (i == 11)
                        {
                            check = false;
                        }
                    }
                }

                if (i >= 12)
                {
                    cardsHolder[12].Tag = reserve[12];
                    if (i > 12)
                    {
                        cardsHolder[13].Tag = reserve[13];
                    }

                    if (i > 13)
                    {
                        cardsHolder[14].Tag = reserve[14];
                    }

                    if (i > 14)
                    {
                        cardsHolder[15].Tag = reserve[15];
                    }

                    if (i > 15)
                    {
                        cardsHolder[16].Tag = reserve[16];
                    }

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;

                    if (cardsHolder[i] != null)
                    {
                        cardsHolder[i].Anchor = AnchorStyles.None;
                        cardsHolder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        cardsHolder[i].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }

                #endregion

                if (playerList[1].Chips <= 0)
                {
                    playerList[1].FoldedTurn = true;
                    cardsHolder[2].Visible = false;
                    cardsHolder[3].Visible = false;
                }
                else
                {
                    playerList[1].FoldedTurn = false;
                    if (i == 3)
                    {
                        if (cardsHolder[3] != null)
                        {
                            cardsHolder[2].Visible = true;
                            cardsHolder[3].Visible = true;
                        }
                    }
                }

                if (playerList[2].Chips <= 0)
                {
                    playerList[2].FoldedTurn = true;
                    cardsHolder[4].Visible = false;
                    cardsHolder[5].Visible = false;
                }
                else
                {
                    playerList[2].FoldedTurn = false;
                    if (i == 5)
                    {
                        if (cardsHolder[5] != null)
                        {
                            cardsHolder[4].Visible = true;
                            cardsHolder[5].Visible = true;
                        }
                    }
                }

                if (playerList[3].Chips <= 0)
                {
                    playerList[3].FoldedTurn = true;
                    cardsHolder[6].Visible = false;
                    cardsHolder[7].Visible = false;
                }
                else
                {
                    playerList[3].FoldedTurn = false;
                    if (i == 7)
                    {
                        if (cardsHolder[7] != null)
                        {
                            cardsHolder[6].Visible = true;
                            cardsHolder[7].Visible = true;
                        }
                    }
                }

                if (playerList[4].Chips <= 0)
                {
                    playerList[4].FoldedTurn = true;
                    cardsHolder[8].Visible = false;
                    cardsHolder[9].Visible = false;
                }
                else
                {
                    playerList[4].FoldedTurn = false;
                    if (i == 9)
                    {
                        if (cardsHolder[9] != null)
                        {
                            cardsHolder[8].Visible = true;
                            cardsHolder[9].Visible = true;
                        }
                    }
                }

                if (playerList[5].Chips <= 0)
                {
                    playerList[5].FoldedTurn = true;
                    cardsHolder[10].Visible = false;
                    cardsHolder[11].Visible = false;
                }
                else
                {
                    playerList[5].FoldedTurn = false;
                    if (i == 11)
                    {
                        if (cardsHolder[11] != null)
                        {
                            cardsHolder[10].Visible = true;
                            cardsHolder[11].Visible = true;
                        }
                    }
                }

                if (!restart)
                {
                    MaximizeBox = true;
                    MinimizeBox = true;
                }

                timer.Start();
            }


            if (foldedPlayers == 5)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?",
                    "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                foldedPlayers = 5;
            }
            //TODO: this is useless (i never reaches 17) ! Last iteration i=17 
            if (i == 17)
            {
                Buttons["Raise"].Enabled = true;
                Buttons["Call"].Enabled = true;
                Buttons["Raise"].Enabled = true;
                Buttons["Raise"].Enabled = true;
                Buttons["Fold"].Enabled = true;
            }

        }

       
        public async Task Turns(IList<IPlayer> playerList, IDictionary<string, Button> Buttons)
        {
            #region Rotating
            if (!playerList[0].FoldedTurn)
            {
                if (playerList[0].Turn)
                {
                    FixCall(playerList[0], 1, Buttons["Call"]);

                    MessageBox.Show("Player's Turn");
                    progressBarTimer.Visible = true;
                    progressBarTimer.Value = 1000;
                    time = 60;
                    up = 10000000;
                    timer.Start();
                    Buttons["Raise"].Enabled = true;
                    Buttons["Call"].Enabled = true;
                    Buttons["Raise"].Enabled = true;
                    Buttons["Fold"].Enabled = true;
                    turnCount++;

                    FixCall(playerList[0], 2, Buttons["Call"]);
                }
            }

            if (playerList[0].FoldedTurn || !playerList[0].Turn)
            {
                await AllIn(playerList,Buttons);
                if (playerList[0].FoldedTurn && !playerList[0].HasFolded)
                {
                    if (Buttons["Call"].Text.Contains("All in") == false || Buttons["Raise"].Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        playerList[0].HasFolded = true;
                    }
                }

                await CheckRaise(playerList,Buttons,0, 0);
                progressBarTimer.Visible = false;
                Buttons["Raise"].Enabled = false;
                Buttons["Call"].Enabled = false;
                Buttons["Raise"].Enabled = false;
                Buttons["Fold"].Enabled = false;
                timer.Stop();
                playerList[1].Turn = true;
                if (!playerList[1].FoldedTurn)
                {
                    if (playerList[1].Turn)
                    {
                        FixCall(playerList[1], 1, Buttons["Call"]);
                        FixCall(playerList[1], 2, Buttons["Call"]);
                        Rules(2, 3, playerList[1]);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, playerList[1]);
                        turnCount++;
                        last = 1;
                        playerList[1].Turn = false;
                        playerList[2].Turn = true;
                    }
                }

                if (playerList[1].FoldedTurn && !playerList[1].HasFolded)
                {
                    bools.RemoveAt(1);
                    bools.Insert(1, null);
                    maxLeft--;
                    playerList[1].HasFolded = true;
                }

                if (playerList[1].FoldedTurn || !playerList[1].Turn)
                {
                    await CheckRaise(playerList,Buttons,1, 1);
                    playerList[2].Turn = true;
                }

                if (!playerList[2].FoldedTurn)
                {
                    if (playerList[2].Turn)
                    {
                        FixCall(playerList[2], 1,Buttons["Call"]);
                        FixCall(playerList[2], 2, Buttons["Call"]);
                        Rules(4, 5, playerList[2]);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, playerList[2]);
                        turnCount++;
                        last = 2;
                        playerList[2].Turn = false;
                        playerList[3].Turn = true;
                    }
                }

                if (playerList[2].FoldedTurn && !playerList[2].HasFolded)
                {
                    bools.RemoveAt(2);
                    bools.Insert(2, null);
                    maxLeft--;
                    playerList[2].HasFolded = true;
                }

                if (playerList[2].FoldedTurn || !playerList[2].Turn)
                {
                    await CheckRaise(playerList,Buttons,2, 2);
                    playerList[3].Turn = true;
                }

                if (!playerList[3].FoldedTurn)
                {
                    if (playerList[3].Turn)
                    {
                        FixCall(playerList[3], 1, Buttons["Call"]);
                        FixCall(playerList[3], 2, Buttons["Call"]);
                        Rules(6, 7, playerList[3]);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, playerList[3]);
                        turnCount++;
                        last = 3;
                        playerList[3].Turn = false;
                        playerList[4].Turn = true;
                    }
                }

                if (playerList[3].FoldedTurn && !playerList[3].HasFolded)
                {
                    bools.RemoveAt(3);
                    bools.Insert(3, null);
                    maxLeft--;
                    playerList[3].HasFolded = true;
                }

                if (playerList[3].FoldedTurn || !playerList[3].Turn)
                {
                    await CheckRaise(playerList, Buttons, 3, 3);
                    playerList[4].Turn = true;
                }

                if (!playerList[4].FoldedTurn)
                {
                    if (playerList[4].Turn)
                    {
                        FixCall(playerList[4], 1, Buttons["Call"]);
                        FixCall(playerList[4], 2, Buttons["Call"]);
                        Rules(8, 9, playerList[4]);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, playerList[4]);
                        turnCount++;
                        last = 4;
                        playerList[4].Turn = false;
                        playerList[5].Turn = true;
                    }
                }

                if (playerList[4].FoldedTurn && !playerList[4].HasFolded)
                {
                    bools.RemoveAt(4);
                    bools.Insert(4, null);
                    maxLeft--;
                    playerList[4].HasFolded = true;
                }

                if (playerList[4].FoldedTurn || !playerList[4].Turn)
                {
                    await CheckRaise(playerList, Buttons,4, 4);
                    playerList[5].Turn = true;
                }

                if (!playerList[5].FoldedTurn)
                {
                    if (playerList[5].Turn)
                    {
                        FixCall(playerList[5], 1, Buttons["Call"]);
                        FixCall(playerList[5], 2, Buttons["Call"]);
                        Rules(10, 11, playerList[5]);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, playerList[5]);
                        turnCount++;
                        last = 5;
                        playerList[5].Turn = false;
                    }
                }

                if (playerList[5].FoldedTurn && !playerList[5].HasFolded)
                {
                    bools.RemoveAt(5);
                    bools.Insert(5, null);
                    maxLeft--;
                    playerList[5].HasFolded = true;
                }

                if (playerList[5].FoldedTurn || !playerList[5].Turn)
                {
                    await CheckRaise(playerList, Buttons, 5, 5);
                    playerList[0].Turn = true;
                }

                if (playerList[0].FoldedTurn && !playerList[0].HasFolded)
                {
                    if (Buttons["Call"].Text.Contains("All in") == false || Buttons["Raise"].Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        playerList[0].HasFolded = true;
                    }
                }
                #endregion
                await AllIn(playerList,Buttons);
                if (!restart)
                {
                    await Turns(playerList,Buttons);
                }

                restart = false;
            }
        }

        public void Rules(int c1, int c2, IPlayer player)
        {//ref b1Type, ref bot1Power, bot1.FoldedTurn

            if (!player.FoldedTurn || c1 == 0 && c2 == 1 && player.StatusLabel.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false;
                bool vf = false;
                int[] straight1 = new int[5];
                int[] straight = new int[7];
                straight[0] = reserve[c1];
                straight[1] = reserve[c2];
                straight1[0] = straight[2] = reserve[12];
                straight1[1] = straight[3] = reserve[13];
                straight1[2] = straight[4] = reserve[14];
                straight1[3] = straight[5] = reserve[15];
                straight1[4] = straight[6] = reserve[16];
                var a = straight.Where(o => o % 4 == 0).ToArray();
                var b = straight.Where(o => o % 4 == 1).ToArray();
                var c = straight.Where(o => o % 4 == 2).ToArray();
                var d = straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);
                #endregion
                for (i = 0; i < 16; i++)
                {
                    if (reserve[i] == int.Parse(cardsHolder[c1].Tag.ToString()) && reserve[i + 1] == int.Parse(cardsHolder[c2].Tag.ToString()))
                    {
                        //Pair from Hand current = 1
                        rPairFromHand(player);

                        #region Pair or Two Pair from Table current = 2 || 0
                        rPairTwoPair(player);
                        #endregion

                        #region Two Pair current = 2
                        rTwoPair(player);
                        #endregion

                        #region Three of a kind current = 3
                        rThreeOfAKind(player, straight);
                        #endregion

                        #region Straight current = 4
                        rStraight(player, straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        rFlush(player, ref vf, straight1);
                        #endregion

                        #region Full House current = 6
                        rFullHouse(player, ref done, straight);
                        #endregion

                        #region Four of a Kind current = 7
                        rFourOfAKind(player, straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        rStraightFlush(player, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        rHighCard(player);
                        #endregion
                    }
                }
            }
        }

        public void rStraightFlush(IPlayer player, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (player.Type >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        player.Type = 8;
                        player.Power = st1.Max() / 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        player.Type = 9;
                        player.Power = st1.Max() / 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        player.Type = 8;
                        player.Power = st2.Max() / 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        player.Type = 9;
                        player.Power = st2.Max() / 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        player.Type = 8;
                        player.Power = st3.Max() / 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        player.Type = 9;
                        player.Power = st3.Max() / 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        player.Type = 8;
                        player.Power = st4.Max() / 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        player.Type = 9;
                        player.Power = st4.Max() / 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rFourOfAKind(IPlayer player, int[] straight)
        {
            if (player.Type >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 && straight[j] / 4 == straight[j + 2] / 4 &&
                        straight[j] / 4 == straight[j + 3] / 4)
                    {
                        player.Type = 7;
                        player.Power = ((straight[j] / 4) * 4) + (player.Type * 100);
                        win.Add(new Type() { Power = player.Power, Current = 7 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        player.Type = 7;
                        player.Power = (13 * 4) + (player.Type * 100);

                        win.Add(new Type() { Power = player.Power, Current = 7 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                 
                }
            }
        }

        public void rFullHouse(IPlayer player, ref bool done, int[] straight)
        {
            if (player.Type >= -1)
            {
                type = player.Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(q => q / 4 == j).ToArray();

                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                player.Type = 6;
                                player.Power = (13 * 2) + (player.Type * 100);
                                win.Add(new Type() { Power = player.Power, Current = 6 });
                                sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                player.Type = 6;
                                player.Power = fh.Max() / 4 * 2 + player.Type * 100;
                                win.Add(new Type() { Power = player.Power, Current = 6 });
                                sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                player.Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                player.Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (player.Type != 6)
                {
                    player.Power = type;
                }
            }
        }

        public void rFlush(IPlayer player, ref bool vf, int[] straight)
        {
            if (player.Type >= -1)
            {
                var f1 = straight.Where(o => o % 4 == 0).ToArray();
                var f2 = straight.Where(o => o % 4 == 1).ToArray();
                var f3 = straight.Where(o => o % 4 == 2).ToArray();
                var f4 = straight.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (reserve[i] % 4 == reserve[i + 1] % 4 && reserve[i] % 4 == f1[0] % 4)
                    {
                        if (reserve[i] / 4 > f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i] + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i + 1] + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f1.Max() / 4 && reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = f1.Max() + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4)//different cards in hand
                {
                    if (reserve[i] % 4 != reserve[i + 1] % 4 && reserve[i] % 4 == f1[0] % 4)
                    {
                        if (reserve[i] / 4 > f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i] + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f1.Max() + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i + 1] + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f1.Max() + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (reserve[i] % 4 == f1[0] % 4 && reserve[i] / 4 > f1.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[i] + (player.Type * 100);
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f1[0] % 4 && reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[i + 1] + (player.Type * 100);
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f1.Min() / 4 && reserve[i + 1] / 4 < f1.Min())
                    {
                        player.Type = 5;
                        player.Power = f1.Max() + (player.Type * 100);
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (reserve[i] % 4 == reserve[i + 1] % 4 && reserve[i] % 4 == f2[0] % 4)
                    {
                        if (reserve[i] / 4 > f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i] + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i + 1] + (player.Type * 100);
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f2.Max() / 4 && reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = f2.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4)//different cards in hand
                {
                    if (reserve[i] % 4 != reserve[i + 1] % 4 && reserve[i] % 4 == f2[0] % 4)
                    {
                        if (reserve[i] / 4 > f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f2.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f2.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (reserve[i] % 4 == f2[0] % 4 && reserve[i] / 4 > f2.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[i] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f2[0] % 4 && reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[i + 1] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f2.Min() / 4 && reserve[i + 1] / 4 < f2.Min())
                    {
                        player.Type = 5;
                        player.Power = f2.Max() + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (reserve[i] % 4 == reserve[i + 1] % 4 && reserve[i] % 4 == f3[0] % 4)
                    {
                        if (reserve[i] / 4 > f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f3.Max() / 4 && reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = f3.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 4)//different cards in hand
                {
                    if (reserve[i] % 4 != reserve[i + 1] % 4 && reserve[i] % 4 == f3[0] % 4)
                    {
                        if (reserve[i] / 4 > f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f3.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f3.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (reserve[i] % 4 == f3[0] % 4 && reserve[i] / 4 > f3.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[i] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f3[0] % 4 && reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[i + 1] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f3.Min() / 4 && reserve[i + 1] / 4 < f3.Min())
                    {
                        player.Type = 5;
                        player.Power = f3.Max() + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (reserve[i] % 4 == reserve[i + 1] % 4 && reserve[i] % 4 == f4[0] % 4)
                    {
                        if (reserve[i] / 4 > f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f4.Max() / 4 && reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = f4.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 4)//different cards in hand
                {
                    if (reserve[i] % 4 != reserve[i + 1] % 4 && reserve[i] % 4 == f4[0] % 4)
                    {
                        if (reserve[i] / 4 > f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f4.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[i + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f4.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (reserve[i] % 4 == f4[0] % 4 && reserve[i] / 4 > f4.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[i] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f4[0] % 4 && reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[i + 1] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f4.Min() / 4 && reserve[i + 1] / 4 < f4.Min())
                    {
                        player.Type = 5;
                        player.Power = f4.Max() + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                //ace
                if (f1.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rStraight(IPlayer player, int[] straight)
        {
            if (player.Type >= -1)
            {
                var op = straight.Select(q => q / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            player.Type = 4;
                            player.Power = op.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 4 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            player.Type = 4;
                            player.Power = op[j + 4] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 4 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        player.Type = 4;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 4 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rThreeOfAKind(IPlayer player, int[] straight)
        {
            if (player.Type >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            player.Power = 13 * 3 + player.Type * 100;
                        }
                        else
                        {
                            player.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + player.Type * 100;
                        }

                        player.Type = 3;
                        win.Add(new Type() { Power = player.Power, Current = 3 });
                        sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                    }
                }
            }
        }

        public void rTwoPair(IPlayer player)
        {
            if (player.Type >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (reserve[i] / 4 != reserve[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }

                            if (tc - k >= 12)
                            {
                                if (reserve[i] / 4 == reserve[tc] / 4 && reserve[i + 1] / 4 == reserve[tc - k] / 4 ||
                                    reserve[i + 1] / 4 == reserve[tc] / 4 && reserve[i] / 4 == reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[i] / 4 == 0)
                                        {
                                            player.Type = 2;
                                            player.Power = 13 * 4 + (reserve[i + 1] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 == 0)
                                        {
                                            player.Type = 2;
                                            player.Power = 13 * 4 + (reserve[i] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 != 0 && reserve[i] / 4 != 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[i] / 4) * 2 + (reserve[i + 1] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void rPairTwoPair(IPlayer player)
        {
            if (player.Type >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }

                        if (tc - k >= 12)
                        {
                            if (reserve[tc] / 4 == reserve[tc - k] / 4)
                            {
                                if (reserve[tc] / 4 != reserve[i] / 4 && reserve[tc] / 4 != reserve[i + 1] / 4 && player.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[i + 1] / 4 == 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[i] / 4) * 2 + 13 * 4 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i] / 4 == 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[i + 1] / 4) * 2 + 13 * 4 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 != 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[tc] / 4) * 2 + (reserve[i + 1] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i] / 4 != 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[tc] / 4) * 2 + (reserve[i] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (player.Type == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (reserve[i] / 4 > reserve[i + 1] / 4)
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                player.Type = 0;
                                                player.Power = 13 + reserve[i] / 4 + player.Type * 100;
                                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                player.Type = 0;
                                                player.Power = reserve[tc] / 4 + reserve[i] / 4 + player.Type * 100;
                                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                player.Type = 0;
                                                player.Power = 13 + reserve[i + 1] + player.Type * 100;
                                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                player.Type = 0;
                                                player.Power = reserve[tc] / 4 + reserve[i + 1] / 4 + player.Type * 100;
                                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }

                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void rPairFromHand(IPlayer player)
        {
            if (player.Type >= -1)
            {
                bool msgbox = false;

                if (reserve[i] / 4 == reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (reserve[i] / 4 == 0)
                        {
                            player.Type = 1;
                            player.Power = 13 * 4 + player.Type * 100;

                        }
                        else
                        {
                            player.Type = 1;
                            player.Power = (reserve[i + 1] / 4) * 4 + player.Type * 100;
                        }

                        win.Add(new Type() { Power = player.Power, Current = 1 });
                        sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (reserve[i + 1] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[i + 1] / 4 == 0)
                            {
                                player.Type = 1;
                                player.Power = 13 * 4 + reserve[i] / 4 + player.Type * 100;
                            }
                            else
                            {
                                player.Type = 1;
                                player.Power = (reserve[i + 1] / 4) * 4 + reserve[i] / 4 + player.Type * 100;
                            }

                            win.Add(new Type() { Power = player.Power, Current = 1 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }

                        msgbox = true;
                    }

                    if (reserve[i] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[i] / 4 == 0)
                            {
                                player.Type = 1;
                                player.Power = 13 * 4 + reserve[i + 1] / 4 + player.Type * 100;
                            }
                            else
                            {
                                player.Type = 1;
                                player.Power = (reserve[tc] / 4) * 4 + reserve[i + 1] / 4 + player.Type * 100;
                            }

                            win.Add(new Type() { Power = player.Power, Current = 1 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }

                        msgbox = true;
                    }
                }
            }
        }

        public void rHighCard(IPlayer player)
        {
            if (player.Type == -1)
            {
                if (reserve[i] / 4 > reserve[i + 1] / 4)
                {
                    player.Type = -1;
                    player.Power = reserve[i] / 4;
                }
                else
                {
                    player.Type = -1;
                    player.Power = reserve[i + 1] / 4;
                }

                if (reserve[i] / 4 == 0 || reserve[i + 1] / 4 == 0)
                {
                    player.Type = -1;
                    player.Power = 13;
                }

                win.Add(new Type() { Power = player.Power, Current = -1 });
                sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
            }
        }

        public void Winner(IList<IPlayer> playerList, IPlayer player, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }

            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (cardsHolder[j].Visible)
                {
                    cardsHolder[j].Image = deckImages[j];
                }
            }

            if (player.Type == sorted.Current)
            {
                if (player.Power == sorted.Power)
                {
                    winners++;
                    checkWinners.Add(player.Name);
                    if (player.Type == -1)
                    {
                        MessageBox.Show(player.Name + " High Card ");
                    }

                    if (player.Type == 1 || player.Type == 0)
                    {
                        MessageBox.Show(player.Name + " Pair ");
                    }

                    if (player.Type == 2)
                    {
                        MessageBox.Show(player.Name + " Two Pair ");
                    }

                    if (player.Type == 3)
                    {
                        MessageBox.Show(player.Name + " Three of a Kind ");
                    }

                    if (player.Type == 4)
                    {
                        MessageBox.Show(player.Name + " Straight ");
                    }

                    if (player.Type == 5 || player.Type == 5.5)
                    {
                        MessageBox.Show(player.Name + " Flush ");
                    }

                    if (player.Type == 6)
                    {
                        MessageBox.Show(player.Name + " Full House ");
                    }

                    if (player.Type == 7)
                    {
                        MessageBox.Show(player.Name + " Four of a Kind ");
                    }

                    if (player.Type == 8)
                    {
                        MessageBox.Show(player.Name + " Straight Flush ");
                    }

                    if (player.Type == 9)
                    {
                        MessageBox.Show(player.Name + " Royal Flush ! ");
                    }
                }
            }

            //lastfixed
            if (player.Name == lastly)
            {
                if (winners > 1)
                {
                    if (checkWinners.Contains(player.Name))
                    {
                        player.Chips += int.Parse(textBoxPot.Text) / winners;
                        playerList[0].ChipsTextBox.Text = player.Chips.ToString();


                    }

                    //if (checkWinners.Contains("Bot 1"))
                    // {
                    //     bot1.Chips += int.Parse(textBoxPot.Text) / winners;
                    //     textBoxBot1Chips.Text = bot1.Chips.ToString();

                    //b1Panel.Visible = true;
                    //  }

                    //if (checkWinners.Contains("Bot 2"))
                    //{
                    //    player.Chips += int.Parse(textBoxPot.Text) / winners;
                    //    textBoxBot2Chips.Text = player.Chips.ToString();

                    //    //b2Panel.Visible = true;
                    //}

                    //if (checkWinners.Contains("Bot 3"))
                    //{
                    //    player.Chips += int.Parse(textBoxPot.Text) / winners;
                    //    textBoxBot3Chips.Text = player.Chips.ToString();

                    //    //b3Panel.Visible = true;
                    //}

                    //if (checkWinners.Contains("Bot 4"))
                    //{
                    //    player.Chips += int.Parse(textBoxPot.Text) / winners;
                    //    textBoxBot4Chips.Text = player.Chips.ToString();

                    //    //b4Panel.Visible = true;
                    //}

                    //if (checkWinners.Contains("Bot 5"))
                    //{
                    //    player.Chips += int.Parse(textBoxPot.Text) / winners;
                    //    textBoxBot5Chips.Text = player.Chips.ToString();

                    //    //b5Panel.Visible = true;
                    //}

                    //await Finish(1);
                }

                if (winners == 1)
                {
                    if (checkWinners.Contains(player.Name))
                    {
                        player.Chips += int.Parse(textBoxPot.Text);

                        //await Finish(1);
                        //pPanel.Visible = true;
                    }

                    //if (checkWinners.Contains("Bot 1"))
                    //{
                    //    bot1.Chips += int.Parse(textBoxPot.Text);

                    //    //await Finish(1);
                    //    //b1Panel.Visible = true;
                    //}

                    //if (checkWinners.Contains("Bot 2"))
                    //{
                    //    bot2Chips += int.Parse(textBoxPot.Text);

                    //    //await Finish(1);
                    //    //b2Panel.Visible = true;
                    //}

                    //if (checkWinners.Contains("Bot 3"))
                    //{
                    //    bot3Chips += int.Parse(textBoxPot.Text);

                    //    //await Finish(1);
                    //    //b3Panel.Visible = true;
                    //}

                    //if (checkWinners.Contains("Bot 4"))
                    //{
                    //    bot4Chips += int.Parse(textBoxPot.Text);

                    //    //await Finish(1);
                    //    //b4Panel.Visible = true;
                    //}

                    //if (checkWinners.Contains("Bot 5"))
                    //{
                    //    bot5Chips += int.Parse(textBoxPot.Text);

                    //    //await Finish(1);
                    //    //b5Panel.Visible = true;
                    //}
                }
            }
        }

        public async Task CheckRaise(IList<IPlayer> playerList, IDictionary<string, Button> Buttons, int currentTurn, int raiseTurn)

        {
            if (raising)
            {
                turnCount = 0;
                raising = false;
                raisedTurn = currentTurn;
                changed = true;
            }
            else
            {
                if (turnCount >= maxLeft - 1 || !changed && turnCount == maxLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !changed && turnCount == maxLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        changed = false;
                        turnCount = 0;
                        Raise = 0;
                        call = 0;
                        raisedTurn = 123;
                        rounds++;
                        foreach (var player in playerList)
                        {
                            if (!player.FoldedTurn)
                            {
                                player.StatusLabel.Text = string.Empty;
                            }
                        }
                    }
                }
            }

            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (cardsHolder[j].Image != deckImages[j])
                    {
                        cardsHolder[j].Image = deckImages[j];
                        foreach (var player in playerList)
                        {
                            player.Call = 0;
                            player.Raise = 0;
                        }
                    }
                }
            }

            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (cardsHolder[j].Image != deckImages[j])
                    {
                        cardsHolder[j].Image = deckImages[j];
                        foreach (var player in playerList)
                        {
                            player.Call = 0;
                            player.Raise = 0;
                        }
                    }
                }
            }
            //mai vsichki ifove pravqt edno i sushto = > nqma smisyl ot tqh

            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (cardsHolder[j].Image != deckImages[j])
                    {
                        cardsHolder[j].Image = deckImages[j];
                        foreach (var player in playerList)
                        {
                            player.Call = 0;
                            player.Raise = 0;
                        }
                    }
                }
            }

            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = null;
                int count = 0;
                foreach (var player in playerList)
                {
                    if(!player.StatusLabel.Text.Contains("Fold"))
                    {
                        fixedLast = player.Name;
                        Rules(count, count + 1, player);
                    }
                    count += 2;
                }
                foreach (var player in playerList)
                {
                    Winner(playerList,player, fixedLast);
                }

                restart = true;
                playerList[0].Turn = true;
                foreach (var player in playerList)
                {
                    player.FoldedTurn = false;
                }

                if (playerList[0].Chips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        foreach (var player in playerList)
                        {
                            player.Chips += f2.a;
                        }
                        playerList[0].FoldedTurn = false;
                        playerList[0].Turn = true;
                        Buttons["Raise"].Enabled = true;
                        Buttons["Fold"].Enabled = true;
                        Buttons["Check"].Enabled = true;
                    }
                }

                foreach (var player in playerList)
                {
                    player.Panel.Visible = false;
                    player.Call = 0;
                    player.Raise = 0;
                }


                last = 0;
                call = bigBlind;
                Raise = 0;
                imagesPathsFromDirectory = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                bools.Clear();
                rounds = 0;
                type = 0;

                foreach (var player in playerList)
                {
                    player.Power = 0;
                    player.Type = -1;
                }

                ints.Clear();
                checkWinners.Clear();
                winners = 0;
                win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int x = 0; x < 17; x++)
                {
                    cardsHolder[x].Image = null;
                    cardsHolder[x].Invalidate();
                    cardsHolder[x].Visible = false;
                }

                textBoxPot.Text = "0";
                playerList[0].StatusLabel.Text = string.Empty;
                await Shuffle(playerList,Buttons);
                await Turns(playerList,Buttons);
            }
        }

        public void FixCall(IPlayer player, int options, Button callButton)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (player.StatusLabel.Text.Contains("Raise"))
                    {
                        string changeRaise = player.StatusLabel.Text.Substring(6);
                        player.Raise = int.Parse(changeRaise);
                    }

                    if (player.StatusLabel.Text.Contains("Call"))
                    {
                        string changeCall = player.StatusLabel.Text.Substring(5);
                        player.Call = int.Parse(changeCall);
                    }

                    if (player.StatusLabel.Text.Contains("Check"))
                    {
                        player.Raise = 0;
                        player.Call = 0;
                    }
                }

                if (options == 2)
                {
                    if (player.Raise != Raise && player.Raise <= Raise)
                    {
                        call = Convert.ToInt32(Raise) - player.Raise;
                    }

                    if (player.Call != call || player.Call <= call)
                    {
                        call = call - player.Call;
                    }

                    if (player.Raise == Raise && Raise > 0)
                    {
                        call = 0;
                        callButton.Enabled = false;
                        callButton.Text = "Call is fuckedup";
                    }
                }
            }
        }

        public async Task AllIn(IList<IPlayer> playerList, IDictionary<string, Button> Buttons )
        {
            #region All in
            if (playerList[0].Chips <= 0 && !intsadded)
            {
                if ((playerList[0].StatusLabel.Text.Contains("Raise")) || (playerList[0].StatusLabel.Text.Contains("Call")))
                {
                    ints.Add(playerList[0].Chips);
                    intsadded = true;
                }
            }

            intsadded = false;
            for (int j = 1; j < playerList.Count; j++)
            {
                if (playerList[j].Chips <= 0 && !playerList[j].FoldedTurn)
                {
                    if (!intsadded)
                    {
                        ints.Add(playerList[j].Chips);
                        intsadded = true;
                    }

                    intsadded = false;
                }
            }


            if (ints.ToArray().Length == maxLeft)
            {
                await Finish(playerList,Buttons,2); 
            }
            else
            {
                ints.Clear();
            }
            #endregion

            var abc = bools.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = bools.IndexOf(false);
                if (index == 0)
                {
                    playerList[0].Chips += int.Parse(textBoxPot.Text);
                    playerList[0].ChipsTextBox.Text = playerList[0].Chips.ToString();
                    playerList[0].Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }

                if (index == 1)
                {
                    playerList[1].Chips += int.Parse(textBoxPot.Text);
                    playerList[0].ChipsTextBox.Text = playerList[1].Chips.ToString();
                    playerList[1].Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }

                if (index == 2)
                {
                    playerList[2].Chips += int.Parse(textBoxPot.Text);
                    playerList[0].ChipsTextBox.Text = playerList[2].Chips.ToString();
                    playerList[2].Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }

                if (index == 3)
                {
                    playerList[3].Chips += int.Parse(textBoxPot.Text);
                    playerList[0].ChipsTextBox.Text = playerList[3].Chips.ToString();
                    playerList[3].Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }

                if (index == 4)
                {
                    playerList[4].Chips += int.Parse(textBoxPot.Text);
                    playerList[0].ChipsTextBox.Text = playerList[4].Chips.ToString();
                    playerList[4].Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }

                if (index == 5)
                {
                    playerList[5].Chips += int.Parse(textBoxPot.Text);
                    playerList[0].ChipsTextBox.Text = playerList[5].Chips.ToString();
                    playerList[5].Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }

                for (int j = 0; j <= 16; j++)
                {
                    cardsHolder[j].Visible = false;
                }

                await Finish(playerList, Buttons,1);
            }

            intsadded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= End)
            {
                await Finish(playerList,Buttons,2);
            }
            #endregion

        }

        public async Task Finish(IList<IPlayer> playerList,IDictionary<string,Button> Buttons, int n)
        {
            if (n == 2)
            {
                FixWinners(playerList);
            }

            foreach (var player in playerList)
            {
                player.Panel.Visible = false;
                player.Power = 0;
                player.Type = -1;
                player.Turn = false;
                player.FoldedTurn = false;
                player.HasFolded = false;
                player.Call = 0;
                player.Raise = 0;
                player.StatusLabel.Text = string.Empty;
            }
            playerList[0].Turn = true;

            call = bigBlind;
            Raise = 0;
            foldedPlayers = 5;
            type = 0;
            rounds = 0;
            Raise = 0;
            restart = false;
            raising = false;
            height = 0;
            width = 0;
            winners = 0;
            Flop = 1;
            Turn = 2;
            River = 3;
            End = 4;
            maxLeft = 6;
            last = 123;
            raisedTurn = 1;
            bools.Clear();
            checkWinners.Clear();
            ints.Clear();
            win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            textBoxPot.Text = "0";
            time = 60;
            up = 10000000;
            turnCount = 0;
            if (playerList[0].Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    playerList[0].Chips = f2.a;
                    playerList[1].Chips += f2.a;
                    playerList[2].Chips += f2.a;
                    playerList[3].Chips += f2.a;
                    playerList[4].Chips += f2.a;
                    playerList[5].Chips += f2.a;
                    playerList[0].FoldedTurn = false;
                    playerList[0].Turn = true;
                    Buttons["Raise"].Enabled = true;
                    Buttons["Fold"].Enabled = true;
                    Buttons["Check"].Enabled = true;
                    Buttons["Raise"].Text = "Raise";
                }
            }

            imagesPathsFromDirectory = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                cardsHolder[os].Image = null;
                cardsHolder[os].Invalidate();
                cardsHolder[os].Visible = false;
            }

            await Shuffle(playerList, Buttons);

            //await Turns();
        }

        public void FixWinners(IList<IPlayer> playerList)
        {
            win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = string.Empty;
            int count = 0;
            for (int j = 0; j < playerList.Count; j++)
            {
                if (playerList[j].StatusLabel.Text.Contains("Fold"))
                {
                    fixedLast = playerList[j].Name;
                    Rules(count, count + 1, playerList[j]);
                }
                count += 2;

            }
            foreach (var player in playerList)
            {
                Winner(playerList,player, fixedLast);
            }
        }

        public void AI(int c1, int c2, IPlayer bot)
        {
            if (!bot.FoldedTurn)
            {
                if (bot.Type == -1)
                {
                    HighCard(bot);
                }

                if (bot.Type == 0)
                {
                    PairTable(bot);
                }

                if (bot.Type == 1)
                {
                    PairHand(bot);
                }

                if (bot.Type == 2)
                {
                    TwoPair(bot);
                }

                if (bot.Type == 3)
                {
                    ThreeOfAKind(bot);
                }

                if (bot.Type == 4)
                {
                    Straight(bot);
                }

                if (bot.Type == 5 || bot.Type == 5.5)
                {
                    Flush(bot);
                }

                if (bot.Type == 6)
                {
                    FullHouse(bot);
                }

                if (bot.Type == 7)
                {
                    FourOfAKind(bot);
                }

                if (bot.Type == 8 || bot.Type == 9)
                {
                    StraightFlush(bot);
                }
            }

            if (bot.FoldedTurn)
            {
                cardsHolder[c1].Visible = false;
                cardsHolder[c2].Visible = false;
            }
        }

        public void HighCard(IPlayer bot)
        {
            int magicNumber1 = 20;
            int magicNumber2 = 25;
            HP(bot, magicNumber1, magicNumber2);
        }

        public void PairTable(IPlayer bot)
        {
            int magicNumber1 = 16;
            int magicNumber2 = 25;
            HP(bot, magicNumber1, magicNumber2);
        }

        public void PairHand(IPlayer bot)
        {
            Random random = new Random();
            int rCall = random.Next(10, 16);
            int rRaise = random.Next(10, 13);
            PH(bot, rCall, 6, rRaise);
            PH(bot, rCall, 9, rRaise);
        }

        public void TwoPair(IPlayer bot)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            PH(bot, rCall, 3, rRaise);
        }

        public void ThreeOfAKind(IPlayer bot)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            Smooth(bot, tCall, tRaise);
        }

        public void Straight(IPlayer bot)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            Smooth(bot, sCall, sRaise);
        }

        public void Flush(IPlayer bot)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(bot, fCall, fRaise);
        }

        public void FullHouse(IPlayer bot)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            Smooth(bot, fhCall, fhRaise);
        }

        public void FourOfAKind(IPlayer bot)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            Smooth(bot, fkCall, fkRaise);
        }

        public void StraightFlush(IPlayer bot)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            Smooth(bot, sfCall, sfRaise);
        }

        public void Fold(IPlayer bot)
        {
            raising = false;
            bot.StatusLabel.Text = "Fold";
            bot.Turn = false;
            bot.FoldedTurn = true;
        }

        public void Check(IPlayer bot)
        {
            bot.StatusLabel.Text = "Check";
            bot.Turn = false;
            raising = false;
        }

        public void Call(IPlayer bot)
        {
            raising = false;
            bot.Turn = false;
            bot.Chips -= call;
            bot.StatusLabel.Text = "Call " + call;
            textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
        }

        public void Raised(IPlayer bot)
        {
            bot.Chips -= Convert.ToInt32(Raise);
            bot.StatusLabel.Text = "Raise " + Raise;
            textBoxPot.Text = (int.Parse(textBoxPot.Text) + Convert.ToInt32(Raise)).ToString();
            call = Convert.ToInt32(Raise);
            raising = true;
            bot.Turn = false;
        }

        public static double RoundN(int chipsAmount, int n)
        {
            double a = Math.Round((chipsAmount / n) / 100d, 0) * 100;
            return a;
        }

        public void HP(IPlayer bot, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (call <= 0)
            {
                Check(bot);
            }

            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(bot.Chips, n))
                    {
                        Call(bot);
                    }
                    else
                    {
                        Fold(bot);
                    }
                }

                if (rnd == 2)
                {
                    if (call <= RoundN(bot.Chips, n1))
                    {
                        Call(bot);
                    }
                    else
                    {
                        Fold(bot);
                    }
                }
            }

            if (rnd == 3)
            {
                if (Raise == 0)
                {
                    Raise = call * 2;
                    Raised(bot);
                }
                else
                {
                    if (Raise <= RoundN(bot.Chips, n))
                    {
                        Raise = call * 2;
                        Raised(bot);
                    }
                    else
                    {
                        Fold(bot);
                    }
                }
            }

            if (bot.Chips <= 0)
            {
                bot.FoldedTurn = true;
            }
        }

        public void PH(IPlayer bot, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(bot);
                }

                if (call > 0)
                {
                    if (call >= RoundN(bot.Chips, n1))
                    {
                        Fold(bot);
                    }

                    if (Raise > RoundN(bot.Chips, n))
                    {
                        Fold(bot);
                    }

                    if (!bot.FoldedTurn)
                    {
                        if (call >= RoundN(bot.Chips, n) && call <= RoundN(bot.Chips, n1))
                        {
                            Call(bot);
                        }

                        if (Raise <= RoundN(bot.Chips, n) && Raise >= RoundN(bot.Chips, n) / 2)
                        {
                            Call(bot);
                        }

                        if (Raise <= RoundN(bot.Chips, n) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(bot.Chips, n);
                                Raised(bot);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(bot);
                            }
                        }
                    }
                }
            }

            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(bot.Chips, n1 - rnd))
                    {
                        Fold(bot);
                    }

                    if (Raise > RoundN(bot.Chips, n - rnd))
                    {
                        Fold(bot);
                    }

                    if (!bot.FoldedTurn)
                    {
                        if (call >= RoundN(bot.Chips, n - rnd) && call <= RoundN(bot.Chips, n1 - rnd))
                        {
                            Call(bot);
                        }

                        if (Raise <= RoundN(bot.Chips, n - rnd) && Raise >= RoundN(bot.Chips, n - rnd) / 2)
                        {
                            Call(bot);
                        }

                        if (Raise <= RoundN(bot.Chips, n - rnd) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(bot.Chips, n - rnd);
                                Raised(bot);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(bot);
                            }
                        }
                    }
                }

                if (call <= 0)
                {
                    Raise = RoundN(bot.Chips, r - rnd);
                    Raised(bot);
                }
            }

            if (bot.Chips <= 0)
            {
                bot.FoldedTurn = true;
            }
        }

        public void Smooth(IPlayer bot, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (call <= 0)
            {
                Check(bot);
            }
            else
            {
                if (call >= RoundN(bot.Chips, n))
                {
                    if (bot.Chips > call)
                    {
                        Call(bot);
                    }
                    else if (bot.Chips <= call)
                    {
                        raising = false;
                        bot.Turn = false;
                        bot.Chips = 0;
                        bot.StatusLabel.Text = "Call " + bot.Chips;
                        textBoxPot.Text = (int.Parse(textBoxPot.Text) + bot.Chips).ToString();
                    }
                }
                else
                {
                    if (Raise > 0)
                    {
                        if (bot.Chips >= Raise * 2)
                        {
                            Raise *= 2;
                            Raised(bot);
                        }
                        else
                        {
                            Call(bot);
                        }
                    }
                    else
                    {
                        Raise = call * 2;
                        Raised(bot);
                    }
                }
            }

            if (bot.Chips <= 0)
            {
                bot.FoldedTurn = true;
            }
        }

        #region UI
        /// <summary>
        /// Controlling time per turn;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void TimerTick(object sender, object e)
        {
            if (progressBarTimer.Value <= 0)
            {
                this.Database.Players[0].FoldedTurn = true;
                await Turns(this.Database.Players, this.Database.Buttons);
            }

            if (time > 0)
            {
                time--;
                progressBarTimer.Value = (time / 6) * 100;
            }
        }

        /// <summary>
        /// Set value for chips amount in UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateChipsAmountOnUI(object sender, object e)
        {

            for (int j = 0; j < this.Database.Players.Count; j++)
            {
                this.Database.Players[j].ChipsTextBox.Text = string.Format("Chips : {0} ", this.Database.Players[j].Chips);
            }

            if (this.Database.Players[0].Chips <= 0)
            {
                this.Database.Players[0].Turn = false;
                this.Database.Players[0].FoldedTurn = true;
                this.Database.Buttons["Call"].Enabled = false;
                this.Database.Buttons["Raise"].Enabled = false;
                this.Database.Buttons["Fold"].Enabled = false;
                this.Database.Buttons["Check"].Enabled = false;
            }

            if (up > 0)
            {
                up--;
            }

            if (this.Database.Players[0].Chips >= call)
            {
                this.Database.Buttons["Call"].Text = "Call " + call.ToString();
            }
            else
            {
                this.Database.Buttons["Call"].Text = "All in";
                this.Database.Buttons["Raise"].Enabled = false;
            }

            if (call > 0)
            {
                this.Database.Buttons["Check"].Enabled = false;
            }

            if (call <= 0)
            {
                this.Database.Buttons["Check"].Enabled = true;
                this.Database.Buttons["Call"].Text = "Call";
                this.Database.Buttons["Call"].Enabled = false;
            }

            if (this.Database.Players[0].Chips <= 0)
            {
                this.Database.Buttons["Raise"].Enabled = false;
            }

            int parsedValue;

            if (textBoxRaise.Text != string.Empty && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (this.Database.Players[0].Chips <= int.Parse(textBoxRaise.Text))
                {
                    this.Database.Buttons["Raise"].Text = "All in";
                }
                else
                {
                    this.Database.Buttons["Raise"].Text = "Raise";
                }
            }

            if (this.Database.Players[0].Chips < call)
            {
                this.Database.Buttons["Raise"].Enabled = false;
            }
        }

        /// <summary>
        /// Provides button Fold click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonFold_Click(object sender, EventArgs e)
        {
            this.Database.Players[0].StatusLabel.Text = "Fold";
            this.Database.Players[0].Turn = false;
            this.Database.Players[0].FoldedTurn = true;
            await Turns(this.Database.Players, this.Database.Buttons);
        }  // bFold_Click

        /// <summary>
        /// Provides button Check click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                this.Database.Players[0].Turn = false;
                this.Database.Players[0].StatusLabel.Text = "Check";
            }
            else
            {
                ////pStatus.Text = "All in " + Chips;
                this.Database.Buttons["Check"].Enabled = false;
            }

            await Turns(this.Database.Players, this.Database.Buttons);
        } // bCheck_Click

        /// <summary>
        /// Provides button Call click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, this.Database.Players[0]);
            if (this.Database.Players[0].Chips >= call)
            {
                this.Database.Players[0].Chips -= call;
                this.Database.Players[0].ChipsTextBox.Text = string.Format("Chips : {0}", this.Database.Players[0].Chips);
                if (textBoxPot.Text != string.Empty)
                {
                    textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
                }
                else
                {
                    textBoxPot.Text = call.ToString();
                }

                this.Database.Players[0].Turn = false;
                this.Database.Players[0].StatusLabel.Text = "Call " + call;
                this.Database.Players[0].Call = call;
            }
            else if (this.Database.Players[0].Chips <= call && call > 0)
            {
                textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.Database.Players[0].Chips).ToString();
                this.Database.Players[0].StatusLabel.Text = "All in " + this.Database.Players[0].Chips;
                this.Database.Players[0].Chips = 0;
                this.Database.Players[0].ChipsTextBox.Text = "Chips : " + this.Database.Players[0].Chips.ToString();
                this.Database.Players[0].Turn = false;
                this.Database.Buttons["Fold"].Enabled = false;
                this.Database.Players[0].Call = this.Database.Players[0].Chips;
            }

            await Turns(this.Database.Players, this.Database.Buttons);
        } // bCall_Click

        /// <summary>
        /// Provides button Rall click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, this.Database.Players[0]);
            int parsedValue;
            if (textBoxRaise.Text != string.Empty && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (this.Database.Players[0].Chips > call)
                {
                    if (Raise * 2 > int.Parse(textBoxRaise.Text))
                    {
                        textBoxRaise.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (this.Database.Players[0].Chips >= int.Parse(textBoxRaise.Text))
                        {
                            call = int.Parse(textBoxRaise.Text);
                            Raise = int.Parse(textBoxRaise.Text);
                            this.Database.Players[0].StatusLabel.Text = "Raise " + call.ToString();
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
                            this.Database.Buttons["Call"].Text = "Call";
                            this.Database.Players[0].Chips -= int.Parse(textBoxRaise.Text);
                            raising = true;
                            last = 0;
                            this.Database.Players[0].Raise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            call = this.Database.Players[0].Chips;
                            Raise = this.Database.Players[0].Chips;
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.Database.Players[0].Chips).ToString();
                            this.Database.Players[0].StatusLabel.Text = "Raise " + call.ToString();
                            this.Database.Players[0].Chips = 0;
                            raising = true;
                            last = 0;
                            this.Database.Players[0].Raise = Convert.ToInt32(Raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }

            this.Database.Players[0].Turn = false;
            await Turns(this.Database.Players, this.Database.Buttons);
        } // bRaise_Click

        /// <summary>
        /// Provides button Add click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (textBoxAdd.Text != string.Empty)
            {
                for (int j = 0; j < this.Database.Players.Count; j++)
                {
                    this.Database.Players[i].Chips += int.Parse(textBoxAdd.Text);
                }
            }

            this.Database.Players[0].ChipsTextBox.Text = "Chips : " + this.Database.Players[0].Chips.ToString();
        } // bAdd_Click

        /// <summary>
        /// Provides button SB/BB click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSBBBOption_Click(object sender, EventArgs e)
        {
            this.textBoxBB.Text = this.bigBlind.ToString();
            this.textBoxSB.Text = this.smallBlind.ToString();

            if (this.textBoxBB.Visible == false)
            {
                this.textBoxBB.Visible = true;
                this.textBoxSB.Visible = true;
                this.Database.Buttons["BB"].Visible = true;
                this.Database.Buttons["SB"].Visible = true;
            }
            else
            {
                this.textBoxBB.Visible = false;
                this.textBoxSB.Visible = false;
                this.Database.Buttons["BB"].Visible = false;
                this.Database.Buttons["SB"].Visible = false;
            }
        } // bOption_Click

        /// <summary>
        /// Provides button SmallBlind click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSmallBlind_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.textBoxSB.Text.Contains(",") || this.textBoxSB.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                this.textBoxSB.Text = this.smallBlind.ToString();
                return;
            }

            if (!int.TryParse(this.textBoxSB.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                this.textBoxSB.Text = this.smallBlind.ToString();
                return;
            }

            if (int.Parse(this.textBoxSB.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                this.textBoxSB.Text = this.smallBlind.ToString();
            }

            if (int.Parse(this.textBoxSB.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
                this.textBoxSB.Text = this.smallBlind.ToString();
            }

            if (int.Parse(this.textBoxSB.Text) >= 250 && int.Parse(this.textBoxSB.Text) <= 100000)
            {
                this.smallBlind = int.Parse(this.textBoxSB.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        } //bSB_Click

        /// <summary>
        /// Provides button BigBlin click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonBigBlind_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.textBoxBB.Text.Contains(",") || this.textBoxBB.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                this.textBoxBB.Text = this.bigBlind.ToString();
                return;
            }

            if (!int.TryParse(this.textBoxSB.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                this.textBoxSB.Text = this.bigBlind.ToString();
                return;
            }

            if (int.Parse(this.textBoxBB.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                this.textBoxBB.Text = this.bigBlind.ToString();
            }

            if (int.Parse(this.textBoxBB.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }

            if (int.Parse(this.textBoxBB.Text) >= 500 && int.Parse(this.textBoxBB.Text) <= 200000)
            {
                this.bigBlind = int.Parse(this.textBoxBB.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        } // bBB_Click


        #endregion
    }

}