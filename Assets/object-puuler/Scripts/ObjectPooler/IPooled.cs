using System;
using UnityEngine;

namespace ObjectPuuler
{
    public interface IPooled
    {
        Action<GameObject> ReleaseCallback { get; set; }
    }
}
