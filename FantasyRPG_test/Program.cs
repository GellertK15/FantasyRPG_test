using System;
using System.Collections.Generic;
using System.Reflection.Emit;

internal class Program
{
    private class Character
    {
        public string CharacterClass { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Shield { get; set; }
        public int Experience { get; set; } // Tapasztalatpontok
        public Inventory Inventory { get; set; }
        public List<Ability> Abilities { get; set; }

        public Character(string characterClass, int x, int y)
        {
            CharacterClass = characterClass;
            X = x;
            Y = y;
            Level = 1;
            Health = 100;
            Shield = 50;
            Experience = 0; // Tapasztalatpontok inicializálása
            Inventory = new Inventory();
            Abilities = new List<Ability>();
        }
    }

    private class Ability
    {
        public string Name { get; set; }
        public int Damage { get; set; }

        public Ability(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }
    }

    private class Level
    {
        public string Name { get; set; }
        public char[,] Map { get; set; }
        public List<Helper> Helpers { get; set; }
        public List<Enemy> Enemies { get; set; }

        public Level(string name, char[,] map)
        {
            Name = name;
            Map = map;
            Helpers = new List<Helper>();
            Enemies = new List<Enemy>();
        }
    }

    private class Helper
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Item { get; set; }

        public Helper(int x, int y, string item)
        {
            X = x;
            Y = y;
            Item = item;
        }
    }

