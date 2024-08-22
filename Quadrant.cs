namespace SudokuGen;


class CoordSystem{
    public int x {get; set;} = 0;

    public int y { get; set;} = 0;

    public int value {get; set;} = 0;

    public bool isFull {get; set;} = false;

    public CoordSystem(int x, int y){
        this.x = x;

        this.y = y;
    }
    public CoordSystem(int x, int y, int value){
        this.x = x;

        this.y = y;

        this.value = value;
    }

    public static bool operator ==(CoordSystem coord1, CoordSystem coord2)
    {
        if(coord1.x == coord2.x && coord1.value == coord2.value)
        {
            return true;
        }
        if(coord1.y == coord2.y && coord1.value == coord2.value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(CoordSystem coord1, CoordSystem coord2)
    {
        if(coord1.x == coord2.x && coord1.value == coord2.value)
        {
            return false;
        }
        if(coord1.y == coord2.y && coord1.value == coord2.value)
        {
            return false;
        }
        else
        {
            return true;
        }

    }


   public CoordSystem(){}
}

class Quad
{
    public int[] ints  {get; set;} = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9};

    public CoordSystem[] coords {get; set;} = new CoordSystem[9];

    public bool IsCompleted {get; set;} = false;

    Random random = new Random();

    public static int resetCounter {get; set;} = 0;


    //Methods

    private int SelectRandomInt()
    {
        int randInt = random.Next(0,9);

        int number = ints[randInt];

        if (number == -1)
        {
            while(number == -1)
            {
                randInt = random.Next(0,9);
                number = ints[randInt];
            }
        }
        
        ints[randInt] = -1;

        return number;
    }

// Assigns the quadrants its Coordonates.
// The coordonates of each quadrant are stored in the coords array
    private void AssignCoords(CoordSystem upperCorner)
    {
        int counter = 0;
        for (int i = upperCorner.y; i < upperCorner.y + 3; i++)
        {
            for (int j = upperCorner.x; j < upperCorner.x + 3; j++)
            {
                coords[counter] = new CoordSystem(j, i);
                counter++;
            }
        }
    }

    private void PrintCoords()
    {
        foreach(var coord in coords)
        {
            Console.WriteLine($"{coord.x} | {coord.y}");
        }
    }

    private bool AreIntsDepleted()
    {
        foreach(var number in ints)
        {
            if(number != -1)
            {
                return false;
            }
        }
        return true;
    }

    public void PrintQuad()
    {
        int counter = 0;
        foreach (var coord in coords)
        {
            if (counter % 3 == 0)
            {
                Console.WriteLine("");
                Console.Write($"{coord.y} | {coord.x} | [{coord.value}]       ");
                counter++;
            }
            else
            {
                Console.Write($"{coord.y} | {coord.x} | [{coord.value}]       ");
                counter++;
            }
            
        }
    }    
    private bool VerifyCoord(CoordSystem coord)
    {
        foreach (var invCoords in InvalidGlobalCoords.invCoords)
        {
            if(coord == invCoords)
            {
                return false;
            }
        }
        return true;
    }

// Used to reset the object if the generation needs to restart
    public void ResetQuad()
    {
        for (int i = 0; i < 9; i++)
        {
            ints[i] = i+1;
        }

        foreach (var coord in coords)
        {
            coord.isFull = false;
            coord.value = 0;
        }
    }

    private void CompleteInvalidList()
    {
        foreach (var coord in coords)
        {
            InvalidGlobalCoords.invCoords.Add(new CoordSystem(coord.x, coord.y, coord.value));            
        }
    }

    //Constructor
    public Quad(CoordSystem upperCorner)
    {
        int counter = 0;
        AssignCoords(upperCorner);
        while(!AreIntsDepleted() && resetCounter < 400000)
        {
            int number = SelectRandomInt();

            coords[counter].value = number;

            if(!VerifyCoord(coords[counter]))
            {
                ResetQuad();
                counter = 0;
                resetCounter++;
            }
            else
            {
                coords[counter].isFull = true;
                counter++;
            }

        }
        if (resetCounter < 400000)
        {
            IsCompleted = true;
            CompleteInvalidList();
        }

    }

}

class InvalidGlobalCoords
{

    public static List<CoordSystem> invCoords {get; set;} = new List<CoordSystem>();

}

class SudokuGen
{
    public Quad[] quads {get; set;}= new Quad[9];

    public SudokuGen()
    {
        int counter = 0;
        GenerateQuads();
        while (!AreQuadsCompleted())
        {
            if(counter == 0)
            {
                ResetSudoku();
            }

            GenerateQuads();

            if(Quad.resetCounter >= 400000)
            {
                ResetSudoku();
                Quad.resetCounter = 0;
            }
        }
    }

    private void GenerateQuads()
    {
        //Upper Corners
        CoordSystem origin = new CoordSystem(0,0);
        CoordSystem pt1 = new CoordSystem(3, 0);
        CoordSystem pt2 = new CoordSystem(6, 0);
        CoordSystem pt3 = new CoordSystem(0, 3);
        CoordSystem pt4 = new CoordSystem(3, 3);
        CoordSystem pt5 = new CoordSystem(6, 3);
        CoordSystem pt6 = new CoordSystem(0, 6);
        CoordSystem pt7 = new CoordSystem(3, 6);
        CoordSystem pt8 = new CoordSystem(6, 6);

        CoordSystem[] points = [origin, pt1, pt2, pt3, pt4, pt5, pt6, pt7, pt8];        

        for (int i = 0; i < 9; i++)
        {
            quads[i] = new Quad(points[i]);
        }
    }

    private void ResetSudoku()
    {
        foreach (var quad in quads)
        {
            quad.ResetQuad();
        }
        InvalidGlobalCoords.invCoords.Clear();
    }

    private bool AreQuadsCompleted()
    {
        foreach (var quad in quads)
        {
            if (!quad.IsCompleted)
            {
                return false;
            }
        }

        return true;
    }

    public void PrintSudoku()
    {
        int yCounter = 0;
        int xCounter = 0;
        while (yCounter != 9)
        {
            foreach (var quad in quads)
            {
                foreach (var coord in quad.coords)
                {
                    if(coord.y == yCounter)
                    {
                        Console.Write($" {coord.value} ");
                        xCounter++;
                    }
                }
                if (xCounter == 9)
                {
                    xCounter = 0;
                    yCounter++;
                    Console.WriteLine("");
                }
            } 
        }
        

    }

}
