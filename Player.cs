namespace OrangeRPGv._1
{
    class Player
    {
        public string Name { get; set; }
        public int Race { get; set; }
        public int Class { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int ExptLvl { get; set; }
        public int HpMax { get; set; }
        public int HpCur { get; set; }
        public int HpAmu { get; set; }
        public int[] Stats { get; set; }
        public int MeleeCap { get; set; }
        public int MagBounus { get; set; }
        public Player(string name, int race, int clas, int hp, int[] stats)
        {
            Name = name;
            Race = race;
            Class = clas;
            Level = 1;
            Exp = 0;
            ExptLvl = 20;
            HpMax = hp + stats[2];
            HpCur = hp + stats[2];
            HpAmu = hp;
            Stats = stats;
            // Damage Max Set
            MeleeCap = 5;
            MagBounus = 5;
        }
        public void Reset()
        {
            Name = "";
            Race = -1;
            Class = -1;
            Level = 1;
            Exp = 0;
            HpMax = 1;
            HpCur = 1;
            HpAmu = 1;
            for (int i = 0; i < 5; i++)
            {
                Stats[i] = 0;
            }
        }
        public void Levelup()
        {
            Level += 1;
            int temp = ExptLvl;
            ExptLvl = temp + temp * 2;
            //Stat Inc
            if (Race == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    Stats[i] += 1;
                }
            }
            else if (Class == 0)
            {
                Stats[0] += 2;
                Stats[2] += 2;
            }
            else if (Class == 1)
            {
                Stats[0] += 1;
                Stats[1] += 1;
                Stats[2] += 2;
            }
            else if (Class == 2)
            {
                Stats[3] += 2;
                Stats[2] += 2;
            }
            else
            {
                Stats[3] += 2;
                Stats[5] += 2;
            }
            //Hp Inc
            HpMax += HpAmu + Stats[2];
            HpCur = HpMax;
        }
    }
}
