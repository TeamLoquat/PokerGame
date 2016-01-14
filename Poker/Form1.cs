using System.Runtime.CompilerServices;

namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        private const int NumberOfCardsInADeck = 52;

        #region Variables
     // private ProgressBar progressBar = new ProgressBar(); - never used
     // private int Nm; - never used
        private readonly Panel playerPanel = new Panel();
        private readonly Panel bot1Panel = new Panel();
        private readonly Panel bot2Panel = new Panel();
        private readonly Panel bot3Panel = new Panel();
        private readonly Panel bot4Panel = new Panel();
        private readonly Panel bot5Panel = new Panel();
        private int call = 500; 
        private int foldedPlayers = 5;
        private int playerChips = 10000;
        private int bot1Chips = 10000;
        private int bot2Chips = 10000;
        private int bot3Chips = 10000;
        private int bot4Chips = 10000;
        private int bot5Chips = 10000;
        private double type;
        private double rounds;
        private double bot1Power;
        private double bot2Power;
        private double bot3Power;
        private double bot4Power;
        private double bot5Power;
        private double pPower;
        private double pType = -1;
        private double Raise;
        private double b1Type = -1;
        private double b2Type = -1;
        private double b3Type = -1;
        private double b4Type = -1;
        private double b5Type = -1;
        private bool bot1Turn;
        private bool bot2Turn;
        private bool bot3Turn;
        private bool bot4Turn;
        private bool bot5Turn;
        private bool B1Fturn;
        private bool B2Fturn;
        private bool B3Fturn;
        private bool B4Fturn;
        private bool B5Fturn;
        private bool playerHasFolded;
        private bool bot1HasFolded;
        private bool bot2HasFolded;
        private bool bot3HasFolded;
        private bool bot4HasFolded;
        private bool bot5HasFolded;
        private bool intsadded;
        private bool changed;
        private int playerCall;
        private int b1Call;
        private int b2Call;
        private int b3Call;
        private int b4Call;
        private int b5Call;
        private int pRaise;
        private int b1Raise;
        private int b2Raise;
        private int b3Raise;
        private int b4Raise;
        private int b5Raise;
        private int height; 
        private int width;
        private int winners;
        private int Flop = 1;
        private int Turn = 2;
        private int River = 3;
        private int End = 4;
        private int maxLeft = 6;
        private int last; // original value 123; - never used
        private int raisedTurn = 1;
        private List<bool?> bools = new List<bool?>();
        private List<Type> win = new List<Type>();
        private List<string> CheckWinners = new List<string>();
        private List<int> ints = new List<int>();
        private bool PFturn = false;
        private bool Pturn = true;
        private bool restart = false;
        private bool raising = false;
        private Poker.Type sorted;
        private string[] imagesPathsFromDirectory = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        /*string[] ImgLocation ={
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/
        private int[] reserve = new int[17];
        private Image[] deckImages = new Image[NumberOfCardsInADeck];
        private PictureBox[] holder = new PictureBox[52];
        private Timer timer = new Timer();
        private Timer updates = new Timer();
        private int time = 60;
        private int i;
        private int bigBlind = 500;
        private int smallBlind = 250;
        private int up = 10000000;
        private int turnCount;
        #endregion

        public Form1()
        {
            //bools.Add(PFturn); bools.Add(B1Fturn); bools.Add(B2Fturn); bools.Add(B3Fturn); bools.Add(B4Fturn); bools.Add(B5Fturn);
            //call = bigBlind;
            MaximizeBox = false;
            MinimizeBox = false;
            updates.Start();
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            Shuffle();
        //  tbPot.Enabled = false;
            textBoxPlayerChips.Enabled = false;
            textBoxBot1Chips.Enabled = false;
            textBoxBot2Chips.Enabled = false;
            textBoxBot3Chips.Enabled = false;
            textBoxBot4Chips.Enabled = false;
            textBoxBot5Chips.Enabled = false;
            textBoxPlayerChips.Text = "Chips : " + playerChips.ToString();
            textBoxBot1Chips.Text = "Chips : " + bot1Chips.ToString();
            textBoxBot2Chips.Text = "Chips : " + bot2Chips.ToString();
            textBoxBot3Chips.Text = "Chips : " + bot3Chips.ToString();
            textBoxBot4Chips.Text = "Chips : " + bot4Chips.ToString();
            textBoxBot5Chips.Text = "Chips : " + bot5Chips.ToString();
            timer.Interval = 1000;   // 1 * 1 * 1000;
            timer.Tick += TimerTick;
            updates.Interval = 100; // 1 * 1 * 100;
            updates.Tick += UpdateChipsAmountOnUI;
            //textBoxBB.Visible = true; -redundant
            //textBoxSB.Visible = true; -redundant
            //bu.Visible = true; -redundant
            //buttonSB.Visible = true; -redundant
            //textBoxBB.Visible = true; -redundant
            //textBoxSB.Visible = true -redundant
            buttonBB.Visible = true;
            buttonSB.Visible = true;
            textBoxBB.Visible = false;
            textBoxSB.Visible = false;
            buttonBB.Visible = false;
            buttonSB.Visible = false;
            textBoxRaise.Text = (bigBlind * 2).ToString();
        }

        public async Task Shuffle()
        {
            bools.Add(PFturn); 
            bools.Add(B1Fturn); 
            bools.Add(B2Fturn); 
            bools.Add(B3Fturn); 
            bools.Add(B4Fturn); 
            bools.Add(B5Fturn);
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
                string[] charsToRemove = new string[] { "Assets\\Cards\\", ".png" };
                foreach (string str in charsToRemove)
                {
                    imagesPathsFromDirectory[i] = imagesPathsFromDirectory[i].Replace(str, string.Empty);
                }

                reserve[i] = int.Parse(imagesPathsFromDirectory[i]) - 1;
                holder[i] = new PictureBox();
                holder[i].SizeMode = PictureBoxSizeMode.StretchImage;
                holder[i].Height = 130;
                holder[i].Width = 80;
                this.Controls.Add(holder[i]);
                holder[i].Name = "pb" + i.ToString();
                await Task.Delay(200);
                #region Throwing Cards
                if (i < 2)
                {
                    if (holder[0].Tag != null)
                    {
                        holder[1].Tag = reserve[1];
                    }

                    holder[0].Tag = reserve[0];
                    holder[i].Image = deckImages[i];
                    holder[i].Anchor = AnchorStyles.Bottom;

                    //Holder[i].Dock = DockStyle.Top;
                    holder[i].Location = new Point(horizontal, vertical);
                    horizontal += holder[i].Width;
                    this.Controls.Add(playerPanel);
                    playerPanel.Location = new Point(holder[0].Left - 10, holder[0].Top - 10);
                    playerPanel.BackColor = Color.DarkBlue;
                    playerPanel.Height = 150;
                    playerPanel.Width = 180;
                    playerPanel.Visible = false;
                }

                if (bot1Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 2 && i < 4)
                    {
                        if (holder[2].Tag != null)
                        {
                            holder[3].Tag = reserve[3];
                        }

                        holder[2].Tag = reserve[2];
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        holder[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                        holder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        holder[i].Location = new Point(horizontal, vertical);
                        horizontal += holder[i].Width;
                        holder[i].Visible = true;
                        this.Controls.Add(bot1Panel);
                        bot1Panel.Location = new Point(holder[2].Left - 10, holder[2].Top - 10);
                        bot1Panel.BackColor = Color.DarkBlue;
                        bot1Panel.Height = 150;
                        bot1Panel.Width = 180;
                        bot1Panel.Visible = false;
                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (bot2Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 4 && i < 6)
                    {
                        if (holder[4].Tag != null)
                        {
                            holder[5].Tag = reserve[5];
                        }

                        holder[4].Tag = reserve[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }

                        check = true;
                        holder[i].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        holder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        holder[i].Location = new Point(horizontal, vertical);
                        horizontal += holder[i].Width;
                        holder[i].Visible = true;
                        this.Controls.Add(bot2Panel);
                        bot2Panel.Location = new Point(holder[4].Left - 10, holder[4].Top - 10);
                        bot2Panel.BackColor = Color.DarkBlue;
                        bot2Panel.Height = 150;
                        bot2Panel.Width = 180;
                        bot2Panel.Visible = false;

                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (bot3Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 6 && i < 8)
                    {
                        if (holder[6].Tag != null)
                        {
                            holder[7].Tag = reserve[7];
                        }

                        holder[6].Tag = reserve[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;
                        holder[i].Anchor = AnchorStyles.Top;
                        holder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        holder[i].Location = new Point(horizontal, vertical);
                        horizontal += holder[i].Width;
                        holder[i].Visible = true;
                        this.Controls.Add(bot3Panel);
                        bot3Panel.Location = new Point(holder[6].Left - 10, holder[6].Top - 10);
                        bot3Panel.BackColor = Color.DarkBlue;
                        bot3Panel.Height = 150;
                        bot3Panel.Width = 180;
                        bot3Panel.Visible = false;
                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (bot4Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 8 && i < 10)
                    {
                        if (holder[8].Tag != null)
                        {
                            holder[9].Tag = reserve[9];
                        }

                        holder[8].Tag = reserve[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }

                        check = true;
                        holder[i].Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        holder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        holder[i].Location = new Point(horizontal, vertical);
                        horizontal += holder[i].Width;
                        holder[i].Visible = true;
                        this.Controls.Add(bot4Panel);
                        bot4Panel.Location = new Point(holder[8].Left - 10, holder[8].Top - 10);
                        bot4Panel.BackColor = Color.DarkBlue;
                        bot4Panel.Height = 150;
                        bot4Panel.Width = 180;
                        bot4Panel.Visible = false;
                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (bot5Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 10 && i < 12)
                    {
                        if (holder[10].Tag != null)
                        {
                            holder[11].Tag = reserve[11];
                        }

                        holder[10].Tag = reserve[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }

                        check = true;
                        holder[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        holder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        holder[i].Location = new Point(horizontal, vertical);
                        horizontal += holder[i].Width;
                        holder[i].Visible = true;
                        this.Controls.Add(bot5Panel);
                        bot5Panel.Location = new Point(holder[10].Left - 10, holder[10].Top - 10);
                        bot5Panel.BackColor = Color.DarkBlue;
                        bot5Panel.Height = 150;
                        bot5Panel.Width = 180;
                        bot5Panel.Visible = false;
                        if (i == 11)
                        {
                            check = false;
                        }
                    }
                }

                if (i >= 12)
                {
                    holder[12].Tag = reserve[12];
                    if (i > 12)
                    {
                        holder[13].Tag = reserve[13];
                    }

                    if (i > 13)
                    {
                        holder[14].Tag = reserve[14];
                    }

                    if (i > 14)
                    {
                        holder[15].Tag = reserve[15];
                    }

                    if (i > 15)
                    {
                        holder[16].Tag = reserve[16];
                    }

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (holder[i] != null)
                    {
                        holder[i].Anchor = AnchorStyles.None;
                        holder[i].Image = backImage;

                        //Holder[i].Image = Deck[i];
                        holder[i].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }
                #endregion

                if (bot1Chips <= 0)
                {
                    B1Fturn = true;
                    holder[2].Visible = false;
                    holder[3].Visible = false;
                }
                else
                {
                    B1Fturn = false;
                    if (i == 3)
                    {
                        if (holder[3] != null)
                        {
                            holder[2].Visible = true;
                            holder[3].Visible = true;
                        }
                    }
                }

                if (bot2Chips <= 0)
                {
                    B2Fturn = true;
                    holder[4].Visible = false;
                    holder[5].Visible = false;
                }
                else
                {
                    B2Fturn = false;
                    if (i == 5)
                    {
                        if (holder[5] != null)
                        {
                            holder[4].Visible = true;
                            holder[5].Visible = true;
                        }
                    }
                }

                if (bot3Chips <= 0)
                {
                    B3Fturn = true;
                    holder[6].Visible = false;
                    holder[7].Visible = false;
                }
                else
                {
                    B3Fturn = false;
                    if (i == 7)
                    {
                        if (holder[7] != null)
                        {
                            holder[6].Visible = true;
                            holder[7].Visible = true;
                        }
                    }
                }

                if (bot4Chips <= 0)
                {
                    B4Fturn = true;
                    holder[8].Visible = false;
                    holder[9].Visible = false;
                }
                else
                {
                    B4Fturn = false;
                    if (i == 9)
                    {
                        if (holder[9] != null)
                        {
                            holder[8].Visible = true;
                            holder[9].Visible = true;
                        }
                    }
                }

                if (bot5Chips <= 0)
                {
                    B5Fturn = true;
                    holder[10].Visible = false;
                    holder[11].Visible = false;
                }
                else
                {
                    B5Fturn = false;
                    if (i == 11)
                    {
                        if (holder[11] != null)
                        {
                            holder[10].Visible = true;
                            holder[11].Visible = true;
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
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
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
            if (!PFturn)
            {
                if (Pturn)
                {
                    FixCall(playerStatus, ref playerCall, ref pRaise, 1);

                    //MessageBox.Show("Player's Turn");
                    progressBartTimer.Visible = true;
                    progressBartTimer.Value = 1000;
                    time = 60;
                    up = 10000000;
                    timer.Start();
                    buttonRaise.Enabled = true;
                    buttonCall.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    turnCount++;
                    FixCall(playerStatus, ref playerCall, ref pRaise, 2);
                }
            }

            if (PFturn || !Pturn)
            {
                await AllIn();
                if (PFturn && !playerHasFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        playerHasFolded = true;
                    }
                }

                await CheckRaise(0, 0);
                progressBartTimer.Visible = false;
                buttonRaise.Enabled = false;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                timer.Stop();
                bot1Turn = true;
                if (!B1Fturn)
                {
                    if (bot1Turn)
                    {
                        FixCall(bot1Status, ref b1Call, ref b1Raise, 1);
                        FixCall(bot1Status, ref b1Call, ref b1Raise, 2);
                        Rules(2, 3, "Bot 1", ref b1Type, ref bot1Power, B1Fturn);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, ref bot1Chips, ref bot1Turn, ref  B1Fturn, bot1Status, 0, bot1Power, b1Type);
                        turnCount++;
                        last = 1;
                        bot1Turn = false;
                        bot2Turn = true;
                    }
                }

                if (B1Fturn && !bot1HasFolded)
                {
                    bools.RemoveAt(1);
                    bools.Insert(1, null);
                    maxLeft--;
                    bot1HasFolded = true;
                }

                if (B1Fturn || !bot1Turn)
                {
                    await CheckRaise(1, 1);
                    bot2Turn = true;
                }

                if (!B2Fturn)
                {
                    if (bot2Turn)
                    {
                        FixCall(b2Status, ref b2Call, ref b2Raise, 1);
                        FixCall(b2Status, ref b2Call, ref b2Raise, 2);
                        Rules(4, 5, "Bot 2", ref b2Type, ref bot2Power, B2Fturn);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, ref bot2Chips, ref bot2Turn, ref  B2Fturn, b2Status, 1, bot2Power, b2Type);
                        turnCount++;
                        last = 2;
                        bot2Turn = false;
                        bot3Turn = true;
                    }
                }

                if (B2Fturn && !bot2HasFolded)
                {
                    bools.RemoveAt(2);
                    bools.Insert(2, null);
                    maxLeft--;
                    bot2HasFolded = true;
                }

                if (B2Fturn || !bot2Turn)
                {
                    await CheckRaise(2, 2);
                    bot3Turn = true;
                }

                if (!B3Fturn)
                {
                    if (bot3Turn)
                    {
                        FixCall(b3Status, ref b3Call, ref b3Raise, 1);
                        FixCall(b3Status, ref b3Call, ref b3Raise, 2);
                        Rules(6, 7, "Bot 3", ref b3Type, ref bot3Power, B3Fturn);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, ref bot3Chips, ref bot3Turn, ref  B3Fturn, b3Status, 2, bot3Power, b3Type);
                        turnCount++;
                        last = 3;
                        bot3Turn = false;
                        bot4Turn = true;
                    }
                }

                if (B3Fturn && !bot3HasFolded)
                {
                    bools.RemoveAt(3);
                    bools.Insert(3, null);
                    maxLeft--;
                    bot3HasFolded = true;
                }

                if (B3Fturn || !bot3Turn)
                {
                    await CheckRaise(3, 3);
                    bot4Turn = true;
                }

                if (!B4Fturn)
                {
                    if (bot4Turn)
                    {
                        FixCall(b4Status, ref b4Call, ref b4Raise, 1);
                        FixCall(b4Status, ref b4Call, ref b4Raise, 2);
                        Rules(8, 9, "Bot 4", ref b4Type, ref bot4Power, B4Fturn);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, ref bot4Chips, ref bot4Turn, ref  B4Fturn, b4Status, 3, bot4Power, b4Type);
                        turnCount++;
                        last = 4;
                        bot4Turn = false;
                        bot5Turn = true;
                    }
                }

                if (B4Fturn && !bot4HasFolded)
                {
                    bools.RemoveAt(4);
                    bools.Insert(4, null);
                    maxLeft--;
                    bot4HasFolded = true;
                }

                if (B4Fturn || !bot4Turn)
                {
                    await CheckRaise(4, 4);
                    bot5Turn = true;
                }

                if (!B5Fturn)
                {
                    if (bot5Turn)
                    {
                        FixCall(b5Status, ref b5Call, ref b5Raise, 1);
                        FixCall(b5Status, ref b5Call, ref b5Raise, 2);
                        Rules(10, 11, "Bot 5", ref b5Type, ref bot5Power, B5Fturn);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, ref bot5Chips, ref bot5Turn, ref  B5Fturn, b5Status, 4, bot5Power, b5Type);
                        turnCount++;
                        last = 5;
                        bot5Turn = false;
                    }
                }

                if (B5Fturn && !bot5HasFolded)
                {
                    bools.RemoveAt(5);
                    bools.Insert(5, null);
                    maxLeft--;
                    bot5HasFolded = true;
                }

                if (B5Fturn || !bot5Turn)
                {
                    await CheckRaise(5, 5);
                    Pturn = true;
                }

                if (PFturn && !playerHasFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        playerHasFolded = true;
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

        public void Rules(int c1, int c2, string currentText, ref double current, ref double power, bool foldedTurn)
        {
            if (c1 == 0 && c2 == 1)
            {
            }

            if (!foldedTurn || c1 == 0 && c2 == 1 && playerStatus.Text.Contains("Fold") == false)
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
                    if (reserve[i] == int.Parse(holder[c1].Tag.ToString()) && reserve[i + 1] == int.Parse(holder[c2].Tag.ToString()))
                    {
                        //Pair from Hand current = 1
                        rPairFromHand(ref current, ref power);

                        #region Pair or Two Pair from Table current = 2 || 0
                        rPairTwoPair(ref current, ref power);
                        #endregion

                        #region Two Pair current = 2
                        rTwoPair(ref current, ref power);
                        #endregion

                        #region Three of a kind current = 3
                        rThreeOfAKind(ref current, ref power, straight);
                        #endregion

                        #region Straight current = 4
                        rStraight(ref current, ref power, straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        rFlush(ref current, ref power, ref vf, straight1);
                        #endregion

                        #region Full House current = 6
                        rFullHouse(ref current, ref power, ref done, straight);
                        #endregion

                        #region Four of a Kind current = 7
                        rFourOfAKind(ref current, ref power, straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        rStraightFlush(ref current, ref power, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        rHighCard(ref current, ref power);
                        #endregion
                    }
                }
            }
        }

        public void rStraightFlush(ref double current, ref double power, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        power = st1.Max() / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        power = st1.Max() / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        power = st2.Max() / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        power = st2.Max() / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        power = st3.Max() / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        power = st3.Max() / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        power = st4.Max() / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        power = st4.Max() / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rFourOfAKind(ref double current, ref double power, int[] straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 && straight[j] / 4 == straight[j + 2] / 4 &&
                        straight[j] / 4 == straight[j + 3] / 4)
                    {
                        current = 7;
                        power = ((straight[j] / 4) * 4) + (current * 100);
                        win.Add(new Type() { Power = power, Current = 7 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        power = (13 * 4) + (current * 100);
                        win.Add(new Type() { Power = power, Current = 7 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rFullHouse(ref double current, ref double power, ref bool done, int[] straight)
        {
            if (current >= -1)
            {
                type = power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                current = 6;
                                power = (13 * 2) + (current * 100);
                                win.Add(new Type() { Power = power, Current = 6 });
                                sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                power = fh.Max() / 4 * 2 + current * 100;
                                win.Add(new Type() { Power = power, Current = 6 });
                                sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (current != 6)
                {
                    power = type;
                }
            }
        }

        public void rFlush(ref double current, ref double power, ref bool vf, int[] straight)
        {
            if (current >= -1)
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
                            current = 5;
                            power = reserve[i] + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f1.Max() / 4 && reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            power = f1.Max() + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
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
                            current = 5;
                            power = reserve[i] + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (reserve[i] % 4 == f1[0] % 4 && reserve[i] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i] + (current * 100);
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f1[0] % 4 && reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i + 1] + (current * 100);
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f1.Min() / 4 && reserve[i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        power = f1.Max() + (current * 100);
                        win.Add(new Type() { Power = power, Current = 5 });
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
                            current = 5;
                            power = reserve[i] + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + (current * 100);
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f2.Max() / 4 && reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
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
                            current = 5;
                            power = reserve[i] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (reserve[i] % 4 == f2[0] % 4 && reserve[i] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f2[0] % 4 && reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i + 1] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f2.Min() / 4 && reserve[i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        power = f2.Max() + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
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
                            current = 5;
                            power = reserve[i] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f3.Max() / 4 && reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
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
                            current = 5;
                            power = reserve[i] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (reserve[i] % 4 == f3[0] % 4 && reserve[i] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f3[0] % 4 && reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i + 1] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f3.Min() / 4 && reserve[i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        power = f3.Max() + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
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
                            current = 5;
                            power = reserve[i] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f4.Max() / 4 && reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
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
                            current = 5;
                            power = reserve[i] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (reserve[i] % 4 == f4[0] % 4 && reserve[i] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f4[0] % 4 && reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i + 1] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f4.Min() / 4 && reserve[i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        power = f4.Max() + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                //ace
                if (f1.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rStraight(ref double current, ref double power, int[] straight)
        {
            if (current >= -1)
            {
                var op = straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            current = 4;
                            power = op.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 4 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            current = 4;
                            power = op[j + 4] + current * 100;
                            win.Add(new Type() { Power = power, Current = 4 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 4 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rThreeOfAKind(ref double current, ref double power, int[] straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            current = 3;
                            power = 13 * 3 + current * 100;
                            win.Add(new Type() { Power = power, Current = 3 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            win.Add(new Type() { Power = power, Current = 3 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        public void rTwoPair(ref double current, ref double power)
        {
            if (current >= -1)
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
                                            current = 2;
                                            power = 13 * 4 + (reserve[i + 1] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            power = 13 * 4 + (reserve[i] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 != 0 && reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (reserve[i] / 4) * 2 + (reserve[i + 1] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
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

        public void rPairTwoPair(ref double current, ref double power)
        {
            if (current >= -1)
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
                                if (reserve[tc] / 4 != reserve[i] / 4 && reserve[tc] / 4 != reserve[i + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            power = (reserve[i] / 4) * 2 + 13 * 4 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            power = (reserve[i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (reserve[tc] / 4) * 2 + (reserve[i + 1] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (reserve[tc] / 4) * 2 + (reserve[i] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (reserve[i] / 4 > reserve[i + 1] / 4)
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                power = 13 + reserve[i] / 4 + current * 100;
                                                win.Add(new Type() { Power = power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                power = reserve[tc] / 4 + reserve[i] / 4 + current * 100;
                                                win.Add(new Type() { Power = power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                power = 13 + reserve[i + 1] + current * 100;
                                                win.Add(new Type() { Power = power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                power = reserve[tc] / 4 + reserve[i + 1] / 4 + current * 100;
                                                win.Add(new Type() { Power = power, Current = 1 });
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

        public void rPairFromHand(ref double current, ref double power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                if (reserve[i] / 4 == reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (reserve[i] / 4 == 0)
                        {
                            current = 1;
                            power = 13 * 4 + current * 100;
                            win.Add(new Type() { Power = power, Current = 1 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            power = (reserve[i + 1] / 4) * 4 + current * 100;
                            win.Add(new Type() { Power = power, Current = 1 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
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
                                current = 1;
                                power = 13 * 4 + reserve[i] / 4 + current * 100;
                                win.Add(new Type() { Power = power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                power = (reserve[i + 1] / 4) * 4 + reserve[i] / 4 + current * 100;
                                win.Add(new Type() { Power = power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }

                    if (reserve[i] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[i] / 4 == 0)
                            {
                                current = 1;
                                power = 13 * 4 + reserve[i + 1] / 4 + current * 100;
                                win.Add(new Type() { Power = power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                power = (reserve[tc] / 4) * 4 + reserve[i + 1] / 4 + current * 100;
                                win.Add(new Type() { Power = power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }
                }
            }
        }

        public void rHighCard(ref double current, ref double power)
        {
            if (current == -1)
            {
                if (reserve[i] / 4 > reserve[i + 1] / 4)
                {
                    current = -1;
                    power = reserve[i] / 4;
                    win.Add(new Type() { Power = power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    power = reserve[i + 1] / 4;
                    win.Add(new Type() { Power = power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (reserve[i] / 4 == 0 || reserve[i + 1] / 4 == 0)
                {
                    current = -1;
                    power = 13;
                    win.Add(new Type() { Power = power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        public void Winner(double current, double power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }

            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (holder[j].Visible)
                {
                    holder[j].Image = deckImages[j];
                }
            }

            if (current == sorted.Current)
            {
                if (power == sorted.Power)
                {
                    winners++;
                    CheckWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }

                    if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }

                    if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }

                    if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }

                    if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }

                    if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }

                    if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }

                    if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }

                    if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }

                    if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }

            //lastfixed
            if (currentText == lastly)
            {
                if (winners > 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        playerChips += int.Parse(tbPot.Text) / winners;
                        textBoxPlayerChips.Text = playerChips.ToString();

                        //pPanel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1Chips += int.Parse(tbPot.Text) / winners;
                        textBoxBot1Chips.Text = bot1Chips.ToString();

                        //b1Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2Chips += int.Parse(tbPot.Text) / winners;
                        textBoxBot2Chips.Text = bot2Chips.ToString();

                        //b2Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3Chips += int.Parse(tbPot.Text) / winners;
                        textBoxBot3Chips.Text = bot3Chips.ToString();

                        //b3Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4Chips += int.Parse(tbPot.Text) / winners;
                        textBoxBot4Chips.Text = bot4Chips.ToString();

                        //b4Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5Chips += int.Parse(tbPot.Text) / winners;
                        textBoxBot5Chips.Text = bot5Chips.ToString();

                        //b5Panel.Visible = true;
                    }

                    //await Finish(1);
                }

                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        playerChips += int.Parse(tbPot.Text);

                        //await Finish(1);
                        //pPanel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1Chips += int.Parse(tbPot.Text);

                        //await Finish(1);
                        //b1Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2Chips += int.Parse(tbPot.Text);

                        //await Finish(1);
                        //b2Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3Chips += int.Parse(tbPot.Text);

                        //await Finish(1);
                        //b3Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4Chips += int.Parse(tbPot.Text);

                        //await Finish(1);
                        //b4Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5Chips += int.Parse(tbPot.Text);

                        //await Finish(1);
                        //b5Panel.Visible = true;
                    }
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
                        if (!PFturn)
                        {
                            playerStatus.Text = string.Empty;
                        }

                        if (!B1Fturn)
                        {
                            bot1Status.Text = string.Empty;
                        }

                        if (!B2Fturn)
                        {
                            b2Status.Text = string.Empty;
                        }

                        if (!B3Fturn)
                        {
                            b3Status.Text = string.Empty;
                        }

                        if (!B4Fturn)
                        {
                            b4Status.Text = string.Empty;
                        }

                        if (!B5Fturn)
                        {
                            b5Status.Text = string.Empty;
                        }
                    }
                }
            }

            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (holder[j].Image != deckImages[j])
                    {
                        holder[j].Image = deckImages[j];
                        playerCall = 0;
                        pRaise = 0;
                        b1Call = 0;
                        b1Raise = 0;
                        b2Call = 0;
                        b2Raise = 0;
                        b3Call = 0;
                        b3Raise = 0;
                        b4Call = 0;
                        b4Raise = 0;
                        b5Call = 0;
                        b5Raise = 0;
                    }
                }
            }

            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (holder[j].Image != deckImages[j])
                    {
                        holder[j].Image = deckImages[j];
                        playerCall = 0;
                        pRaise = 0;
                        b1Call = 0; 
                        b1Raise = 0;
                        b2Call = 0; 
                        b2Raise = 0;
                        b3Call = 0; 
                        b3Raise = 0;
                        b4Call = 0; 
                        b4Raise = 0;
                        b5Call = 0; 
                        b5Raise = 0;
                    }
                }
            }

            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (holder[j].Image != deckImages[j])
                    {
                        holder[j].Image = deckImages[j];
                        playerCall = 0; 
                        pRaise = 0;
                        b1Call = 0; 
                        b1Raise = 0;
                        b2Call = 0; 
                        b2Raise = 0;
                        b3Call = 0; 
                        b3Raise = 0;
                        b4Call = 0; 
                        b4Raise = 0;
                        b5Call = 0; 
                        b5Raise = 0;
                    }
                }
            }

            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = null;
                if (!playerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref pType, ref pPower, PFturn);
                }

                if (!bot1Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", ref b1Type, ref bot1Power, B1Fturn);
                }

                if (!b2Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", ref b2Type, ref bot2Power, B2Fturn);
                }

                if (!b3Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", ref b3Type, ref bot3Power, B3Fturn);
                }

                if (!b4Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", ref b4Type, ref bot4Power, B4Fturn);
                }

                if (!b5Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", ref b5Type, ref bot5Power, B5Fturn);
                }

                Winner(pType, pPower, "Player", playerChips, fixedLast);
                Winner(b1Type, bot1Power, "Bot 1", bot1Chips, fixedLast);
                Winner(b2Type, bot2Power, "Bot 2", bot2Chips, fixedLast);
                Winner(b3Type, bot3Power, "Bot 3", bot3Chips, fixedLast);
                Winner(b4Type, bot4Power, "Bot 4", bot4Chips, fixedLast);
                Winner(b5Type, bot5Power, "Bot 5", bot5Chips, fixedLast);
                restart = true;
                Pturn = true;
                PFturn = false;
                B1Fturn = false;
                B2Fturn = false;
                B3Fturn = false;
                B4Fturn = false;
                B5Fturn = false;
                if (playerChips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        playerChips = f2.a;
                        bot1Chips += f2.a;
                        bot2Chips += f2.a;
                        bot3Chips += f2.a;
                        bot4Chips += f2.a;
                        bot5Chips += f2.a;
                        PFturn = false;
                        Pturn = true;
                        buttonRaise.Enabled = true;
                        buttonFold.Enabled = true;
                        buttonCheck.Enabled = true;
                        buttonRaise.Text = "Raise";
                    }
                }

                playerPanel.Visible = false; 
                bot1Panel.Visible = false;
                bot2Panel.Visible = false; 
                bot3Panel.Visible = false; 
                bot4Panel.Visible = false; 
                bot5Panel.Visible = false;
                playerCall = 0; 
                pRaise = 0;
                b1Call = 0; 
                b1Raise = 0;
                b2Call = 0; 
                b2Raise = 0;
                b3Call = 0;
                b3Raise = 0;
                b4Call = 0; 
                b4Raise = 0;
                b5Call = 0; 
                b5Raise = 0;
                last = 0;
                call = bigBlind;
                Raise = 0;
                imagesPathsFromDirectory = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                bools.Clear();
                rounds = 0;
                pPower = 0; 
                pType = -1;
                type = 0; 
                bot1Power = 0; 
                bot2Power = 0; 
                bot3Power = 0; 
                bot4Power = 0; 
                bot5Power = 0;
                b1Type = -1;
                b2Type = -1; 
                b3Type = -1; 
                b4Type = -1; 
                b5Type = -1;
                ints.Clear();
                CheckWinners.Clear();
                winners = 0;
                win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int x = 0; x < 17; x++)
                {
                    holder[x].Image = null;
                    holder[x].Invalidate();
                    holder[x].Visible = false;
                }

                tbPot.Text = "0";
                playerStatus.Text = string.Empty;
                await Shuffle();
                await Turns();
            }
        }

        public void FixCall(Label status, ref int cCall, ref int cRaise, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cRaise = int.Parse(changeRaise);
                    }

                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        cCall = int.Parse(changeCall);
                    }

                    if (status.Text.Contains("Check"))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }

                if (options == 2)
                {
                    if (cRaise != Raise && cRaise <= Raise)
                    {
                        call = Convert.ToInt32(Raise) - cRaise;
                    }

                    if (cCall != call || cCall <= call)
                    {
                        call = call - cCall;
                    }

                    if (cRaise == Raise && Raise > 0)
                    {
                        call = 0;
                        buttonCall.Enabled = false;
                        buttonCall.Text = "Callisfuckedup";
                    }
                }
            }
        }

        public async Task AllIn()
        {
            #region All in
            if (playerChips <= 0 && !intsadded)
            {
                if (playerStatus.Text.Contains("Raise"))
                {
                    ints.Add(playerChips);
                    intsadded = true;
                }

                if (playerStatus.Text.Contains("Call"))
                {
                    ints.Add(playerChips);
                    intsadded = true;
                }
            }

            intsadded = false;
            if (bot1Chips <= 0 && !B1Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot1Chips);
                    intsadded = true;
                }

                intsadded = false;
            }

            if (bot2Chips <= 0 && !B2Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot2Chips);
                    intsadded = true;
                }

                intsadded = false;
            }

            if (bot3Chips <= 0 && !B3Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot3Chips);
                    intsadded = true;
                }

                intsadded = false;
            }

            if (bot4Chips <= 0 && !B4Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot4Chips);
                    intsadded = true;
                }

                intsadded = false;
            }

            if (bot5Chips <= 0 && !B5Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot5Chips);
                    intsadded = true;
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
                    playerChips += int.Parse(tbPot.Text);
                    textBoxPlayerChips.Text = playerChips.ToString();
                    playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }

                if (index == 1)
                {
                    bot1Chips += int.Parse(tbPot.Text);
                    textBoxPlayerChips.Text = bot1Chips.ToString();
                    bot1Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }

                if (index == 2)
                {
                    bot2Chips += int.Parse(tbPot.Text);
                    textBoxPlayerChips.Text = bot2Chips.ToString();
                    bot2Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }

                if (index == 3)
                {
                    bot3Chips += int.Parse(tbPot.Text);
                    textBoxPlayerChips.Text = bot3Chips.ToString();
                    bot3Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }

                if (index == 4)
                {
                    bot4Chips += int.Parse(tbPot.Text);
                    textBoxPlayerChips.Text = bot4Chips.ToString();
                    bot4Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }

                if (index == 5)
                {
                    bot5Chips += int.Parse(tbPot.Text);
                    textBoxPlayerChips.Text = bot5Chips.ToString();
                    bot5Panel.Visible = true;

                    MessageBox.Show("Bot 5 Wins");
                }

                for (int j = 0; j <= 16; j++)
                {
                    holder[j].Visible = false;
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

            playerPanel.Visible = false; 
            bot1Panel.Visible = false; 
            bot2Panel.Visible = false;
            bot3Panel.Visible = false; 
            bot4Panel.Visible = false; 
            bot5Panel.Visible = false;
            call = bigBlind;
            Raise = 0;
            foldedPlayers = 5;
            type = 0; 
            rounds = 0; 
            bot1Power = 0; 
            bot2Power = 0; 
            bot3Power = 0; 
            bot4Power = 0; 
            bot5Power = 0; 
            pPower = 0; 
            pType = -1; 
            Raise = 0;
            b1Type = -1; 
            b2Type = -1; 
            b3Type = -1; 
            b4Type = -1; 
            b5Type = -1;
            bot1Turn = false; 
            bot2Turn = false; 
            bot3Turn = false; 
            bot4Turn = false; 
            bot5Turn = false;
            B1Fturn = false; 
            B2Fturn = false; 
            B3Fturn = false; 
            B4Fturn = false;
            B5Fturn = false;
            playerHasFolded = false; 
            bot1HasFolded = false; 
            bot2HasFolded = false; 
            bot3HasFolded = false; 
            bot4HasFolded = false; 
            bot5HasFolded = false;
            PFturn = false; 
            Pturn = true; 
            restart = false; 
            raising = false;
            playerCall = 0; 
            b1Call = 0; 
            b2Call = 0; 
            b3Call = 0; 
            b4Call = 0; 
            b5Call = 0; 
            pRaise = 0; 
            b1Raise = 0; 
            b2Raise = 0; 
            b3Raise = 0; 
            b4Raise = 0; 
            b5Raise = 0;
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
            CheckWinners.Clear();
            ints.Clear();
            win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            tbPot.Text = "0";
            time = 60; 
            up = 10000000; 
            turnCount = 0;
            playerStatus.Text = string.Empty;
            bot1Status.Text = string.Empty;
            b2Status.Text = string.Empty;
            b3Status.Text = string.Empty;
            b4Status.Text = string.Empty;
            b5Status.Text = string.Empty;
            if (playerChips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    playerChips = f2.a;
                    bot1Chips += f2.a;
                    bot2Chips += f2.a;
                    bot3Chips += f2.a;
                    bot4Chips += f2.a;
                    bot5Chips += f2.a;
                    PFturn = false;
                    Pturn = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    buttonCheck.Enabled = true;
                    buttonRaise.Text = "Raise";
                }
            }

            imagesPathsFromDirectory = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                holder[os].Image = null;
                holder[os].Invalidate();
                holder[os].Visible = false;
            }

            await Shuffle();

            //await Turns();
        }

        public void FixWinners()
        {
            win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref this.pType, ref this.pPower, PFturn);
            }

            if (!bot1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", ref b1Type, ref bot1Power, B1Fturn);
            }

            if (!b2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", ref b2Type, ref bot2Power, B2Fturn);
            }

            if (!b3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", ref b3Type, ref bot3Power, B3Fturn);
            }

            if (!b4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", ref b4Type, ref bot4Power, B4Fturn);
            }

            if (!b5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", ref b5Type, ref bot5Power, B5Fturn);
            }

            Winner(pType, pPower, "Player", playerChips, fixedLast);
            Winner(b1Type, bot1Power, "Bot 1", bot1Chips, fixedLast);
            Winner(b2Type, bot2Power, "Bot 2", bot2Chips, fixedLast);
            Winner(b3Type, bot3Power, "Bot 3", bot3Chips, fixedLast);
            Winner(b4Type, bot4Power, "Bot 4", bot4Chips, fixedLast);
            Winner(b5Type, bot5Power, "Bot 5", bot5Chips, fixedLast);
        }

        public void AI(int c1, int c2, ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                if (botCurrent == -1)
                {
                    HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 0)
                {
                    PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 1)
                {
                    PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 2)
                {
                    TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 3)
                {
                    ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }

                if (botCurrent == 4)
                {
                    Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }

                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }

                if (botCurrent == 6)
                {
                    FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }

                if (botCurrent == 7)
                {
                    FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }

                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
            }

            if (sFTurn)
            {
                holder[c1].Visible = false;
                holder[c2].Visible = false;
            }
        }

        public void HighCard(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 20, 25);
        }

        public void PairTable(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 16, 25);
        }

        public void PairHand(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 6, rRaise);
            }

            if (botPower <= 139 && botPower >= 128)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 7, rRaise);
            }

            if (botPower < 128 && botPower >= 101)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 9, rRaise);
            }
        }

        public void TwoPair(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 3, rRaise);
            }

            if (botPower <= 244 && botPower >= 234)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }

            if (botPower < 234 && botPower >= 201)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
        }

        public void ThreeOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (botPower <= 390 && botPower >= 330)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }

            //10  8
            if (botPower <= 327 && botPower >= 321)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }

            //7 2
            if (botPower < 321 && botPower >= 303)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
        }

        public void Straight(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }

            if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            
            if (botPower < 407 && botPower >= 404)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
        }

        public void Flush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fCall, fRaise);
        }

        public void FullHouse(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }

            if (botPower < 620 && botPower >= 602)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
        }

        public void FourOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fkCall, fkRaise);
            }
        }

        public void StraightFlush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sfCall, sfRaise);
            }
        }

        public void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            raising = false;
            sStatus.Text = "Fold";
            sTurn = false;
            sFTurn = true;
        }

        public void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = "Check";
            cTurn = false;
            raising = false;
        }

        public void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            raising = false;
            sTurn = false;
            sChips -= call;
            sStatus.Text = "Call " + call;
            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
        }

        public void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(Raise);
            sStatus.Text = "Raise " + Raise;
            tbPot.Text = (int.Parse(tbPot.Text) + Convert.ToInt32(Raise)).ToString();
            call = Convert.ToInt32(Raise);
            raising = true;
            sTurn = false;
        }

        public static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        public void HP(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (call <= 0)
            {
                Check(ref sTurn, sStatus);
            }

            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(sChips, n))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }

                if (rnd == 2)
                {
                    if (call <= RoundN(sChips, n1))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }

            if (rnd == 3)
            {
                if (Raise == 0)
                {
                    Raise = call * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (Raise <= RoundN(sChips, n))
                    {
                        Raise = call * 2;
                        Raised(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }

            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        public void PH(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(ref sTurn, sStatus);
                }

                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (Raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n) && call <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (Raise <= RoundN(sChips, n) && Raise >= RoundN(sChips, n) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (Raise <= RoundN(sChips, n) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
            }

            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (Raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n - rnd) && call <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (Raise <= RoundN(sChips, n - rnd) && Raise >= RoundN(sChips, n - rnd) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (Raise <= RoundN(sChips, n - rnd) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }

                if (call <= 0)
                {
                    Raise = RoundN(sChips, r - rnd);
                    Raised(ref sChips, ref sTurn, sStatus);
                }
            }

            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        public void Smooth(ref int botChips, ref bool botTurn, ref bool botFTurn, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (call <= 0)
            {
                Check(ref botTurn, botStatus);
            }
            else
            {
                if (call >= RoundN(botChips, n))
                {
                    if (botChips > call)
                    {
                        Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= call)
                    {
                        raising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = "Call " + botChips;
                        tbPot.Text = (int.Parse(tbPot.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (Raise > 0)
                    {
                        if (botChips >= Raise * 2)
                        {
                            Raise *= 2;
                            Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        Raise = call * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }

            if (botChips <= 0)
            {
                botFTurn = true;
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
            if (progressBartTimer.Value <= 0)
            {
                PFturn = true;
                await Turns();
            }

            if (time > 0)
            {
                time--;
                progressBartTimer.Value = (time / 6) * 100;
            }
        }

        /// <summary>
        /// Set value for chips amount in UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateChipsAmountOnUI(object sender, object e)
        {
           /* if (playerChips <= 0)
            {
                textBoxPlayerChips.Text = "Chips : 0";
            }

            if (bot1Chips <= 0)
            {
                textBoxBot1Chips.Text = "Chips : 0";
            }

            if (bot2Chips <= 0)
            {
                textBoxBot2Chips.Text = "Chips : 0";
            }

            if (bot3Chips <= 0)
            {
                textBoxBot3Chips.Text = "Chips : 0";
            }

            if (bot4Chips <= 0)
            {
                textBoxBot4Chips.Text = "Chips : 0";
            }

            if (bot5Chips <= 0)
            {
                textBoxBot5Chips.Text = "Chips : 0";
            }*/

            textBoxPlayerChips.Text = string.Format("Chips : {0} ",playerChips);
            textBoxBot1Chips.Text = string.Format("Chips : {0} ",bot1Chips);
            textBoxBot2Chips.Text = string.Format("Chips : {0} ", bot2Chips);
            textBoxBot3Chips.Text = string.Format("Chips : {0} ", bot3Chips);
            textBoxBot4Chips.Text = string.Format("Chips : {0} ", bot4Chips);
            textBoxBot5Chips.Text = string.Format("Chips : {0} ", bot5Chips);

            if (playerChips <= 0)
            {
                Pturn = false;
                PFturn = true;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                buttonCheck.Enabled = false;
            }

            if (up > 0)
            {
                up--;
            }

            if (playerChips >= call)
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

            if (playerChips <= 0)
            {
                buttonRaise.Enabled = false;
            }

            int parsedValue;

            if (textBoxRaise.Text != string.Empty && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (playerChips <= int.Parse(textBoxRaise.Text))
                {
                    buttonRaise.Text = "All in";
                }
                else
                {
                    buttonRaise.Text = "Raise";
                }
            }

            if (playerChips < call)
            {
                buttonRaise.Enabled = false;
            }
        }

        public async void bFold_Click(object sender, EventArgs e)
        {
            playerStatus.Text = "Fold";
            Pturn = false;
            PFturn = true;
            await Turns();
        }

        public async void bCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                Pturn = false;
                playerStatus.Text = "Check";
            }
            else
            {
                ////pStatus.Text = "All in " + Chips;
                buttonCheck.Enabled = false;
            }

            await Turns();
        }

        public async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref pType, ref pPower, PFturn);
            if (playerChips >= call)
            {
                playerChips -= call;
                textBoxPlayerChips.Text = "Chips : " + playerChips.ToString();
                if (tbPot.Text != string.Empty)
                {
                    tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                }
                else
                {
                    tbPot.Text = call.ToString();
                }

                Pturn = false;
                playerStatus.Text = "Call " + call;
                playerCall = call;
            }
            else if (playerChips <= call && call > 0)
            {
                tbPot.Text = (int.Parse(tbPot.Text) + playerChips).ToString();
                playerStatus.Text = "All in " + playerChips;
                playerChips = 0;
                textBoxPlayerChips.Text = "Chips : " + playerChips.ToString();
                Pturn = false;
                buttonFold.Enabled = false;
                playerCall = playerChips;
            }

            await Turns();
        }

        public async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref pType, ref pPower, PFturn);
            int parsedValue;
            if (textBoxRaise.Text != string.Empty && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (playerChips > call)
                {
                    if (Raise * 2 > int.Parse(textBoxRaise.Text))
                    {
                        textBoxRaise.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (playerChips >= int.Parse(textBoxRaise.Text))
                        {
                            call = int.Parse(textBoxRaise.Text);
                            Raise = int.Parse(textBoxRaise.Text);
                            playerStatus.Text = "Raise " + call.ToString();
                            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                            buttonCall.Text = "Call";
                            playerChips -= int.Parse(textBoxRaise.Text);
                            raising = true;
                            last = 0;
                            pRaise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            call = playerChips;
                            Raise = playerChips;
                            tbPot.Text = (int.Parse(tbPot.Text) + playerChips).ToString();
                            playerStatus.Text = "Raise " + call.ToString();
                            playerChips = 0;
                            raising = true;
                            last = 0;
                            pRaise = Convert.ToInt32(Raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }

            Pturn = false;
            await Turns();
        }

        public void bAdd_Click(object sender, EventArgs e)
        {
            if (tbAdd.Text == string.Empty) { }
            else
            {
                playerChips += int.Parse(tbAdd.Text);
                bot1Chips += int.Parse(tbAdd.Text);
                bot2Chips += int.Parse(tbAdd.Text);
                bot3Chips += int.Parse(tbAdd.Text);
                bot4Chips += int.Parse(tbAdd.Text);
                bot5Chips += int.Parse(tbAdd.Text);
            }

            this.textBoxPlayerChips.Text = "Chips : " + this.playerChips.ToString();
        }

        public void bOptions_Click(object sender, EventArgs e)
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
        }

        public void bSB_Click(object sender, EventArgs e)
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
            }

            if (int.Parse(this.textBoxSB.Text) >= 250 && int.Parse(this.textBoxSB.Text) <= 100000)
            {
                this.smallBlind = int.Parse(this.textBoxSB.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        public void bBB_Click(object sender, EventArgs e)
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
        }

        public void Layout_Change(object sender, LayoutEventArgs e)
        {
            this.width = this.Width;
            this.height = this.Height;
        }
        #endregion
    }
}