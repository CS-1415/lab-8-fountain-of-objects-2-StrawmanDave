namespace Lab08;

public class Maze
{
    //4by4 grid of rooms room (Row=0, Column=0) is the start and the end.
    //A maze is has rows and Columns
    public Room[,] rowColumn = new Room[,]{};
    //The Current rooms is where the player is.
    public Room Current;
    public Player player = new Player();
    public int x = 0; //Split for moveing up and down columns
    public int y = 0;//Split for moveing up and down Rows
    public Maze()
    {
        rowColumn = new Room[4,4];
        setArray();
        setStart();
        setFountainLocation();
        setMonsters();
        setPits();
        setAromas();
        Current = rowColumn[y,x];
    }
    public Maze(int size)
    {
        rowColumn = new Room[size,size];
        setArray();
        setStart();
        setFountainLocation();
        setMonsters();
        setPits();
        setAromas();
        Current = rowColumn[y,x];
    }
    public Maze(int row, int column)
    {
        rowColumn = new Room[row,column];
        setArray();
        setStart();
        setFountainLocation();
        setMonsters();
        setPits();
        setAromas();
        Current = rowColumn[y,x];
    }

    public void setArray()
    {
        for(int row = 0; row< rowColumn.GetLength(0); row++)
        {
            for(int column = 0; column<rowColumn.GetLength(1); column++)
            {
                rowColumn[row,column] =  new Room();
            }
        }
    }
    public void setStart()
    {
        rowColumn[0,0].sense = "You see light coming from the cavern entrance.";
        rowColumn[0,0].item = 'D';
        rowColumn[0,0].occupied = new object();
    }
    public void setFountainLocation()
    {
        Random rand = new Random();
        int randomRow = 0;
        int randomColumn = 0;
        while(randomRow == 0 && randomColumn == 0)
        {
            randomRow = rand.Next(0,rowColumn.GetLength(0));
            randomColumn = rand.Next(0,rowColumn.GetLength(1));
        }
        rowColumn[randomRow,randomColumn] = new FountainRoom();
    }
    public void setMonsters()
    {
        Random rand = new Random();
        for(int i = 0; i<rowColumn.GetLength(0); i ++)
        {
            for(int j = 0; j<rowColumn.GetLength(1); j++)
            {
                int isMonster = rand.Next(0,100);
                if(isMonster > 75)
                {
                    if(canBePlaced(i,j) == true)
                    {
                        rowColumn[i,j] = new MonsterRoom();
                    }
                }
            }
        }
    }
    public void setPits()
    {
        Random rand = new Random();
        for(int i = 1; i<rowColumn.GetLength(0); i++) // the row is set to one so there will never be pit on row 1 this makes you can alwayse get out if you have a rope
        {
            for(int j = 0; j<rowColumn.GetLength(1); j++)
            {
                int isPit = rand.Next(0,100);

                if(isPit > 90)
                {
                    if(canBePlaced(i,j) == true)
                    {
                        rowColumn[i,j] = new PitRoom();
                    }
                }
            }
        }
    }
    public void setAromas()
    {
        for(int i = 0; i<rowColumn.GetLength(0); i++)
        {
            for(int j = 0; j<rowColumn.GetLength(1); j++)
            {
                if(rowColumn[i,j].item == 'M')
                {
                    Maelstrom newMonster = new Maelstrom();
                    if(newMonster.Aroma() == null)
                    {
                        throw new NullReferenceException();
                    }else
                    {
                        setSurroundings(i,j, newMonster.Aroma());
                    }
                }
                
                if(rowColumn[i,j].item == 'A')
                {
                    Amarok newMonster = new Amarok();
                    if(newMonster.Aroma() == null)
                    {
                        throw new NullReferenceException();
                    }
                    setSurroundings(i,j, newMonster.Aroma());
                }

                if(rowColumn[i,j].item == 'P')
                {
                    PitRoom newRoom = new PitRoom();
                    setSurroundings(i,j, PitRoom.Aroma);
                }
            }
        }
    }
    public void resetMaze()
    {
        for(int i = 0; i <rowColumn.GetLength(0); i++) // idea is to go through each item in the maze and set the sense of each room to " "
        {
            for(int j = 0; j <rowColumn.GetLength(1); j++)
            {
                if(canMove(i,j) == true)
                {
                    rowColumn[i,j].sense = " ";
                }
            }
        }

        for(int row = 0; row<rowColumn.GetLength(0); row ++)
        {
            for(int column = 0; column<rowColumn.GetLength(1); column++)
            {
                if(rowColumn[row,column].item == 'F')
                {
                    rowColumn[row,column].sense = "You hear water dripping in this room. The Fountain of Objects is here!";
                }else if(rowColumn[row,column].item == 'P')
                {
                    rowColumn[row, column].sense = new PitRoom().sense;
                    setSurroundings(row,column,"You feel a draft. There is a pit in a nearby room");
                }else if(rowColumn[row,column].item == 'M')
                {
                    rowColumn[row, column].sense = new MonsterRoom(new Maelstrom()).sense;
                    setSurroundings(row,column, "You hear the growling and groaning of a maelstrom nearby.");
                }else if(rowColumn[row,column].item == 'A')
                {
                    rowColumn[row, column].sense = new MonsterRoom(new Amarok()).sense;
                    setSurroundings(row,column,"You can smell the rotten stench of an amarok in a nearby room");
                }else if(rowColumn[row,column].item == 'D')
                {
                    rowColumn[row,column].sense = "You see light coming from the cavern entrance.";
                }
            }
        }
    }

