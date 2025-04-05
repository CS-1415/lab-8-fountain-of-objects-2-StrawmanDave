using System.Data;
using System.Net.Security;
using System.Reflection.Metadata;
using Lab08;

string description1 = @"
You enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects.
Light is visible only in the entrance, and no other light is seen anywhere in the caverns.
You must navigate the Caverns with your other senses.
Find the Fountain of Objects, activate it, and return to the entrance.
Look out for pits. You will feel a breeze if a pit is in an adjacent room. If you enter a room with a pit, you will take damage and be trapped.
Maelstroms are violent forces of sentient wind. Entering a room with one could transport you to any other location in the caverns.
You will be able to hear thier growling and groaning in nearby rooms.
Amaroks roam the caverns. If you encounter one it will surly attack you, but you can smell their rotten stench in nearby rooms.
";
string description2 = @"
You come acrost a canvern entrance with a little shop right by it.
The shop keeper tells you.
Inside the cavern is the legendery Fountain of Objects. 
It is guarded by many traps and monsters.
";

string help = @"
List of things you can do:
move north(moves you one up while in the cavern)
move south(moves you one down while in the cavern)
move east(moves you one to the left while in the cavern)
move west(moves you one to the right while in the cavern)
enable fountain(enables the fountain only if you are in the fountain room)
inventory(displayes your inventory while outside of the cavern and inside)
shop(enters the shop while you are outside of the cavern)
(while in the shop just type the option)
heal(regain 20 health points while in battle)
attack(deals damage to the monster while in battle)
exam armor(displays the name of your current armor)
exam weapon(diplays the name of your current weapon)
";

Console.Clear();
Console.WriteLine("Welcome to the Fountain of Objects Game!");
Console.WriteLine("Would you like to play a small, medium, or large game?");
string? mazeSize = Console.ReadLine();

Console.WriteLine(description2);

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
    bool displayHelp = false;
    string? input2 = null;
    while(input2 != "")
    {
        Console.Clear();
        Console.WriteLine("You stand outside the cavern what would you like to do? You can enter help to toggle a list of commands or just press enter to continue.");
        if(displayHelp == true)
        {
            Console.WriteLine(help);
        }
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
            case "exam weapon":
            if(chosenMaze.player.currentWeapon == null)
            {
                throw new NullReferenceException();
            }
            Console.WriteLine(chosenMaze.player.currentWeapon.Name);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            break;
            case "exam armor":
            if(chosenMaze.player.currentArmor == null)
            {
                throw new NullReferenceException();
            }
            Console.WriteLine(chosenMaze.player.currentArmor.Name);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            break;
            case "help":
            if(displayHelp == false)
            {
                displayHelp = true;
            }else
            {
                displayHelp = false;
            }
            break;
        }
    }
    Console.WriteLine(description1);

    Console.BackgroundColor = ConsoleColor.Green;
    Console.WriteLine("Press any key to continue");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ReadKey();


    while(win == false)
    {
        if(chosenMaze.Current.sense == "The Fountain of Objects has been reactivated, and you have escaped with your life!") // detect wins
        {
            win = true;
            Console.WriteLine("You Win!");
            continue;
        }

        if(chosenMaze.player.currentHealth <= 0)
        {
            Console.WriteLine("You died");
            break;
        }

        Console.Clear();
        Console.WriteLine($"Coins {chosenMaze.player.Gold}");
        chosenMaze.DisplayMaze();
        Console.WriteLine($"You are in (Row={chosenMaze.y}, Column={chosenMaze.x})");
        Console.WriteLine(chosenMaze.Current.sense);

        if(chosenMaze.Current.item == 'P')
        {
            chosenMaze.player.currentHealth = chosenMaze.player.currentHealth - 5;
            Console.WriteLine("You can either use a rope if you have one or yell for help");
            Console.WriteLine("What do you want to do?");
            string? input = Console.ReadLine();
            switch(input)
            {
                case "use rope":
                // No pits should be allowed to be on the top row because it will always move them north they can't get out if they can't move north.
                for(int i = 0; i<chosenMaze.player.Inventory.Count(); i++)
                {
                    if(chosenMaze.player.Inventory[i].Name == "rope")
                    {
                        chosenMaze.moveNorth();
                        chosenMaze.player.Inventory.Remove(chosenMaze.player.Inventory[i]);
                    }
                }
                break;
                case "yell for help":
                Random rand = new Random();
                int monsterProbabilty = rand.Next(0,101);
                switch(monsterProbabilty)
                {
                    case > 50:
                    startBattle(new Maelstrom(), chosenMaze.player);
                    break;
                    case <50:
                    break;
                }
                break;
            }
            continue;
        }

        if(chosenMaze.Current.item == 'M' || chosenMaze.Current.item == 'A')
        {
            Random rand = new Random();
            // will need to run battle before continueing.
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            int doesAttack = rand.Next(0,101);
            if(doesAttack < 50)
            {
                startBattle(chosenMaze.Current.monster, chosenMaze.player);
                if(chosenMaze.Current.monster.currentHealth < 1)
                {
                    chosenMaze.Current.item = ' ';
                    chosenMaze.resetMaze();
                }
            }else
            {
                chosenMaze.maelstromEffect();
            }
            continue;
        }

        Console.WriteLine("What do you want to do? You can type help to toggle list of commands.");
        if(displayHelp == true)
        {
            Console.WriteLine(help);
        }
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
            case "inventory":
            chosenMaze.player.displayInventory();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            break;
            case "exam weapon":
            Console.WriteLine(chosenMaze.player.currentWeapon.Name);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            break;
            case "exam armor":
            Console.WriteLine(chosenMaze.player.currentArmor.Name);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            break;
            case "help":
            if(displayHelp == false)
            {
                displayHelp = true;
            }else
            {
                displayHelp = false;
            }
            break;
        }
    }
}

