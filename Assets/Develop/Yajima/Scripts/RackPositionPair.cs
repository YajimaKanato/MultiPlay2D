using UnityEngine;
using System;

namespace Yajima
{
    /// <summary>
    /// ラックの位置ペアを表すクラス
    /// </summary>
    [Serializable]
    public class RackPositionPair
    {
        [SerializeField, Tooltip("ラックの位置番号")] RackPositionNumber _rackPositionNumber;
        [SerializeField, Tooltip("ラックの位置に配置する球のプレハブ")] BallBase _transform;

        public RackPositionNumber RackPositionNumber => _rackPositionNumber;
        public BallBase BallPrefab => _transform;
    }
}