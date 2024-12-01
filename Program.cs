using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        ArmyGenerator armyGenerator = new ArmyGenerator(random);

        List<Soldier> soldiers1 = new List<Soldier>(armyGenerator.Generate());
        List<Soldier> soldiers2 = new List<Soldier>(armyGenerator.Generate());

        Army a1 = new Army(soldiers1, "Orks", random);
        Army a2 = new Army(soldiers2, "Gnoms", random);

        a1.ShowAllSoldiers();
        Console.WriteLine();
        Console.WriteLine();
        a2.ShowAllSoldiers();

        Console.ReadKey();

        Fight fight = new Fight();
        fight.GoFight(a1, a2);
        fight.ShowWinner(a1, a2);

        a1.ShowAllSoldiers();
        Console.WriteLine();
        Console.WriteLine();
        a2.ShowAllSoldiers();

    }
}

class Soldier
{
    protected string Type;
    protected int Health;
    protected int Armor;
    protected int Damage;
    protected int QuantityAttacks;
    protected bool CanRepitAttakedSoldirs;

    public Soldier(int health = 0, int armor = 0, int damage = 0)
    {
        Type = "T1";
        Health = health;
        Armor = armor;
        Damage = damage;
        QuantityAttacks = 1;
        CanRepitAttakedSoldirs = false;
    }

    public void ShowStats() => Console.WriteLine($"Type: {Type} health: {Health} damage: {Damage}");

    public bool IsAlife() => (Health > 0);

    public void TakeDamage(Soldier attackedSoldier) => Health -= (attackedSoldier.Damage - Armor);

    public virtual Soldier GiveDamage() => Clone(Health, Armor, Damage);

    public virtual Soldier Clone(int health, int armor, int damage) => new Soldier(health, armor, damage);
}

class Soldier2 : Soldier
{
    private int _multiplier;

    public Soldier2(int health = 0, int armor = 0, int damage = 0)
        : base(health, armor, damage)
    {
        Type = "T2";
        _multiplier = 2;
    }

    public override Soldier GiveDamage() => Clone(Health, Armor, Damage * _multiplier);

    public override Soldier Clone(int health, int armor, int damage) => new Soldier2(health, armor, damage);
}

class Soldier3 : Soldier
{
    public Soldier3(int health = 0, int armor = 0, int damage = 0)
       : base(health, armor, damage)
    {
        Type = "T3";
        QuantityAttacks = 2;
        CanRepitAttakedSoldirs = false;
    }

    public override Soldier Clone(int health, int armor, int damage) => new Soldier3(health, armor, damage);
}

class Soldier4 : Soldier
{
    public Soldier4(int health = 0, int armor = 0, int damage = 0)
       : base(health, armor, damage)
    {
        Type = "T4";
        QuantityAttacks = 3;
        CanRepitAttakedSoldirs = true;
    }

    public override Soldier Clone(int health, int armor, int damage) => new Soldier4(health, armor, damage);
}

class ArmyGenerator
{
    private List<Soldier> _soldiersType;
    private Random _random;
    private int _minSoldiers = 10;
    private int _maxSoldiers = 15;
    private int _minHealth = 100;
    private int _maxHealth = 150;
    private int _minArmor = 20;
    private int _maxArmor = 30;
    private int _minDamage = 20;
    private int _maxDamage = 30;

    public ArmyGenerator(Random random)
    {
        _soldiersType = new List<Soldier>() { new Soldier(),
                                              new Soldier2(),
                                              new Soldier3(),
                                              new Soldier4() };
        _random = random;
    }

    public List<Soldier> Generate()
    {
        List<Soldier> soldiers = new List<Soldier>();

        for (int i = 0; i < _random.Next(_minSoldiers, _maxSoldiers); i++)
        {
            int soldierType = _random.Next(0, _soldiersType.Count);
            int health = _random.Next(_minHealth, _maxHealth);
            int armor = _random.Next(_minArmor, _maxArmor);
            int damage = _random.Next(_minDamage, _maxDamage);

            soldiers.Add(_soldiersType[soldierType].Clone(health, armor, damage));
        }

        return soldiers;
    }
}


class Army
{
    public List<Soldier> Soldiers;

    private Random _random;

    public string Name { get; private set; }

    public Army(List<Soldier> soldiers, string name, Random random)
    {
        Soldiers = new List<Soldier>(soldiers);
        Name = name;
        _random = random;
    }

    public void ShowAllSoldiers()
    {
        foreach (var soldier in Soldiers)
            soldier.ShowStats();
    }

    public void DeleteDeadBody()
    {
        foreach (var soldier in Soldiers)
        {
            if (soldier.IsAlife() == false)
            {
                Soldiers.Remove(soldier);
                break;
            }
        }
    }

    public int GetQuantityOfSoldiers() => Soldiers.Count;

    public Soldier GetRandomSoldier() => Soldiers[_random.Next(0, Soldiers.Count())];
}

class Fight
{
    public void GoFight(Army army1, Army army2)
    {
        while (army1.GetQuantityOfSoldiers() > 0 & army2.GetQuantityOfSoldiers() > 0)
        {
            army2.GetRandomSoldier().TakeDamage(army1.GetRandomSoldier().GiveDamage());
            army2.DeleteDeadBody();

            army1.GetRandomSoldier().TakeDamage(army1.GetRandomSoldier().GiveDamage());
            army1.DeleteDeadBody();
        }
    }

    public void ShowWinner(Army army1, Army army2)
    {
        if (army1.GetQuantityOfSoldiers() > army2.GetQuantityOfSoldiers())
            Console.WriteLine($"Army {army1.Name} - win!");
        else
            Console.WriteLine($"Army {army2.Name} - win!");
    }
}