void openShop(Player player)
{
    string? input = "";
    while(input != "exit" || player.Gold < 10)
    {
        Console.Clear();
        Console.WriteLine("Oh, so you need help? What type of items would you like to buy here are the options.");
        Console.WriteLine($"You have coins {player.Gold}");
        string options = @"
        general
        weapon
        armor
        exit(if you want to leave the shop)
        ";
        Console.WriteLine(options);
        input = Console.ReadLine();
        if(input == "exit")
        {
            break;
        }
        if(player.canAddItems() == false)
        {
            break;
        }
        
        switch(input)
        {
            case "general":
            Console.WriteLine("Do you want a rope or a health potion.");
            generalItem newItem;
            string? getItem = Console.ReadLine();
            if(getItem == null)
            {
                break;
            }
            if(getItem != "rope" && getItem != "health potion")
            {
                Console.WriteLine("Sorry we don't have that item");
            }
                newItem = new generalItem(getItem);
                player.removeGold(newItem.Cost());
                player.addToInventory(newItem);

            break;
            case "weapon":
            Console.WriteLine("What weapon would you like a sword, bow or do you need an arrow");
            string? getWeapon = Console.ReadLine();
            switch(getWeapon)
            {
                case "sword":
                Console.WriteLine("What material do you like wood, iron, steel, titanium, carbon fiber");
                string? getSword = Console.ReadLine();
                if(getSword == null)
                {
                    break;
                }else
                {
                    if(getSword != "wood" && getSword != "iron" && getSword != "steel" && getSword != "titanium" && getSword != "carbon fiber")
                    {
                        Console.WriteLine("We do not have that material");
                    }else
                    {
                        Sword newSword = new Sword(getSword);
                        player.removeGold(newSword.Cost());
                        player.addToInventory(newSword);
                        player.setCurrentWeapon(newSword); // this is here until the player is always set to have the best weapon in thier invnetory
                    }
                }
                break;
                case "bow": //Not sure yet how to make the bow and arrow work together so when you buy a new bow and you have arrows in your inventory it doesn't set the bow to your current weapon
                Console.WriteLine("What material do you like wood, horn, carbon fiber");
                string? getBow = Console.ReadLine();
                if(getBow == null)
                {
                    break;
                }else
                {
                    if(getBow != "wood" && getBow != "horn" && getBow != "carbon fiber")
                    {
                        Console.WriteLine("We do not have that materail.");
                    }else
                    {
                        Bow newBow = new Bow(getBow);
                        player.removeGold(newBow.Cost());
                        player.addToInventory(newBow);
                        player.setCurrentWeapon(newBow);
                    }
                }
                break;
                case "arrow": 
                Console.WriteLine("What type of fleching do you like goose, turkey, plastic");
                string? getFlecching = Console.ReadLine();
                Console.WriteLine("What arrow head do you like wood,obsidian,steel");
                string? getArrowhead = Console.ReadLine();
                Arrow newArrow = new Arrow(getFlecching, getArrowhead);
                player.addToInventory(newArrow);
                player.setCurrentWeapon(newArrow); // this is hear until i figure out how alwayse have the best weapon in the invenotry equiped
                player.removeGold(newArrow.Cost());
                break;
                case null:
                default:
                break;
            }
            player.setBestWeapon();
            break;
            case "armor":
            Console.WriteLine("What material do you like wood, iron, steel, titanium, carbon fiber");
            string? getArmor = Console.ReadLine();
            if(getArmor == null)
            {
                break;
            }else
            {
                if(getArmor != "wood" && getArmor != "iron" && getArmor !="steel" && getArmor != "titanium" && getArmor != "carbon fiber")
                {
                    Console.WriteLine("We do not have that material");
                }else
                {
                    Armor newArmor = new Armor(getArmor);
                    player.addToInventory(newArmor);
                    player.setCurrentArmor(newArmor); // this is here until I figure out how to alwayse have the best armor equiped.
                    player.removeGold(newArmor.Cost());
                }
            }
            player.setBestArmor();
            break;
            case null:
            default:
            break;
        }
        Console.WriteLine($"You have coins {player.Gold}");
    }

}

