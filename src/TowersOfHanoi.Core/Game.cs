using System.Diagnostics;

namespace TowersOfHanoi.Core;

public sealed class Game
{
    public const int TowerCount = 3;
    public const int LeftTowerIndex = 0;
    public const int RightTowerIndex = TowerCount - 1;

    private readonly Stopwatch _stopwatch = new();

    public Game(int diskCount)
    {
        DiskCount = diskCount;

        var towers = new Tower[TowerCount];
        for (var i = 0; i < TowerCount; i++)
        {
            towers[i] = new Tower();
        }

        Towers = towers;

        for (var size = diskCount; size >= 1; size--)
        {
            towers[LeftTowerIndex].Push(new Disk(size));
        }

        _stopwatch.Start();
    }

    public int DiskCount { get; }

    public IReadOnlyList<Tower> Towers { get; }

    public int MoveCount { get; private set; }

    public GameStatus Status { get; private set; } = GameStatus.InProgress;

    public bool IsWon => Status == GameStatus.Completed;

    public TimeSpan ElapsedTime => _stopwatch.Elapsed;

    public int OptimalMoveCount => (1 << DiskCount) - 1;

    public MoveResult TryMove(int sourceIndex, int destIndex)
    {
        ValidateTowerIndex(sourceIndex, nameof(sourceIndex));
        ValidateTowerIndex(destIndex, nameof(destIndex));

        if (sourceIndex == destIndex)
        {
            return MoveResult.Success;
        }

        var source = Towers[sourceIndex];
        var dest = Towers[destIndex];

        if (!source.TryPeek(out var disk))
        {
            return MoveResult.SourceEmpty;
        }

        if (dest.TryPeek(out var destTop) && destTop.Size < disk.Size)
        {
            return MoveResult.DestinationDiskTooSmall;
        }

        source.TryPop(out _);
        dest.Push(disk);
        MoveCount++;

        if (Towers[RightTowerIndex].Count == DiskCount)
        {
            Status = GameStatus.Completed;
            _stopwatch.Stop();
        }

        return MoveResult.Success;
    }

    public void Quit()
    {
        if (Status != GameStatus.InProgress)
        {
            return;
        }

        Status = GameStatus.QuitEarly;
        _stopwatch.Stop();
    }

    private static void ValidateTowerIndex(int index, string paramName)
    {
        if (index < 0 || index >= TowerCount)
        {
            throw new ArgumentOutOfRangeException(paramName, index, $"Tower index must be between 0 and {TowerCount - 1}.");
        }
    }
}
