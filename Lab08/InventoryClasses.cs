using System.Globalization;

namespace Lab08;

public class GameObject
{
    public string? Name{get; set;}
    public int Tier{get; set;}
    public int Durability {get; set;}
    public int Damage{get; set;}
    public int Protection{get; set;}

    public GameObject()
    {
        setName();
        Tier = setTier();
        Durability = setDurability();
    }

    public virtual int setDamage()
    {
        return 0;
    }
    public virtual void setProtactionLevel()
    {
        
    }
    public virtual int setDurability()
    {
        return 0;
    }
    public virtual int setTier()
    {
        return 0;
    }
    public virtual int Cost()
    {
        return 0;
    }
    public virtual void setName()
    {
        Name = "GameObject";
    }
}

public class generalItem : GameObject
{
    generalItem()
    {
        setName();
    }
    public generalItem(string name)
    {
        Name = name;
    }

    public override int Cost()
    {
        switch(Name)
        {
            case "health potion":
            return 40;
            case "rope":
            return 20;
            default:
            return 0;
        }
    }
}

public class Armor : GameObject
{
    //If you have an armor in your invetory you will have protection
    //The higher tier armors will automaticly be equiped
    public string? Material;
    public int Durabilty{get; set;}
    
    public Armor()
    {
        //Material will be random as well I just need to create a random material generator. 
        Material = null;
        Durability = randomDurabilty();
        Protection = randomProtectionLevel();
        setName();
    }
    public Armor(string material)
    {
        if(IsAcceptedMaterial(material) == true)
        {
            Material = material;
        }
        setDurability();
        setProtactionLevel();
        setName();
    }
    public Armor(GameObject gameObject)
    {
        Name = gameObject.Name;
        Durability = gameObject.Durability;

    }

    public string? getMaterial(string input)
    {
        string[] split = input.Split();
        if(IsAcceptedMaterial(split[0]) == true)
        {
            return split[0];
        }else if(IsAcceptedMaterial($"{split[0]} {split[1]}"))
        {
            return $"{split[0]} {split[1]}";
        }else
        {
            return Material;
        }
    }
    public override void setProtactionLevel() //taking damage from a weapon will depend on the protection level
    {
        switch(Material)
        {
            case "wood":
            Protection = 5;
            break;
            case "iron":
            Protection = 10;
            break;
            case "steel":
            Protection = 15;
            break;
            case "titanium":
            Protection = 20;
            break;
            case "carbon fiber":
            Protection = 25;
            break;
            case null:
            default:
            Protection = 0;
            break;
        }
    }
    public int randomProtectionLevel()
    {
        Random rand = new Random();
        return rand.Next(0,26);
    }
    public override int setDurability()
    {
        switch(Material)
        {
            case "wood":
            return 5;
            case "iron":
            return 10;
            case "steel":
            return 15;
            case "titanium":
            return 20;
            case "carbon fiber":
            return 25;
            case null:
            default:
            return base.setDurability();
        }
    }
    public int randomDurabilty()
    {
        //Random durability
        Random rand = new Random();
        return rand.Next(0,26);
    }
    public override int setTier()
    {
        switch(Material)
        {
            case "wood":
            return 1;
            case "iron":
            return 2;
            case "steel":
            return 3;
            case "titanium":
            return 4;
            case "carbon fiber":
            //highest tier because the degregation is the lowest and the protection level is the highest.
            return 5;
            case null:
            default:
            return 0;
        }
    }

    public bool IsAcceptedMaterial(string? input)
    {
        switch(input)
        {
            case "wood":
            case "iron":
            case "steel":
            case "titanium":
            case "carbon fiber":
            Material = input;
            return true;
            //very light weight and high protection level so the duribility will go down less
            case null:
            default:
            return false;
        }
    }
    public override int Cost()
    {
        switch(Material)
        {
            case "wood":
            return 10;
            case "iron":
            return 20;
            case "steel":
            return 30;
            case "titanium":
            return 40;
            case "carbon fiber":
            return 50;
            case null:
            default:
            return 0;
        }
    }

