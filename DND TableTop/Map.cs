using static System.Net.Mime.MediaTypeNames;

namespace DND_TableTop
{
    internal class Map
    {
        char[,] map = new char[9,9];
        char[,] discoveredMap = new char[9, 9];
        int[,] treeMap = new int[9, 9];
        List<int[]> roomsList = new List<int[]>();

        int mapLen = 20;

        public void Init(int[] playerPos)
        {
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
            map[currentPos[0], currentPos[1]] = '*';
            int iterationDebug = 0;
            while (roomsList.Count < mapLen)
            {
                currentPos = AddRoom(currentPos);
                
                if (roomsList.Count == mapLen / 2 - 1) { map[currentPos[0], currentPos[1]] = 'M'; }
                iterationDebug++;
                if (iterationDebug > 20)
                {
                    mapLen -= 1;
                }
            }

            //Random random = new Random();
            //while (roomsList.Count < 10 + random.Next(10))
            //{
            //    int rndRoom = random.Next(roomsList.Count);
            //    currentPos = roomsList[rndRoom];
                
            //    for (int i = 0;i < random.Next(7); i++)
            //    {
            //        currentPos = AddRoom(currentPos);
            //    }
            //}

            List<int[]> treeList = new List<int[]>();
            //int[] bestTree = Tree(treeList, [playerPos[0], playerPos[1], 0], [playerPos[0], playerPos[1], 0]);
            //map[bestTree[0], bestTree[1]] = 'B';
            Waheur(treeList, [8,4], 0);
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

        private int[] Tree(List<int[]> way,int[] pos, int[]bestPos)
        {
            treeMap[pos[0], pos[1]] =  pos[2];
            int[] next = pos;
            way.Add(pos);
            if (pos[0] > 0)
            {
                next[0] -= 1;
                if(!way.Contains(next) && map[next[0], next[1]] == '*')
                {
                    int[] north = Tree(way, [next[0], next[1], next[2] + 1], bestPos);
                    if (north[2] > bestPos[2])
                    {
                        bestPos = north;
                    }
                }
            }
            
            next = pos;
            if (pos[1] > 0)
            {
                next[1] -= 1;
                if (!way.Contains(next) && map[next[0], next[1]] == '*')
                {
                    int[] west = Tree(way, [next[0], next[1], next[2] + 1], bestPos);
                    if (west[2] > bestPos[2])
                    {
                        bestPos = west;
                    }
                }
            }

            next = pos;
            if (pos[0] < map.GetLength(0) - 1)
            {
                next[0] += 1;
                if (!way.Contains(next) && map[next[0], next[1]] == '*')
                {
                    int[] south = Tree(way, [next[0], next[1], next[2] + 1], bestPos);
                    if (south[2] > bestPos[2])
                    {
                        bestPos = south;
                    }
                }
            }

            next = pos;
            if (pos[1] < map.GetLength(1) - 1)
            {
                next[1] += 1;
                if (!way.Contains(next) && map[next[0], next[1]] == '*')
                {
                    int[] east = Tree(way, [next[0], next[1], next[2] + 1], bestPos);
                    if (east[2] > bestPos[2])
                    {
                        bestPos = east;
                    }
                }
            }
            return bestPos;
        }

        void Waheur(List<int[]>map, int[] pos, int iteration)
        {
            if (treeMap[pos[0], pos[1]] == 0 || treeMap[pos[0], pos[1]] > iteration)
            {
                treeMap[pos[0], pos[1]] = iteration;
                PrintMap();
            }
            Console.WriteLine(pos[0] + " " + pos[1]);
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
                    Waheur(map, i, iteration + 1);
                }
            }

            return;
        }

        public void PrintMap()
        {
            Console.WriteLine("       ~#~#~#~#~#~#~#~#####~#~#~#~#~#~#~#~");
            string print;
            for (int i = 0; i < 9; i++)
            {
                print = "        ~       ";
                for (int j = 0; j < 9; j++)
                {
                    print += treeMap[i, j] + " ";
                }
                print += "       ~";
                Console.WriteLine(print);
            }
            Console.WriteLine("       ~#~#~#~#~#~#~#~#####~#~#~#~#~#~#~#~");
        }

    }
}
