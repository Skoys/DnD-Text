using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND_TableTop
{
    internal class Fight
    {
        private Player player;
        List<Classes> ennemies;
        Events events;

        public void Encounter(Player _player, List<Classes> _ennemies, Events _event)
        { 
            player = _player;
            ennemies = _ennemies;
            events = _event;
            bool playerTurn = true;
            while (ennemies.Count > 0 || player.currentClasses.Count > 0)
            {
                if (playerTurn)
                {
                    int choice = ChooseParty() - 1;
                    if (choice + 1 > player.currentClasses.Count) { return; }

                    int attackChoice = ChooseAttack(choice);
                    if(attackChoice > player.currentClasses[choice].nameAttack.Count + player.currentClasses[choice].nameMagic.Count) { return; }

                    int[] attackDamage;    
                    if(attackChoice - 1 < player.currentClasses[choice].nameAttack.Count)
                    {
                        attackDamage = [player.currentClasses[choice].attack, 0, Convert.ToInt32(player.currentClasses[choice].nameAttack[attackChoice - 1][1])];
                    }
                    else
                    {
                        attackChoice -= player.currentClasses[choice].nameAttack.Count;
                        attackDamage = [0, player.currentClasses[choice].magic, Convert.ToInt32(player.currentClasses[choice].nameMagic[attackChoice - 1][1])];
                    }

                    DrawFrame();
                    Console.WriteLine("    ~ Against which ennemy");
                    for (int i = 0; i < ennemies.Count; i++)
                    {
                        Console.WriteLine(i + 1 + ": " + ennemies[i].name);
                    }
                    int ennemyChoice = Convert.ToInt32(Console.ReadLine());
                    if (ennemyChoice > ennemies.Count) { return; }

                    DrawFrame ();
                    if (attackDamage[2] == 1)
                    {
                        foreach(Classes e in ennemies)
                        {
                            int attack = (attackDamage[0] - e.physicDefense < 0 ? 0 : attackDamage[0] - e.physicDefense) + (attackDamage[1] - e.magicDefense < 0 ? 0 : attackDamage[1] - e.magicDefense);
                            Console.WriteLine("     X " + e.name + " took " + attack + " damages!");
                            e.life -= attack;
                        }
                    }
                    else if (attackDamage[2] == 0)
                    {
                        int attack = (attackDamage[0] - ennemies[ennemyChoice - 1].physicDefense < 0 ? 0 : attackDamage[0] - ennemies[ennemyChoice - 1].physicDefense) + (attackDamage[1] - ennemies[ennemyChoice - 1].magicDefense < 0 ? 0 : attackDamage[1] - ennemies[ennemyChoice - 1].magicDefense);
                        Console.WriteLine("     X " + ennemies[ennemyChoice - 1].name + " took " + attack + " damages! ..");
                        ennemies[ennemyChoice - 1].life -= attack;
                    }
                    if(ennemies[ennemyChoice - 1].life <= 0)
                    {
                        ennemies.RemoveAt(ennemyChoice - 1);
                        Console.WriteLine("\n     X " + ennemies[ennemyChoice - 1].name + " is dead! ..");
                    }
                    Console.ReadLine();
                    playerTurn = false;
                }
                else
                {
                    if(ennemies.Count > 0)
                    {
                        Random random = new Random();
                        int ennAttack = random.Next(ennemies.Count);
                        int att = random.Next(2);
                        string[] nextAtt;
                        if (ennemies[ennAttack].nameAttack.Count + ennemies[ennAttack].nameMagic.Count > 1)
                        {
                            if (att == 0)
                            {
                                nextAtt = [ennemies[ennAttack].nameAttack[0][0], Convert.ToString(ennemies[ennAttack].attack), "0", ennemies[ennAttack].nameAttack[0][1]];
                            }
                            else
                            {
                                nextAtt = [ennemies[ennAttack].nameMagic[0][0], "0", Convert.ToString(ennemies[ennAttack].attack), ennemies[ennAttack].nameMagic[0][1]];
                            }
                        }
                        else
                        {
                            if (ennemies[ennAttack].nameAttack.Count > 0)
                            {
                                nextAtt = [ennemies[ennAttack].nameAttack[0][0], Convert.ToString(ennemies[ennAttack].attack), "0", ennemies[ennAttack].nameAttack[0][1]];
                            }
                            else
                            {
                                nextAtt = [ennemies[ennAttack].nameMagic[0][0], "0", Convert.ToString(ennemies[ennAttack].attack), ennemies[ennAttack].nameMagic[0][1]];
                            }
                        }
                        if (nextAtt[0] == "Summon")
                        {
                            ennemies.Add(events.monsters[1]);
                            DrawFrame();
                            Console.WriteLine("    ~ Lich used Summon. A skeleton rise from the ground ..");
                        }
                        else
                        {
                            DrawFrame();
                            Console.WriteLine("    ~ " + ennemies[ennAttack].name " used " + nextAtt[0] + ".");
                        }
                    }
                }
            }
        }

        private int ChooseAttack(int choice)
        {
            DrawFrame();
            Console.WriteLine("    ~ " + player.currentClasses[choice].name);
            Console.WriteLine("[Attack] ");
            int i = 1;
            foreach(var a in player.currentClasses[choice].nameAttack)
            {
                Console.WriteLine(" " +i + ": " + a[0]);
                i++;
            }            
            Console.WriteLine("[Magic] ");
            foreach(var a in player.currentClasses[choice].nameMagic)
            {
                Console.WriteLine(" " + i + ": " + a[0]);
                i++;
            }
            Console.WriteLine("\nChoose an attack..");
            return Convert.ToInt32(Console.ReadLine());
        }

        private int ChooseParty()
        {
            DrawFrame();
            for (int i = 0; i < player.currentClasses.Count; i++)
            {
                Console.WriteLine(i + 1 + ": " + player.currentClasses[i].name);
            }
            Console.WriteLine("\nChoose a party member..");
            return Convert.ToInt32(Console.ReadLine());
        }

        private void DrawFrame()
        {
            Console.Clear();
            foreach (Classes e in ennemies)
            {
                DrawMonster(e.name);
                Console.WriteLine("\n    ~ " + e.name + " ~ \n");
            }
        }

        private void DrawMonster(string monster)
        {
            if(monster == "Goblin")
            {
                Console.WriteLine("     /|__/|  \n" +
                                  "     €  € /   \n" +
                                  "   <|-/ /| \n" +
                                  "       | | ");
            }
            if (monster == "Skeleton")
            {
                Console.WriteLine("     ^^^^^      \n" +
                                  "     O ^ O |  /   \n" +
                                  "      '''|   /    \n" +
                                  "       /-T-oT      \n" +
                                  "        / N       ");
            }
            if (monster == "Slime")
            {
                Console.WriteLine("     _______ \n" +
                                  "    /       S   \n" +
                                  "   | O   O   S  \n" +
                                  "   C       u S  \n" +
                                  "   U   uu U  U ");
            }
            if (monster == "Lich")
            {
                Console.WriteLine("    |nOn|nn|      \n" +
                                  "     O ^ O |  O   \n" +
                                  "      '''|    T  \n" +
                                  "     /    |---|   \n" +
                                  "     |/ N |   |  ");
            }
            if (monster == "Golem")
            {
                Console.WriteLine("      _______    \n" +
                                  "     [       ]    \n" +
                                  "     [ o  o  ]    \n" +
                                  "    ____/ /|        \n" +
                                  "   [HHHO] N         "); 
            }
        }

    }
}
