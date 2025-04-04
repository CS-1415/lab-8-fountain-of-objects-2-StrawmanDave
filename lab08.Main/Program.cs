using System.Collections;
using Lab08;


string description = @"
You enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects.
Light is visible only in the entrance, and no other light is seen anywhere in the caverns.
You must navigate the Caverns with your other senses.
Find the Fountain of Objects, activate it, and return to the entrance.
Look out for pits. You will feel a breeze if a pit is in an adjacent room. If you enter a room with a pit, you will take damage and be trapped.
Maelstroms are violent forces of sentient wind. Entering a room with one could transport you to any other location in the caverns.
You will be able to hear thier growling and groaning in nearby rooms.
Amaroks roam the caverns. If you encounter one it will surly attack you, but you can smell their rotten stench in nearby rooms.
Before you enter the cavern you go to the shop you start off with 500 coins choice what you buy wisely the cavern is unforgiving.
Enter help if you need any help
";

string help = @"
List of things you can do:
move north(moves you one up)
move south(moves you one down)
move east(moves you one to the left)
move west(moves you one to the right)
enable fountain(enables the fountain only if you are in the fountain room)
open inventory opens your inventory and lets you switch your weapon and armor around.
discard weapon will delete the current weapon you are holding forever.
discard armor will delete the current equiped armor forever.
use rope will use the rope in your inventory if you have one.
yell for help has 50/50 chance to help you get out.
";

Console.Clear();
Console.WriteLine("Welcome to the Fountain of Objects Game!");
Console.WriteLine("Would you like to play a small, medium, or large game?");
string? mazeSize = Console.ReadLine();

Console.WriteLine(description);



Console.BackgroundColor = ConsoleColor.Green;
Console.WriteLine("Press any key to continue");
Console.BackgroundColor = ConsoleColor.Black;
Console.ReadKey();

switch(mazeSize)
{
    case "small":
    Console.Clear();
    Maze small = new Maze(4);
    runGame(small);
    break;
    case "medium":
    Console.Clear();
    Maze medium = new Maze(6);
    runGame(medium);
    break;
    case "large":
    Console.Clear();
    Maze large = new Maze(6);
    runGame(large);
    break;
}

void runGame(Maze chosenMaze)
{
    bool win = false;
    string input2 = "";
    while(chosenMaze.player.getGold() > 0 || input2 != "continue" || chosenMaze.player.canAddItems() == false)
    {
        Console.WriteLine("You are at the shop what do you want to do? shop, inventory or continue");
        Console.WriteLine($"Coins {chosenMaze.player.Gold}");
        input2 = Console.ReadLine();
        switch(input2)
        {
            case "shop":
            openShop(chosenMaze.player);
            break;
            case "inventory":
            chosenMaze.player.displayInventory();

            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            break;
            case "continue":
            break;
        }

        if(input2 == "continue")
        {
            break;
        }
        
    }

    while(win == false)
    {
        chosenMaze.DisplayMaze();
        if(chosenMaze.Current.item == 'P')
        {
            chosenMaze.player.currentHealth = chosenMaze.player.currentHealth - 5;
            Console.WriteLine("You can either use a rope if you have one or yell for help");
            Console.WriteLine("What do you want to do?");
            string? input = Console.ReadLine();
            switch(input)
            {
                case "use rope":
                chosenMaze.moveNorth();// No pits should be allowed to be on the top row because it will always move them north they can't get out if they can't move north.
                break;
                case "yell for help":
                Random rand = new Random();
                int monsterProbabilty = rand.Next(0,101);
                switch(monsterProbabilty)
                {
                    case > 50:
                    //start random battle with a newly created random Maelstrom
                    break;
                    case <50:
                    break;
                }
                break;
            }
            continue;
        }

        Console.WriteLine($"Coins{chosenMaze.player.Gold}");
        Console.WriteLine($"You are in (Row={chosenMaze.y}, Column={chosenMaze.x})");
        Console.WriteLine(chosenMaze.Current.sense);
        
        if(chosenMaze.Current.sense == "The Fountain of Objects has been reactivated, and you have escaped with your life!") // detect wins
        {
            win = true;
            Console.WriteLine("You Win!");
            continue;
        }

        if(chosenMaze.Current.item == 'M' || chosenMaze.Current.item == 'A')
        {
            // will need to run battle before continueing.
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            startBattle(chosenMaze.Current.monster, chosenMaze.player);
            if(chosenMaze.Current.monster.currentHealth < 1)
            {
                chosenMaze.Current.item = ' ';
                chosenMaze.resetMaze();
            }
            continue;
        }
        if(chosenMaze.player.currentHealth <= 0)
        {
            Console.WriteLine("You died");
            break;
        }

        Console.Write("What do you want to do?");

        string? command = Console.ReadLine();
        switch(command)
        {
            case "move north":
            chosenMaze.moveNorth();
            break;
            case "move south":
            chosenMaze.moveSouth();
            break;
            case "move east":
            chosenMaze.moveEast();
            break;
            case "move west":
            chosenMaze.moveWest();
            break;
            case "enable fountain":
                if(chosenMaze.Current.item == 'F')
                {
                    chosenMaze.Current.sense = "You hear the rushing waters from the Fountain of Objects. It has been reactivated!";
                    chosenMaze.rowColumn[0,0].sense = "The Fountain of Objects has been reactivated, and you have escaped with your life!";
                }
            break;
            case "help":
            Console.WriteLine(help);
            break;
        }
    }
}

