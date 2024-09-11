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

        List<Soldier> _soldiers = new List<Soldier>(armyGenerator.Generate());

        foreach (var item in _soldiers)
            item.ShowStats();

        Console.ReadKey();

    }
}

class Soldier
{
    protected string Type;
    protected int Health;
    protected int Armor;
    protected int Damage;


    public Soldier(int health = 0, int armor = 0, int damage = 0)
    {
        Type = "T1";
        Health = health;
        Armor = armor;
        Damage = damage;
    }

    public void ShowStats()
    {
        Console.WriteLine($"Type: {Type} health: {Health} damage: {Damage}");
    }

    public bool IsAlife() => (Health > 0);

    public void TakeDamage(int damage, bool isAttack)
    {
        if (isAttack)
            Health -= damage;
        else
            Health -= (damage - Armor);
    }

    public virtual int GiveDamage() => Damage;

    public virtual Soldier Clone(int health, int armor, int damage) => new Soldier(health, armor, damage);
}

class Soldier2 : Soldier
{
    private int _multiplier;

    public Soldier2(int health = 0, int armor = 0, int damage = 0, int multiplier = 2)
        : base(health, armor, damage)
    {
        Type = "T2";
        Health = health;
        Armor = armor;
        Damage = damage;
        _multiplier = multiplier;
    }

    public override int GiveDamage() => Damage * _multiplier;

    public override Soldier Clone(int health, int armor, int damage) => new Soldier2(health, armor, damage, _multiplier);
}

class Soldier3 : Soldier
{
    private int _quantityAttacks;
    protected bool _canRepitAttakedSoldirs = false;

    public Soldier3(int health = 0, int armor = 0, int damage = 0, int quantityAttacks = 2)
       : base(health, armor, damage)
    {
        Type = "T3";
        Health = health;
        Armor = armor;
        Damage = damage;
        _quantityAttacks = quantityAttacks;
    }

    public override Soldier Clone(int health, int armor, int damage) => new Soldier3(health, armor, damage, _quantityAttacks);

}

class Soldier4 : Soldier
{
    private int _quantityAttacks;
    protected bool _canRepitAttakedSoldirs = true;

    public Soldier4(int health = 0, int armor = 0, int damage = 0, int quantityAttacks = 3)
       : base(health, armor, damage)
    {
        Type = "T4";
        Health = health;
        Armor = armor;
        Damage = damage;
        _quantityAttacks = quantityAttacks;
    }

    public override Soldier Clone(int health, int armor, int damage) => new Soldier4(health, armor, damage, _quantityAttacks);

}


class ArmyGenerator
{
    private List<Soldier> _soldiersType;
    private Random _random;
    private int _minQuantitySoldiers = 10;
    private int _maxQuantitySoldiers = 15;
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
        int _quantitySoldiers = _random.Next(_minQuantitySoldiers, _maxQuantitySoldiers);

        for (int i = 0; i < _quantitySoldiers; i++)
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

interface IClonable
{

}



class Army
{
    private List<Soldier> _soldiers;
    private int _minQuantityOfSoldiers = 100;
    private int _maxQuantityOfSoldiers = 150;
    public string Name { get; private set; }

    public Army(Random random)
    {


        _soldiers = new List<Soldier>();

    }

    public void ShowAll()
    {
        foreach (var soldier in _soldiers)
            soldier.ShowStats();
    }

    public void DeleteDeadBodys()
    {
        int quantityOfRepit = 0;

        while (quantityOfRepit < _soldiers.Count)
        {
            if (_soldiers[quantityOfRepit].IsAlife() == true)
                quantityOfRepit++;
            else
                _soldiers.RemoveAt(quantityOfRepit);
        }
    }

    public int GetQuantityOfSoldiers()
    {
        return _soldiers.Count;
    }

    public void TakeDamage(Army enemy, int lenght, bool isAttack)
    {
        for (int i = 0; i < lenght; i++)
            _soldiers[i].TakeDamage(enemy._soldiers[i].GiveDamage(), isAttack);
    }
}

class Fight
{
    public void GoFight(Army army1, Army army2)
    {
        bool isAttack = true;
        bool isProtection = false;

        if (army1.GetQuantityOfSoldiers() >= army2.GetQuantityOfSoldiers())
        {
            int length = army2.GetQuantityOfSoldiers();
            army2.TakeDamage(army1, length, isProtection);
            army1.TakeDamage(army2, length, isAttack);
        }
        else
        {
            int length = army1.GetQuantityOfSoldiers();
            army1.TakeDamage(army2, length, isProtection);
            army2.TakeDamage(army1, length, isAttack);
        }

        army2.DeleteDeadBodys();
        army1.DeleteDeadBodys();
    }

    public void ShowWinner(Army army1, Army army2)
    {
        if (army1.GetQuantityOfSoldiers() > army2.GetQuantityOfSoldiers())
            Console.WriteLine($"Army {army1.Name} - win!");
        else
            Console.WriteLine($"Army {army2.Name} - win!");
    }
}