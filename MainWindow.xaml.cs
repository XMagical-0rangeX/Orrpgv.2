using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace OrangeRPGv._1
{
    /// <summary>
    /// OrangeRpg Made By Atlantis 
    /// 
    /// Fix List
    /// 
    /// stats ~never?
    /// castle
    /// more events?
    /// castle roomz
    /// exit btn?
    /// descriptions <-- no
    /// magic? int and wis attack? ??
    /// items mayhaps?
    /// 
    /// </summary>

    public partial class MainWindow : Window
    {
#nullable disable

        private string classname, racename;
        readonly int speed = 5;
        readonly bool[] go =
        {
            false, false, false, false
        };
        int points = 6, tryfade = 0, tryfadeE = 0, currentevent = -1, tryfadeV = 0, tryfadeS = 0;
        readonly Random Rand = new();
        bool battleNotif = false, playgaurd = false, enemgaurd = false;
        Player player;
        Enemy enemy;
        Image Slime, EventNotif;
        Grid popup;
        System.Windows.Shapes.Rectangle NotifEncounterSqu;
        DispatcherTimer DST = new DispatcherTimer(DispatcherPriority.Render);

        public int[] CurrentSpace = { 2, 3 }, castlMove = { 0, 0, 0, 0 };
        public MainWindow()
        {
            InitializeComponent();
            DST.Tick += Timer_Tick;
            DST.Interval = TimeSpan.FromMilliseconds(20);
            DST.Start();
        }
        // BEGINING FUNCTIONS
        private void PlayGame(object sender, RoutedEventArgs e)
        {
            CharacterCreation.Visibility = Visibility.Visible;
        }
        private void MakeCharacter(object sender, RoutedEventArgs e)
        {
            if (points >= 0)
            {
                string name = PlayerName.Text;
                int clas = -1;
                int race = -1;
                race = CheckRace();
                clas = CheckClass();
                int hp = HitDie(clas);
                int[] statpoint = { 0, 0, 0, 0, 0, 0 };
                InitStat(statpoint);
                player = new Player(name, race, clas, hp, statpoint);
                CharacterCreation.Visibility = Visibility.Hidden;
                InitPlayerMenu();
            }
        }
        private int CheckRace()
        {
            int var;
            if (Hum.IsChecked == true)
            {
                var = 0;
            }
            else if (Halfling.IsChecked == true)
            {
                var = 1;
            }
            else if (Elf.IsChecked == true)
            {
                var = 2;
            }
            else
            {
                var = 3;
            }
            return var;
        }
        private int CheckClass()
        {
            int var;
            if (Barb.IsChecked == true)
            {
                var = 0;
            }
            else if (Figh.IsChecked == true)
            {
                var = 1;
            }
            else if (Cler.IsChecked == true)
            {
                var = 2;
            }
            else
            {
                var = 3;
            }
            return var;
        }
        static int HitDie(int clas)
        {
            int hp;
            if (clas == 0)
            {
                hp = 12;
            }
            else if (clas == 1)
            {
                hp = 10;
            }
            else if (clas == 2)
            {
                hp = 8;
            }
            else
            {
                hp = 6;
            }
            return hp;
        }
        private void InitStat(int[] stat)
        {
            stat[0] = int.Parse(StrSco.Text);
            stat[1] = int.Parse(DexSco.Text);
            stat[2] = int.Parse(ConSco.Text);
            stat[3] = int.Parse(WisSco.Text);
            stat[4] = int.Parse(ChaSco.Text);
            stat[5] = int.Parse(IntSco.Text);
        }
        public string InitClass(int[] stat)
        {
            string var;
            if (player.Class == 0)
            {
                var = "Barbarian";
                stat[2] += 1;
            }
            else if (player.Class == 1)
            {
                var = "Fighter";
                stat[0] += 1;
            }
            else if (player.Class == 2)
            {
                var = "Cleric";
                stat[3] += 1;
            }
            else
            {
                var = "Wizard";
                stat[5] += 1;
            }
            return var;
        }
        public string InitRace(int[] stat)
        {
            string var;
            if (player.Race == 0)
            {
                var = "Human";
            }
            else if (player.Race == 1)
            {
                var = "Halfling";
                stat[1] += 1;
            }
            else if (player.Race == 2)
            {
                var = "Elf";
                stat[5] += 1;
            }
            else
            {
                var = "Dwarf";
                stat[2] += 1;
            }
            return var;
        }
        private void InitPlayerMenu()
        {
            classname = InitClass(player.Stats);
            racename = InitRace(player.Stats);
            PName.Text = "Name: " + player.Name;
            PRace.Text = "Race: " + racename;
            PClas.Text = "Class: " + classname;
            PlacePlayerLvlandStats();
            Stats.Visibility = Visibility.Visible;
        }
        public void PlacePlayerLvlandStats()
        {
            PLvl.Text = "Level: " + player.Level;
            pHp.Content = "Hp: " + player.HpCur + "/" + player.HpMax;
            scoreStr.Text = "" + player.Stats[0];
            scoreDex.Text = "" + player.Stats[1];
            scoreCon.Text = "" + player.Stats[2];
            scoreWis.Text = "" + player.Stats[3];
            scoreCha.Text = "" + player.Stats[4];
            scoreInt.Text = "" + player.Stats[5];
        }
        //GOTO FUNCTIONS
        private void GoToMap(object sender, RoutedEventArgs e)
        {
            Stats.Visibility = Visibility.Hidden;
            FightMenu.Visibility = Visibility.Hidden;
            MainMap.Visibility = Visibility.Visible;
        }
        private void GoToStats(object sender, RoutedEventArgs e)
        {
            MainMap.Visibility = Visibility.Hidden;
            Stats.Visibility = Visibility.Visible;
        }
        //Keypress Functions
        private void MapCan_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    go[1] = false;
                    go[0] = true;
                    MoveCasl(0);
                    break;
                case Key.S:
                    go[0] = false;
                    go[1] = true;
                    MoveCasl(4);
                    break;
                case Key.A:
                    go[3] = false;
                    go[2] = true;
                    MoveCasl(2);
                    break;
                case Key.D:
                    go[2] = false;
                    go[3] = true;
                    MoveCasl(1);
                    break;
            }
        }
        private void MapCan_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    go[0] = false;
                    break;
                case Key.S:
                    go[1] = false;

                    break;
                case Key.A:
                    go[2] = false;

                    break;
                case Key.D:
                    go[3] = false;
                    break;
            }
        }
        //TIMER
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (VillageMap.Visibility == Visibility.Hidden)
            {
                MovePlayer(mc, MainMap);
            }
            else if (Blacksmith.Visibility == Visibility.Hidden && WitchHut.Visibility == Visibility.Hidden)
            {
                MovePlayer(villagemc, VillageMap);
                Arroz(villagemc);
            }
            TryFadeZ(FightMenu, tryfade);
            TryFadeZ(Encounter, tryfadeE);
            TryFadeZ(VillageMap, tryfadeV);
            TryFadeZ(Popupsheild, tryfadeS);
            if (popup != null)
            {
                if (popup.Opacity <= 0.5)
                {
                    Notification.Visibility = Visibility.Visible;
                }
                if (popup.Opacity <= 0)
                {
                    Notification.Opacity -= 0.01;
                }
                if (Notification.Opacity <= 0)
                {
                    if (popup == Popupsheild)
                    {
                        Popupsheild.Opacity = 0.5;
                    }
                    Notification.Visibility = Visibility.Hidden;
                    Notification.Opacity = 1;
                    popup = null;
                }
            }
        }
        // Movement Funct
        private void MovePlayer(Image image, Grid grid)
        {
            bool check = false;
            if (grid.Visibility == Visibility.Visible && !battleNotif)
            {
                if (go[0] && Canvas.GetTop(image) > 0)
                {
                    Canvas.SetTop(image, Canvas.GetTop(image) - speed);
                    check = true;
                }
                if (go[1] && Canvas.GetTop(image) + image.Height < 375)
                {
                    Canvas.SetTop(image, Canvas.GetTop(image) + speed);
                    check = true;
                }
                if (go[2] && Canvas.GetLeft(image) > 0)
                {
                    Canvas.SetLeft(image, Canvas.GetLeft(image) - speed);
                    check = true;
                }
                if (go[3] && Canvas.GetLeft(image) + image.Width < 575)
                {
                    Canvas.SetLeft(image, Canvas.GetLeft(image) + speed);
                    check = true;
                }
                if (check && image == mc)
                {
                    EcounterChance();
                }
            }
        }
        private void PlayerCollide(Image main, int x, int y, int w, int h, int resetintx, int resetinty)
        {
            Rect mrect = new Rect(Canvas.GetLeft(main), Canvas.GetTop(main), main.Width, main.Height);
            Rect collid = new Rect(x, y, w, h);
            if (mrect.IntersectsWith(collid))
            {
                Initair();
                VillageMap.Visibility = Visibility.Visible;
                tryfadeV = 1;
                Canvas.SetTop(main, resetinty);
                Canvas.SetLeft(main, resetintx);
            }
        }
        private void Arroz(Image main)
        {
            foreach (var i in villageCanvas.Children.OfType<Image>())
            {
                if ((string)i.Tag == "arrow")
                {
                    Rect mainr = new Rect(Canvas.GetLeft(main), Canvas.GetTop(main), main.Width, main.Height);
                    Rect airo = new Rect(Canvas.GetLeft(i), Canvas.GetTop(i), i.Width, i.Height);

                    if (mainr.IntersectsWith(airo))
                    {
                        i.Opacity = 1;
                    }
                    else
                    {
                        i.Opacity = 0;
                    }
                }
            }
        }
        private void Initair()
        {
            foreach (var i in villageCanvas.Children.OfType<Image>())
            {
                if ((string)i.Tag == "arrow")
                {
                    i.Opacity = 0;
                }
            }
        }
        // Try Fading out
        static void TryFadeZ(Grid grid, int fade)
        {
            if (fade == 1 && grid.Opacity < 1)
            {
                grid.Opacity += 0.02;

            }
            else if (fade == 2 && grid.Opacity > 0)
            {
                grid.Opacity -= 0.02;
            }
            else if (grid.Opacity <= 0)
            {
                fade = 0;
                grid.Visibility = Visibility.Hidden;
            }
        }
        // STATUS CLICKS
        private void StrClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int incdec = 0;
            int.TryParse(StrSco.Text, out int parse);

            if (b.Content.ToString() == "⬆" && parse < 5)
            {
                incdec = 1;
            }
            else if (b.Content.ToString() != "⬆" && parse > 1)
            {
                incdec = -1;
            }
            parse += incdec;
            PointUpdate(incdec, parse);
            StrSco.Text = "" + parse;
        }
        private void DexClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int incdec = 0;
            int.TryParse(DexSco.Text, out int parse);

            if (b.Content.ToString() == "⬆" && parse < 5)
            {
                incdec = 1;
            }
            else if (b.Content.ToString() != "⬆" && parse > 1)
            {
                incdec = -1;
            }
            parse += incdec;
            PointUpdate(incdec, parse);
            DexSco.Text = "" + parse;
        }
        private void ConClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int incdec = 0;
            int.TryParse(ConSco.Text, out int parse);

            if (b.Content.ToString() == "⬆" && parse < 5)
            {
                incdec = 1;
            }
            else if (b.Content.ToString() != "⬆" && parse > 1)
            {
                incdec = -1;
            }
            parse += incdec;
            PointUpdate(incdec, parse);
            ConSco.Text = "" + parse;
        }
        private void ChaClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int incdec = 0;
            int.TryParse(ChaSco.Text, out int parse);

            if (b.Content.ToString() == "⬆" && parse < 5)
            {
                incdec = 1;
            }
            else if (b.Content.ToString() != "⬆" && parse > 1)
            {
                incdec = -1;
            }
            parse += incdec;
            PointUpdate(incdec, parse);
            ChaSco.Text = "" + parse;
        }
        private void WisClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int incdec = 0;
            int.TryParse(WisSco.Text, out int parse);

            if (b.Content.ToString() == "⬆" && parse < 5)
            {
                incdec = 1;
            }
            else if (b.Content.ToString() != "⬆" && parse > 1)
            {
                incdec = -1;
            }
            parse += incdec;
            PointUpdate(incdec, parse);
            WisSco.Text = "" + parse;
        }
        private void IntClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int incdec = 0;
            int.TryParse(IntSco.Text, out int parse);

            if (b.Content.ToString() == "⬆" && parse < 5)
            {
                incdec = 1;
            }
            else if (b.Content.ToString() != "⬆" && parse > 1)
            {
                incdec = -1;
            }
            parse += incdec;
            PointUpdate(incdec, parse);
            IntSco.Text = "" + parse;
        }
        private void PointUpdate(int ud, int statval)
        {
            if (ud != 0)
            {
                if (ud == 1)
                {
                    if (statval > 13)
                    {
                        points -= 2;
                    }
                    else
                    {
                        points -= 1;
                    }
                }
                else
                {
                    if (statval >= 13)
                    {
                        points += 2;
                    }
                    else
                    {
                        points += 1;
                    }

                }
                StatPointCheck.Content = "Stats Points(" + points + ")";
            }
        }
        //PrepFight/Encounter
        private void EcounterChance()
        {
            int encount = Rand.Next(1, 100);
            if (encount > 95)
            {
                NotifEncounterSqu = new System.Windows.Shapes.Rectangle()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    Fill = new SolidColorBrush(Colors.White),
                    Width = 50,
                    Height = 50,
                };
                Canvas.SetLeft(NotifEncounterSqu, Canvas.GetLeft(mc) - mc.Width / 2);
                Canvas.SetTop(NotifEncounterSqu, Canvas.GetTop(mc) - 40);
                mapCan.Children.Add(NotifEncounterSqu);
                string imgtype;
                if (Rand.Next(1, 101) <= 60)
                {
                    imgtype = "Fight";
                    encount = Rand.Next(1, 101);
                    if (encount <= 20)
                    {
                        GenSlime("Red");
                    }
                    else if (encount <= 40)
                    {
                        GenSlime("Blue");

                    }
                    else if (encount <= 60)
                    {
                        GenSlime("Pink");

                    }
                    else if (encount <= 80)
                    {
                        GenSlime("Green");

                    }
                    else
                    {
                        GenSlime("Orange");

                    }
                    FightMenu.Opacity = 0;
                    FightMenu.Visibility = Visibility.Visible;
                    tryfade = 1;
                }
                else
                {
                    imgtype = "Rock";
                    string pictofenc;
                    encount = Rand.Next(1, 101);
                    if (encount <= 50)
                    {
                        pictofenc = "HypnoCave";
                        encounterDescription.Text = "You feel drawn to this cave,\n almost as if something\n is pulling you deeper";
                        option1E.Content = "Go into it";
                        option2E.Content = "Investigate";
                        option3E.Content = "Leave";
                        currentevent = 0;
                    }
                    else
                    {
                        pictofenc = "CartMan";
                        encounterDescription.Text = "You spy a man on the road\n with a cart";
                        option1E.Content = "Buy a camel";
                        option2E.Content = "Steal a camel";
                        option3E.Content = "Leave";
                        currentevent = 1;
                    }
                    encounterFullImg.Source = new BitmapImage(new Uri("Assets/EncounterFull/" + pictofenc + ".png", UriKind.Relative));
                    Encounter.Opacity = 0;
                    Encounter.Visibility = Visibility.Visible;
                    tryfadeE = 1;
                }
                EventNotif = new Image()
                {
                    Source = new BitmapImage(new Uri("Assets/EncounterImg/" + imgtype + ".png", UriKind.Relative)),
                    Width = 50,
                    Height = 50,
                };
                Canvas.SetTop(EventNotif, Canvas.GetTop(NotifEncounterSqu));
                Canvas.SetLeft(EventNotif, Canvas.GetLeft(NotifEncounterSqu));
                mapCan.Children.Add(EventNotif);
                battleNotif = true;
            }
        }
        private void EventConfirm(object sender, RoutedEventArgs e)
        {
            string notifi;
            if (currentevent == 0)
            {
                if (option1E.IsChecked == true)
                {
                    notifi = "You wak into the cave and find... \nA rabid dog that bite you for " + DamagePlayer(1, 6, 2) + " Damage";
                }
                else if (option2E.IsChecked == true)
                {
                    notifi = "You Investigate.. \n";
                    if (Rand.Next(1, 21) + player.Stats[5] >= 13)
                    {
                        player.Stats[5] += 1;
                        notifi += "and find an airborn hypnotic poison,\nleaving the cave you feel smarter";
                    }
                    else
                    {
                        notifi += "and step in a bear trap dealing " + DamagePlayer(1, 6, 2) + " Damage";
                    }
                }
                else
                {
                    notifi = "You attempt to leave.. \n";
                    if (Rand.Next(1, 21) + player.Stats[3] >= 15)
                    {
                        player.Stats[3] += 1;
                        notifi += "and you do successfully! \nFeeling wiser you carry on";
                    }
                    else
                    {
                        notifi += "but feel entralled by the cave, \nstepping on a rusted nail you take " + DamagePlayer(1, 6, 2) + " Damage and flee";
                    }
                }
            }
            else
            {
                if (option1E.IsChecked == true)
                {
                    notifi = "You try to buy a camel.. \n";
                    if (Rand.Next(1, 21) + player.Stats[4] >= 13)
                    {
                        player.Stats[4] += 1;
                        notifi += "but unfortunatally you dont have any money, \nbut he appritiates it";
                    }
                    else
                    {
                        notifi += "He doesent appritiate how little money you have, \nand hits you for " + DamagePlayer(1, 1, 0) + " Damage";
                    }
                }
                else if (option2E.IsChecked == true)
                {
                    notifi = "You try stealing a camel.. \n";
                    if (Rand.Next(1, 21) + player.Stats[1] >= 13)
                    {
                        player.Stats[1] += 1;
                        notifi += "Unfortunatally he has no camels, \nbut you feel stealthier";
                    }
                    else
                    {
                        notifi += "but he catches you, \nand beats you for " + DamagePlayer(1, 8, 2) + " Damage";
                    }
                }
                else
                {
                    notifi = "You leave the man and his carrage";
                }
            }
            Popup(Encounter, notifi);
            CheckExp();
            battleNotif = false;
            mapCan.Children.Remove(NotifEncounterSqu);
            mapCan.Children.Remove(EventNotif);
            LogCombatLog();
            PlacePlayerLvlandStats();
            tryfadeE = 2;
        }
        private void GenSlime(string color)
        {
            Slime = new Image()
            {
                Source = new BitmapImage(new Uri("Assets/Slime/Slime" + color + ".png", UriKind.Relative)),
                Name = "Enemy",
            };
            enemy = new Enemy(Rand.Next(1, 12) + 3, 10, color + " Slime");

            Canvas.SetTop(Slime, 75);
            Canvas.SetLeft(Slime, 350);
            FightCanvas.Children.Add(Slime);
            EnemyName.Text = enemy.Name;
            HitPointUpdate();
        }
        //Battle functions
        private void FightChoice(object sender, RoutedEventArgs e)
        {
            if (battleNotif)
            {
                Button b = (Button)sender;
                HitPointUpdate();

                if (b.Content.ToString() == "Action")
                {
                    actbtn("Attack", "Gaurd", "Talk", "Back");
                }
                else if (b.Content.ToString() == "Talk")
                {
                    DisplayFightText("You talk to it, but it seems to be ineffective..");
                    enemyTurn();
                }
                else if (b.Content.ToString() == "Attack")
                {
                    Attackz(enemgaurd, 0, player.MeleeCap);
                    enemgaurd = false;
                    EnemyDeath();
                }
                else if (b.Content.ToString() == "Gaurd")
                {
                    DisplayFightText("You gaurd..");
                    playgaurd = true;
                    enemyTurn();
                }
                else if (b.Content.ToString() == "Item")
                {
                    actbtn("Health Pot", "Poison Pot", "I3", "Back");
                }
                else if (b.Content.ToString() == "Health Pot")
                {
                    DisplayFightText("You drink the potion and are healed");
                    HealPlayer(1, 8, 5);
                }
                else if (b.Content.ToString() == "Magic")
                {
                    actbtn("Fire", "Ice", "Heal", "Back");
                }
                else if (b.Content.ToString() == "Fire")
                {
                    Attackz(enemgaurd, 0, player.MagBounus);
                    enemgaurd = false;
                    enemyTurn();
                }
                else if (b.Content.ToString() == "Ice")
                {
                    Attackz(enemgaurd, 0, player.MagBounus);
                    enemgaurd = false;
                    enemyTurn();
                }
                else if (b.Content.ToString() == "Heal")
                {
                    DisplayFightText("You use divine emanance and are healed");
                    HealPlayer(1, player.MagBounus, player.Stats[3]);
                }
                else
                {
                    if (b.Content.ToString() == "Flee")
                    {
                        if (Rand.Next(10, 21) == 20)
                        {
                            DisplayFightText("You Successfully Fled");
                            LeaveFightscreen();
                        }
                        else
                        {
                            DisplayFightText("You Fled Unsuccessfully");
                            enemyTurn();
                        }
                    }
                }
            }

        }
        private void DisplayFightText(string text)
        {
            bool full = true;
            for (int i = 0; i < 4; i++)
            {
                if ((string)CheckPlayrow(i).Content == "")
                {
                    CheckPlayrow(i).Content = text;
                    full = false;
                    break;
                }
            }
            if (full)
            {
                Playrow0.Content = Playrow1.Content;
                Playrow1.Content = Playrow2.Content;
                Playrow2.Content = Playrow3.Content;
                Playrow3.Content = text;
            }
        }
        private Label CheckPlayrow(int i)
        {
            Label Playrow;
            if (i == 0)
            {
                Playrow = Playrow0;
            }
            else if (i == 1)
            {
                Playrow = Playrow1;
            }
            else if (i == 2)
            {
                Playrow = Playrow2;
            }
            else
            {
                Playrow = Playrow3;
            }
            return Playrow;
        }
        private void enemyTurn()
        {
            enemgaurd = false;
            actbtn("Action", "Item", "Magic", "Flee");
            int enemyrandact = Rand.Next(1, 101);
            if (enemyrandact <= 20)
            {
                DisplayFightText("The enemy waits..");
            }
            else if (enemyrandact <= 40)
            {
                DisplayFightText("The enemy gaurds..");
                enemgaurd = true;
            }
            else
            {
                Attackz(playgaurd, 1, 5);
                playgaurd = false;
            }
            CheckPlayerDeath();
        }
        private void Attackz(bool gradz, int i, int damagecap)
        {
            int gaurd = 0;
            if (gradz)
            {
                gaurd = player.Stats[0] + player.Level;
            }
            int damage = Rand.Next(1, damagecap) + 2 - gaurd;
            if (i == 0)
            {
                DisplayFightText("You attack dealing.. " + damage + " damage");
                enemy.HpCur -= damage;
            }
            else
            {
                DisplayFightText("The enemy attacks.. " + damage + " damage");
                player.HpCur -= damage;
            }
            HitPointUpdate();
        }
        private void actbtn(string a, string b, string c, string d)
        {
            Act.Content = a;
            Ite.Content = b;
            Mag.Content = c;
            Fle.Content = d;
        }
        // Player Help Functions
        private void HitPointUpdate()
        {
            PlayerHealth.Text = "Health: " + player.HpCur + "/" + player.HpMax;
            if (enemy != null)
            {
                EnemyHp.Text = "Health: " + enemy.HpCur + "/" + enemy.HpMax;
            }
            pHp.Content = "Hp: " + player.HpCur + "/" + player.HpMax;
        }
        private int DamagePlayer(int min, int max, int bonus)
        {
            int damage = Rand.Next(min, max + 1) + bonus;
            player.HpCur -= damage;
            HitPointUpdate();
            CheckPlayerDeath();
            return damage;
        }
        //Arrow Events
        private void AirClick(object sender, MouseButtonEventArgs e)
        {
            Image arrow = (Image)sender;
            if (arrow.Height == 90)
            {
                tryfadeV = 2;
            }
            else if (Canvas.GetLeft(arrow) == 415)
            {
                Blacksmith.Visibility = Visibility.Visible;
            }
            else if (Canvas.GetLeft(arrow) == 210)
            {
                WitchHut.Visibility = Visibility.Visible;
            }
            else
            {
                tryfadeS = 2;
                Popup(Popupsheild, "The door is Locked");
            }
        }
        private void HilightAir(object sender, MouseEventArgs e)
        {
            Image air = (Image)sender;
            air.Source = new BitmapImage(new Uri("Assets/Mapobj/arrowredyell.png", UriKind.Relative));
        }
        // NPc Btn Interact
        private void SmithBtn(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if ((string)b.Content == "Leave")
            {
                Blacksmith.Visibility = Visibility.Hidden;
            }
            else if ((string)b.Content == "Talk")
            {
                tryfadeS = 2;
                Popup(Popupsheild, "The Blacksmith looks at you and says:\n Dont talk to me.");
            }
            else if ((string)b.Content == "Upgrade")
            {
                if (player.Level >= 3)
                {
                    tryfadeS = 2;
                    Popup(Popupsheild, "Success!");
                    player.MeleeCap += 1;
                }
                else
                {
                    tryfadeS = 2;
                    Popup(Popupsheild, "You are too weak.");
                }
            }
            else
            {
                tryfadeS = 2;
                Popup(Popupsheild, "You Have nothing to sell");
            }
        }
        private void WitchBtn(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if ((string)b.Content == "Leave")
            {
                WitchHut.Visibility = Visibility.Hidden;
            }
            else if ((string)b.Content == "Talk")
            {
                tryfadeS = 2;
                Popup(Popupsheild, "I'd love to! But im busy, come back later!");
            }
            else if ((string)b.Content == "Upgrade")
            {
                if (player.Level >= 3)
                {
                    tryfadeS = 2;
                    Popup(Popupsheild, "Success!");
                    player.MagBounus += 1;
                }
                else
                {
                    tryfadeS = 2;
                    Popup(Popupsheild, "Sorry! You need to get a bit stronger first!");
                }
            }
            else
            {
                tryfadeS = 2;
                Popup(Popupsheild, "You Have nothing to sell");
            }
        }
        //Village Stuf
        private void VilHilight(object sender, MouseEventArgs e)
        {
            Rect mrect = new(Canvas.GetLeft(mc), Canvas.GetTop(mc), mc.Width, mc.Height);
            Rect collid = new(Canvas.GetLeft(VillageIco), Canvas.GetTop(VillageIco), VillageIco.Width, VillageIco.Height);

            if (mrect.IntersectsWith(collid))
            {
                Image b = (Image)sender;
                b.Source = new BitmapImage(new Uri("Assets/Mapobj/oorrphousezyell.png", UriKind.Relative));
            }

        }
        private void VilUnHilight(object sender, MouseEventArgs e)
        {
            Image b = (Image)sender;
            b.Source = new BitmapImage(new Uri("Assets/Mapobj/oorrphousez.png", UriKind.Relative));
        }
        private void GoVil(object sender, MouseButtonEventArgs e)
        {
            Rect mrect = new(Canvas.GetLeft(mc), Canvas.GetTop(mc), mc.Width, mc.Height);
            Rect collid = new(Canvas.GetLeft(VillageIco), Canvas.GetTop(VillageIco), VillageIco.Width, VillageIco.Height);
            if (mrect.IntersectsWith(collid))
            {
                Initair();
                VillageMap.Visibility = Visibility.Visible;
                tryfadeV = 1;
            }
        }

        private void UnHilightAir(object sender, MouseEventArgs e)
        {
            Image air = (Image)sender;
            air.Source = new BitmapImage(new Uri("Assets/Mapobj/arrowred.png", UriKind.Relative));
        }
        private int HealPlayer(int min, int max, int bonus)
        {
            int healing = Rand.Next(min, max + 1) + bonus;
            player.HpCur += healing;
            if (player.HpCur > player.HpMax)
            {
                player.HpCur = player.HpMax;
            }
            HitPointUpdate();
            enemyTurn();
            return healing;
        }
        // Log/Leave
        private void LeaveFightscreen()
        {
            CheckExp();
            actbtn("Action", "Item", "Magic", "Flee");
            battleNotif = false;
            FightCanvas.Children.Remove(Slime);
            mapCan.Children.Remove(NotifEncounterSqu);
            mapCan.Children.Remove(EventNotif);
            LogCombatLog();
            tryfade = 2;
        }
        private void LogCombatLog()
        {
            string var = "";
            for (int i = 0; i < 4; i++)
            {
                var += CheckPlayrow(i).Content + "\n";
            }
            combatLog.Text = var;
        }
        //Death Check
        private void EnemyDeath()
        {
            if (enemy.HpCur <= 0)
            {
                DisplayFightText(enemy.Name + " Defeated! gained " + enemy.Exp + "xp");
                player.Exp += enemy.Exp;
                LeaveFightscreen();
            }
            else
            {
                enemyTurn();
            }
        }
        private void CheckPlayerDeath()
        {
            if (player.HpCur <= 0)
            {
                DeathWindow.Visibility = Visibility.Visible;
            }
        }
        //DeathReset
        private void RetryBtnCLick(object sender, RoutedEventArgs e)
        {
            player.Reset();
            LeaveFightscreen();
            DeathWindow.Visibility = Visibility.Hidden;
            MainMap.Visibility = Visibility.Hidden;
            Stats.Visibility = Visibility.Hidden;
            FightMenu.Visibility = Visibility.Hidden;
        }
        // Lvl Stuff?
        private void CheckExp()
        {
            if (player.Exp >= player.ExptLvl)
            {
                DisplayFightText("Level Up!");
                player.Levelup();
                PlacePlayerLvlandStats();
            }
        }
        // Notification Window
        private void Popup(Grid grid, string message)
        {
            popup = grid;
            popupText.Text = message;
        }



        private void MoveCasl(int i)
        {
            if (i == 2 && CurrentSpace[0] > 0 && castlMove[2] == 0)
            {
                CurrentSpace[0] -= 1;
            }
            else if (i == 4 && CurrentSpace[1] < 3 && castlMove[4] == 0)
            {
                CurrentSpace[1] += 1;
            }
            else if (i == 0 && CurrentSpace[1] > 0 && castlMove[0] == 0)
            {
                CurrentSpace[1] -= 1;
            }
            else if (i == 1 && CurrentSpace[0] < 4 && castlMove[1] == 0)
            {
                CurrentSpace[0] += 1;
            }
            CheckCastleTile();
            CastlPlay.SetValue(Grid.ColumnProperty, CurrentSpace[0]);
            CastlPlay.SetValue(Grid.RowProperty, CurrentSpace[1]);
        }
        private void CheckCastleTile()
        {

        }
    }
}
