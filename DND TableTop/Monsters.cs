using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND_TableTop
{
    internal class Monsters
    {
        public string name = "";
        public int life;
        public int physicDefense;
        public int magicDefense;
        public int attack;
        public int magic;
        public int dodge;


        public void Init(string _name, int _life, int _phyDef, int _magDef, int _att, int _mag, int _dodge)
        {
            name = _name;
            life = _life;
            physicDefense = _phyDef;
            magicDefense = _magDef;
            attack = _att;
            magic = _mag;
            dodge = _dodge;
        }
    }
}