void startBattle(Monster monster, Player player)
{
    Random rand = new Random();
    int block;
    while(monster.currentHealth > 0 || player.currentHealth > 0)
    {
        if(monster.currentHealth < 1)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("You won the battle!");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            player.addToInventory(monster.currentArmor);
            player.addToInventory(monster.currentWeapon);
            if(monster.currentArmor.Protection > player.currentArmor.Protection)
            {
                player.currentArmor = monster.currentArmor;
            }
            if(monster.currentWeapon.Damage > player.currentWeapon.Damage)
            {
                player.currentWeapon = monster.currentWeapon;
            }
            int getPotion = rand.Next(0,101);
            int rope = rand.Next(0,101);
            if(getPotion > 50)
            {
                player.Inventory.Add(new generalItem("health potion"));
            }
            if(rope > 25)
            {
                player.Inventory.Add(new generalItem("rope"));
            }
            player.addGold(monster.Gold);

            player.setBestArmor();
            player.setBestWeapon();
            break;
        }

        if(player.currentHealth < 1)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("You lost the battle!");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            break;
        }

        Console.Clear();
        Console.WriteLine($"{monster.monsterName} attacks dealing {monster.currentWeapon.Damage} damage");

        block = rand.Next(0, 51);
        if(player.currentArmor.Protection < block)
        {
            if(monster.currentWeapon.Damage < player.currentArmor.Protection)
            {
                player.currentHealth = player.currentHealth;
                Console.WriteLine("You took no damage");
            }else
            {
                player.currentHealth = player.currentHealth - (monster.currentWeapon.Damage - player.currentArmor.Protection);
                Console.WriteLine($"You took {monster.currentWeapon.Damage - player.currentArmor.Protection} damage");
            }
        }else
        {
            player.currentHealth = player.currentHealth;
            Console.WriteLine("You blocked the monsters attack");
        }
        Console.WriteLine($"Player's health {player.currentHealth}, Monster's health {monster.currentHealth}");
        Console.WriteLine("What do you want to do attack or heal");
        
        string? input = Console.ReadLine();
        switch(input)
        {
            case "attack":
            if(player.currentWeapon == null)
            {
                throw new NullReferenceException();
            }
            
            Console.WriteLine($"You attacked the {monster.monsterName} dealing {player.currentWeapon.Damage} damage");
            block = rand.Next(0,51);
            if(monster.currentArmor.Protection < block)
            {
                if(monster.currentArmor.Protection > player.currentWeapon.Damage)
                {
                    monster.currentHealth = monster.currentHealth;
                    Console.WriteLine("The monster took no damage");
                }else
                {
                    monster.currentHealth = monster.currentHealth - (player.currentWeapon.Damage - monster.currentArmor.Protection);
                    Console.WriteLine($"The monster took {player.currentWeapon.Damage - monster.currentArmor.Protection}");
                }
            }else 
            {
                monster.currentHealth = monster.currentHealth;
                Console.WriteLine("The monster blocked your attack");
            }
            
            Console.WriteLine($"Player's health {player.currentHealth}, Monster's health {monster.currentHealth}");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();

            break;
            case "heal":
            //Iterate through the players inventory if they have health potion use it and then delete it
            for(int i = 0; i <player.Inventory.Count(); i++)
            {
                if(player.Inventory[i].Name == "health potion")
                {
                    player.currentHealth = player.currentHealth + 20;
                    player.Inventory.Remove(player.Inventory[i]);
                    break;
                }
            }
            Console.WriteLine($"Player's health {player.currentHealth}, Monster's health {monster.currentHealth}");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
            break;
        }
        Console.WriteLine($"Player's health {player.currentHealth}, Monster's health {monster.currentHealth}");
    }
}