using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            int warCry = 1;
            float darkness = 1;   
            while (ennemies.Count > 0 && player.currentClasses.Count > 0)
            {
                if (playerTurn)
                {
                    int choice = ChooseParty() - 1;
                    if (choice + 1 > player.currentClasses.Count) { choice = 0; }

                    int attackChoice = ChooseAttack(choice);
                    if(attackChoice > player.currentClasses[choice].nameAttack.Count + player.currentClasses[choice].nameMagic.Count) { attackChoice = 1; }

                    int[] attackDamage;
                    string attackName;
                    if(attackChoice - 1 < player.currentClasses[choice].nameAttack.Count)
                    {
                        attackDamage = [player.currentClasses[choice].attack * warCry, 0, Convert.ToInt32(player.currentClasses[choice].nameAttack[attackChoice - 1][1])];
                        attackName = player.currentClasses[choice].nameAttack[attackChoice - 1][0];
                    }
                    else
                    {
                        attackChoice -= player.currentClasses[choice].nameAttack.Count;
                        attackDamage = [0, player.currentClasses[choice].magic * warCry, Convert.ToInt32(player.currentClasses[choice].nameMagic[attackChoice - 1][1])];
                        attackName = player.currentClasses[choice].nameMagic[attackChoice - 1][0];
                    }
                    warCry = 1;

                    if (attackName == "Heal")
                    {
                        DrawFrame();     
                        player.currentClasses[choice].life += 10;
                        Console.WriteLine(" Your Knight Healed 10 health !..");
                    }
                    else if (attackName == "War cry")
                    {
                        DrawFrame();
                        warCry++;
                        Console.WriteLine(" The next attack deal double damages..");
                    }
                    else if (attackName == "Heal fountain")
                    {
                        DrawFrame();
                        player.ChangeLife(5);
                        Console.WriteLine(" The magic of your wizard healed your party 5 health..");
                    }
                    else
                    {
                        DrawFrame();
                        Console.WriteLine("    ~ Against which ennemy");
                        for (int i = 0; i < ennemies.Count; i++)
                        {
                            Console.WriteLine(i + 1 + ": " + ennemies[i].name);
                        }
                        int ennemyChoice = Convert.ToInt32(Console.ReadLine());
                        if (ennemyChoice > ennemies.Count) { ennemyChoice = 1; }

                        DrawFrame();
                        if (attackName == "Darkness")
                        {
                            darkness = 0.5f;
                            Console.WriteLine(" Your rogue throw a poisonous knife at the ennemy..");
                        }

                        if (attackDamage[2] == 1)
                        {
                            for (int i = 0; i < ennemies.Count; i++)
                            {
                                int attack = (attackDamage[0] - ennemies[i].physicDefense < 0 ? 0 : attackDamage[0] - ennemies[i].physicDefense) + (attackDamage[1] - ennemies[i].magicDefense < 0 ? 0 : attackDamage[1] - ennemies[i].magicDefense);
                                Console.WriteLine("     X " + ennemies[i].name + " took " + attack + " damages!");
                                ennemies[i].life -= attack;
                                if (ennemies[i].life <= 0)
                                {
                                    Console.WriteLine("\n     X " + ennemies[i].name + " is dead You gained " + ennemies[i].dodge + " coins! ..\n");
                                    player.gold += ennemies[i].dodge;
                                    ennemies.Remove(ennemies[i]);
                                    i--;
                                }
                            }
                        }
                        else if (attackDamage[2] == 0)
                        {
                            int attack = (attackDamage[0] - ennemies[ennemyChoice - 1].physicDefense < 0 ? 0 : attackDamage[0] - ennemies[ennemyChoice - 1].physicDefense) + (attackDamage[1] - ennemies[ennemyChoice - 1].magicDefense < 0 ? 0 : attackDamage[1] - ennemies[ennemyChoice - 1].magicDefense);
                            Console.WriteLine("     X " + ennemies[ennemyChoice - 1].name + " took " + attack + " damages! ..");
                            ennemies[ennemyChoice - 1].life -= attack;
                            if (ennemies[ennemyChoice - 1].life <= 0)
                            {
                                Console.WriteLine("\n     X " + ennemies[ennemyChoice - 1].name + " is dead You gained " + ennemies[ennemyChoice - 1].dodge + " coins! ..\n");
                                player.gold += ennemies[ennemyChoice - 1].dodge;
                                ennemies.Remove(ennemies[ennemyChoice - 1]);
                            }
                        }
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
                        int target = random.Next(player.currentClasses.Count);
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
                                nextAtt = [ennemies[ennAttack].nameMagic[0][0], "0", Convert.ToString(ennemies[ennAttack].magic), ennemies[ennAttack].nameMagic[0][1]];
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
                                nextAtt = [ennemies[ennAttack].nameMagic[0][0], "0", Convert.ToString(ennemies[ennAttack].magic), ennemies[ennAttack].nameMagic[0][1]];
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
                            Console.WriteLine("    ~ " + ennemies[ennAttack].name + " used " + nextAtt[0] + ".");

                            if (nextAtt[3] == "1")
                            {
                                for (int i = 0; i < player.currentClasses.Count; i++)
                                {
                                    float attack = (Convert.ToInt32(nextAtt[1]) - player.currentClasses[i].physicDefense < 0 ? 0 : Convert.ToInt32(nextAtt[1]) - player.currentClasses[i].physicDefense) + (Convert.ToInt32(nextAtt[2]) - player.currentClasses[i].magicDefense < 0 ? 0 : Convert.ToInt32(nextAtt[2]) - player.currentClasses[i].magicDefense) * darkness;
                                    player.currentClasses[i].life -= Convert.ToInt32(MathF.Floor(attack));
                                    Console.WriteLine("    X " + player.currentClasses[i].name + " was hit for " + Convert.ToInt32(MathF.Floor(attack)) + " damages !");
                                    if (player.currentClasses[i].life <= 0)
                                    {
                                        Console.WriteLine("\n     X " + player.currentClasses[i].name + " is dead! ..");
                                        player.currentClasses.Remove(player.currentClasses[i]);
                                        i--;
                                    }
                                }
                            }
                            else if (nextAtt[3] == "0")
                            {
                                float attack = (Convert.ToInt32(nextAtt[1]) - player.currentClasses[target].physicDefense < 0 ? 0 : Convert.ToInt32(nextAtt[1]) - player.currentClasses[target].physicDefense) + (Convert.ToInt32(nextAtt[2]) - player.currentClasses[target].magicDefense < 0 ? 0 : Convert.ToInt32(nextAtt[2]) - player.currentClasses[target].magicDefense) * darkness;
                                player.currentClasses[target].life -= Convert.ToInt32(MathF.Floor(attack));
                                Console.WriteLine("    X " + player.currentClasses[target].name + " was hit for " + Convert.ToInt32(MathF.Floor(attack)) + " damages !");
                                if (player.currentClasses[target].life <= 0)
                                {
                                    Console.WriteLine("\n     X " + player.currentClasses[target].name + " is dead! ..");
                                    _player.currentClasses.Remove(player.currentClasses[target]);
                                }
                            }
                        }
                        if (darkness < 1) { darkness += 0.25f; }
                        Console.ReadLine();
                        playerTurn = true;
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
                Console.WriteLine(i + 1 + ": " + player.currentClasses[i].name + " - " + player.currentClasses[i].life);
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
            if (monster == "Dragon")
            {
                Console.WriteLine("         /|    /   \n" +
                                  "      __/_|/ // |  \n" +
                                  "    //o  o   ] /   \n" +
                                  "   [ °°      ]    \n" +
                                  "     v v |  |        \n" +
                                  "     ___/  /|        \n" +
                                  "  /__ | | | |         ");
            }
        }

    }
}
