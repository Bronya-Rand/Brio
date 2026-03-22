using System;
using System.Collections.Generic;
using System.Text;

namespace Brio.Core;

public interface ITransformable
{
    public bool HasOverride { get; }

    public bool IsTransformFrozen { get; set; }

    public Transform Transform { get; set; }
    public Transform OriginalTransform { get; set; }
}
