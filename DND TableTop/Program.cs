using DND_TableTop;

Player player = new();
Map dungeon = new Map();
Events eventManager = new Events();
List<Classes> roles = new();

bool _continue = true;

void Init()
{
    Classes knight = new Classes();
    knight.Init("Knight", 20, 4, 3, 5, 4, 4);
    Classes barbarian = new Classes();
    barbarian.Init("Barbarian", 25, 1, 1, 7, 2, 3);
    Classes wizard = new Classes();
    wizard.Init("Wizard", 15, 2, 5, 2, 7, 5);
    Classes rogue = new Classes();
    rogue.Init("Rogue", 15, 3, 3, 5, 3, 7);

    roles.Add(knight);
    knight.nameAttack.Add(["Sword Slash", "0", "0"]);
    roles.Add(barbarian);
    knight.nameAttack.Add(["Fist Fight", "0", "0"]);
    roles.Add(wizard);
    knight.nameAttack.Add(["Fireball", "1", "0"]);
    roles.Add(rogue);
    knight.nameAttack.Add(["Secret Knife", "0", "0"]);

    player.Init(roles);
    dungeon.Init(player.position, player);
}

void Update()
{
    dungeon.PrintMap();
    string nextMove = Console.ReadLine();
    if (!dungeon.NextMove(nextMove)) { return;  }
    char mapEvent = dungeon.CheckMap();
    if (mapEvent == '*')
    {
        eventManager.RandomEvent(player);
    }
}

Init();
while (_continue)
{
    Update();
}