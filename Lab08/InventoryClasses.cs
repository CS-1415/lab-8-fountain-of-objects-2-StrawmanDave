namespace Lab08;

public class GameObject
{
    //What do all game objects have in comman?
    int mass; //Players and monsters will could have a max mass they can carry
    int volume; //Consider making a backpack object with only mass that adds to players volume or item count.

    public virtual int Cost()
    {
        return 0;
    }
    //Revist inventory with mass and volume considerations
}

public class Armor : GameObject, IMaterial
{
    //If you have an armor in your invetory you will have protection
    //The higher tier armors will automaticly be equiped
    public string? Material;
    public int Durabilty{get; set;}
    public int Protection{get; set;}
    int degration; //Depends on the tier and the weapon used

    public Armor()
    {
        //Material will be random as well I just need to create a random material generator. 
        Durabilty = randomDurabilty();
        Material = "Wood";
        setProtactionLevel();
    }

    public Armor(string material)
    {
        if(IsAcceptedMaterial(material) == true)
        {
            Material = material;
            Durabilty = 100;
        }
        setProtactionLevel();
    }

    public void DescribeMaterial()
    {
        Console.WriteLine(Material);
    }
    public int randomDurabilty()
    {
        //Random durability
        Random rand = new Random();
        return rand.Next(0,101);
    }

    public void  setProtactionLevel() //taking damage from a weapon will depend on the protection level
    {
        switch(Material)
        {
            case "Wood":
            Protection = 5;
            break;
            case "Iron":
            Protection = 10;
            break;
            case "Steel":
            Protection = 15;
            break;
            case "Titanium":
            Protection = 20;
            break;
            case "Carbon Fiber":
            //very light weight and high protection level so the duribility will go down less
            Protection = 25;
            break;
        }
    }
    public int Tier()
    {
        int tier = 0;
        switch(Material)
        {
            case "Wood":
            tier = 1;
            break;
            case "Iron":
            tier = 2;
            break;
            case "Steel":
            tier = 3;
            break;
            case "Titanium":
            tier = 4;
            break;
            case "Carbon Fiber":
            //highest tier because the degregation is the lowest and the protection level is the highest.
            tier = 5;
            break;
        }
        return tier;
    }

    public bool IsAcceptedMaterial(string? input)
    {
        switch(input)
        {
            case "Wood":
            case "Iron":
            case "Steel":
            case "Titanium":
            case "Carbon Fiber":
            Material = input;
            return true;
            //very light weight and high protection level so the duribility will go down less
            case null:
            default:
            Console.WriteLine("Sorry we can not make armor out of that material please use a material that we can work with.");
            return false;
        }
    }
}

public class generalItem : GameObject
{
    string? Name;

    public generalItem(string name)
    {
        Name = name;
    }

    public override int Cost()
    {
        switch(Name)
        {
            case "Health Potion":
            return 30;
            case "Rope":
            return 20;
            default:
            return 0;
        }
    }
}

public class Weapon : GameObject
{
    public string? Name{get; set;}
    public int Damage;

    public override int Cost()
    {
        return base.Cost();
    }

    public int setDamage()
    {
        if(Name == "Fists")
        {
            Damage = 1;
            return Damage;
        }else
        {
            return GetDamage();
        }
    }

    public virtual int GetDamage()
    {
        return Damage;
    }

    public virtual int getTier()
    { // based off the name of the weapon the tier will change.
        return 0;
    }
}

public class Sword : Weapon, IMaterial
{
    public  string? Material;
    public int Durabilty {get; set;}

    public Sword(string material, int durability)
    {
        setName();
        IsAcceptedMaterial(material);
        Durabilty = durability;
    }
    public Sword(string material)
    {
        setName();
        IsAcceptedMaterial(material);
        Durabilty = randomDurabilty();
    }

    public override int GetDamage()
    {
        switch(Material)
        {
            case "Wood":
            Damage = 20;
            break;
            case "Iron":
            Damage = 30;
            break;
            case "Steel":
            Damage = 50;
            break;
            case "Carbon Fiber":
            Damage = 65;
            break;
        }
        return Damage;
    }

    public int randomDurabilty()
    {
        Random rand = new Random();
        return rand.Next(0,101);
    }

    public void DescribeMaterial()
    {
        Console.WriteLine(Material);
    }
    public bool IsAcceptedMaterial(string? input)
    {
        switch(input)
        {
            case "Wood":
            case "Iron":
            case "Steel":
            case "Titanium":
            case "Carbon Fiber":
            Material = input;
            return true;
            //very light weight and high protection level so the duribility will go down less
            case null:
            default:
            Console.WriteLine("Sorry we can not make armor out of that material please use a material that we can work with.");
            return false;
        }
    }

    public override int getTier()
    {
        switch(Material)
        {
            case "Wood":
            return 1;
            case "Iron":
            return 2;
            case "Steel":
            return 3;
            case "Titanium":
            return 4;
            case "Carbon Fiber":
            //highest tier because the degregation is the lowest and the protection level is the highest.
            return 5;
            default:
            return 0;
        }
    }
    public override int Cost()
    {
        return base.Cost(); // needs to be set based off the material or tier
    }

    public void setName()
    {
        Name = "Sword";
    }
}

public class Bow : Weapon, IMaterial
{
    public string? Material;
    public int Durabilty{get; set;}

    public Bow(string material)
    {
        if(IsAcceptedMaterial(material) == true)
        {
            Material = material;
        }
    }
    public override int Cost()
    {
        switch(Material)
        {
            case "Wood":
            return 5;
            case "Horn":
            return 15;
            case "Carbon Fiber":
            return 45;
            default:
            return 0;
        }
    }
    public void setName()
    {
        Name = "Bow";
    }

    public void DescribeMaterial()
    {
        Console.WriteLine($"This is a {Material} {Name}");
    }

    public bool IsAcceptedMaterial(string? input)
    {
        switch(input)
        {
            case "Wood":
            case "Horn":
            case "Carbon Fiber":
            Material = input;
            return true;
            case null:
            default:
            return false;
        }
    }

    public int randomDurabilty()
    {
        Random rand = new Random();
        return rand.Next(0,101);
    }

    public override int getTier()
    {
        switch(Material)
        {
            case "Wood":
            return 1;
            case "Horn":
            return 2;
            case "Carbon Fiber":
            return 3;
            default:
            return 0;
        }
    }

    public int DamageMultiplyer() //If you shoot a certain arrow with a bow the multipler will mutiply that damage.
    {
        switch(Material)
        {
            case "Wood":
            return 1;
            case "Horn":
            return 3;
            case "Carbon Fiber":
            return 5;
            default:
            return 0;
        }
    }
}

public class Arrow : Weapon, IFleching , IArrowHead
{
    public string? fleching;
    public string? arrowhead;

    public Arrow(string? fleching, string? arrowhead)
    {
        this.fleching = fleching;
        this.arrowhead = arrowhead;
    }
    public void DescribeFletching()
    {
        Console.WriteLine(fleching);
    }

    public int GetCostFleching()
    {
        //plastic costs 10,turkey costs 5 and goose costs 3
        int cost = 0;
        switch (fleching)
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
        Console.WriteLine(arrowhead);
    }

    public int GetCostArrowHead()
    {
        //steel costs 10, wood costs 3 obsidian costs 5
        int cost = 0;
        switch (arrowhead)
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

    public int Damage()
    {
        switch(getTier())
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
    public override int getTier()
    {
        string? tier = arrowhead  + " " + fleching;
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
}