    public override void setName()
    {
        Name = $"{Material} Armor";
    }
}

public class Weapon : GameObject, IMaterial
{
    public string? Material {get; set;}

    public Weapon()
    {
        Tier = setTier();
    }
    public Weapon(string name)
    {
        Name = name;
        Tier = setTier();
    }
    public Weapon(GameObject gameObject)
    {
        Name = gameObject.Name;
        gameObject.Durability = Durability;
        gameObject.Damage = Damage;
        gameObject.Tier = Tier;
        Material = getMaterial(gameObject.Name);
    }

    public string? getMaterial(string? input)
    {
        string[] split = input.Split();
        if(IsAcceptedMaterial(split[0]) == true)
        {  
            return split[0];
        }else if(IsAcceptedMaterial($"{split[0]} {split[1]}"))
        {
            return $"{split[0]} {split[1]}";
        }else
        {
            return Material;
        }
    }
    public override int setDamage()
    {
        if(Name == "fists")
        {
            return 1;
        }else
        {
            return 0;
        }
    }
    public virtual bool IsAcceptedMaterial(string input)
    {
       if(new Sword().IsAcceptedMaterial(input) == true || new Bow().IsAcceptedMaterial(input) == true)
       {
            return true;
       }else
       {
            return false;
       }
    }
    public override int setDurability()
    {
        if(Name == "Fists")
        {
            return 50;
        }else
        {
           return base.setDurability();
        }
    }
    public override int setTier()
    { // based off the name of the weapon the tier will change.
        return 0;
    }
    public override void setName()
    {
        Name = "Weapon";
    }
}

public class Sword : Weapon
{
    public Sword()
    {
        setName();
        Damage = randomDamage();
        Durability = randomDurabilty();
    }
    public Sword(string material)
    {
        if (IsAcceptedMaterial(material) == true)
        {
            Material = material;
        }
        setName();
        Damage = setDamage();
        Durability = setDurability();
    }

    public override int setDamage()
    {
        switch(Material)
        {
            case "wood":
            return 10;
            case "iron":
            return 20;
            case "steel":
            return 30;
            case "titanium":
            return 40;
            case "carbon fiber":
            return 50;
            default:
            return base.setDamage();
        }
    }

    public int randomDamage()
    {
        Random rand = new Random();
        return rand.Next(0,51);
    }

    public override int setDurability()
    {
        switch(Material)
        {
            case "wood":
            return 5;
            case "iron":
            return 10;
            case "steel":
            return 15;
            case "titanium":
            return 20;
            case "carbon fiber":
            return 30;
            default:
            return base.setDurability();
        }
    }
    public int randomDurabilty()
    {
        Random rand = new Random();
        return rand.Next(0,31);
    }
    public override bool IsAcceptedMaterial(string? input)
    {
        switch(input)
        {
            case "wood":
            case "iron":
            case "steel":
            case "titanium":
            case "carbon fiber":
            Material = input;
            return true;
            //very light weight and high protection level so the duribility will go down less
            case null:
            default:
            return false;
        }
    }

    public override int setTier()
    {
        switch(Material)
        {
            case "wood":
            return 1;
            case "iron":
            return 2;
            case "steel":
            return 3;
            case "titanium":
            return 4;
            case "carbon fiber":
            return 5;
            default:
            return base.setTier();
        }
    }
    public override int Cost()
    {
         // needs to be set based off the material or tier
        switch(Material)
        {
            case "wood":
            return 15;
            case "iron":
            return 30;
            case "steel":
            return 45;
            case "titanium":
            return 60;
            case "carbon fiber":
            return 85;
            default:
            return base.Cost();
        }
    }

    public override void setName()
    {
        Name = $"{Material} Sword";
    }
}

public class Bow : Weapon
{
    Arrow currentArrow{get;set;}

