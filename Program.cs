using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        ArmyGenerator armyGenerator = new ArmyGenerator();
        List<Soldier> soldiers1 = new List<Soldier>(armyGenerator.Generate());
        List<Soldier> soldiers2 = new List<Soldier>(armyGenerator.Generate());
        Army orks = new Army(soldiers1, "Orks");
        Army gnoms = new Army(soldiers2, "Gnoms");
        Battle battle = new Battle();

        orks.ShowAllSoldiers();
        Console.WriteLine();
        gnoms.ShowAllSoldiers();

        Console.WriteLine();
        battle.Fight(orks, gnoms);
        battle.ShowWinner(orks, gnoms);
        Console.WriteLine();

        orks.ShowAllSoldiers();
        Console.WriteLine();
        gnoms.ShowAllSoldiers();
    }
}

public static class Utils
{
    private static Random s_random = new Random();

    public static int GenerateRandomNumber(int min, int max)
    {
        return s_random.Next(min, max);
    }
}

public class Soldier
{
    protected int Health;
    protected int Armor;
    protected int Damage;

    public string Type { get; protected set; }
    //public bool IsAlive { get { return (Health > 0); } }
    public bool IsAlive => Health > 0;

    public Soldier(int health = 0, int armor = 0, int damage = 0)
    {
        Type = "Type 1";
        Health = health;
        Armor = armor;
        Damage = damage;
    }

    public void ShowStats() =>
        Console.WriteLine($"Type: {Type} health: {Health} damage: {Damage} armor: {Armor}");

    public void TakeDamage(int damage) =>
        Health -= (damage - Armor);

    public virtual bool GiveDamage(Army army)
    {
        if (Attack(army) == false)
            return false;

        return true;
    }

    protected bool Attack(Army army, int multiplier = 1)
    {
        if (army.TryGetRandomSoldier(out Soldier attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage * multiplier);
        army.DeleteDeadBody();
        return true;
    }

    public virtual Soldier Clone(int health, int armor, int damage) => new Soldier(health, armor, damage);
}

public class Soldier2 : Soldier
{
    private int _multiplier;

    public Soldier2(int health = 0, int armor = 0, int damage = 0)
        : base(health, armor, damage)
    {
        Type = "Type 2";
        _multiplier = 2;
    }

    public override bool GiveDamage(Army army)
    {
        if (Attack(army, _multiplier) == false)
            return false;

        return true;
    }

    public override Soldier Clone(int health, int armor, int damage) =>
        new Soldier2(health, armor, damage);
}

public class Soldier3 : Soldier
{
    public Soldier3(int health = 0, int armor = 0, int damage = 0)
       : base(health, armor, damage)
    {
        Type = "Type 3";
    }

    public override bool GiveDamage(Army army)
    {
        if (Attack(army) == false)
            return false;

        if (Attack(army) == false)
            return false;

        return true;
    }

    public override Soldier Clone(int health, int armor, int damage) =>
        new Soldier3(health, armor, damage);
}

public class Soldier4 : Soldier
{
    public Soldier4(int health = 0, int armor = 0, int damage = 0)
       : base(health, armor, damage)
    {
        Type = "Type 4";
    }

    public override bool GiveDamage(Army army)
    {
        if (Attack(army) == false)
            return false;

        if (Attack(army) == false)
            return false;

        if (Attack(army) == false)
            return false;

        return true;
    }

    public override Soldier Clone(int health, int armor, int damage) =>
        new Soldier4(health, armor, damage);
}

public class ArmyGenerator
{
    private List<Soldier> _soldiersType;
    private int _minSoldiers = 10;
    private int _maxSoldiers = 15;
    private int _minHealth = 100;
    private int _maxHealth = 150;
    private int _minArmor = 20;
    private int _maxArmor = 25;
    private int _minDamage = 30;
    private int _maxDamage = 35;

    public ArmyGenerator()
    {
        _soldiersType = new List<Soldier>() { new Soldier(),
                                              new Soldier2(),
                                              new Soldier3(),
                                              new Soldier4() };
    }

    public List<Soldier> Generate()
    {
        List<Soldier> soldiers = new List<Soldier>();

        for (int i = 0; i < Utils.GenerateRandomNumber(_minSoldiers, _maxSoldiers); i++)
        {
            int soldierType = Utils.GenerateRandomNumber(0, _soldiersType.Count);
            int health = Utils.GenerateRandomNumber(_minHealth, _maxHealth);
            int armor = Utils.GenerateRandomNumber(_minArmor, _maxArmor);
            int damage = Utils.GenerateRandomNumber(_minDamage, _maxDamage);

            soldiers.Add(_soldiersType[soldierType].Clone(health, armor, damage));
        }

        return soldiers;
    }
}

public class Army
{
    private List<Soldier> _soldiers;

    public string Name { get; /*private set;*/ }
    //public int QuantitySoldiers { get { return _soldiers.Count; } }
    public int QuantitySoldiers => _soldiers.Count; 

    public Army(List<Soldier> soldiers, string name)
    {
        _soldiers = new List<Soldier>(soldiers);
        Name = name;
    }

    public void ShowAllSoldiers()
    {
        foreach (var soldier in _soldiers)
        {
            Console.Write($"Name: {Name} ");
            soldier.ShowStats();
        }
    }

    public void DeleteDeadBody()
    {
        foreach (var soldier in _soldiers)
        {
            if (soldier.IsAlive == false)
            {
                _soldiers.Remove(soldier);
                break;
            }
        }
    }

    public bool TryGetRandomSoldier(out Soldier soldier)
    {
        if (_soldiers.Count() > 0)
        {
            soldier = _soldiers[Utils.GenerateRandomNumber(0, _soldiers.Count())];
            return true;
        }
        else
        {
            soldier = null;
            return false;
        }
    }
}

public class Battle
{
    private bool Attack(Army army1, Army army2)
    {
        if (army1.TryGetRandomSoldier(out Soldier attakedSoldier) == false)
            return false;

        attakedSoldier.GiveDamage(army2);
        return true;
    }

    public void Fight(Army army1, Army army2)
    {
        while (army1.QuantitySoldiers > 0 & army2.QuantitySoldiers > 0)
        {
            if (Attack(army1, army2) == false)
                break;

            if (Attack(army2, army1) == false)
                break;
        }
    }

    public void ShowWinner(Army army1, Army army2)
    {
        if (army1.QuantitySoldiers > army2.QuantitySoldiers)
            Console.WriteLine($"Army {army1.Name} - win!");
        else
            Console.WriteLine($"Army {army2.Name} - win!");
    }
}