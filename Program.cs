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
        Army orks = new Army(soldiers1, "Orks", random);
        Army gnoms = new Army(soldiers2, "Gnoms", random);
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

class Soldier
{
    protected int Health;
    protected int Armor;
    protected int Damage;

    public string Type { get; protected set; }

    public Soldier(int health = 0, int armor = 0, int damage = 0)
    {
        Type = "Type 1";
        Health = health;
        Armor = armor;
        Damage = damage;
    }

    public void ShowStats() =>
        Console.WriteLine($"Type: {Type} health: {Health} damage: {Damage} armor: {Armor}");

    public bool IsAlife() =>
        (Health > 0);

    public void TakeDamage(int damage) =>
        Health -= (damage - Armor);

    public virtual bool GiveDamage(Army army)
    {
        if (army.TryGetRandomSoldier(out Soldier attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage);
        army.DeleteDeadBody();
        return true;
    }

    public virtual Soldier Clone(int health, int armor, int damage) => new Soldier(health, armor, damage);
}

class Soldier2 : Soldier
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
        if (army.TryGetRandomSoldier(out Soldier attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage * _multiplier);
        army.DeleteDeadBody();
        return true;
    }

    public override Soldier Clone(int health, int armor, int damage) =>
        new Soldier2(health, armor, damage);
}

class Soldier3 : Soldier
{
    public Soldier3(int health = 0, int armor = 0, int damage = 0)
       : base(health, armor, damage)
    {
        Type = "Type 3";
    }

    public override bool GiveDamage(Army army)
    {
        if (army.TryGetRandomSoldier(out Soldier attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage);
        army.DeleteDeadBody();

        if (army.TryGetRandomSoldier(out attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage);
        army.DeleteDeadBody();
        return true;
    }

    public override Soldier Clone(int health, int armor, int damage) =>
        new Soldier3(health, armor, damage);
}

class Soldier4 : Soldier
{
    public Soldier4(int health = 0, int armor = 0, int damage = 0)
       : base(health, armor, damage)
    {
        Type = "Type 4";
    }

    public override bool GiveDamage(Army army)
    {
        if (army.TryGetRandomSoldier(out Soldier attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage);
        army.DeleteDeadBody();

        if (army.TryGetRandomSoldier(out attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage);
        army.DeleteDeadBody();

        if (army.TryGetRandomSoldier(out attakedSoldier) == false)
            return false;

        attakedSoldier.TakeDamage(Damage);
        army.DeleteDeadBody();
        return true;
    }

    public override Soldier Clone(int health, int armor, int damage) =>
        new Soldier4(health, armor, damage);
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
    private int _maxArmor = 25;
    private int _minDamage = 30;
    private int _maxDamage = 35;

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
    private List<Soldier> _soldiers;
    private Random _random;

    public string Name { get; private set; }

    public Army(List<Soldier> soldiers, string name, Random random)
    {
        _soldiers = new List<Soldier>(soldiers);
        _random = random;
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
            if (soldier.IsAlife() == false)
            {
                _soldiers.Remove(soldier);
                break;
            }
        }
    }

    public int GetQuantityOfSoldiers() =>
        _soldiers.Count;

    public bool TryGetRandomSoldier(out Soldier soldier)
    {
        if (_soldiers.Count() > 0)
        {
            soldier = _soldiers[_random.Next(0, _soldiers.Count())];
            return true;
        }
        else
        {
            soldier = null;
            return false;
        }

    }
}

class Battle
{
    public void Fight(Army army1, Army army2)
    {
        while (army1.GetQuantityOfSoldiers() > 0 & army2.GetQuantityOfSoldiers() > 0)
        {
            if (army1.TryGetRandomSoldier(out Soldier attakedSoldierArmy1) == false)
                break;
            attakedSoldierArmy1.GiveDamage(army2);

            if (army2.GetQuantityOfSoldiers() == 0)
                break;

            if (army2.TryGetRandomSoldier(out Soldier attakedSoldierArmy2) == false)
                break;
            attakedSoldierArmy2.GiveDamage(army1);
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