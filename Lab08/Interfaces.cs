namespace Lab08;

public interface IMonster // The idea is to add mosnters to the room
{
    //All monsters have an Aroma that spreads to nearby rooms this way the player can sense them.
    string displayMonster();
}

public interface IHealth // The idea is to be used by any monster or the player
{
    int MaxHealth{get; set;}
    int currentHealth{get; set;}

    void setMaxHealth();
    void setCurrentHealth();
}

public interface IInventory // The idea is to have an inventory of the monsters and the player
{
    List<GameObject> Inventory{get; set;}
    int maxItems {get; set;}
    void setMaxItems();
    void addToInventory(GameObject gameObject);
    bool canAddItems();
}

public interface IRoom
{
    string sense{get; set;}

    char item{get; set;}

    object occupied{get; set;}
}


