using UnityEngine;
using System;
using System.Linq;

namespace Yajima
{
    /// <summary>
    /// <para>テーブル上の管理を行うクラス</para>
    /// <para>球の座標を管理する</para>
    /// </summary>
    public class TableManager : MonoBehaviour
    {
        [SerializeField, Tooltip("ラックの組み方")] RackBallPosition[] _balls;

        public void Init(RackAsset rackAsset)
        {
            if(rackAsset == null)
            {
                GameDebug.LogError("ラックの組み方が設定されていません");
                return;
            }
        }

        /// <summary>
        /// ラックを組み立てるメソッド
        /// </summary>
        public void CreateRack()
        {
            
        }
    }
}