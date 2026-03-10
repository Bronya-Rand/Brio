using FFXIVClientStructs.Havok.Animation.Rig;
using System.Collections.Generic;

namespace Brio.Game.Posing.Skeletons;



public unsafe class PartialSkeleton(Skeleton skeleton, int id)
{
    public int Id { get; } = id;

    public Skeleton Skeleton { get; } = skeleton;

    public List<nint> Poses { get; set; } = [];

    private readonly Dictionary<int, Bone> _bones = [];

    public List<Bone> RootBones { get; set; } = [];

    public Dictionary<int, Bone> Bones => _bones;

    public Bone?[] BoneArray { get; private set; } = [];

    public Bone GetOrCreateBone(int index)
    {
        if(_bones.TryGetValue(index, out var bone))
            return bone;

        return _bones[index] = new Bone(index, Skeleton, this);
    }

    public Bone? GetBone(int index)
    {
        if(_bones.TryGetValue(index, out var bone))
            return bone;

        return null;
    }

    public Bone? GetBone(string name)
    {
        foreach(var bone in _bones.Values)
        {
            if(bone.Name == name)
                return bone;
        }

        return null;
    }

    public hkaPose* GetBestPose()
    {
        return Poses.Count > 0 ? (hkaPose*)Poses[0] : null;
    }

    internal void SealToBoneArray()
    {
        if(Poses.Count == 0)
            return;

        var pose = (hkaPose*)Poses[0];
        if(pose == null || pose->Skeleton == null)
            return;

        var count = pose->Skeleton->Bones.Length;
        if(count == 0)
            return;

        var arr = new Bone?[count];
        foreach(var (idx, bone) in _bones)
        {
            if((uint)idx < (uint)count)
            {
                arr[idx] = bone;
            }
        }

        BoneArray = arr;
    }
}

