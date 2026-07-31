internal static class StartScreen
{
    private const int MinDiskCount = 3;
    private const int MaxDiskCount = 9;
    private const int DefaultDiskCount = 4;

    public static int Run()
    {
        var diskCount = DefaultDiskCount;

        while (true)
        {
            Render(diskCount);

            var key = System.Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    diskCount = Math.Max(MinDiskCount, diskCount - 1);
                    break;
                case ConsoleKey.RightArrow:
                    diskCount = Math.Min(MaxDiskCount, diskCount + 1);
                    break;
                case ConsoleKey.Enter:
                    return diskCount;
            }
        }
    }

    private static void Render(int diskCount)
    {
        System.Console.Clear();
        System.Console.WriteLine("=== Towers of Hanoi ===");
        System.Console.WriteLine();
        System.Console.WriteLine("Rules:");
        System.Console.WriteLine("  Move all disks from the leftmost tower to the rightmost tower.");
        System.Console.WriteLine("  You may only move one disk at a time, taking the top disk from a tower.");
        System.Console.WriteLine("  A disk may never be placed on top of a smaller disk.");
        System.Console.WriteLine();
        System.Console.WriteLine("Controls:");
        System.Console.WriteLine("  Left/Right : move the cursor between towers, or hover a lifted disk between towers");
        System.Console.WriteLine("  Space      : pick up the selected tower's top disk, or drop a lifted disk");
        System.Console.WriteLine("  Esc        : quit the current game early");
        System.Console.WriteLine();
        System.Console.WriteLine($"Number of disks: use Left/Right to adjust ({MinDiskCount}-{MaxDiskCount}), Enter to start.");
        System.Console.WriteLine($"  > {diskCount}");
    }
}
