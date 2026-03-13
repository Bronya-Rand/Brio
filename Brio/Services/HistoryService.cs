using Brio.Capabilities.Posing;
using Brio.Core;
using Brio.Entities;
using Brio.Entities.Core;
using Brio.Game.Posing;
using System.Collections.Generic;
using System.Linq;

namespace Brio.Services;

public class HistoryService(EntityManager entityManager)
{
    private readonly EntityManager _entityManager = entityManager;

    private readonly Stack<GroupEntry> _undo = [];
    private readonly Stack<GroupEntry> _redo = [];

    public void Snapshot(IEnumerable<(EntityId id, PosingCapability capability, Transform transform)> entries)
    {
        var group = new GroupEntry
        {
            Entries = [.. entries.Select(e => new HistoryEntry(e.id, e.capability, e.transform))]
        };

        _undo.Push(group);
        _redo.Clear();
    }

    public bool CanUndo => _undo.Count is not 0 and not 1;
    public bool CanRedo => _redo.Count > 0;

    public void Undo()
    {
        if(!CanUndo)
            return;

        var pop = _undo.Pop();
        var entries = new GroupEntry();

        foreach(var entry in pop.Entries)
        {
            entries.Entries.Add(new HistoryEntry(entry.Id, entry.Capability, entry.ModelTransform));

            entry.Capability.SkeletonPosing.PoseInfo = entry.Capability.SkeletonPosing.PoseInfo.Clone();
            entry.Capability.ModelPosing.Transform = entry.ModelTransform;
        }

        _redo.Push(entries);
    }

    public void Redo()
    {
        if(!CanRedo)
            return;

        var pop = _redo.Pop();
        var entries = new GroupEntry();

        foreach(var entry in pop.Entries)
        {
            entries.Entries.Add(new HistoryEntry(entry.Id, entry.Capability, entry.ModelTransform));

            entry.Capability.SkeletonPosing.PoseInfo = entry.Capability.SkeletonPosing.PoseInfo.Clone();
            entry.Capability.ModelPosing.Transform = entry.ModelTransform;
        }

        _undo.Push(entries);
    }

    public void Clear()
    {
        _undo.Clear();
        _redo.Clear();
    }

    private class GroupEntry
    {
        public List<HistoryEntry> Entries = [];
    }
}

public record class HistoryEntry(EntityId Id, PosingCapability Capability, Transform ModelTransform);