    public Room maelstrom()
    {
        Room Maelstrom = new MonsterRoom();
        return Maelstrom;
    }

    public void maelstromEffect()
    {
        if(Current.item == 'M')
        {
            clearSurroundings(y,x);
            rowColumn[y,x].item = ' ';

            if(canMove(y+1, x-2) == true && canBePlaced(y+1, x-2))
            {
                rowColumn[y+1, x-2] = maelstrom();
                //setSurroundings(y+1,x-2, "You hear the growling and groaning of a malestrom nearby.");
            }else if(canMove(y+1,x -1) == true && canBePlaced(y+1, x-1) == true)
            {
                rowColumn[y+1, x-1] = maelstrom();
                //setSurroundings(y+1,x-1, "You hear the growling and groaning of a malestrom nearby.");
            }else if(canMove(y+1, x) == true && canBePlaced(y+1, x) == true)
            {
                rowColumn[y+1, x] = maelstrom();
                //setSurroundings(y+1,x, "You hear the growling and groaning of a malestrom nearby.");
            }else if(canMove(y, x-2) == true && canBePlaced(y+1, x-2) == true)
            {
                rowColumn[y, x-2] = maelstrom();
                //setSurroundings(y,x-2, "You hear the growling and groaning of a malestrom nearby.");
            }else if(canMove(y,x-1) == true && canBePlaced(y, x-2) == true)
            {
                rowColumn[y, x-1] = maelstrom();
                //setSurroundings(y,x-1, "You hear the growling and groaning of a malestrom nearby.");
            }else
            {

            }
            resetMaze();

            moveNorth();
            moveEast();
            moveEast();
        }
    }
    public void clearSurroundings(int row, int column)
    {
        if(canMove(row -1, column) == true) // one south
        {
            rowColumn[row -1, column].sense = " ";
        }

        if(canMove(row +1, column) == true) // one North
        {
            rowColumn[row +1, column].sense = " ";
        }
    
        if(canMove(row, column +1) == true) // one east
        {
            rowColumn[row, column +1].sense = " ";
        }

        if(canMove(row, column -1) == true) // one west
        {
            rowColumn[row, column -1].sense = " ";
        }

        if(canMove(row -1, column -1) == true) // one south west
        {
            rowColumn[row -1, column -1].sense = " ";
        }

        if(canMove(row -1, column +1) == true) // one south east of the pit
        {
            rowColumn[row -1, column +1].sense = " ";
        }

        if(canMove(row +1, column -1) == true) // one north west of the pit
        {
            rowColumn[row +1, column -1].sense = " ";
        }

        if(canMove(row +1, column +1) == true) // one north east of the pit
        {
            rowColumn[row +1, column +1].sense = " ";
        }
    }
    public void setSurroundings(int row, int column, string? words)
    {
        
        if(canMove(row -1, column) == true) // one south
        {
            if(rowColumn[row -1, column].sense == " ")
            {
                rowColumn[row -1, column].sense = words;
            }else
            {
                rowColumn[row -1, column].sense = rowColumn[row -1, column].sense + "\n" + words;
            }
        }

        if(canMove(row +1, column) == true) // one North
        {
            if(rowColumn[row +1, column].sense == " ")
            {
                rowColumn[row +1, column].sense = words;
            }else
            {
                rowColumn[row +1, column].sense = rowColumn[row +1, column].sense + "\n" + words;
            }
        }
    
        if(canMove(row, column +1) == true) // one east
        {
            if(rowColumn[row, column +1].sense == " ")
            {
                rowColumn[row, column +1].sense = words;
            }else
            {
                rowColumn[row, column +1].sense = rowColumn[row, column +1].sense + "\n" + words;
            }
        }

        if(canMove(row, column -1) == true) // one west
        {
            if(rowColumn[row, column -1].sense == " ")
            {
                rowColumn[row, column -1].sense = words;
            }else
            {
                rowColumn[row, column -1].sense = rowColumn[row, column -1].sense  + "\n" + words;
            }
        }

        if(canMove(row -1, column -1) == true) // one south west
        {
            if(rowColumn[row -1, column -1].sense == " ")
            {
                rowColumn[row -1, column -1].sense = words;
            }else
            {
                rowColumn[row -1, column -1].sense = rowColumn[row -1, column -1].sense + "\n" + words;
            }
        }

        if(canMove(row -1, column +1) == true) // one south east
        {
            if(rowColumn[row -1, column +1].sense == " ")
            {
                rowColumn[row -1, column +1].sense = words;
            }else
            {
                rowColumn[row -1, column +1].sense = rowColumn[row -1, column +1].sense + "\n" + words;
            }
        }

        if(canMove(row +1, column -1) == true) // one north west 
        {
            if(rowColumn[row +1, column -1].sense == " ")
            {
                rowColumn[row +1, column -1].sense = words;
            }else
            {
                rowColumn[row +1, column -1].sense = rowColumn[row +1, column -1].sense + "\n" + words;
            }
        }

        if(canMove(row +1, column +1) == true) // one north east 
        {
            if(rowColumn[row +1, column +1].sense == " ")
            {
                rowColumn[row +1, column +1].sense = words;
            }else
            {
                rowColumn[row +1, column +1].sense = rowColumn[row +1, column +1].sense + "\n" + words;
            }
        }
    }
    public void removeAllRoomTypes(char contains)
    {
        for(int row = 0; row< rowColumn.GetLength(0); row++)
        {
            for(int column = 0; column<rowColumn.GetLength(1); column++)
            {
                if(rowColumn[row,column].item == contains)
                {
                    rowColumn[row,column] = new Room();
                }
            }
        }
    }
    public bool canBePlaced(int row, int column)
    {
        if(rowColumn[row,column].item == 'F')
        {
            return false;
        }else if(rowColumn[row,column].item == 'D')
        {
            return false;
        }else if(rowColumn[row,column].item == 'P')
        {
            return false;
        }else if(rowColumn[row,column].item == 'M')
        {
            return false;
        }else if(rowColumn[row,column].item == 'A')
        {
            return false;
        }else
        {
            return true;
        }
    }
    public bool canMove(int row, int column)
    {
        if(row >= rowColumn.GetLength(0) || row < 0)
        {
            return false;
        }

        if(column >= rowColumn.GetLength(1) || column < 0)
        {
            return false;
        }
        return true;
    }
    public void DisplayMaze()
    {
        for(int row = 0; row< rowColumn.GetLength(0); row++)
        {
            for(int column = 0; column<rowColumn.GetLength(1); column++)
            {
                Console.Write(rowColumn[row,column].item);
            }
            Console.WriteLine();
        }
    }
    public void moveNorth()
    {
        if(canMove(y-1, x) == true) //current.Row current.column
        {
            y--;
            Current = rowColumn[y,x];
        }
    }
    public void moveSouth()
    {
        if(canMove(y+1, x) == true) //current.Row current.column
        {
            y++;
            Current = rowColumn[y,x];
        }        
    }
    public void moveWest()
    {
        if(canMove(y, x-1) == true) //current.Row current.column
        {
            x--;
            Current = rowColumn[y,x];
        }
    }
    public void moveEast()
    {
        if(canMove(y, x+1) == true) //current.Row current.column
        {
            x++;
            Current = rowColumn[y,x];
        }
    }
}