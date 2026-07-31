using TowersOfHanoi.Core;

namespace TowersOfHanoi.Core.Tests;

public class GameTests
{
    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(9)]
    public void InitialState_StacksAllDisksOnLeftTowerInDescendingOrder(int diskCount)
    {
        var game = new Game(diskCount);

        Assert.Equal(diskCount, game.Towers[0].Count);
        Assert.Empty(game.Towers[1].Disks);
        Assert.Empty(game.Towers[2].Disks);

        var expectedSizes = Enumerable.Range(1, diskCount).Reverse();
        Assert.Equal(expectedSizes, game.Towers[0].Disks.Select(d => d.Size));

        Assert.Equal(0, game.MoveCount);
        Assert.Equal(GameStatus.InProgress, game.Status);
    }

    [Fact]
    public void TryMove_OntoEmptyTower_IsLegal()
    {
        var game = new Game(3);

        var result = game.TryMove(0, 1);

        Assert.Equal(MoveResult.Success, result);
        Assert.Equal(1, game.Towers[1].Count);
        Assert.True(game.Towers[1].TryPeek(out var moved));
        Assert.Equal(1, moved.Size);
    }

    [Fact]
    public void TryMove_OntoLargerDisk_IsLegal()
    {
        var game = new Game(3);
        game.TryMove(0, 2); // smallest disk (1) -> tower 2
        game.TryMove(0, 1); // next disk (2) -> tower 1

        var result = game.TryMove(2, 1); // disk 1 onto disk 2 (larger): legal

        Assert.Equal(MoveResult.Success, result);
        Assert.Equal(2, game.Towers[1].Count);
        Assert.True(game.Towers[1].TryPeek(out var top));
        Assert.Equal(1, top.Size);
    }

    [Fact]
    public void TryMove_OntoSmallerDisk_IsIllegalAndLeavesStateUnchanged()
    {
        var game = new Game(3);
        game.TryMove(0, 1); // smallest disk -> tower 1

        var result = game.TryMove(0, 1); // next disk (larger) onto smallest: illegal

        Assert.Equal(MoveResult.DestinationDiskTooSmall, result);
        Assert.Equal(2, game.Towers[0].Count);
        Assert.Equal(1, game.Towers[1].Count);
        Assert.Equal(1, game.MoveCount);
    }

    [Fact]
    public void TryMove_FromEmptyTower_IsIllegalAndLeavesStateUnchanged()
    {
        var game = new Game(3);

        var result = game.TryMove(1, 2);

        Assert.Equal(MoveResult.SourceEmpty, result);
        Assert.Equal(3, game.Towers[0].Count);
        Assert.Empty(game.Towers[1].Disks);
        Assert.Empty(game.Towers[2].Disks);
        Assert.Equal(0, game.MoveCount);
    }

    [Fact]
    public void TryMove_SameTower_IsNoOpSuccessAndDoesNotCountAsMove()
    {
        var game = new Game(3);

        var result = game.TryMove(0, 0);

        Assert.Equal(MoveResult.Success, result);
        Assert.Equal(3, game.Towers[0].Count);
        Assert.Equal(0, game.MoveCount);
    }

    [Fact]
    public void TryMove_CrossTowerMove_IncrementsMoveCount()
    {
        var game = new Game(3);

        game.TryMove(0, 1);

        Assert.Equal(1, game.MoveCount);
    }

    [Fact]
    public void TryMove_IllegalMove_DoesNotIncrementMoveCount()
    {
        var game = new Game(3);
        game.TryMove(0, 1); // count = 1

        game.TryMove(0, 1); // illegal: larger disk onto smaller

        Assert.Equal(1, game.MoveCount);
    }

    [Fact]
    public void TryMove_SameTowerMove_DoesNotIncrementMoveCount()
    {
        var game = new Game(3);
        game.TryMove(0, 1); // count = 1

        game.TryMove(1, 1); // drop back on origin

        Assert.Equal(1, game.MoveCount);
    }

    [Fact]
    public void IsWon_FalseWhileDisksRemainOffRightmostTower()
    {
        var game = new Game(3);
        game.TryMove(0, 2);

        Assert.False(game.IsWon);
        Assert.Equal(GameStatus.InProgress, game.Status);
    }

    [Fact]
    public void IsWon_TrueOnlyWhenAllDisksCorrectlyStackedOnRightmostTower()
    {
        var game = new Game(3);

        foreach (var (source, dest) in SolveMoves(3, 0, 2, 1))
        {
            Assert.False(game.IsWon);
            game.TryMove(source, dest);
        }

        Assert.True(game.IsWon);
        Assert.Equal(GameStatus.Completed, game.Status);
        Assert.Equal(3, game.Towers[2].Count);
    }

    [Theory]
    [InlineData(3, 7)]
    [InlineData(4, 15)]
    [InlineData(5, 31)]
    [InlineData(6, 63)]
    [InlineData(7, 127)]
    [InlineData(8, 255)]
    [InlineData(9, 511)]
    public void OptimalMoveCount_MatchesTwoToTheNMinusOne(int diskCount, int expected)
    {
        var game = new Game(diskCount);

        Assert.Equal(expected, game.OptimalMoveCount);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void FullSolveSequence_EndsWonWithOptimalMoveCount(int diskCount)
    {
        var game = new Game(diskCount);

        foreach (var (source, dest) in SolveMoves(diskCount, 0, 2, 1))
        {
            var result = game.TryMove(source, dest);
            Assert.Equal(MoveResult.Success, result);
        }

        Assert.True(game.IsWon);
        Assert.Equal(GameStatus.Completed, game.Status);
        Assert.Equal(game.OptimalMoveCount, game.MoveCount);
    }

    private static IEnumerable<(int Source, int Dest)> SolveMoves(int diskCount, int source, int dest, int aux)
    {
        if (diskCount == 0)
        {
            yield break;
        }

        foreach (var move in SolveMoves(diskCount - 1, source, aux, dest))
        {
            yield return move;
        }

        yield return (source, dest);

        foreach (var move in SolveMoves(diskCount - 1, aux, dest, source))
        {
            yield return move;
        }
    }
}
