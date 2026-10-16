using UnityEngine;
using System.Collections.Generic;

namespace Yajima
{
    [CreateAssetMenu(fileName = "RackAsset", menuName = "Table/RackAsset")]
    public class RackAsset : ScriptableObject
    {
        [Header("ラックの組み方")]
        [SerializeField] RackPositionPair[] _rackBalls;

        public IReadOnlyList<RackPositionPair> RackBalls => _rackBalls;
    }
}