using System.Timers;

class GameOfLife
{
    // Variables
    static string dead = "M"; // dead cell
    static string living = "W"; // life Cell

    static int width = 60;
    static int height = 30;
    static int neighbourGap = 1;
    static ConsoleColor originalForegroundColor = Console.ForegroundColor;

    static bool add = true;
    static bool pause = false;
    static bool timerUpdate = false;

    static List<int[]> lifeCells = new List<int[]>();

    static string input;

    static string[,] cellStatus = new string[height, width];
    static string[,] cellOutput = new string[height, width];

    static System.Timers.Timer timer;

    static void Main(string[] args)
    {
        Console.Write("Welcome to the Game of Life (... crafted by Julius Grzyl)");

        setCells();

        InitializeGameOfLife();

        timer = new System.Timers.Timer(1);

        timer.Elapsed += Timer_Elapsed;
        timer.AutoReset = true;
        timer.Enabled = true;

        while (true)
        {
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.P)
            {
                pause = true;
            }
            else if (key == ConsoleKey.Enter)
            {
                pause = false;
            }
        }
    }

    private static void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (!pause)
        {
            timer.Enabled = false;
            UpdateGameOfLife();
            timer.Enabled = true;
        }
    }

    static void setCells()
    {
        bool wrongInput = false;

        while (add)
        {
            if (!wrongInput)
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Wrong Input!");
                wrongInput = false;
            }

            Console.WriteLine();

            bool alive = true;

            InitializeGameOfLife();

            Console.WriteLine("\nType cell coordinates (form: up -> down, left -> right)");
            try
            {
                input = Console.ReadLine();
                string[] inputArray = input.Split(',');

                int y = Convert.ToInt32(inputArray[0]);
                int x = Convert.ToInt32(inputArray[1]);

                Console.WriteLine(y);
                Console.WriteLine(x);

                try
                {
                    foreach (int[] cell in lifeCells)
                    {
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                if (i == cell[0] && j == cell[1] && i == y && j == x)
                                {
                                    lifeCells.Remove(cell);
                                    alive = false;
                                }
                            }
                        }
                    }
                }
                catch { }

                if (alive)
                {
                    lifeCells.Add([y, x]);
                }
            }
            catch
            {
                if (input.ToLower() == "start")
                {
                    add = false;
                }
                else if (input.ToLower() == "glidergun")
                {
                    GliderGun();
                    InitializeGameOfLife();
                }
                else if (input.ToLower() == "glider")
                {
                    Glider();
                    InitializeGameOfLife();
                }
                else if (input.ToLower() == "clear")
                {
                    lifeCells.Clear();
                }
                else if (input.ToLower() == "exit")
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    wrongInput = true;
                }
            }

            Console.Clear();
        }
    }

    static void RemoveInitializingCell()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                foreach (int[] cell in lifeCells)
                {
                    if (i == cell[0] && j == cell[1])
                    {
                        lifeCells.Remove(cell);
                    }
                }
            }
        }
    }

    static void GliderGun()
    {
        lifeCells.Add([10, 0]);
        lifeCells.Add([11, 0]);
        lifeCells.Add([10, 1]);
        lifeCells.Add([11, 1]);
        lifeCells.Add([10, 10]);
        lifeCells.Add([11, 10]);
        lifeCells.Add([12, 10]);
        lifeCells.Add([9, 11]);
        lifeCells.Add([13, 11]);
        lifeCells.Add([8, 12]);
        lifeCells.Add([14, 12]);
        lifeCells.Add([8, 13]);
        lifeCells.Add([14, 13]);
        lifeCells.Add([11, 14]);
        lifeCells.Add([9, 15]);
        lifeCells.Add([13, 15]);
        lifeCells.Add([13, 15]);
        lifeCells.Add([10, 16]);
        lifeCells.Add([11, 16]);
        lifeCells.Add([12, 16]);
        lifeCells.Add([12, 16]);
        lifeCells.Add([11, 17]);
        lifeCells.Add([10, 20]);
        lifeCells.Add([8, 20]);
        lifeCells.Add([9, 20]);
        lifeCells.Add([10, 20]);
        lifeCells.Add([8, 21]);
        lifeCells.Add([9, 21]);
        lifeCells.Add([10, 21]);
        lifeCells.Add([7, 22]);
        lifeCells.Add([11, 22]);
        lifeCells.Add([6, 24]);
        lifeCells.Add([7, 24]);
        lifeCells.Add([11, 24]);
        lifeCells.Add([12, 24]);
        lifeCells.Add([8, 34]);
        lifeCells.Add([9, 34]);
        lifeCells.Add([8, 35]);
        lifeCells.Add([9, 35]);
    }

    static void Glider()
    {
        lifeCells.Add([0, 1]);
        lifeCells.Add([1, 2]);
        lifeCells.Add([2, 0]);
        lifeCells.Add([2, 1]);
        lifeCells.Add([2, 2]);
    }

    static void InitializeGameOfLife()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                bool alive = false;

                foreach (int[] cell in lifeCells)
                {
                    if (i == cell[0] && j == cell[1])
                    {
                        alive = true;
                    }
                }

                if (alive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    cellStatus[i, j] = living;
                }
                else if (!alive)
                {
                    cellStatus[i, j] = dead;
                }

                Console.Write(cellStatus[i, j]);


                Console.ForegroundColor = originalForegroundColor;
            }

            Console.WriteLine();
        }
    }

    static void UpdateGameOfLife()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int livingNeighbours = 0;

                for (int k = -neighbourGap; k <= neighbourGap; k++)
                {
                    for (int l = -neighbourGap; l <= neighbourGap; l++)
                    {
                        int neighbourH = i + k;
                        int neighbourW = j + l;
                        if (neighbourH < 0 || neighbourH >= height)
                        {
                            neighbourH = Math.Abs(height - Math.Abs(neighbourH));
                        }
                        if (neighbourW < 0 || neighbourW >= width)
                        {
                            neighbourW = Math.Abs(width - Math.Abs(neighbourW));
                        }

                        if (!(k == 0 && l == 0))
                        {
                            if (cellStatus[neighbourH, neighbourW] == living)
                            {
                                livingNeighbours++;
                            }
                        }
                    }
                }

                if (cellStatus[i, j] == living && (livingNeighbours == 2 || livingNeighbours == 3))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    cellOutput[i, j] = living;
                }
                else if (cellStatus[i, j] == dead && livingNeighbours == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    cellOutput[i, j] = living;
                }
                else if (cellStatus[i, j] == living && (livingNeighbours < 2 || livingNeighbours > 3))
                {
                    cellOutput[i, j] = dead;
                }
                else
                {
                    cellOutput[i, j] = dead;
                }

                if (cellOutput[i, j] != cellStatus[i, j])
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(cellOutput[i, j]);
                }

                Console.ForegroundColor = originalForegroundColor;
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cellStatus[i, j] = cellOutput[i, j];
            }
        }
    }
}