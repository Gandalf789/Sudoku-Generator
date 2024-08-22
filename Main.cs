namespace SudokuGen{

using System.Diagnostics;

class MainApp{

    public static void Main(string[] args)
    {
        System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Console.WriteLine("|-------------------------|\n");

        SudokuGen sudoku = new SudokuGen();

        sudoku.PrintSudoku();

        stopwatch.Stop();

        Console.WriteLine($"\n--------Elapsed Time--------");
        Console.WriteLine($"-----[{stopwatch.Elapsed}]-----");
        
    }
}
}