    public Bow()
    {
        Durability = randomDurabilty();
        currentArrow = new Arrow();
    }
    public Bow(string material)
    {
        if(IsAcceptedMaterial(material) == true)
        {
            Material = material;
        }
        setName();
        currentArrow = new Arrow();
    }
    public Bow(string material, Arrow arrow)
    {
        if(IsAcceptedMaterial(material) == true)
        {
            Material = material;
        }
        currentArrow = arrow;
    }

    public override int setDamage()
    {
        return DamageMultiplyer() + currentArrow.setDamage();
    }
    public int DamageMultiplyer() //If you shoot a certain arrow with a bow the multipler will mutiply that damage.
    {
        switch(Material)
        {
            case "wood":
            return 2;
            case "horn":
            return 5;
            case "carbon fiber":
            return 7;
            default:
            return 0;
        }
    }
    public override int setDurability()
    {
        switch(Material)
        {
            case "wood":
            return 5;
            case "horn":
            return 10;
            case "carbon fiber":
            return 15;
        }
        return base.setDurability();
    }
    public int randomDurabilty()
    {
        Random rand = new Random();
        return rand.Next(0,15);
    }
    public override bool IsAcceptedMaterial(string? input)
    {
        switch(input)
        {
            case "wood":
            case "horn":
            case "carbon fiber":
            Material = input;
            return true;
            case null:
            default:
            return false;
        }
    }
    public override int setTier()
    {
        switch(Material)
        {
            case "wood":
            return 1;
            case "horn":
            return 2;
            case "carbon fiber":
            return 3;
            default:
            return 0;
        }
    }
    public override int Cost()
    {
        switch(Material)
        {
            case "wood":
            return 5;
            case "horn":
            return 25;
            case "carbon fiber":
            return 25;
            default:
            return base.Cost();
        }
    }
    public override void setName()
    {
        Name = $"{Material} Bow";
    }
}

public class Arrow : Weapon, IFleching , IArrowHead
{
    public string? Fleching;
    public string? Arrowhead;

    public Arrow()
    {
        Fleching = null;
        Arrowhead = null;
    }
    public Arrow(string? fleching, string? arrowhead)
    {
        Fleching = fleching;
        Arrowhead = arrowhead;
        setName();
    }

    public void DescribeFletching()
    {
        Console.WriteLine(Fleching);
    }
    public int GetCostFleching()
    {
        //plastic costs 10,turkey costs 5 and goose costs 3
        int cost = 0;
        switch (Fleching)
        {
            case "plastic":
            cost = 10;
            break;
            case "turkey":
            cost = 5;
            break;
            case "goose":
            cost = 3;
            break;
        }
        return cost;
    }
    public void DescribeArrowHead()
    {
        Console.WriteLine(Arrowhead);
    }
    public int GetCostArrowHead()
    {
        //steel costs 10, wood costs 3 obsidian costs 5
        int cost = 0;
        switch (Arrowhead)
        {
            case "steel":
            cost = 10;
            break;
            case "obsidian":
            cost = 5;
            break;
            case "wood":
            cost = 3;
            break;
        }
        return cost;
    }
    public override int setDamage()
    {
        switch(setTier())
        {
            case 1:
            case 2:
            case 3:
            return 2;
            case 4:
            case 5:
            case 6:
            return 5;
            case 7:
            case 8:
            case 9:
            return 7;
            default:
            return 0;
        }
    }
    public override int setTier()
    {
        string? tier = Arrowhead  + " " + Fleching;
        switch (tier)
        {
            case "wood goose":
            return 1;
            case "wood turkey":
            return 2;
            case "wood plastic":
            return 3;
            case "obsidian goose":
            return 4;
            case "obsidian turkey":
            return 5;
            case "obsidian plastic":
            return 6;
            case "steel goose":
            return 7;
            case "steel turkey":
            return 8;
            case "steel plastic":
            return 9;
            default:
            return 0;
        }
    }
    public override int Cost()
    {
        return GetCostArrowHead() + GetCostFleching();
    }
    public override void setName()
    {
        Name = $"{Arrowhead} {Fleching} Arrow";
    }
}