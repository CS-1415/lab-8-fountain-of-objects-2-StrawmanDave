namespace Lab08;

public class Room
{
    //What defines a room?
    //A room gives off sense and has an actual thing in it

    public string? sense = null;
    public char item;
    public virtual object occupied{get; set;}
    public virtual Monster monster{get; set;}

    public Room()
    {
        //An empty room just has a space in it and gives off no sense.
        sense = " ";
        item = ' ';
        occupied = new object();
        monster = new Monster();
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

    public MonsterRoom(Monster monster)
    {
        currentMonster = monster;
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
        currentArmor = new Armor();
        currentWeapon = new Weapon();
        equip();
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
        currentArmor = new Armor();
        currentWeapon = new Weapon();
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
        if(monsterName == null)
        {
            throw new NullReferenceException();
        }
        return monsterName;
    }

    public virtual string? Aroma()
    {
        return "You here growling there is a monster nearby.";
    }

    public override string ToString()
    {
        setMonsterName();
        if(monsterName == null)
        {
            throw new NullReferenceException();
        }
        return monsterName;
    }
}

public class Amarok : Monster
{
    //Amaroks will die and clear the room when defeated in battle
    //If you lose to them in battle or run out of health you die.
    public override void setMaxHealth()
    {
        MaxHealth = 200;
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

    public override void equip()
    {
        currentArmor = new Armor();
        currentWeapon = new Sword();
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
        MaxHealth = 150;
    }
    public override void setMonsterName()
    {
        monsterName = "Maelstrom";
    }

    public override void setMaxItems()
    {
        maxItems = 3;
    }

    public override void equip()
    {
        currentArmor = new Armor();
        currentWeapon = new Sword();
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
    public List<GameObject> Inventory {get; set;}
    public Weapon? currentWeapon;
    public Armor? currentArmor;

    public Player()
    {
        setMaxItems();
        setMaxHealth();
        Inventory = new List<GameObject>();
        setCurrentHealth();
        setCurrentArmor(new Armor());
        setCurrentWeapon(new Weapon("fists"));
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
        setBestArmor();
        setBestWeapon();
    }
    public void setMaxHealth()
    {
        MaxHealth = 100;
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

    public bool canAddItems()
    {
        if(Inventory == null)
        {
            Inventory = new List<GameObject>();
        }
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
        if(Inventory == null)
        {
            Inventory = new List<GameObject>();
        }

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
    public void setBestWeapon()
    {
        for(int i = 0; i< Inventory.Count(); i++)
        {
            if(Inventory[i].Tier > currentWeapon.Tier)
            {
                setCurrentWeapon(new Weapon(Inventory[i]));
            }
        }
    }
    public void setBestArmor()
    {
        for(int i = 0; i<Inventory.Count(); i++)
        {
            if(Inventory[i].Tier > currentArmor.Tier)
            {
                setCurrentArmor(new Armor(Inventory[i]));
            }
        }
    }
    
    public void displayInventory()
    {
        foreach(GameObject item in Inventory)
        {
            Console.Write($"{item.Name}, ");
        }
    }
}