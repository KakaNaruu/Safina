using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Safina.Program;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Safina
{
    public class Game
    {
        private const int DungeonSize = 10;
        private Room[] dungeonMap;
        private Player player;
        private Random random = new Random();
        private List<Quest> quests = new List<Quest>();

        public Game()
        {
            player = new Player();
            dungeonMap = new Room[DungeonSize];
            quests = InitializeQuests();
        }

        public void InitializeDungeon()
        {
            for (int i = 0; i < DungeonSize; i++)
            {
                dungeonMap[i] = new Room();
                if (i == DungeonSize - 1)
                {
                    dungeonMap[i].Event = RoomEvent.Boss;
                    dungeonMap[i].Monster = new Monster("Финальный босс", 70, 20);
                }
                else
                {
                    Array roomEvents = Enum.GetValues(typeof(RoomEvent));
                    RoomEvent randomEvent = (RoomEvent)roomEvents.GetValue(random.Next(roomEvents.Length - 1));
                    dungeonMap[i].Event = randomEvent;

                    switch (randomEvent)
                    {
                        case RoomEvent.Monster:
                            dungeonMap[i].Monster = GenerateRandomMonster();
                            break;
                        case RoomEvent.Chest:
                            dungeonMap[i].Chest = GenerateRandomChest();
                            break;
                        case RoomEvent.Trader:
                            dungeonMap[i].Trader = GenerateRandomTrader();
                            break;
                    }
                }
            }
        }

        private Monster GenerateRandomMonster()
        {
            string[] monsterNames = { "Гоблин", "Скелет", "Паук" };
            string name = monsterNames[random.Next(monsterNames.Length)];
            int health = random.Next(20, 51);
            int damage = random.Next(5, 16);
            return new Monster(name, health, damage);
        }

        private Chest GenerateRandomChest()
        {
            Chest chest = new Chest();
            
            double itemChance = random.NextDouble();
            if (itemChance < 0.33)
            {
                chest.Item = new Potion(random.Next(10, 30));
            }
            else if (itemChance < 0.66)
            {
                chest.Item = new Arrows(random.Next(3, 8));
            }
            else
            {
                chest.Item = new Gold(random.Next(20, 50));
            }

            
            Quest randomQuest = quests[random.Next(quests.Count)];
            chest.Riddle = randomQuest.Question;
            chest.CorrectAnswer = randomQuest.Answer;
            return chest;
        }

        private Trader GenerateRandomTrader()
        {
            Trader trader = new Trader();
            trader.PotionForSale = new Potion(20);
            trader.PotionPrice = 30;
            return trader;
        }

        private List<Quest> InitializeQuests()
        {
            return new List<Quest>
        {
            new Quest { Question = "2 + 2 * 2 = ?", Answer = 6, Reward = "Золото" },
            new Quest { Question = "36 / 6 + 4 = ?", Answer = 10, Reward = "Зелье" },
            new Quest { Question = "15 - 5 * 3 = ?", Answer = 0, Reward = "Стрелы" },
            new Quest { Question = "25 + 5 * 1 = &", Answer = 20, Reward = "Лук"},
            new Quest { Question = "11 / 11 + 4 = ?", Answer = 5, Reward = "Зелье"}
        };
        }

        private void DisplayPlayerStats()
        {
            Console.WriteLine($"\n--- Состояние игрока ---");
            Console.WriteLine($"Здоровье: {player.Health}");
            Console.WriteLine($"Зелья: {player.Potions}");
            Console.WriteLine($"Золото: {player.Gold}");
            Console.WriteLine($"Стрелы: {player.Arrows}");
            Console.WriteLine("----------------------");
        }


        public void StartGame()
        {
            InitializeDungeon();
            Console.WriteLine("Добро пожаловать в игру 'Числовой квест PLUS'");
            Console.WriteLine("Вы начинаете свое путешествие по подземелью...");

            
            for (int i = 0; i < DungeonSize; i++)
            {
                Console.WriteLine($"\nКомната {i + 1}:");
                EnterRoom(i);

                if (player.Health <= 0)
                {
                    Console.WriteLine("Вы погибли в подземелье (ಥ_ಥ) ");
                    return;
                }
            }

            Console.WriteLine("Поздравляем! Вы прошли подземелье и победили финального босса <('.'<)");
        }

        private void EnterRoom(int roomNumber)
        {
            Room room = dungeonMap[roomNumber];

            switch (room.Event)
            {
                case RoomEvent.Monster:
                    HandleMonster(room.Monster);
                    break;
                case RoomEvent.Trap:
                    HandleTrap();
                    break;
                case RoomEvent.Chest:
                    HandleChest(room.Chest);
                    break;
                case RoomEvent.Trader:
                    HandleTrader(room.Trader);
                    break;
                case RoomEvent.Boss:
                    HandleMonster(room.Monster);
                    break;
                case RoomEvent.Empty:
                    Console.WriteLine("Комната пуста. Здесь ничего не происходит.");
                    break;
            }
        }

        private void HandleMonster(Monster monster)
        {
            Console.WriteLine($"Вы встретили монстра: {monster.Name} (Здоровье: {monster.Health}, Урон: {monster.Damage})");

            while (player.Health > 0 && monster.Health > 0)
            {
                Console.WriteLine("\nВаш ход:");
                Console.WriteLine("1. Атаковать мечом");
                if (player.Arrows > 0 && player.HasBow)
                {
                    Console.WriteLine("2. Атаковать луком");
                }

                Console.WriteLine("3. Использовать зелье");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        int swordDamage = random.Next(10, 21);
                        Console.WriteLine($"Вы нанесли {swordDamage} урона мечом.");
                        monster.Health -= swordDamage;
                        break;
                    case "2":
                        if (player.Arrows > 0 && player.HasBow)
                        {
                            int bowDamage = random.Next(5, 16);
                            Console.WriteLine($"Вы нанесли {bowDamage} урона луком.");
                            monster.Health -= bowDamage;
                            player.Arrows--;
                            Console.WriteLine($"У вас осталось {player.Arrows} стрел.");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет стрел или лука.");
                            continue;
                        }
                        break;
                    case "3":
                        if (player.Potions > 0)
                        {
                            int healAmount = random.Next(10, 31);
                            player.Health += healAmount;
                            player.Potions--;
                            Console.WriteLine($"Вы использовали зелье и восстановили {healAmount} здоровья. У вас осталось {player.Potions} зелий. Ваше здоровье: {player.Health}");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет зелий.");
                            continue;
                        }
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        continue;
                }

                if (monster.Health <= 0)
                {
                    Console.WriteLine($"Вы победили монстра {monster.Name}!");
                    break;
                }

                int monsterDamage = random.Next(5, 16);
                Console.WriteLine($"{monster.Name} нанес вам {monsterDamage} урона.");
                player.Health -= monsterDamage;
                Console.WriteLine($"Ваше здоровье: {player.Health}");
            }
        }

        private void HandleTrap()
        {
            int trapDamage = random.Next(10, 21);
            Console.WriteLine($"Вы попали в ловушку и потеряли {trapDamage} здоровья.");
            player.Health -= trapDamage;
            Console.WriteLine($"Ваше здоровье: {player.Health}");
        }

        private void HandleChest(Chest chest)
        {
            Console.WriteLine("Вы нашли сундук! Чтобы его открыть, решите загадку:");
            Console.WriteLine(chest.Riddle);
            Console.Write("Ваш ответ: ");
            if (int.TryParse(Console.ReadLine(), out int answer))
            {
                if (answer == chest.CorrectAnswer)
                {
                    Console.WriteLine("Вы открыли сундук и нашли: " + chest.Item.Name);
                    if (chest.Item is Potion)
                    {
                        player.Inventory.Add(chest.Item);
                    }
                    else if (chest.Item is Arrows)
                    {
                        player.Arrows += ((Arrows)chest.Item).Quantity;
                    }
                    else if (chest.Item is Gold)
                    {
                        player.Gold += ((Gold)chest.Item).Amount;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ответ. Сундук остается закрытым.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ввода.");
            }
        }

        private void HandleTrader(Trader trader)
        {
            Console.WriteLine("Вы встретили торговца. Он предлагает зелье за " + trader.PotionPrice + " золота.");
            Console.WriteLine("1. Купить зелье");
            Console.WriteLine("2. Уйти");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (player.Gold >= trader.PotionPrice)
                    {
                        Console.WriteLine("Вы купили зелье.");
                        player.Inventory.Add(trader.PotionForSale);
                        player.Gold -= trader.PotionPrice;
                        Console.WriteLine($"У вас осталось {player.Gold} золота.");
                    }
                    else
                    {
                        Console.WriteLine("У вас недостаточно золота.");
                    }
                    break;
                case "2":
                    Console.WriteLine("Вы ушли от торговца.");
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
}
