using TowersOfHanoi.Core;

internal static class GameplayScreen
{
    private const char PoleChar = '|';
    private const char DiskChar = '#';
    private const char BaseChar = '=';
    private const int ColumnGap = 2;

    public static void Run(Game game)
    {
        InputState state = new Browsing(0);
        string? statusMessage = null;

        Render(game, state, statusMessage);

        while (game.Status == GameStatus.InProgress)
        {
            var key = System.Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.Escape)
            {
                game.Quit();
                Render(game, state, statusMessage);
                break;
            }

            var update = state switch
            {
                Browsing browsing => HandleBrowsing(game, browsing, key),
                Lifted lifted => HandleLifted(game, lifted, key),
                _ => null,
            };

            if (update is null)
            {
                continue;
            }

            (state, statusMessage) = update.Value;
            Render(game, state, statusMessage);
        }
    }

    private static (InputState, string?)? HandleBrowsing(Game game, Browsing browsing, ConsoleKey key) =>
        key switch
        {
            ConsoleKey.LeftArrow => (browsing with { SelectedTower = Math.Max(0, browsing.SelectedTower - 1) }, (string?)null),
            ConsoleKey.RightArrow => (browsing with { SelectedTower = Math.Min(Game.TowerCount - 1, browsing.SelectedTower + 1) }, (string?)null),
            ConsoleKey.Spacebar => PickUp(game, browsing),
            _ => null,
        };

    private static (InputState, string?) PickUp(Game game, Browsing browsing)
    {
        if (game.Towers[browsing.SelectedTower].TryPeek(out _))
        {
            return (new Lifted(browsing.SelectedTower, browsing.SelectedTower), null);
        }

        return (browsing, null);
    }

    private static (InputState, string?)? HandleLifted(Game game, Lifted lifted, ConsoleKey key) =>
        key switch
        {
            ConsoleKey.LeftArrow => (lifted with { HoverTower = Math.Max(0, lifted.HoverTower - 1) }, (string?)null),
            ConsoleKey.RightArrow => (lifted with { HoverTower = Math.Min(Game.TowerCount - 1, lifted.HoverTower + 1) }, (string?)null),
            ConsoleKey.Spacebar => Drop(game, lifted),
            _ => null,
        };

    private static (InputState, string?) Drop(Game game, Lifted lifted)
    {
        var result = game.TryMove(lifted.OriginTower, lifted.HoverTower);

        return result switch
        {
            MoveResult.Success => (new Browsing(lifted.HoverTower), null),
            MoveResult.DestinationDiskTooSmall => (lifted, "Can't drop a disk onto a smaller one."),
            MoveResult.SourceEmpty => (lifted, "Nothing to drop."),
            _ => (lifted, "Illegal move."),
        };
    }

    private static void Render(Game game, InputState state, string? statusMessage)
    {
        System.Console.Clear();
        System.Console.WriteLine("=== Towers of Hanoi ===");
        System.Console.WriteLine();

        var columnWidth = game.DiskCount * 2 + 1;
        var gap = new string(' ', ColumnGap);

        var liftedOriginTower = state is Lifted originLifted ? originLifted.OriginTower : (int?)null;
        var hoverTower = state is Lifted hoverLifted ? hoverLifted.HoverTower : (int?)null;
        var selectedTower = state is Browsing browsing ? browsing.SelectedTower : hoverTower!.Value;

        Disk? liftedDisk = null;
        if (liftedOriginTower is int origin)
        {
            game.Towers[origin].TryPeek(out var top);
            liftedDisk = top;
        }

        var hoverCells = new string[Game.TowerCount];
        for (var t = 0; t < Game.TowerCount; t++)
        {
            hoverCells[t] = t == hoverTower && liftedDisk is Disk hovering
                ? RenderDisk(hovering.Size, columnWidth)
                : Center(string.Empty, columnWidth);
        }

        System.Console.WriteLine(string.Join(gap, hoverCells));

        for (var row = 0; row < game.DiskCount; row++)
        {
            var slotFromBottom = game.DiskCount - 1 - row;
            var cells = new string[Game.TowerCount];

            for (var t = 0; t < Game.TowerCount; t++)
            {
                var disks = game.Towers[t].Disks;
                var visibleCount = disks.Count - (t == liftedOriginTower ? 1 : 0);
                cells[t] = slotFromBottom < visibleCount
                    ? RenderDisk(disks[slotFromBottom].Size, columnWidth)
                    : RenderDisk(null, columnWidth);
            }

            System.Console.WriteLine(string.Join(gap, cells));
        }

        var baseCells = Enumerable.Repeat(new string(BaseChar, columnWidth), Game.TowerCount).ToArray();
        System.Console.WriteLine(string.Join(gap, baseCells));

        var labelCells = Enumerable.Range(1, Game.TowerCount).Select(n => Center(n.ToString(), columnWidth)).ToArray();
        System.Console.WriteLine(string.Join(gap, labelCells));

        var cursorCells = new string[Game.TowerCount];
        for (var t = 0; t < Game.TowerCount; t++)
        {
            cursorCells[t] = t == selectedTower ? Center("^", columnWidth) : Center(string.Empty, columnWidth);
        }

        System.Console.WriteLine(string.Join(gap, cursorCells));

        System.Console.WriteLine();
        System.Console.WriteLine($"Moves: {game.MoveCount}");
        System.Console.WriteLine(statusMessage ?? string.Empty);
    }

    private static string RenderDisk(int? size, int columnWidth) =>
        size is int s ? Center(new string(DiskChar, s * 2 - 1), columnWidth) : Center(PoleChar.ToString(), columnWidth);

    private static string Center(string text, int width)
    {
        if (text.Length >= width)
        {
            return text;
        }

        var totalPad = width - text.Length;
        var left = totalPad / 2;
        var right = totalPad - left;
        return new string(' ', left) + text + new string(' ', right);
    }

    private abstract record InputState;

    private sealed record Browsing(int SelectedTower) : InputState;

    private sealed record Lifted(int OriginTower, int HoverTower) : InputState;
}
