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
  

    public Soldier(string type = "", int health = 0, int armor = 0, int damage = 0)//конструктор для метода клон
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

    public virtual Soldier Clone() => new Soldier(Type, Health, Armor, Damage);

    public  Soldier Clone(string type , int health , int armor , int damage ) => new Soldier(type, health, armor, damage);
}

class Soldier2 : Soldier
{
    private int _multiplier;

    public Soldier2(string type = "", int health = 0, int armor = 0, int damage = 0, int multiplier = 2)//конструктор для клона
        : base(type, health, armor, damage)
    {
        Type = type;
        Health = health;
        Armor = armor;
        Damage = damage;
        _multiplier = multiplier;
    }

    public override int GiveDamage() => Damage * _multiplier;

    public override Soldier Clone() => new Soldier2(Type, Health, Armor, Damage, _multiplier);

    public  Soldier Clone(string type , int health , int armor , int damage , int multiplier ) => new Soldier2(type, health, armor, damage, multiplier);
}

class Soldier3 : Soldier
{
    private int _quantityAttacks;
    protected bool _canRepitAttakedSoldirs=false;

    public Soldier3(string type = "", int health = 0, int armor = 0, int damage = 0, int quantityAttacks = 2, bool canRepitAttacedSoldirs = false)//конструктор для клона
       : base(type, health, armor, damage)
    {
        Health = health;
        Armor = armor;
        Damage = damage;
        _quantityAttacks = quantityAttacks;
        _canRepitAttakedSoldirs = canRepitAttacedSoldirs;
    }

    public override Soldier Clone() => new Soldier3(Type, Health, Armor, Damage, _quantityAttacks, _canRepitAttakedSoldirs);

    public Soldier Clone(string type, int health, int armor, int damage, int quantityAttacks, bool canRepitAttacedSoldirs) => new Soldier3(type, health, armor, damage, quantityAttacks, canRepitAttacedSoldirs);

}

class Soldier4 : Soldier
{
    private int _quantityAttacks;
    protected bool _canRepitAttakedSoldirs;

    public Soldier4(string type = "", int health = 0, int armor = 0, int damage = 0, int quantityAttacks = 0, bool canRepitAttacedSoldirs = false)//конструктор для клона
       : base(type, health, armor, damage)
    {
        Health = health;
        Armor = armor;
        Damage = damage;
        _quantityAttacks = quantityAttacks;
        _canRepitAttakedSoldirs = canRepitAttacedSoldirs;
    }

    public override Soldier Clone() => new Soldier4(Type, Health, Armor, Damage, _quantityAttacks, _canRepitAttakedSoldirs);

    public Soldier Clone(string type, int health, int armor, int damage, int quantityAttacks, bool canRepitAttacedSoldirs) => new Soldier3(type, health, armor, damage, quantityAttacks, canRepitAttacedSoldirs);

}


class ArmyGenerator
{
    private List<Soldier> _soldiersType;
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
        _soldiersType = new List<Soldier>() { new Soldier("Type1"),
                                               new Soldier2("Type2"),
                                               new Soldier3("Type3"),
                                               new Soldier4("Type4") };
        _random = random;
    }
 

    public List<Soldier> Generate()
    {
        List<Soldier> soldiers = new List<Soldier>();
        int _quantityOfSoldiers = _random.Next(_minQuantityOfSoldiers, _maxQuantityOfSoldiers);

        for (int i = 0; i < _quantityOfSoldiers; i++)
        {
            int soldierType = _random.Next(0, _soldiersType.Count);
            int health = _random.Next(_minHealth, _maxHealth);
            int armor = _random.Next(_minArmor, _maxArmor);
            int damage = _random.Next(_minDamage, _maxDamage);

            var s = _soldiersType[soldierType];

            Soldier sold = new Soldier ();


            soldiers.Add(_soldiersType[soldierType].Clone());




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