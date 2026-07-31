using TowersOfHanoi.Core;

internal static class EndScreen
{
    public static void Run(Game game)
    {
        System.Console.Clear();
        System.Console.WriteLine("=== Towers of Hanoi ===");
        System.Console.WriteLine();
        System.Console.WriteLine(game.Status == GameStatus.Completed ? "You solved it!" : "Game quit early.");
        System.Console.WriteLine();
        System.Console.WriteLine($"Disks used    : {game.DiskCount}");
        System.Console.WriteLine($"Moves made    : {game.MoveCount}");
        System.Console.WriteLine($"Optimal moves : {game.OptimalMoveCount}");
        System.Console.WriteLine($"Elapsed time  : {game.ElapsedTime:hh\\:mm\\:ss}");
        System.Console.WriteLine($"Status        : {(game.Status == GameStatus.Completed ? "Completed" : "Quit early")}");
        System.Console.WriteLine();
        System.Console.WriteLine("Press any key to exit.");
        System.Console.ReadKey(intercept: true);
    }
}
