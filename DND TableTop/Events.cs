using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND_TableTop
{
    internal class Events
    {
        Fight fight = new();
        Player player = new();
        public List<Classes> monsters = new List<Classes>();

        public void Init()
        {
            fight = new Fight();

            Classes goblin = new Classes();
            goblin.Init("Goblin", 6, 2, 1, 5, 1, 2);
            goblin.nameAttack.Add(["Punch", "0"]);
            monsters.Add(goblin);
            Classes skeleton = new Classes();
            skeleton.Init("Skeleton", 10, 3, 5, 6, 1, 5);
            skeleton.nameAttack.Add(["Sword", "0"]);
            monsters.Add(skeleton);
            Classes slime = new Classes();
            slime.Init("Slime", 12, 3, 5, 6, 6, 8);
            slime.nameAttack.Add(["Body Slam", "0"]);
            slime.nameMagic.Add(["Explosion", "1"]);
            monsters.Add(slime);
            Classes lich = new Classes();
            lich.Init("Lich", 7, 3, 6, 1, 7, 12);
            lich.nameMagic.Add(["Dark Ray", "0"]);
            lich.nameAttack.Add(["Summon", "0"]);
            monsters.Add(lich);
            Classes golem = new Classes();
            golem.Init("Golem", 15, 5, 2, 7, 1, 15);
            golem.nameAttack.Add(["Throw Rock", "0"]);
            monsters.Add(golem);
        }

        public void Rooms(int roomNbr)
        {
            if (roomNbr == 0)
            {
                Console.Clear();
                Console.WriteLine("        --------------------------------- ");
                Console.WriteLine("       |   *                         *   |");
                Console.WriteLine("       |        -----------------        |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |        -----------------        |");
                Console.WriteLine("       |     * .,,,.;??..?...??... *     |");
                Console.WriteLine("       |  *.,.??;?,?;.??.;.?...?;...?.*  |");
                Console.WriteLine("        --------------------------------- \n");
            }
            if (roomNbr == 1)
            {
                Console.Clear();
                Console.WriteLine("        --------------------------------- ");
                Console.WriteLine("       |   *                         *   |");
                Console.WriteLine("       |        -----------------        |");
                Console.WriteLine("       |   /_____________        |       |");
                Console.WriteLine("       |       |          __________     |");
                Console.WriteLine("       |       |         ===========/    |");
                Console.WriteLine("       |      ----------         |       |");
                Console.WriteLine("       | _______________       ========| |");
                Console.WriteLine("       | /=================      |       |");
                Console.WriteLine("       |       |     _________________   |");
                Console.WriteLine("       |        -----------------        |");
                Console.WriteLine("       |     * .,,,.;??..?...??... *     |");
                Console.WriteLine("       |  *.,.??;?,?;.??.;.?...?;...?.*  |");
                Console.WriteLine("        --------------------------------- \n");
            }
            if (roomNbr == 2)
            {
                Console.Clear();
                Console.WriteLine("        --------------------------------- ");
                Console.WriteLine("       |   *                         *   |");
                Console.WriteLine("       |        -----------------        |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |                 |       |");
                Console.WriteLine("       |       |     _______     |       |");
                Console.WriteLine("       |        ---|___   ___| --        |");
                Console.WriteLine("       |     *     |   [o]   |     *     |");
                Console.WriteLine("       |  *        (_________)        *  |");
                Console.WriteLine("        --------------------------------- \n");
            }
        }

        public void RandomEvent(Player _player, Events _events, List<Classes> roles)
        {
            player = _player;
            Random random = new Random();
            int randEvent = random.Next(3);
            if(randEvent == 0)
            {
                Trap();
                return;
            }
            if(randEvent == 1)
            {
                List<Classes> attackList = new List<Classes>();
                randEvent = random.Next(monsters.Count);
                if (monsters[randEvent].name == "Goblin")
                {
                    attackList.Add(monsters[randEvent]);
                    attackList.Add(monsters[randEvent]);
                    attackList.Add(monsters[randEvent]);
                }
                else { attackList.Add(monsters[randEvent]); }

                fight.Encounter(_player, attackList, _events);
                return;
            }
            Chests(roles);
            return;
        }

        public void Trap()
        {
            Random random = new();
            int rand = random.Next(5);
            if(rand == 0)
            {
                Rooms(1);
                Console.WriteLine("The floor collapse under your feets.\n\n  - Roll the dice to Dodge it .. ");
                Console.ReadLine();
                for (int i = 0; i < 10; i++)
                {
                    rand = random.Next(20);
                    Rooms(1);
                    Console.WriteLine("     ~ "+ rand + " ~");
                }
                Rooms(1);
                int playerChara = GetPlayerBestAttribute(5);
                Console.WriteLine("     ~ " + rand + " ~ + " + playerChara + " = " + (rand + playerChara) + "\n");
                if (rand + playerChara >= 14)
                {
                    Console.WriteLine("You dodge the trap with no difficulty, you can continue your path..");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("You tried to dodge the trap but you were unfortunaly too slow.");
                Console.WriteLine("Everyone in your party take 3 damages..");
                player.ChangeLife(-3);
                Console.ReadLine();
            }
            if(rand == 1)
            {
                Rooms(1);
                Console.WriteLine("The room starts to heat you understand it's a fireball.\n\n  - Roll the dice to Protect you with Magic .. ");
                Console.ReadLine();
                for (int i = 0; i < 10; i++)
                {
                    rand = random.Next(20);
                    Rooms(1);
                    Console.WriteLine("     ~ " + rand + " ~");
                }
                Rooms(1);
                int playerChara = GetPlayerBestAttribute(4);
                Console.WriteLine("     ~ " + rand + " ~ + " + playerChara + " = " + (rand + playerChara) + "\n");
                if (rand + playerChara >= 10)
                {
                    Console.WriteLine("You cast a protection spell that protect you from the fireball.");
                    Console.WriteLine("The road is now clear. You can continue..");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("The fireball hit you and consume some of your clothe..");
                Console.WriteLine("Everyone in your party take 4 damages..");
                player.ChangeLife(-4);
                Console.ReadLine();
            }
            if(rand == 2)
            {
                Rooms(1);
                Console.WriteLine("The wind is blowing in this room. You feel your coins escaping your pockets.\n\n  - Roll the dice to see how many coins escape.. ");
                Console.ReadLine();
                for (int i = 0; i < 10; i++)
                {
                    rand = random.Next(10);
                    Rooms(1);
                    Console.WriteLine("     ~ " + rand + " ~");
                }
                Rooms(1);
                Console.WriteLine("     ~ " + rand + " ~ \n");
                if(rand > player.gold)
                {
                    rand += player.gold - rand;
                }
                Console.WriteLine("You lost " + rand + " coins in this room..");
                player.gold -= rand;
                Console.ReadLine();
                return;
            }
            if(rand == 3)
            {
                Rooms(1);
                Console.WriteLine("The room feels empty. You tried to make a step in but you suddently understand that you have activated a trap.\n\n  - Roll the dice to Defend yourself .. ");
                Console.ReadLine();
                for (int i = 0; i < 10; i++)
                {
                    rand = random.Next(20);
                    Rooms(1);
                    Console.WriteLine("     ~ " + rand + " ~");
                }
                Rooms(1);
                int playerChara = GetPlayerBestAttribute(3);
                Console.WriteLine("     ~ " + rand + " ~ + " + playerChara + " = " + (rand + playerChara) + "\n");
                if (rand + playerChara >= 12)
                {
                    Console.WriteLine("A giant axe come out from the wall but it's no match against your strength and armor..");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("A giant axe come out from the wall. It hits you with it's full strength and blow you away to the nearest wall.");
                Console.WriteLine("Everyone in your party take 4 damages..");
                player.ChangeLife(-4);
                Console.ReadLine();
            }
            if(rand == 4)
            {
                Rooms(1);
                Console.WriteLine("The room is full of holes. Each one has something shiny in it. You feel nothing can protect you from it.\n\n  - Roll the dice for your luck .. ");
                Console.ReadLine();
                for (int i = 0; i < 10; i++)
                {
                    rand = random.Next(20);
                    Rooms(1);
                    Console.WriteLine("     ~ " + rand + " ~");
                }
                Rooms(1);
                Console.WriteLine("     ~ " + rand + " ~ \n");
                if (rand >= 8)
                {
                    Console.WriteLine("Your luck saved you from being impaled. You feel relieved..");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("You made a bad footstep and every shiny thing come rushing to you. It's speed and strength is impaling you.");
                Console.WriteLine("Everyone in your party take 5 damages..");
                player.ChangeLife(-5);
                Console.ReadLine();
            }
        }

        public void Chests(List<Classes> roles)
        {
            Random random = new();
            int rand = random.Next(4);
            if (rand == 0)
            {
                Rooms(0);
                Console.WriteLine("The Room is empty.\n\n  - Type anything to continue..");
                Console.ReadLine();
            }
            if (rand == 1)
            {
                Rooms(2);
                Console.WriteLine("A chest is in the center of the room. You opened it.");
                Console.ReadLine();

                Rooms(2);
                Console.WriteLine("Wind come from the chest. You feel lighter and relieved.");
                Console.WriteLine("Everyone in your party gain 5 health..");
                player.ChangeLife(5);
                Console.ReadLine();
            }
            if (rand == 2)
            {
                Rooms(2);
                Console.WriteLine("Someone stand in the corner of the room.. ");
                Console.ReadLine();

                Rooms(2);
                rand = random.Next(roles.Count);
                player.currentClasses.Add(roles[rand]);
                Console.WriteLine("A " + roles[rand].name + " joined your party ! .. ");
                Console.ReadLine();
                return;
            }
            if (rand == 3)
            {
                Rooms(2);
                Console.WriteLine("A chest is in the center of the room. You opened it.");
                Console.ReadLine();

                Rooms(2);
                rand = random.Next(20);
                Console.WriteLine("Someone already took his content.");
                Console.WriteLine("You find " + rand + " gold left in it.");
                player.gold += rand;
                Console.ReadLine();
            }
        }

        private int GetPlayerBestAttribute(int which)
        {
            int best = 0;
            foreach (Classes i in player.currentClasses)
            {
                if(which == 1 && i.physicDefense > best)
                {
                    best = i.physicDefense;
                }
                if (which == 2 && i.magicDefense > best)
                {
                    best = i.magicDefense;
                }
                if (which == 3 && i.attack > best)
                {
                    best = i.attack;
                }
                if (which == 4 && i.magic > best)
                {
                    best = i.magic;
                }
                if (which == 5 && i.dodge > best)
                {
                    best = i.dodge;
                }
            }
            return best;
        }
    }
}
