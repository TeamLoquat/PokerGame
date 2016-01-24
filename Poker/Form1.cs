using System.Runtime.CompilerServices;

namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Poker.Interfaces;
    using Poker.Models;

    public partial class Form1 : Form
    {
        private const int NumberOfCardsInADeck = 52;

        #region Variables
        
        private  IList<IPlayer> players;

        public IList<IPlayer> Players
        {
            get { return this.players; }
            set { this.players = value; }
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
        private IList<Type>  win;
        private IList<string>checkWinners;
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
        
        #endregion

        public Form1()
        { 
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
            
            this.players = new List<IPlayer>();
            this.players.Add(new Human(new Panel(), 10000, -1, false, false, false, "Player", new TextBox(), new Label()));

            for (int j = 0; j < 5; j++)
            {
                this.players.Add(new Bot(new Panel(), 10000, -1, false, false, false, string.Format("Bot {0}", i + 1), new TextBox(), new Label()));
            }

            updates.Start();

            InitializeComponent(this.players);
            
            this.width = this.Width;
            this.height = this.Height;

            

            Shuffle(this.players);
            foreach (var player in this.players)
            {
                player.ChipsTextBox.Enabled = false;
                player.ChipsTextBox.Text = string.Format("Chips : {0}", player.Chips);
            }


            timer.Interval = 1000;   // 1 * 1 * 1000;
            timer.Tick += TimerTick;
            updates.Interval = 100; // 1 * 1 * 100;
            updates.Tick += UpdateChipsAmountOnUI;

            buttonBB.Visible = true;
            buttonSB.Visible = true;
            textBoxBB.Visible = false;
            textBoxSB.Visible = false;
            buttonBB.Visible = false;
            buttonSB.Visible = false;
            textBoxRaise.Text = (bigBlind * 2).ToString();

           
        }

        public async Task Shuffle(IList<IPlayer> playerList)
        {
            foreach (var player in playerList)
            {
                bools.Add(player.FoldedTurn);
            }

            buttonCall.Enabled = false;
            buttonRaise.Enabled = false;
            buttonFold.Enabled = false;
            buttonCheck.Enabled = false;

            MaximizeBox = false;
            MinimizeBox = false;

            bool check = false;

            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");

            int horizontal = 580;
            int vertical = 480;

            Random random = new Random();
            for (i = imagesPathsFromDirectory.Length; i > 0; i--)
            {
                int j = random.Next(i);
                var k = imagesPathsFromDirectory[j];
                imagesPathsFromDirectory[j] = imagesPathsFromDirectory[i - 1];
                imagesPathsFromDirectory[i - 1] = k;
            }

            for (i = 0; i < 17; i++)
            {
                deckImages[i] = Image.FromFile(imagesPathsFromDirectory[i]);
                string[] charsToRemove = new string[] {"Assets\\Cards\\", ".png"};
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

                #region Throwing Cards

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

                    this.Controls.Add(playerList[0].Panel);
                    playerList[0].Panel.Location = new Point(cardsHolder[0].Left - 10, cardsHolder[0].Top - 10);
                    playerList[0].Panel.BackColor = Color.DarkBlue;
                    playerList[0].Panel.Height = 150;
                    playerList[0].Panel.Width = 180;
                    playerList[0].Panel.Visible = false;
                }

                if (this.players[1].Chips > 0)
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
                        this.Controls.Add(this.players[1].Panel);
                        this.players[1].Panel.Location = new Point(cardsHolder[2].Left - 10, cardsHolder[2].Top - 10);
                        this.players[1].Panel.BackColor = Color.DarkBlue;
                        this.players[1].Panel.Height = 150;
                        this.players[1].Panel.Width = 180;
                        this.players[1].Panel.Visible = false;

                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (this.players[2].Chips > 0)
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
                        this.Controls.Add(this.players[2].Panel);
                        this.players[2].Panel.Location = new Point(cardsHolder[4].Left - 10, cardsHolder[4].Top - 10);
                        this.players[2].Panel.BackColor = Color.DarkBlue;
                        this.players[2].Panel.Height = 150;
                        this.players[2].Panel.Width = 180;
                        this.players[2].Panel.Visible = false;

                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (this.players[3].Chips > 0)
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
                        this.Controls.Add(this.players[3].Panel);
                        this.players[3].Panel.Location = new Point(cardsHolder[6].Left - 10, cardsHolder[6].Top - 10);
                        this.players[3].Panel.BackColor = Color.DarkBlue;
                        this.players[3].Panel.Height = 150;
                        this.players[3].Panel.Width = 180;
                        this.players[3].Panel.Visible = false;
                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (this.players[4].Chips > 0)
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
                        this.Controls.Add(this.players[4].Panel);
                        this.players[4].Panel.Location = new Point(cardsHolder[8].Left - 10, cardsHolder[8].Top - 10);
                        this.players[4].Panel.BackColor = Color.DarkBlue;
                        this.players[4].Panel.Height = 150;
                        this.players[4].Panel.Width = 180;
                        this.players[4].Panel.Visible = false;
                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (this.players[5].Chips > 0)
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
                        this.Controls.Add(this.players[5].Panel);
                        this.players[5].Panel.Location = new Point(cardsHolder[10].Left - 10, cardsHolder[10].Top - 10);
                        this.players[5].Panel.BackColor = Color.DarkBlue;
                        this.players[5].Panel.Height = 150;
                        this.players[5].Panel.Width = 180;
                        this.players[5].Panel.Visible = false;
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

                if (this.players[1].Chips <= 0)
                {
                    this.players[1].FoldedTurn = true;
                    cardsHolder[2].Visible = false;
                    cardsHolder[3].Visible = false;
                }
                else
                {
                    this.players[1].FoldedTurn = false;
                    if (i == 3)
                    {
                        if (cardsHolder[3] != null)
                        {
                            cardsHolder[2].Visible = true;
                            cardsHolder[3].Visible = true;
                        }
                    }
                }

                if (this.players[2].Chips <= 0)
                {
                    this.players[2].FoldedTurn = true;
                    cardsHolder[4].Visible = false;
                    cardsHolder[5].Visible = false;
                }
                else
                {
                    this.players[2].FoldedTurn = false;
                    if (i == 5)
                    {
                        if (cardsHolder[5] != null)
                        {
                            cardsHolder[4].Visible = true;
                            cardsHolder[5].Visible = true;
                        }
                    }
                }

                if (this.players[3].Chips <= 0)
                {
                    this.players[3].FoldedTurn = true;
                    cardsHolder[6].Visible = false;
                    cardsHolder[7].Visible = false;
                }
                else
                {
                    this.players[3].FoldedTurn = false;
                    if (i == 7)
                    {
                        if (cardsHolder[7] != null)
                        {
                            cardsHolder[6].Visible = true;
                            cardsHolder[7].Visible = true;
                        }
                    }
                }

                if (this.players[4].Chips <= 0)
                {
                    this.players[4].FoldedTurn = true;
                    cardsHolder[8].Visible = false;
                    cardsHolder[9].Visible = false;
                }
                else
                {
                    this.players[4].FoldedTurn = false;
                    if (i == 9)
                    {
                        if (cardsHolder[9] != null)
                        {
                            cardsHolder[8].Visible = true;
                            cardsHolder[9].Visible = true;
                        }
                    }
                }

                if (this.players[5].Chips <= 0)
                {
                    this.players[5].FoldedTurn = true;
                    cardsHolder[10].Visible = false;
                    cardsHolder[11].Visible = false;
                }
                else
                {
                    this.players[5].FoldedTurn = false;
                    if (i == 11)
                    {
                        if (cardsHolder[11] != null)
                        {
                            cardsHolder[10].Visible = true;
                            cardsHolder[11].Visible = true;
                        }
                    }
                }

                if (i == 16)
                {
                    if (!restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }

                    timer.Start();
                }
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

            if (i == 17)
            {
                buttonRaise.Enabled = true;
                buttonCall.Enabled = true;
                buttonRaise.Enabled = true;
                buttonRaise.Enabled = true;
                buttonFold.Enabled = true;
            }

        }

        public async Task Turns()
        {
            #region Rotating
            if (!this.players[0].FoldedTurn)
            {
                if (this.players[0].Turn)
                {
                    FixCall(this.players[0], 1);

                    MessageBox.Show("Player's Turn");
                    progressBarTimer.Visible = true;
                    progressBarTimer.Value = 1000;
                    time = 60;
                    up = 10000000;
                    timer.Start();
                    buttonRaise.Enabled = true;
                    buttonCall.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    turnCount++;

                    FixCall(this.players[0], 2);
                }
            }

            if (this.players[0].FoldedTurn || !this.players[0].Turn)
            {
                await AllIn();
                if (this.players[0].FoldedTurn && !this.players[0].HasFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        this.players[0].HasFolded = true;
                    }
                }

                await CheckRaise(0, 0);
                progressBarTimer.Visible = false;
                buttonRaise.Enabled = false;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                timer.Stop();
                this.players[1].Turn = true;
                if (!this.players[1].FoldedTurn)
                {
                    if (this.players[1].Turn)
                    {
                        FixCall(this.players[1], 1);
                        FixCall(this.players[1], 2);
                        Rules(2, 3, this.players[1]);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, this.players[1]);
                        turnCount++;
                        last = 1;
                        this.players[1].Turn = false;
                        this.players[2].Turn = true;
                    }
                }

                if (this.players[1].FoldedTurn && !this.players[1].HasFolded)
                {
                    bools.RemoveAt(1);
                    bools.Insert(1, null);
                    maxLeft--;
                    this.players[1].HasFolded = true;
                }

                if (this.players[1].FoldedTurn || !this.players[1].Turn)
                {
                    await CheckRaise(1, 1);
                    this.players[2].Turn = true;
                }

                if (!this.players[2].FoldedTurn)
                {
                    if (this.players[2].Turn)
                    {
                        FixCall(this.players[2], 1);
                        FixCall(this.players[2], 2);
                        Rules(4, 5, this.players[2]);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, this.players[2]);
                        turnCount++;
                        last = 2;
                        this.players[2].Turn = false;
                        this.players[3].Turn = true;
                    }
                }

                if (this.players[2].FoldedTurn && !this.players[2].HasFolded)
                {
                    bools.RemoveAt(2);
                    bools.Insert(2, null);
                    maxLeft--;
                    this.players[2].HasFolded = true;
                }

                if (this.players[2].FoldedTurn || !this.players[2].Turn)
                {
                    await CheckRaise(2, 2);
                    this.players[3].Turn = true;
                }

                if (!this.players[3].FoldedTurn)
                {
                    if (this.players[3].Turn)
                    {
                        FixCall(this.players[3], 1);
                        FixCall(this.players[3], 2);
                        Rules(6, 7, this.players[3]);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, this.players[3]);
                        turnCount++;
                        last = 3;
                        this.players[3].Turn = false;
                        this.players[4].Turn = true;
                    }
                }

                if (this.players[3].FoldedTurn && !this.players[3].HasFolded)
                {
                    bools.RemoveAt(3);
                    bools.Insert(3, null);
                    maxLeft--;
                    this.players[3].HasFolded = true;
                }

                if (this.players[3].FoldedTurn || !this.players[3].Turn)
                {
                    await CheckRaise(3, 3);
                    this.players[4].Turn = true;
                }

                if (!this.players[4].FoldedTurn)
                {
                    if (this.players[4].Turn)
                    {
                        FixCall(this.players[4], 1);
                        FixCall(this.players[4], 2);
                        Rules(8, 9, this.players[4]);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, this.players[4]);
                        turnCount++;
                        last = 4;
                        this.players[4].Turn = false;
                        this.players[5].Turn = true;
                    }
                }

                if (this.players[4].FoldedTurn && !this.players[4].HasFolded)
                {
                    bools.RemoveAt(4);
                    bools.Insert(4, null);
                    maxLeft--;
                    this.players[4].HasFolded = true;
                }

                if (this.players[4].FoldedTurn || !this.players[4].Turn)
                {
                    await CheckRaise(4, 4);
                    this.players[5].Turn = true;
                }

                if (!this.players[5].FoldedTurn)
                {
                    if (this.players[5].Turn)
                    {
                        FixCall(this.players[5], 1);
                        FixCall(this.players[5], 2);
                        Rules(10, 11, this.players[5]);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, this.players[5]);
                        turnCount++;
                        last = 5;
                        this.players[5].Turn = false;
                    }
                }

                if (this.players[5].FoldedTurn && !this.players[5].HasFolded)
                {
                    bools.RemoveAt(5);
                    bools.Insert(5, null);
                    maxLeft--;
                    this.players[5].HasFolded = true;
                }

                if (this.players[5].FoldedTurn || !this.players[5].Turn)
                {
                    await CheckRaise(5, 5);
                    this.players[0].Turn = true;
                }

                if (this.players[0].FoldedTurn && !this.players[0].HasFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        this.players[0].HasFolded = true;
                    }
                }
                #endregion
                await AllIn();
                if (!restart)
                {
                    await Turns();
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
                    if (straight[j]/4 == straight[j + 1]/4 && straight[j]/4 == straight[j + 2]/4 &&
                        straight[j]/4 == straight[j + 3]/4)
                    {
                        player.Type = 7;
                        player.Power = ((straight[j]/4)*4) + (player.Type*100);
                    }

                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        player.Type = 7;
                        player.Power = (13 * 4) + (player.Type * 100);
                    }

                    win.Add(new Type() { Power = player.Power, Current = 7 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            player.Power= reserve[i] + (player.Type * 100);
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
                            player.Type = 3;
                            player.Power = 13 * 3 + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 3 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            player.Type = 3;
                            player.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 3 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
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

                if (reserve[i]/4 == reserve[i + 1]/4)
                {
                    if (!msgbox)
                    {
                        if (reserve[i]/4 == 0)
                        {
                            player.Type = 1;
                            player.Power = 13*4 + player.Type*100;
                        }
                        else
                        {
                            player.Type = 1;
                            player.Power = (reserve[i + 1]/4)*4 + player.Type*100;
                        }

                        win.Add(new Type() {Power = player.Power, Current = 1});
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
                if (reserve[i]/4 > reserve[i + 1]/4)
                {
                    player.Type = -1;
                    player.Power = reserve[i]/4;
                }
                else
                {
                    player.Type = -1;
                    player.Power = reserve[i + 1]/4;
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

        public void Winner(IPlayer player, string lastly)
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
                        this.players[0].ChipsTextBox.Text = player.Chips.ToString();

                        
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

        public async Task CheckRaise(int currentTurn, int raiseTurn)

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
                        foreach (var player in this.players)
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
                        foreach (var player in this.players)
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
                        foreach (var player in this.players)
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
                        for (int k = 0; k < this.players.Count; k++)
                        {
                            this.players[k].Call = 0;
                            this.players[k].Raise= 0;
                        }
                    }
                }
            }

            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = null;
                int count = 0;
                for (int k = 0; k < this.players.Count; k++)
                {
                    if (!this.players[k].StatusLabel.Text.Contains("Fold"))
                    {
                        fixedLast = this.players[k].Name;
                        Rules(count,count+1,this.players[k]);
                    }
                    count+=2;
                }
                for (int k = 0; k < this.players.Count; k++)
                {
                    Winner(this.players[k],fixedLast);
                }

                restart = true;
                this.players[0].Turn = true;
                foreach (var player in this.players)
                {
                    player.FoldedTurn = false;
                }

                if (this.players[0].Chips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        foreach (var player in this.players)
                        {
                            player.Chips += f2.a;
                        }
                        this.players[0].FoldedTurn = false;
                        this.players[0].Turn = true;
                        buttonRaise.Enabled = true;
                        buttonFold.Enabled = true;
                        buttonCheck.Enabled = true;
                        buttonRaise.Text = "Raise";
                    }
                }

                foreach (var player in this.players)
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

                foreach (var player in this.players)
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
                this.players[0].StatusLabel.Text = string.Empty;
                await Shuffle(this.players);
                await Turns();
            }
        }

        public void FixCall(IPlayer player, int options)
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
                        buttonCall.Enabled = false;
                        buttonCall.Text = "Call is fuckedup";
                    }
                }
            }
        }

        public async Task AllIn()
        {
            #region All in
            if (this.players[0].Chips <= 0 && !intsadded)
            {
                if ((this.players[0].StatusLabel.Text.Contains("Raise"))|| (this.players[0].StatusLabel.Text.Contains("Call")))
                {
                    ints.Add(this.players[0].Chips);
                    intsadded = true;
                }
            }

            intsadded = false;
            for (int j = 1; j < this.players.Count; j++)
            {
                if (this.players[j].Chips <= 0 && !this.players[j].FoldedTurn)
                {
                    if (!intsadded)
                    {
                        ints.Add(this.players[j].Chips);
                        intsadded = true;
                    }

                    intsadded = false;
                }
            }
            

            if (ints.ToArray().Length == maxLeft)
            {
                await Finish(2);
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
                    this.players[0].Chips += int.Parse(textBoxPot.Text);
                    this.players[0].ChipsTextBox.Text = this.players[0].Chips.ToString();
                    this.players[0].Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }

                if (index == 1)
                {
                    this.players[1].Chips += int.Parse(textBoxPot.Text);
                    this.players[0].ChipsTextBox.Text = this.players[1].Chips.ToString();
                    this.players[1].Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }

                if (index == 2)
                {
                    this.players[2].Chips += int.Parse(textBoxPot.Text);
                    this.players[0].ChipsTextBox.Text = this.players[2].Chips.ToString();
                    this.players[2].Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }

                if (index == 3)
                {
                    this.players[3].Chips += int.Parse(textBoxPot.Text);
                    this.players[0].ChipsTextBox.Text = this.players[3].Chips.ToString();
                    this.players[3].Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }

                if (index == 4)
                {
                    this.players[4].Chips += int.Parse(textBoxPot.Text);
                    this.players[0].ChipsTextBox.Text = this.players[4].Chips.ToString();
                    this.players[4].Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }

                if (index == 5)
                {
                    this.players[5].Chips += int.Parse(textBoxPot.Text);
                    this.players[0].ChipsTextBox.Text = this.players[5].Chips.ToString();
                    this.players[5].Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }

                for (int j = 0; j <= 16; j++)
                {
                    cardsHolder[j].Visible = false;
                }

                await Finish(1);
            }

            intsadded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= End)
            {
                await Finish(2);
            }
            #endregion

        }

        public async Task Finish(int n)
        {
            if (n == 2)
            {
                FixWinners();
            }

            foreach (var player in this.players)
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
            this.players[0].Turn = true;

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
            if (this.players[0].Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    this.players[0].Chips = f2.a;
                    this.players[1].Chips += f2.a;
                    this.players[2].Chips += f2.a;
                    this.players[3].Chips += f2.a;
                    this.players[4].Chips += f2.a;
                    this.players[5].Chips += f2.a;
                    this.players[0].FoldedTurn = false;
                    this.players[0].Turn = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    buttonCheck.Enabled = true;
                    buttonRaise.Text = "Raise";
                }
            }

            imagesPathsFromDirectory = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                cardsHolder[os].Image = null;
                cardsHolder[os].Invalidate();
                cardsHolder[os].Visible = false;
            }

            await Shuffle(this.players);

            //await Turns();
        }

        public void FixWinners()
        {
            win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";
            int count = 0;
            for (int j = 0; j < this.players.Count; j++)
            {
                if (this.players[j].StatusLabel.Text.Contains("Fold"))
                {
                    fixedLast = this.players[j].Name;
                    Rules(count,count+1,this.players[j]);
                }
                count += 2;

            }
            foreach (var player in this.players)
            {
                Winner(player, fixedLast);
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
                this.players[0].FoldedTurn = true;
                await Turns();
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

            for(int j = 0; j< this.players.Count; j++)
            {
                this.players[j].ChipsTextBox.Text = string.Format("Chips : {0} ", this.players[j].Chips);
            }

            if (this.players[0].Chips <= 0)
            {
                this.players[0].Turn = false;
                this.players[0].FoldedTurn = true;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                buttonCheck.Enabled = false;
            }

            if (up > 0)
            {
                up--;
            }

            if (this.players[0].Chips >= call)
            {
                buttonCall.Text = "Call " + call.ToString();
            }
            else
            {
                buttonCall.Text = "All in";
                buttonRaise.Enabled = false;
            }

            if (call > 0)
            {
                buttonCheck.Enabled = false;
            }

            if (call <= 0)
            {
                buttonCheck.Enabled = true;
                buttonCall.Text = "Call";
                buttonCall.Enabled = false;
            }

            if (this.players[0].Chips <= 0)
            {
                buttonRaise.Enabled = false;
            }

            int parsedValue;

            if (textBoxRaise.Text != string.Empty && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (this.players[0].Chips <= int.Parse(textBoxRaise.Text))
                {
                    buttonRaise.Text = "All in";
                }
                else
                {
                    buttonRaise.Text = "Raise";
                }
            }

            if (this.players[0].Chips < call)
            {
                buttonRaise.Enabled = false;
            }
        }

        /// <summary>
        /// Provides button Fold click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonFoldClick(object sender, EventArgs e)
        {
            this.players[0].StatusLabel.Text = "Fold";
            this.players[0].Turn = false;
            this.players[0].FoldedTurn = true;
            await Turns();
        }  // bFold_Click

        /// <summary>
        /// Provides button Check click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonCheckClick(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                this.players[0].Turn = false;
                this.players[0].StatusLabel.Text = "Check";
            }
            else
            {
                ////pStatus.Text = "All in " + Chips;
                buttonCheck.Enabled = false;
            }

            await Turns();
        } // bCheck_Click

        /// <summary>
        /// Provides button Call click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonCallClick(object sender, EventArgs e)
        {
            Rules(0, 1, this.players[0]);
            if (this.players[0].Chips >= call)
            {
                this.players[0].Chips -= call;
                this.players[0].ChipsTextBox.Text = string.Format("Chips : {0}", this.players[0].Chips);
                if (textBoxPot.Text != string.Empty)
                {
                    textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
                }
                else
                {
                    textBoxPot.Text = call.ToString();
                }

                this.players[0].Turn = false;
                this.players[0].StatusLabel.Text = "Call " + call;
                this.players[0].Call = call;
            }
            else if (this.players[0].Chips <= call && call > 0)
            {
                textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.players[0].Chips).ToString();
                this.players[0].StatusLabel.Text = "All in " + this.players[0].Chips;
                this.players[0].Chips = 0;
                this.players[0].ChipsTextBox.Text = "Chips : " + this.players[0].Chips.ToString();
                this.players[0].Turn = false;
                buttonFold.Enabled = false;
                this.players[0].Call = this.players[0].Chips;
            }

            await Turns();
        } // bCall_Click

        /// <summary>
        /// Provides button Rall click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonRaiseClick(object sender, EventArgs e)
        {
            Rules(0, 1, this.players[0]);
            int parsedValue;
            if (textBoxRaise.Text != string.Empty && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (this.players[0].Chips > call)
                {
                    if (Raise * 2 > int.Parse(textBoxRaise.Text))
                    {
                        textBoxRaise.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (this.players[0].Chips >= int.Parse(textBoxRaise.Text))
                        {
                            call = int.Parse(textBoxRaise.Text);
                            Raise = int.Parse(textBoxRaise.Text);
                            this.players[0].StatusLabel.Text = "Raise " + call.ToString();
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
                            buttonCall.Text = "Call";
                            this.players[0].Chips -= int.Parse(textBoxRaise.Text);
                            raising = true;
                            last = 0;
                            this.players[0].Raise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            call = this.players[0].Chips;
                            Raise = this.players[0].Chips;
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.players[0].Chips).ToString();
                            this.players[0].StatusLabel.Text = "Raise " + call.ToString();
                            this.players[0].Chips = 0;
                            raising = true;
                            last = 0;
                            this.players[0].Raise = Convert.ToInt32(Raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }

            this.players[0].Turn = false;
            await Turns();
        } // bRaise_Click

        /// <summary>
        /// Provides button Add click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonAddClick(object sender, EventArgs e)
        {
            if (textBoxAdd.Text != string.Empty)
            {
                for (int j = 0; j < this.players.Count; j++)
                {
                    this.players[i].Chips += int.Parse(textBoxAdd.Text);
                }
            }

            this.players[0].ChipsTextBox.Text = "Chips : " + this.players[0].Chips.ToString();
        } // bAdd_Click

        /// <summary>
        /// Provides button SB/BB click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSBBBOptionClick(object sender, EventArgs e)
        {
            this.textBoxBB.Text = this.bigBlind.ToString();
            this.textBoxSB.Text = this.smallBlind.ToString();

            if (this.textBoxBB.Visible == false)
            {
                this.textBoxBB.Visible = true;
                this.textBoxSB.Visible = true;
                this.buttonBB.Visible = true;
                this.buttonSB.Visible = true;
            }
            else
            {
                this.textBoxBB.Visible = false;
                this.textBoxSB.Visible = false;
                this.buttonBB.Visible = false;
                this.buttonSB.Visible = false;
            }
        } // bOption_Click


        /// <summary>
        /// Provides button SmallBlind click functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSmallBlindClick(object sender, EventArgs e)
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
        public void ButtonBigBlincdClick(object sender, EventArgs e)
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

        public void Layout_Change(object sender, LayoutEventArgs e)
        {
            this.width = this.Width;
            this.height = this.Height;
        }
        #endregion
    } 

}