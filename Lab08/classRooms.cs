using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace Lab08;

public class Room
{
    //What defines a room?
    //A room gives off sense and has an actual thing in it

    public string? sense = null;
    public char item;
    public virtual Object occupied{get; set;}
    public virtual Monster monster{get; set;}

    public Room()
    {
        //An empty room just has a space in it and gives off no sense.
        sense = " ";
        item = ' ';
        occupied = new object();
    }
}

public class Monster : IMonster, IInventory, IHealth
{
    public int Gold{get; set;}
    public string? monsterName{get; set;}
    public int MaxHealth{get; set;}
    public int currentHealth{get; set;}
    public int maxItems { get; set; }
    public List<GameObject> Inventory{ get; set;}
    public Weapon currentWeapon{get; set;}
    public Armor currentArmor{get; set;}

    public Monster()
    {
        Inventory = new List<GameObject>();
        currentArmor = new Armor("Iron");
        currentWeapon = new Sword("Iron");
        setMaxHealth();
        setCurrentHealth();
        setMonsterName();
        setCoinAmount();
    }

    public void setCurrentHealth()
    {
        currentHealth = MaxHealth;
    }

    public virtual void setMaxHealth()
    {
        MaxHealth = 0;
    }
    public virtual void setMaxItems()
    {
        maxItems = 0;
    }
    public virtual void setCoinAmount()
    {
        Gold = 10;
    }
    public void addToInventory(GameObject gameObject)
    {
        if(canAddItems() == true)
        {
            Inventory.Add(gameObject);
        }
    }

    public virtual void equip()
    {
        Sword basic = new Sword("Iron");
        Armor basicArmor = new Armor("Iron");
        currentWeapon = basic;
        currentArmor = basicArmor;
        addToInventory(basic);
        addToInventory(basicArmor);
    }

    public bool canAddItems()
    {
        if(Inventory.Count() < maxItems)
        {
            return true;
        }
        return false;
    }

    public virtual void setMonsterName()
    {
        monsterName = "Monster";
    }
    public string displayMonster()
    {
        setMonsterName();
        return monsterName;
    }

    public virtual string? Aroma()
    {
        return "You here growling there is a monster nearby.";
    }

    public override string ToString()
    {
        setMonsterName();
        return monsterName;
    }
}

public class MonsterRoom : Room
{
    //A monster room has a monster in it.
    Monster currentMonster{get; set;}
    public override Monster monster { get => currentMonster; set => currentMonster = value; }

    public MonsterRoom()
    {
        currentMonster = GetMonster();
        occupied = currentMonster;
        sense = $"You encounter a {currentMonster} as you scramble to pull out your weapon";

    }


    public Monster getCurrentMonster()
    {
        return currentMonster;
    }    
    public Monster GetMonster()
    {
        Random rand = new Random();
        int pickMonster = rand.Next(0,101);
        if(pickMonster > 49)
        {
            item = 'A';
            return new Amarok();
        }else
        {
            item = 'M';
            return new Maelstrom();
        }
    }
}

public class FountainRoom : Room
{
    //The Fountain has the Fountain of Objects in it
    //The Fountain of objects can not be a monster room or a pit room.
    //You should be able to sense other things around it though.
    public FountainRoom()
    {
        //If the fountain is not enabled you only hear a small trickle
        sense = "You hear water dripping in this room. The fountain of objects is here!";
        item = 'F';
        occupied = new object();
    }

    public bool isEnabled()
    {
        return false;
    }
}

public class PitRoom : Room
{
    //Still will not have instant death
    //When the player enters a pit room they take damage 
    //If they have a rope they can move one room up or North of the pit room thats it.
    //If the player does not have a rope and cannot get out they can call for help
    //Calling for help will have a 50% chance of doing nothing. The other 50% chance a monster will come into the pit and attack.
    //This will give the person a chance of running into a Maelstrom witch will telport them out or getting a rope from the inventory of the monster
    public static string Aroma = "You feel a draft. There is a pit in a nearby room";
    public PitRoom()
    {
        item = 'P';
        sense = "You hit the ground hard. You fell in a pit";
    }
}

public class Amarok : Monster
{
    //Amaroks will die and clear the room when defeated in battle
    //If you lose to them in battle or run out of health you die.
    public override void setMaxHealth()
    {
        MaxHealth = 30;
    }

    public override void setMonsterName()
    {
        monsterName = "Amarok";
    }

    public override void setMaxItems()
    {
        maxItems = 3;
    }

    public override string? Aroma()
    {
        return "You can smell the rotten stench of an amarock in a nearby room";
    }
    public override void setCoinAmount()
    {
        Gold = 20;
    }
}

public class Maelstrom : Monster
{
    //Maelstroms never really die if you defeat them in battle they just move you to another location and move themselfs
    //If you lose to them in battle or run out of health you die.

    public override void setMaxHealth()
    {
        MaxHealth = 20;
    }
    public override void setMonsterName()
    {
        monsterName = "Maelstrom";
    }

    public override void setMaxItems()
    {
        maxItems = 2;
    }

    public override void setCoinAmount()
    {
        Gold = 15;
    }

    public override string? Aroma()
    {
        return "You here the growling and groaning of a maelstrom nearby";
    }
}

public class Player : IHealth, IInventory
{
    public int Gold = 500;
    public int MaxHealth{get; set;}
    public int currentHealth{get; set;}
    public int maxItems {get; set;}
    public List<GameObject>? Inventory{get; set;}
    public Weapon? currentWeapon;
    public Armor? currentArmor;

    public Player()
    {
        Inventory = new List<GameObject>();
        currentWeapon = Fists();
        Inventory.Add(Fists());
        currentArmor = basic();
        Inventory.Add(basic());
        setMaxHealth();
        setCurrentHealth();
        setMaxItems();
    }

    public Armor basic()
    {
        return new Armor("Wood");
    }
    public int getGold()
    {
        return Gold;
    }
    public void addGold(int gold)
    {
        Gold = Gold + gold;
    }

    public void removeGold(int gold)
    {
        Gold = Gold - gold;
    }

    public void addToInventory(GameObject gameObject)
    {
        if(canAddItems() == true)
        {
            Inventory.Add(gameObject);
        }else
        {
            Console.WriteLine("You have no weapon equiped");
        }
    }

    public void discardWeapon()
    {
        if(canRemoveItem(currentWeapon) == true)
        {
            Inventory.Remove(currentWeapon);
        }else
        {
            Console.WriteLine("You have no armor equuiped");
        }
    }

    public void discardArmor()
    {
        if(canRemoveItem(currentArmor) == true)
        {
            Inventory.Remove(currentArmor);
        }
    }
    public void setMaxHealth()
    {
        MaxHealth = 50;
    }

    public void setCurrentHealth()
    {
        currentHealth = MaxHealth;
    }

    public void setMaxItems()
    {
        maxItems = 10;
    }

    public void setCurrentWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }
    public void setCurrentArmor(Armor armor)
    {
        currentArmor = armor;
    }

    public Weapon Fists()
    {
        Weapon fists = new Weapon();
        fists.Name = "Fists";
        return fists;
    }

    public bool canAddItems()
    {
        if(Inventory.Count > maxItems)
        {
            return false;
        }else
        {
            return true;
        }
    }

    public bool canRemoveItem(GameObject gameObject)
    {
        if(Inventory.Contains(gameObject))
        {
            return true;
        }
        if(Inventory.Count > 0)
        {
            return true;
        }
        return false;
    }

    public void displayInventory()
    {
        foreach(GameObject item in Inventory)
        {
            Console.Write($"{item}, ");
        }
    }
}