    private class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Enemy(string name, int health, int damage, int x, int y)
        {
            Name = name;
            Health = health;
            Damage = damage;
            X = x;
            Y = y;
        }
    }

    private class Inventory
    {
        public List<string> Items { get; set; }

        public Inventory()
        {
            Items = new List<string>();
        }
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Válassz egy karakter osztályt:");
        Console.WriteLine("1. Hero");
        Console.WriteLine("2. Necromancer");
        Console.WriteLine("3. Wizard");
        Console.WriteLine("4. Archer");
        string characterClass;
        switch (Convert.ToInt32(Console.ReadLine()))
        {
            case 1:
                characterClass = "Hero";
                break;
            case 2:
                characterClass = "Necromancer";
                break;
            case 3:
                characterClass = "Wizard";
                break;
            case 4:
                characterClass = "Archer";
                break;
            default:
                Console.WriteLine("Nincs ilyen karakter osztály!");
                return;
        }
        Character player = new Character(characterClass, 1, 1);
        switch (player.CharacterClass)
        {
            case "Hero":
                player.Abilities.Add(new Ability("Sword Slash", 20));
                break;
            case "Necromancer":
                player.Abilities.Add(new Ability("Dark Magic", 25));
                break;
            case "Wizard":
                player.Abilities.Add(new Ability("Fireball", 30));
                break;
            case "Archer":
                player.Abilities.Add(new Ability("Arrow Shot", 15));
                player.Abilities.Add(new Ability("Multi-Shot", 25));
                break;
        }
        Console.WriteLine("Kiválasztott karakter osztály: " + player.CharacterClass);
        Level[] levels = new Level[1]
        {
            new Level("Suttogó Erdő", new char[9, 9]
            {
                { '_', '_', '_', '_', '_', '_', '_', '_', '_' },
                { '|', '.', '.', '.', '.', '.', '.', '.', '|' },
                { '|', '.', '.', '.', '.', '.', '.', '.', '|' },
                { '|', '.', '.', '.', '.', '.', '.', '.', '|' },
                { '|', '.', '.', '.', '.', '.', '.', '.', '|' },
                { '|', '.', '.', '.', '.', '.', '.', '.', '|' },
                { '|', '.', '.', '.', '.', '.', '.', '.', '|' },
                { '|', '.', '.', '.', '.', '.', '.', '.', '|' },
                { '_', '_', '_', '_', '_', '_', '_', '_', '_' }
            })
        };



        PlaceHelpers(levels[0]);
        PlaceEnemies(levels[0]);
        PlaceAnotherEnemy(levels[0]);
        Console.WriteLine("Válassz egy pályát:");
        for (int i = 0; i < levels.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {levels[i].Name}");
        }
        int levelChoice = Convert.ToInt32(Console.ReadLine());
        if (levelChoice > 0 && levelChoice <= levels.Length)
        {
            Console.WriteLine("Kiválasztott pálya: " + levels[levelChoice - 1].Name);
            PlayGame(levels[levelChoice - 1], player);
        }
        else
        {
            Console.WriteLine("Nincs ilyen pálya!");
        }
    }

    private static void PlaceHelpers(Level level)
    {
        level.Helpers.Add(new Helper(3, 3, "Pajzs amulett"));
        level.Helpers.Add(new Helper(7, 5, "Életerő varázsital"));
    }

    private static void PlaceEnemies(Level level)
    {
        level.Enemies.Add(new Enemy("Goblin", 50, 10, 5, 3));
        level.Enemies.Add(new Enemy("Skeleton", 40, 15, 7, 6));
    }
    private static void PlaceAnotherEnemy(Level level)
    {
        // Új ellenség hozzáadása a pályához
        level.Enemies.Add(new Enemy("Orc", 60, 20, 3, 7)); // Például: Orc elhelyezése
    }

    private static void PlayGame(Level level, Character player)
    {
        PrintLevel(level, player);

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            int newX = player.X;
            int newY = player.Y;

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    newY = player.Y - 1;
                    break;
                case ConsoleKey.DownArrow:
                    newY = player.Y + 1;
                    break;
                case ConsoleKey.LeftArrow:
                    newX = player.X - 1;
                    break;
                case ConsoleKey.RightArrow:
                    newX = player.X + 1;
                    break;
                case ConsoleKey.Escape:
                    return;
                default:
                    break;
            }

            if (IsValidMove(newX, newY, level))
            {
                player.X = newX;
                player.Y = newY;

                foreach (Enemy enemy in level.Enemies)
                {
                    if (enemy.X == newX && enemy.Y == newY)
                    {
                        Fight(player, enemy, level);
                        if (enemy.Health <= 0)
                        {
                            player.Experience += 25;
                            level.Enemies.Remove(enemy);
                            CheckLevelUp(player);
                        }
                        break;
                    }
                }

                foreach (Helper helper in level.Helpers)
                {
                    if (helper.X == newX && helper.Y == newY)
                    {
                        InteractWithHelper(helper, player, level);
                        break;
                    }
                }
            }

            PrintLevel(level, player);
            CheckLevelUp(player);
        }
    }






    private static void UseAbility(Character player, int index, int playerX, int playerY, Level level)
    {
        if (index < player.Abilities.Count)
        {
            Ability ability = player.Abilities[index];
            Console.WriteLine($"Képesség használata: {ability.Name}");
            int damage = CalculateAbilityDamage(player, ability);

            foreach (Enemy enemy in level.Enemies)
            {
                if ((Math.Abs(enemy.X - playerX) <= 1 && enemy.Y == playerY) || (Math.Abs(enemy.Y - playerY) <= 1 && enemy.X == playerX))
                {
                    enemy.Health -= damage;
                    Console.WriteLine($"Az {enemy.Name} ellenfélnek okozott sebzés: {damage}");

                    // Kérdés a játékosnak a további lépésekről
                    Console.WriteLine("Mit szeretnél tenni?");
                    Console.WriteLine("1. Védekezés");
                    Console.WriteLine("2. Menekülés");
                    Console.WriteLine("3. Tovább");

                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            if (enemy.Health <= 0)
                            {
                                Console.WriteLine("Az ellenfél legyőzve!");
                                player.Experience += 25; // XP hozzáadása a harc után
                                level.Enemies.Remove(enemy);
                                CheckLevelUp(player);
                            }
                            else
                            {
                                // Védekezés ugyanúgy, mint támadásnál
                                int playerDefense = CalculatePlayerDefense(player);
                                int enemyAttack = CalculateEnemyDamage(enemy);
                                enemyAttack -= playerDefense;
                                player.Health -= Math.Max(0, enemyAttack);
                                Console.WriteLine($"Védekezéseddel az ellenfél sebzése csökkent és {Math.Max(0, enemyAttack)} sebzést kaptál.");

                                if (player.Health <= 0)
                                {
                                    Console.WriteLine("Vesztettél! Az életerőd nullára csökkent.");
                                    Console.ReadKey();
                                    Environment.Exit(0); // Kilépés a játékból, ha a játékos meghal
                                }
                            }
                            break;

                        case 2:
                            Console.WriteLine("Sikeresen elmenekültél!");
                            return;

                        case 3:
                            // Ha tovább lép, akkor semmi különleges nem történik
                            break;

                        default:
                            Console.WriteLine("Érvénytelen választás.");
                            break;
                    }

                    return;
                }
            }

            Console.WriteLine("Nincs ellenfél a közeledben.");
        }
        else
        {
            Console.WriteLine("Érvénytelen választás.");
        }
    }




    private static int CalculateAbilityDamage(Character player, Ability ability)
    {
        // Képesség sebzésének számítása véletlenszerűen egy adott intervallumon belül
        Random rnd = new Random();
        int baseDamage = rnd.Next(40, 50);
        // Az intervallum beállítása
        // Például: A képesség sebzése függ az adott karakter szintjétől vagy más tulajdonságaitól
        // Ide illeszthető további logika a sebzés
        if (IsCriticalHit())
        {
            Console.WriteLine("Kritikus találat!");
            baseDamage *= 2;
        }
        return baseDamage;
    }
    private static void ChooseAbilityToUse(Character player, Level level)
    {
        Console.WriteLine("Válassz egy képességet:");
        for (int i = 0; i < player.Abilities.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {player.Abilities[i].Name}");
        }
        int choice = Convert.ToInt32(Console.ReadLine()) - 1;
        if (choice >= 0 && choice < player.Abilities.Count)
        {
            UseAbility(player, choice, player.X, player.Y, level);
        }
        else
        {
            Console.WriteLine("Érvénytelen választás.");
        }
    }






    private static bool IsValidMove(int x, int y, Level level)
    {
        return x >= 0 && x < level.Map.GetLength(1) && y >= 0 && y < level.Map.GetLength(0) && level.Map[y, x] != '|';
    }

    private static void InteractWithHelper(Helper helper, Character player, Level level)
    {
        Console.Clear();
        Console.WriteLine("A segítő mondja: Szia! Láttalak, hogy küzdesz a veszélyekkel. Én egy " + helper.Item + "-t kínálok neked. Elfogadod?");
        Console.WriteLine("1. Igen");
        Console.WriteLine("2. Nem");
        int choice = Convert.ToInt32(Console.ReadLine());
        if (choice == 1)
        {
            Console.WriteLine("A segítő mondja: Nagyszerű! Itt van a " + helper.Item + ". Sok sikert a továbbiakban!");
            player.Inventory.Items.Add(helper.Item);
            level.Helpers.Remove(helper); // Segítő eltávolítása
        }
        else if (choice == 2)
        {
            Console.WriteLine("A segítő mondja: Rendben, ha mégis szeretnéd, gyere vissza hozzám bármikor.");
        }
        else
        {
            Console.WriteLine("Érvénytelen választás.");
        }
        Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
        Console.ReadKey();
    }

    private static void Fight(Character player, Enemy enemy, Level level)
    {
        Console.Clear();
        Console.WriteLine("Ellenfél: " + enemy.Name);
        Console.WriteLine($"Ellenfél életerő: {enemy.Health}");

        bool isEnemyDefeated = false;

        while (enemy.Health > 0 && player.Health > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Jelenlegi állapot:");
            Console.WriteLine($"Játékos életerő: {player.Health}");
            Console.WriteLine($"Ellenfél életerő: {enemy.Health}");
            Console.WriteLine();

            

            if (enemy.Health > 0)
            {
                Console.WriteLine("Mit szeretnél tenni?");
                Console.WriteLine("1. Támadás");
                Console.WriteLine("2. Képesség használata");
                Console.WriteLine("3. Menekülés");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        int playerDamage = CalculatePlayerDamage(player);
                        enemy.Health -= playerDamage;
                        if (IsCriticalHit())
                        {
                            Console.WriteLine("Kritikus találat! Két-szeres sebzést okoztál az ellenfélnek.");
                        }
                        Console.WriteLine($"Az ellenfélnek okozott sebzés: {playerDamage}");

                        if (enemy.Health <= 0)
                        {
                            Console.WriteLine("Az ellenfél legyőzve!");
                            player.Experience += 25; // XP hozzáadása a harc után
                            level.Enemies.Remove(enemy);
                            CheckLevelUp(player);
                            isEnemyDefeated = true;
                        }
                        break;


                    case 2:
                        ChooseAbilityToUse(player, level);
                        break;

                    case 3:
                        Console.WriteLine("Sikeresen elmenekültél!");
                        return;

                    default:
                        Console.WriteLine("Érvénytelen választás.");
                        break;
                }

                if (isEnemyDefeated)
                {
                    break; // kilépés a ciklusból, ha az ellenfél már legyőzött
                }

                if (choice == 1)
                {
                    Console.WriteLine("Mit szeretnél tenni?");
                    Console.WriteLine("1. Védekezés");
                    Console.WriteLine("2. Tovább");

                    int defenseChoice = Convert.ToInt32(Console.ReadLine());

                    switch (defenseChoice)
                    {
                        case 1:
                            int playerDefense = CalculatePlayerDefense(player);
                            int enemyAttack = CalculateEnemyDamage(enemy);
                            enemyAttack -= playerDefense;
                            player.Health -= Math.Max(0, enemyAttack);
                            Console.WriteLine($"Védekezéseddel az ellenfél sebzése csökkent és {Math.Max(0, enemyAttack)} sebzést kaptál.");
                            break;

                        case 2:
                            ChooseAbilityToUse(player, level);
                            break;

                        default:
                            Console.WriteLine("Érvénytelen választás.");
                            break;
                    }
                }
            }
        }

        if (enemy.Health <= 0)
        {
            level.Enemies.Remove(enemy);
            Console.WriteLine("Az ellenfél legyőzve!");
            player.Experience += 25;
            Console.WriteLine($"Kaptál 25 tapasztalatpontot. Összesen: {player.Experience}");
            CheckLevelUp(player); // Szintlépés ellenőrzése
            if (level.Enemies.Count == 0)
            {
                Console.WriteLine("Nincs több ellenfél a pályán.");
                return;
            }
        }

        Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
        Console.ReadKey();
    }




    private const int ExperiencePerLevel = 25;

    // A játékos szintlépését ellenőrző metódus
    private static void CheckLevelUp(Character player)
    {
        while (player.Experience >= ExperiencePerLevel * player.Level)
        {
            player.Level++;
            player.Experience -= ExperiencePerLevel * (player.Level - 1);
            Console.WriteLine($"Gratulálunk! Feljebb léptél egy szintet! Az új szinted: {player.Level}");

            // Új képesség hozzáadása a játékoshoz a 3. szinttől
            if (player.Level >= 3)
            {
                switch (player.CharacterClass)
                {
                    case "Hero":
                        player.Abilities.Add(new Ability("Shield Bash", 25));
                        Console.WriteLine("Új képesség nyílt meg: Shield Bash");
                        break;
                    case "Necromancer":
                        player.Abilities.Add(new Ability("Drain Life", 30));
                        Console.WriteLine("Új képesség nyílt meg: Drain Life");
                        break;
                    case "Wizard":
                        player.Abilities.Add(new Ability("Lightning Bolt", 35));
                        Console.WriteLine("Új képesség nyílt meg: Lightning Bolt");
                        break;
                    case "Archer":
                        player.Abilities.Add(new Ability("Explosive Shot", 20));
                        Console.WriteLine("Új képesség nyílt meg: Explosive Shot");
                        break;
                }
            }
        }
    }

    private static int CalculatePlayerDefense(Character player)
    {
        // Játékos védekezésének véletlenszerű számítása egy adott intervallumon belül
        Random rnd = new Random();
        return rnd.Next(5, 15); // Az intervallum beállítása
    }


    private static bool IsCriticalHit()
    {
        Random rnd = new Random(); // Random objektum létrehozása
        int chance = rnd.Next(1, 101); // 1 és 100 közötti véletlenszerű szám generálása
        return chance <= 25; // 20%-os eséllyel kritikus találat
    }



    private static int CalculatePlayerDamage(Character player)
    {
        // Játékos sebzésének véletlenszerű számítása egy adott intervallumon belül
        Random rnd = new Random();
        int baseDamage = rnd.Next(15, 25);

        // Kritikus találat esetén a sebzés duplázódik
        if (IsCriticalHit())
        {
            Console.WriteLine("Kritikus találat!");
            baseDamage *= 2;
        }

        return baseDamage;
    }






    private static int CalculateEnemyDamage(Enemy enemy)
    {
        // Ellenfél sebzésének véletlenszerű számítása egy adott intervallumon belül
        Random rnd = new Random();
        return rnd.Next(8, 18); // Az intervallum beállítása
    }

    private static bool IsDefenseSuccessful()
    {
        Random rnd = new Random();
        int chance = rnd.Next(1, 101); // Véletlenszerű szám generálása 1 és 100 között
        return chance <= 60; // 60% eséllyel sikeres a védekezés
    }


    private static void PrintLevel(Level level, Character player)
    {
        Console.Clear();
        Console.WriteLine($"Életerő: {player.Health} | Szint: {player.Level} | Tapasztalatpont: {player.Experience} | Tárgyak: {string.Join(", ", player.Inventory.Items)}");

        // Térkép megjelenítése
        for (int i = 0; i < level.Map.GetLength(0); i++)
        {
            for (int j = 0; j < level.Map.GetLength(1); j++)
            {
                if (i == player.Y && j == player.X)
                {
                    Console.Write('@');
                }
                else if (IsHelperPosition(level, j, i))
                {
                    Console.Write('H');
                }
                else if (IsEnemyPosition(level, j, i))
                {
                    Console.Write('E');
                }
                else
                {
                    Console.Write(level.Map[i, j]);
                }
            }
            Console.WriteLine();
        }
    }

    private static void Defense(Character player, Level level)
    {
        // Védekezés sikerességének ellenőrzése
        bool isSuccessful = IsDefenseSuccessful();

        if (isSuccessful)
        {
            Console.WriteLine("Sikeresen védekeztél! Nem szenvedtél sebzést.");
        }
        else
        {
            // Ha a védekezés nem sikeres, akkor számítani kell a játékosra eső sebzést
            int playerDefense = CalculatePlayerDefense(player);
            foreach (Enemy enemy in level.Enemies)
            {
                int enemyAttack = CalculateEnemyDamage(enemy);
                enemyAttack -= playerDefense;
                player.Health -= Math.Max(0, enemyAttack);
                Console.WriteLine($"Védekezéseddel az ellenfél sebzése csökkent és {Math.Max(0, enemyAttack)} sebzést kaptál.");

                if (player.Health <= 0)
                {
                    Console.WriteLine("Vesztettél! Az életerőd nullára csökkent.");
                    Console.ReadKey();
                    Environment.Exit(0); // Kilépés a játékból, ha a játékos meghal
                }
            }
        }
    }


    private static bool IsHelperPosition(Level level, int x, int y)
    {
        foreach (Helper helper in level.Helpers)
        {
            if (helper.X == x && helper.Y == y)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsEnemyPosition(Level level, int x, int y)
    {
        foreach (Enemy enemy in level.Enemies)
        {
            if (enemy.X == x && enemy.Y == y)
            {
                return true;
            }
        }
        return false;
    }
}