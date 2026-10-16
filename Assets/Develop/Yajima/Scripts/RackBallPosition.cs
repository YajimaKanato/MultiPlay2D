using UnityEngine;
using System;

namespace Yajima
{
    [Serializable]
    public class RackBallPosition
    {
        [SerializeField, Tooltip("ラックの位置番号")] RackPositionNumber _rackPositionNumber;
        [SerializeField, Tooltip("ラックの位置に配置する球のプレハブ")] Transform _transform;
        
        public RackPositionNumber RackPositionNumber => _rackPositionNumber;
        public Transform BallPrefab => _transform;
    }
}