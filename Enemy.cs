namespace OrangeRPGv._1
{
    internal class Enemy
    {
        public int HpCur { get; set; }
        public int HpMax { get; set; }
        public int Exp { get; set; }
        public string Name { get; set; }
        public Enemy(int hp, int exp, string name)
        {

            Exp = exp;
            HpCur = hp;
            HpMax = hp;
            Name = name;
        }
    }
}
