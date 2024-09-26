using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace DND_TableTop
{
    internal class Merchant
    {
        Player player = new();
        List<string[]> itemList = [["Object", "Potion of heal", "10", "Heal all your party for 5 Health."],
                                   ["Object", "Strength Talisman", "20", "Add 3 Strength to a party member."],
                                   ["Object", "Magic Pearl", "20", "Add 2 Magic to a party member."],
                                   ["Capacity", "Heal", "30", "Heal your knight for 10 health."],
                                   ["Capacity", "War cry", "30", "Next attack from a party member is doubled."],
                                   ["Capacity", "Heal fountain", "30", "Heal all your party for 5 Health."],
                                   ["Capacity", "Darkness", "30", "Throw a poisonous knife that blind the ennemy for 2 turns and make it deal less damage."]];
        public void Encounter(Player _player)
        {
            player = _player;
            bool _continue = true;
            while (_continue)
            {
                ShowItems();
                Console.WriteLine(" ~ Your golds : " + player.gold);
                Console.WriteLine("\n What do you want to buy ? \n");
                string itemId = Console.ReadLine();
                if (itemId.ToLower() == "exit") { Leave(); return; }
                BuyItem(itemId);
            }
        }

        private void ShowItems()
        {
            Console.Clear();
            Console.WriteLine(" ~# The Merchant shows you his goods. #~\n");
            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - -\n");
            int i = 1;
            foreach (var item in itemList)
            {
                Console.WriteLine(i + ". [" + item[0] + "] : " + item[1] + " -> " + item[2] + " golds.");
                Console.WriteLine("   - " + item[3] + "\n");
                i++;
            }
            Console.WriteLine("EXIT : Leave the merchant \n");
            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - -\n\n\n");
        }

        private void BuyItem(string itemId)
        {
            int intID = Convert.ToInt32(itemId);
            ShowItems();
            for (int i = 1; i <= itemList.Count; i++)
            {
                if(intID == i)
                {
                    if (player.gold < Convert.ToInt32(itemList[i - 1][2]))
                    {
                        Console.WriteLine("You know you need this item but you can't afford it.. \n");
                        Console.ReadLine();
                        return;
                    }
                    Console.WriteLine("To which party member ?\n");
                    for (int j = 0; j < player.currentClasses.Count; j++)
                    {
                        Console.WriteLine(j + 1 + ": " + player.currentClasses[j].name);
                    }
                    int partyChoosed = Convert.ToInt32(Console.ReadLine()) - 1;
                    if (partyChoosed > player.currentClasses.Count - 1) { partyChoosed = player.currentClasses.Count - 1; }

                    if (itemList[i - 1][0] == "Capacity")
                    {
                        if (itemList[i - 1][1] == "Heal")
                        {
                            if(player.currentClasses[partyChoosed].name != "Knight")
                            {
                                ShowItems();
                                Console.WriteLine(" You can't give him this capacity.. \n");
                                Console.ReadLine();
                                return;
                            }
                            ShowItems();
                            player.currentClasses[partyChoosed].nameMagic.Add(["Heal", "0", "0"]);
                            Console.WriteLine(" Your Knight can use Heal to heal him.. \n");
                        }
                        if (itemList[i - 1][1] == "War cry")
                        {
                            if (player.currentClasses[partyChoosed].name != "Barbarian")
                            {
                                ShowItems();
                                Console.WriteLine(" You can't give him this capacity.. \n");
                                Console.ReadLine();
                                return;
                            }
                            ShowItems();
                            player.currentClasses[partyChoosed].nameMagic.Add(["War cry", "1", "0"]);
                            Console.WriteLine(" Your Barbarian can now increase the strength of your party.. \n");
                        }
                        if (itemList[i - 1][1] == "Heal fountain")
                        {
                            if (player.currentClasses[partyChoosed].name != "Wizard")
                            {
                                ShowItems();
                                Console.WriteLine(" You can't give him this capacity.. \n");
                                Console.ReadLine();
                                return;
                            }
                            ShowItems();
                            player.currentClasses[partyChoosed].nameMagic.Add(["Heal fountain", "1", "0"]);
                            Console.WriteLine(" Your Wizard can now heal your party.. \n");
                        }
                        if (itemList[i - 1][1] == "Darkness")
                        {
                            if (player.currentClasses[partyChoosed].name != "Rogue")
                            {
                                ShowItems();
                                Console.WriteLine(" You can't give him this capacity.. \n");
                                Console.ReadLine();
                                return;
                            }
                            ShowItems();
                            player.currentClasses[partyChoosed].nameMagic.Add(["Darkness", "0", "0"]);
                            Console.WriteLine(" The poison on this knife weaken your opponent.. \n");
                        }
                    }
                    else
                    {
                        if(itemList[i - 1][1] == "Potion of heal")
                        {
                            player.ChangeLife(5);
                            ShowItems();
                            Console.WriteLine(" Every members of your party drink the potion and feel a little bit restored.. \n");
                        }
                        if (itemList[i - 1][1] == "Strength Talisman")
                        {
                            player.currentClasses[partyChoosed].attack += 3;
                            ShowItems();
                            Console.WriteLine(" You gave your " + player.currentClasses[partyChoosed].name + " the Strength Talisman. He feel stronger now..\n");
                        }
                        if (itemList[i - 1][1] == "Magic Pearl")
                        {
                            player.currentClasses[partyChoosed].magic += 2;
                            ShowItems();
                            Console.WriteLine(" You gave your " + player.currentClasses[partyChoosed].name + " the Magic Pearl. He feel the magic flowing throught him..\n");
                        }
                    }
                    Console.ReadLine();
                    player.gold -= Convert.ToInt32(itemList[i - 1][2]);
                }
            }
        }

        private void Leave()
        {
            Console.WriteLine("You leave the merchant knowing its the last time you will see him.");
            Console.ReadLine();
        }
    }
}
