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

        Soldier1 s1 = new Soldier1(random, "SS1");
        Soldier1 s2 = new Soldier2(random, "SS2");
        Soldier1 s3 = new Soldier3(random, "SS3");
        Soldier1 s4 = new Soldier4(random, "SS4");


        List<Soldier1> _soldiers = new List<Soldier1>(armyGenerator.Generate());

        foreach (var item in _soldiers)
            item.ShowStats();

        Console.ReadKey();

    }
}

class Soldier1
{
    protected string Type;
    protected int Health;
    protected int Armor;
    protected int Damage;
    private int _minHealth = 100;
    private int _maxHealth = 150;
    private int _minArmor = 20;
    private int _maxArmor = 30;
    private int _minDamage = 20;
    private int _maxDamage = 30;

    public Soldier1(Random random, string type)
    {
        Type = type;
        Health = random.Next(_minHealth, _maxHealth);
        Armor = random.Next(_minArmor, _maxArmor);
        Damage = random.Next(_minDamage, _maxDamage);
    }

    public Soldier1(string type, int health, int armor, int damage)//конструктор для метода клон
    {
        Type = type;
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

    public virtual Soldier1 Clone() => new Soldier1(Type, Health, Armor, Damage);
}

class Soldier2 : Soldier1
{
    private int _multiplier;

    public Soldier2(Random random, string type, int multiplier = 2)
        : base(random, type)
    {
        _multiplier = multiplier;
    }

    public Soldier2(string type, int health, int armor, int damage, int multiplier)//конструктор для клона
        : base(type, health, armor, damage)
    {
        Type = type;
        Health = health;
        Armor = armor;
        Damage = damage;
        _multiplier = multiplier;
    }

    public override int GiveDamage() => Damage * _multiplier;

    public override Soldier1 Clone() => new Soldier2(Type, Health, Armor, Damage, _multiplier);
}

class Soldier3 : Soldier1
{
    private int _quantityAttacks;
    protected bool _canRepitAttakedSoldirs;

    public Soldier3(Random random, string type, int quantityAttacks = 2, bool canRepitAttacedSoldirs = false)
        : base(random, type)
    {
        _quantityAttacks = quantityAttacks;
        _canRepitAttakedSoldirs = canRepitAttacedSoldirs;
    }

    public Soldier3(string type, int health, int armor, int damage, int quantityAttacks, bool canRepitAttacedSoldirs)//конструктор для клона
       : base(type, health, armor, damage)
    {
        Health = health;
        Armor = armor;
        Damage = damage;
        _quantityAttacks = quantityAttacks;
        _canRepitAttakedSoldirs = canRepitAttacedSoldirs;
    }

    public override Soldier1 Clone() => new Soldier3(Type, Health, Armor, Damage, _quantityAttacks, _canRepitAttakedSoldirs);
}

class Soldier4 : Soldier1
{
    private int _quantityAttacks;
    protected bool _canRepitAttakedSoldirs;

    public Soldier4(Random random, string type, int quantityAttacks = 2, bool canRepitAttacedSoldirs = true)
        : base(random, type)
    {
        _quantityAttacks = quantityAttacks;
        _canRepitAttakedSoldirs = canRepitAttacedSoldirs;
    }

    public Soldier4(string type, int health, int armor, int damage, int quantityAttacks, bool canRepitAttacedSoldirs)//конструктор для клона
       : base(type, health, armor, damage)
    {
        Health = health;
        Armor = armor;
        Damage = damage;
        _quantityAttacks = quantityAttacks;
        _canRepitAttakedSoldirs = canRepitAttacedSoldirs;
    }

    public override Soldier1 Clone() => new Soldier4(Type, Health, Armor, Damage, _quantityAttacks, _canRepitAttakedSoldirs);
}


class ArmyGenerator
{
    private List<Soldier1> _soldiersType;
    private int _minQuantityOfSoldiers = 10;
    private int _maxQuantityOfSoldiers = 15;
    private Random _random;

    private int _minHealth = 100;
    private int _maxHealth = 150;
    private int _minArmor = 20;
    private int _maxArmor = 30;
    private int _minDamage = 20;
    private int _maxDamage = 30;

    public ArmyGenerator(Random random)
    {
        _soldiersType = new List<Soldier1>() { new Soldier1(random,"Type1"),
                                               new Soldier2(random,"Type2"),
                                               new Soldier3(random,"Type3"),
                                               new Soldier4(random,"Type4") };
        _random = random;

    }

    private void GenerateSoldier()
    {
        int Health = _random.Next(_minHealth, _maxHealth);
        int Armor = _random.Next(_minArmor, _maxArmor);
        int Damage = _random.Next(_minDamage, _maxDamage);


    }





    public List<Soldier1> Generate()
    {
        List<Soldier1> soldiers = new List<Soldier1>();
        int _quantityOfSoldiers = _random.Next(_minQuantityOfSoldiers, _maxQuantityOfSoldiers);

        for (int i = 0; i < _quantityOfSoldiers; i++)
            soldiers.Add(_soldiersType[_random.Next(0, _soldiersType.Count)].Clone());

        return soldiers;
    }
}




class Army
{
    private List<Soldier1> _soldiers;
    private int _minQuantityOfSoldiers = 100;
    private int _maxQuantityOfSoldiers = 150;
    public string Name { get; private set; }

    public Army(Random random)
    {


        _soldiers = new List<Soldier1>();

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