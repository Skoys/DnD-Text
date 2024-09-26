using DND_TableTop;

Player player = new();
Map dungeon = new Map();
Events eventManager = new Events();
List<Classes> roles = new();

bool visitMerchant = false;

bool _continue = true;

void Init()
{
    Classes knight = new Classes();
    knight.Init("Knight", 20, 4, 3, 7, 6, 4);
    Classes barbarian = new Classes();
    barbarian.Init("Barbarian", 25, 1, 1, 9, 4, 3);
    Classes wizard = new Classes();
    wizard.Init("Wizard", 15, 2, 5, 4, 9, 5);
    Classes rogue = new Classes();
    rogue.Init("Rogue", 15, 3, 3, 7, 5, 7);

    roles.Add(knight);
    knight.nameAttack.Add(["Sword Slash", "0", "0"]);
    roles.Add(barbarian);
    barbarian.nameAttack.Add(["Fist Fight", "0", "0"]);
    roles.Add(wizard);
    wizard.nameMagic.Add(["Fireball", "1", "0"]);
    roles.Add(rogue);
    rogue.nameAttack.Add(["Secret Knife", "0", "0"]);

    player.Init(roles);
    player.gold = 10;
    dungeon.Init(player.position, player);
    eventManager.Init();
}

void Update()
{
    dungeon.PrintMap();
    int nextMove = Convert.ToInt32(Console.ReadLine());
    if (!dungeon.NextMove(nextMove)) { return;  }
    char mapEvent = dungeon.CheckMap();
    if (mapEvent == '*')
    {
        eventManager.RandomEvent(player, eventManager, roles);
        if(player.currentClasses.Count == 0)
        {
            Console.WriteLine("Your party members are dead...\n");
            Console.WriteLine("     * GAME OVER *\n");
            Console.ReadLine();
            _continue = false;
            return;
        }
    }
    if (mapEvent == 'M' && visitMerchant == false)
    {
        Merchant merchant = new Merchant();
        merchant.Encounter(player);
        visitMerchant = true;
    }
    if (mapEvent == 'B')
    {
        eventManager.BossFight(player, eventManager, roles);
        if (player.currentClasses.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("Your party members are dead...\n");
            Console.WriteLine("     * GAME OVER *\n");
            Console.ReadLine();
            _continue = false;
            return;
        }
        Console.WriteLine("-#~~#-#~~#-#~~#-#~~#-#~~#-#~~#-#~~#-\n");
        Console.WriteLine("      * CONGLATURATION ! *\n");
        Console.WriteLine("     You Finished The Game !   \n");
        Console.WriteLine("          Well Played !   \n");
        Console.WriteLine("-#~~#-#~~#-#~~#-#~~#-#~~#-#~~#-#~~#-\n");
        Console.ReadLine();
        _continue = false;
        return;
    }
}

Init();
while (_continue)
{
    Update();
}