using DND_TableTop;

Player player = new();
Map dungeon = new Map();
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
    roles.Add(barbarian);
    roles.Add(wizard);
    roles.Add(rogue);

    //player.Init(roles);
    dungeon.Init(player.position);
}

void Update()
{
    dungeon.PrintMap();
    string nextMove = Console.ReadLine();
}

Init();
while (_continue)
{
    Update();
}