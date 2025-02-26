using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safina
{
    public abstract class Item
    {
        public string Name { get; protected set; }
    }

    public class Potion : Item
    {
        public int HealAmount { get; private set; }

        public Potion(int healAmount)
        {
            Name = "Зелье лечения";
            HealAmount = healAmount;
        }
    }

    public class Arrows : Item
    {
        public int Quantity { get; set; }

        public Arrows(int quantity)
        {
            Name = "Стрелы";
            Quantity = quantity;
        }
    }

    public class Chest
    {
        public Item Item { get; set; }
        public string Riddle { get; set; }
        public int CorrectAnswer { get; set; }
    }

    public class Trader
    {
        public Potion PotionForSale { get; set; }
        public int PotionPrice { get; set; }
    }

    public class Quest
    {
        public string Question { get; set; }
        public int Answer { get; set; }
        public string Reward { get; set; }
    }

    public class Room
    {
        public RoomEvent Event { get; set; }
        public Monster Monster { get; set; }
        public Chest Chest { get; set; }
        public Trader Trader { get; set; }
    }

    public class Player
    {
        public int Health { get; set; }
        public int Potions { get; set; }
        public int Gold { get; set; }
        public int Arrows { get; set; }
        public bool HasSword { get; set; }
        public bool HasBow { get; set; }
        public List<Item> Inventory { get; set; }

        public Player()
        {
            Health = 100;
            Potions = 3;
            Gold = 0;
            Arrows = 5;
            HasSword = true;
            HasBow = true;
            Inventory = new List<Item>(5);
        }
    }

    public class Monster
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }

        public Monster(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
        }
    }

    public enum RoomEvent
    {
        Monster,
        Trap,
        Chest,
        Trader,
        Empty,
        Boss
    }

    public enum Weapon
    {
        Sword,
        Bow
    }
}
