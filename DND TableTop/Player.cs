namespace DND_TableTop
{
    internal class Player
    {
        public List<Classes> currentClasses = new List<Classes>();
        public int gold;
        public int[] position = { 8, 4 };

        private List<Classes> roles = [];
        public void Init(List<Classes> _roles)
        {
            bool _continue = true;
            while (_continue) 
            {
                roles = _roles;
                Console.WriteLine(" - Choose your starting member - ");
                for (int i = 0; i < roles.Count; i++)
                {
                    Console.WriteLine(i + 1 + ": " + roles[i].name);
                }
                Console.WriteLine("");
                int choice = Convert.ToInt32(Console.ReadLine()) - 1;
                Console.Clear();

                Console.WriteLine(" - " + roles[choice].name.ToUpper() + " - ");
                Console.WriteLine(" Life :              " + roles[choice].life);
                Console.WriteLine(" Attack :            " + roles[choice].life);
                Console.WriteLine(" Magic :             " + roles[choice].life);
                Console.WriteLine(" Physic Defence :    " + roles[choice].life);
                Console.WriteLine(" Magic Defense :     " + roles[choice].life);
                Console.WriteLine(" Dodging Chance :    " + roles[choice].life);
                Console.WriteLine("\n ~ Continue with this member? ~ ");
                Console.WriteLine("1: Yes           - 2: No\n");
                if (Convert.ToInt32(Console.ReadLine()) == 1)
                {
                    _continue = false;
                    currentClasses.Add(roles[choice]);
                }
                Console.Clear();
            }
            
        }

        public void ChangeLife(int _life)
        {
            foreach (var role in currentClasses)
            {
                role.life += _life;
            }
        }
    }
}
