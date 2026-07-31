namespace TowersOfHanoi.Core;

/// <summary>An ordered stack of disks; only the top disk is ever addressable.</summary>
public sealed class Tower
{
    private readonly List<Disk> _disks = new();

    /// <summary>Disks ordered bottom-to-top, for rendering.</summary>
    public IReadOnlyList<Disk> Disks => _disks;

    public int Count => _disks.Count;

    public bool TryPeek(out Disk disk)
    {
        if (_disks.Count == 0)
        {
            disk = default;
            return false;
        }

        disk = _disks[^1];
        return true;
    }

    public void Push(Disk disk) => _disks.Add(disk);

    public bool TryPop(out Disk disk)
    {
        if (!TryPeek(out disk))
        {
            return false;
        }

        _disks.RemoveAt(_disks.Count - 1);
        return true;
    }
}
