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
        List<BaseSoldier> soldiers1 = new List<BaseSoldier>(armyGenerator.Generate());
        List<BaseSoldier> soldiers2 = new List<BaseSoldier>(armyGenerator.Generate());
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

public class BaseSoldier
{
    protected int Armor;
    protected int Damage;

    private int Health;

    public BaseSoldier(int health = 0, int armor = 0, int damage = 0)
    {
        Type = "Type 1";
        Health = health;
        Armor = armor;
        Damage = damage;
    }

    public string Type { get; protected set; }
    public bool IsAlive => Health > 0;

    public void ShowStats() =>
        Console.WriteLine($"Type: {Type} health: {Health} damage: {Damage} armor: {Armor}");

    public void TakeDamage(int damage) =>
        Health -= (damage - Armor);

    public virtual void GiveDamage(Army army) => TryAttack(army);

    protected bool TryAttack(Army army, int multiplier = 1)
    {
        if (army.TryGetRandomSoldier(out BaseSoldier attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage * multiplier);
        army.DeleteDeadBody();
        return true;
    }

    public virtual BaseSoldier Clone(int health, int armor, int damage) => new BaseSoldier(health, armor, damage);
}

public class Soldier2 : BaseSoldier
{
    private int _multiplier;

    public Soldier2(int health = 0, int armor = 0, int damage = 0)
        : base(health, armor, damage)
    {
        Type = "Type 2";
        _multiplier = 2;
    }

    public override void GiveDamage(Army army) => TryAttack(army, _multiplier);

    public override BaseSoldier Clone(int health, int armor, int damage) =>
        new Soldier2(health, armor, damage);
}

public class Soldier3 : BaseSoldier
{
    public Soldier3(int health = 0, int armor = 0, int damage = 0)
       : base(health, armor, damage)
    {
        Type = "Type 3";
    }

    public override void GiveDamage(Army army)
    {
        if (TryAttack(army) == false)
            return;

        TryAttack(army);
    }

    public override BaseSoldier Clone(int health, int armor, int damage) =>
        new Soldier3(health, armor, damage);
}

public class Soldier4 : BaseSoldier
{
    public Soldier4(int health = 0, int armor = 0, int damage = 0)
       : base(health, armor, damage)
    {
        Type = "Type 4";
    }

    public override void GiveDamage(Army army)
    {
        if (TryAttack(army) == false)
            return;

        if (TryAttack(army) == false)
            return;

        TryAttack(army);
    }

    public override BaseSoldier Clone(int health, int armor, int damage) =>
        new Soldier4(health, armor, damage);
}

public class ArmyGenerator
{
    private List<BaseSoldier> _soldiersType;
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
        _soldiersType = new List<BaseSoldier>() { new BaseSoldier(),
                                              new Soldier2(),
                                              new Soldier3(),
                                              new Soldier4() };
    }

    public List<BaseSoldier> Generate()
    {
        List<BaseSoldier> soldiers = new List<BaseSoldier>();

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
    private List<BaseSoldier> _soldiers;

    public Army(List<BaseSoldier> soldiers, string name)
    {
        _soldiers = new List<BaseSoldier>(soldiers);
        Name = name;
    }

    public string Name { get; }
    public int QuantitySoldiers => _soldiers.Count;

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

    public bool TryGetRandomSoldier(out BaseSoldier soldier)
    {
        if (_soldiers.Count() > 0)
        {
            soldier = _soldiers[Utils.GenerateRandomNumber(0, _soldiers.Count())];
            return true;
        }

        soldier = null;
        return false;
    }
}

public class Battle
{
    private bool TryAttack(Army army1, Army army2)
    {
        if (army1.TryGetRandomSoldier(out BaseSoldier attakedSoldier) == false)
            return false;

        attakedSoldier.GiveDamage(army2);
        return true;
    }

    public void Fight(Army army1, Army army2)
    {
        bool isRun = true;

        while (army1.QuantitySoldiers > 0 & army2.QuantitySoldiers > 0 & isRun)
        {
            if (TryAttack(army1, army2) == false)
                isRun = false;

            if (TryAttack(army2, army1) == false)
                isRun = false;
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