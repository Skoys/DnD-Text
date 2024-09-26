using static System.Net.Mime.MediaTypeNames;

namespace DND_TableTop
{
    internal class Map
    {
        Player player;
        char[,] map = new char[9,9];
        char[,] discoveredMap = new char[9, 9];
        int[,] treeMap = new int[9, 9];
        List<int[]> roomsList = new List<int[]>();

        int mapLen = 20;

        int[] playerPosition = { 8, 4 };

        public void Init(int[] playerPos, Player _player)
        {
            player = _player;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    map[i,j] = '@';
                    discoveredMap[i, j] = '@';
                    treeMap[i, j] = 0;
                }
            }

            int[] currentPos = playerPos;
            map[currentPos[0], currentPos[1]] = 'O';
            int iterationDebug = 0;
            while (roomsList.Count < mapLen)
            {
                currentPos = AddRoom(currentPos);
                
                if (roomsList.Count == mapLen / 2 - 2) { map[currentPos[0], currentPos[1]] = 'M'; }
                iterationDebug++;
                if (iterationDebug > 20)
                {
                    mapLen -= 1;
                }
            }
            Tree([8,4], 0);

            int longest = 0;
            List<int[]> bossPos = new List<int[]>();
            foreach (int[] rooms in roomsList)
            {
                if (treeMap[rooms[0], rooms[1]] > longest)
                {
                    longest = treeMap[rooms[0], rooms[1]];
                    bossPos.Clear();
                }
                if (treeMap[rooms[0], rooms[1]] == longest)
                {
                    bossPos.Add(rooms);
                }
            }
            Random rand = new Random();
            int rnd = rand.Next(bossPos.Count);
            map[bossPos[rnd][0], bossPos[rnd][1]] = 'B';
            NextMove(5);
        }

        private int[] AddRoom(int[] currentPos)
        {
            Random random = new Random();
            int next = random.Next(5);
            if ((next == 0 || next == 5) && currentPos[0] > 0)
            {
                if (map[currentPos[0] - 1, currentPos[1]] == '@')
                {
                    currentPos[0] -= 1;
                    map[currentPos[0], currentPos[1]] = '*';
                    roomsList.Add([currentPos[0], currentPos[1]]);
                }
            }
            else if (next == 1 && currentPos[0] + 1 < map.GetLength(0) && roomsList.Count >= 6)
            {
                if (map[currentPos[0] + 1, currentPos[1]] == '@')
                {
                    currentPos[0] += 1;
                    map[currentPos[0], currentPos[1]] = '*';
                    roomsList.Add([currentPos[0], currentPos[1]]);
                }
            }
            else if (next == 2 && currentPos[1] - 1 > 0)
            {
                if (map[currentPos[0], currentPos[1] - 1] == '@')
                {
                    currentPos[1] -= 1;
                    map[currentPos[0], currentPos[1]] = '*';
                    roomsList.Add([currentPos[0], currentPos[1]]);
                }
            }
            else if (next == 3 && currentPos[1] + 1 < map.GetLength(1))
            {
                if (map[currentPos[0], currentPos[1] + 1] == '@')
                {
                    currentPos[1] += 1;
                    map[currentPos[0], currentPos[1]] = '*';
                    roomsList.Add([currentPos[0], currentPos[1]]);
                }
            }
            return currentPos;
        }

        void Tree(int[] pos, int iteration)
        {
            if (map[pos[0], pos[1]] != '@' && treeMap[pos[0], pos[1]] == 0)
            {
                treeMap[pos[0], pos[1]] = iteration;
            }
            else
            {
                return;
            }
            List<int[]> way = new List<int[]>();

            if (pos[1] > 0)
            {
                way.Add(new int[] { pos[0], pos[1] - 1 });
            }

            if (pos[1] < treeMap.GetLength(1) - 1)
            {
                way.Add(new int[] { pos[0], pos[1] + 1 });
            }

            if (pos[0] > 0)
            {
                way.Add(new int[] { pos[0] - 1, pos[1] });
            }

            if (way.Any())
            {
                foreach (int[] i in way)
                {
                    Tree(i, iteration + 1);
                }
            }

            return;
        }

        public bool NextMove(int move)
        {
            bool moved = false;
            discoveredMap[playerPosition[0], playerPosition[1]] = 'O';

            if (move == 8 && playerPosition[0] > 0)
            {
                if (map[playerPosition[0] - 1, playerPosition[1]] == '@')
                {
                    return moved;
                }
                playerPosition = [playerPosition[0] - 1, playerPosition[1]];
                moved = true;
            }
            if (move == 2 && playerPosition[0] < map.GetLength(0) - 1)
            {
                if (map[playerPosition[0] + 1, playerPosition[1]] == '@')
                {
                    return moved;
                }
                playerPosition = [playerPosition[0] + 1, playerPosition[1]];
                moved = true;
            }
            if (move == 4 && playerPosition[1] > 0)
            {
                if (map[playerPosition[0], playerPosition[1] - 1] == '@')
                {
                    return moved;
                }
                playerPosition = [playerPosition[0], playerPosition[1] - 1];
                moved = true;
            }
            if (move == 6 && playerPosition[1] < map.GetLength(1) - 1)
            {
                if (map[playerPosition[0], playerPosition[1] + 1] == '@')
                {
                    return moved;
                }
                playerPosition = [playerPosition[0], playerPosition[1] + 1];
                moved = true;
            }

            if (playerPosition[0] > 0)
            {
                if(discoveredMap[playerPosition[0] - 1, playerPosition[1]] == '@' && map[playerPosition[0] - 1, playerPosition[1]] == '*')
                {
                    discoveredMap[playerPosition[0] - 1, playerPosition[1]] = '?';
                }
                else if (map[playerPosition[0] - 1, playerPosition[1]] == 'M')
                {
                    discoveredMap[playerPosition[0] - 1, playerPosition[1]] = 'M';
                }
                else if (map[playerPosition[0] - 1, playerPosition[1]] == 'B')
                {
                    discoveredMap[playerPosition[0] - 1, playerPosition[1]] = 'B';
                }
            }
            if (playerPosition[0] < map.GetLength(0) - 1)
            {
                if (discoveredMap[playerPosition[0] + 1, playerPosition[1]] == '@' && map[playerPosition[0] + 1, playerPosition[1]] == '*')
                {
                    discoveredMap[playerPosition[0] + 1, playerPosition[1]] = '?';
                }
                else if (map[playerPosition[0] + 1, playerPosition[1]] == 'M')
                {
                    discoveredMap[playerPosition[0] + 1, playerPosition[1]] = 'M';
                }
                else if (map[playerPosition[0] + 1, playerPosition[1]] == 'B')
                {
                    discoveredMap[playerPosition[0] + 1, playerPosition[1]] = 'B';
                }
            }
            if (playerPosition[1] > 0)
            {
                if (discoveredMap[playerPosition[0], playerPosition[1] - 1] == '@' && map[playerPosition[0], playerPosition[1] - 1] == '*')
                {
                    discoveredMap[playerPosition[0], playerPosition[1] - 1] = '?';
                }
                else if (map[playerPosition[0], playerPosition[1] - 1] == 'M')
                {
                    discoveredMap[playerPosition[0], playerPosition[1] - 1] = 'M';
                }
                else if (map[playerPosition[0], playerPosition[1] - 1] == 'B')
                {
                    discoveredMap[playerPosition[0], playerPosition[1] - 1] = 'B';
                }
            }
            if (playerPosition[1] < map.GetLength(1) - 1)
            {
                if (discoveredMap[playerPosition[0], playerPosition[1] + 1] == '@' && map[playerPosition[0], playerPosition[1] + 1] == '*')
                {
                    discoveredMap[playerPosition[0], playerPosition[1] + 1] = '?';
                }
                else if (map[playerPosition[0], playerPosition[1] + 1] == 'M')
                {
                    discoveredMap[playerPosition[0], playerPosition[1] + 1] = 'M';
                }
                else if (map[playerPosition[0], playerPosition[1] + 1] == 'B')
                {
                    discoveredMap[playerPosition[0], playerPosition[1] + 1] = 'B';
                }
            }

            return moved;
        }

        public char CheckMap()
        {
            if(discoveredMap[playerPosition[0], playerPosition[1]] != 'O')
            {
                return map[playerPosition[0], playerPosition[1]];
            }
            return 'O';
        }

        public void PrintMap()
        {
            discoveredMap[playerPosition[0], playerPosition[1]] = 'X';
            Console.Clear();
            Console.WriteLine("       ~#~#~#~#~#~#~#~#####~#~#~#~#~#~#~#~");
            string print;
            for (int i = 0; i < 9; i++)
            {
                print = "        ~       ";
                for (int j = 0; j < 9; j++)
                {
                    print += discoveredMap[i, j] + " ";
                }
                print += "       ~";
                Console.WriteLine(print);
            }
            Console.WriteLine("       ~#~#~#~#~#~#~#~#####~#~#~#~#~#~#~#~\n");
            
            Console.WriteLine("     =Class, life, attack, magic, defense, magic defense, dodge\n");
            foreach (Classes i in player.currentClasses)
            {
                Console.WriteLine("     -" + i.name + " : " + i.life + ", "+ i.attack + ", " + i.magic + ", " + i.physicDefense + ", " + i.magicDefense + ", " + i.dodge);
            }

            Console.WriteLine("     .Gold = " + player.gold);
            
            Console.WriteLine("\n     --Movements--\n");
            Console.WriteLine("          8");
            Console.WriteLine("          |");
            Console.WriteLine("      4 --¤-- 6");
            Console.WriteLine("          |");
            Console.WriteLine("          2");

        }
        
    }
}