void openShop(Player player)
{
    Console.WriteLine("What type of items whould you like to buy");
    string options = @"
    General
    Weapon
    Armor
    ";
    Console.WriteLine(options);


    string? input = Console.ReadLine();
    switch(input)
    {
        case "General":
        Console.WriteLine("Do you want a rope or a healing potion");
        string? getItem = Console.ReadLine();
        switch(getItem)
        {
            case "rope":
            player.addToInventory(new generalItem("rope"));
            player.removeGold(10);
            break;
            case "healing potion":
            player.addToInventory(new generalItem("healing potion"));
            player.removeGold(10);
            break;
        }
        break;
        case "Weapon":
        Console.WriteLine("What weapon would you like a Sword, Bow or do you need an Arrow");
        string? getWeapon = Console.ReadLine();
        switch(getWeapon)
        {
            case "Sword":
            Console.WriteLine("What material do you like Wood, Iron, Titanium, Carbon Fiber");
            string? getSword = Console.ReadLine();
            Sword newSword = new Sword(getSword);
            player.addToInventory(newSword);
            player.currentWeapon = newSword;
            player.removeGold(newSword.Cost());
            break;
            case "Bow": //Not sure yet how to make the bow and arrow work together so when you buy a new bow and you have arrows in your inventory it doesn't set the bow to your current weapon
            Console.WriteLine("What material do you like Wood, Horn, Carbon Fiber");
            string? getBow = Console.ReadLine();
            Bow newBow = new Bow(getBow);
            player.addToInventory(newBow);
            player.removeGold(newBow.Cost());
            break;
            case "Arrow": 
            Console.WriteLine("What tier of Arrow do like 1-9");
            string? getFlecching = Console.ReadLine();
            Console.WriteLine("What arrow head do you like wood,obsidian,steel");
            string? getArrowhead = Console.ReadLine();
            Arrow newArrow = new Arrow(getFlecching, getArrowhead);
            player.addToInventory(newArrow);
            player.removeGold(newArrow.Cost());
            break;
        }
        break;
        case "Armor":
        Console.WriteLine("What material do you like Wood, Iron, Titanium, Carbon Fiber");
        string getArmor = Console.ReadLine();
        Armor newArmor = new Armor(getArmor);
        player.addToInventory(newArmor);
        player.currentArmor = newArmor;
        player.removeGold(newArmor.Cost());
        break;
        default:
        break;
    }
}

void startBattle(Monster monster, Player player)
{
    while(monster.currentHealth > 0 || player.currentHealth > 0)
    {
        if(monster.currentHealth < 1)
        {
            Console.WriteLine("You won the battle");
            player.Inventory.AddRange(monster.Inventory);
            player.addGold(monster.Gold);
            break;
        }

        if(player.currentHealth < 1)
        {
            Console.WriteLine("You lost the battle!");
            break;
        }

        Console.Clear();
        Console.WriteLine($"Player's health {player.currentHealth}, Monster's health {monster.currentHealth}");
        Console.WriteLine($"{monster.monsterName} attacks dealing {monster.currentWeapon.GetDamage()} damage");
        if(player.currentArmor.Protection > monster.currentWeapon.Damage)
        {
            player.currentHealth = player.currentHealth;
        }else
        {
            player.currentHealth = player.currentHealth - (monster.currentWeapon.GetDamage() - player.currentArmor.Protection);
        }
        

        Console.WriteLine("What do you want to do Attack or Heal");
        string? input = Console.ReadLine();
        switch(input)
        {
            case "Attack":
            monster.currentHealth = monster.currentHealth - (player.currentWeapon.GetDamage() - monster.currentArmor.Protection);
            Console.WriteLine($"You attacked the {monster.monsterName} dealing {player.currentWeapon.GetDamage()} damage");

            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();

            break;
            case "Heal":
            if(player.Inventory.Contains(new generalItem("Health Potion"))) // I am sure this doesn't ever really work so sorry if you tried to heal and got no health
            {
                player.currentHealth = player.currentHealth + 30;
                player.Inventory.Remove(new generalItem("Health Potion")); // I am sure this doesn't work because I am not sure how to let the compiler know what is equal to new item.
            }
            break;
        }


    }
}