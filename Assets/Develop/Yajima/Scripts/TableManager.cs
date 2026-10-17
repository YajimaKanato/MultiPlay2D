using UnityEngine;
using System.Linq;
using System;

namespace Yajima
{
    /// <summary>
    /// <para>テーブル上の管理を行うクラス</para>
    /// <para>球の座標を管理する</para>
    /// </summary>
    public class TableManager : MonoBehaviour
    {
        [SerializeField, Tooltip("ラックの組み方")] RackBallPosition[] _balls;
        RackPositionPair[] _rackBalls;
        RuleBlackBoard _ruleBlackBoard;
        bool _isRegisterdAction = false;

        public void Init(RackAsset rackAsset, RuleBlackBoard ruleBlackBoard)
        {
            if (rackAsset == null)
            {
                GameDebug.LogError("ラックの組み方が設定されていません");
                return;
            }

            _rackBalls = rackAsset.RackBalls.ToArray().Clone() as RackPositionPair[];
            _ruleBlackBoard = ruleBlackBoard ?? throw new ArgumentNullException(nameof(ruleBlackBoard));
        }

        void OnEnable()
        {
            RegisterAction();
        }

        void OnDisable()
        {
            UnregisterAction();
        }

        void RegisterAction()
        {
            if (_isRegisterdAction) return;
        }
        
        void UnregisterAction()
        {
            if (!_isRegisterdAction) return;
        }

        /// <summary>
        /// ラックを組み立てるメソッド
        /// </summary>
        public void CreateRack()
        {
            if (_rackBalls == null)
            {
                GameDebug.LogError("ラックの組み方が設定されていません");
                return;
            }

            foreach (var rackBall in _rackBalls)
            {
                var ballPrefab = rackBall.BallPrefab;
                if (ballPrefab == null)
                {
                    GameDebug.LogError($"ラックの位置 {rackBall.RackPositionNumber} に配置する球のプレハブが設定されていません");
                    continue;
                }

                var transform = _balls[(int)rackBall.RackPositionNumber].Transform;
                if (transform == null)
                {
                    GameDebug.LogError($"ラックの位置 {rackBall.RackPositionNumber} に配置する球のTransformが設定されていません");
                    continue;
                }
                Instantiate(ballPrefab, transform.position, Quaternion.identity);
            }
        }

        /// <summary>
        /// 手球が的球が衝突した時に呼ばれるメソッド
        /// </summary>
        /// <param name="collideBall">衝突した的球の番号</param>
        void CollideBall(Balls collideBall)
        {
            if (!_ruleBlackBoard.IsShot) return;

            // 初めてボールに触れた場合
            if(!_ruleBlackBoard.IsTouchAnyBall)
            {
                _ruleBlackBoard.IsTouchAnyBall = true;
                _ruleBlackBoard.TouchFirstTargetBall = collideBall == _ruleBlackBoard.FirstTouchBall;
            }
        }

        /// <summary>
        /// 球がポケットインした時に呼ばれるメソッド
        /// </summary>
        /// <param name="pocketInBall">ポケットインした球</param>
        void PocketInBall(Balls pocketInBall)
        {
            if (!_ruleBlackBoard.IsShot) return;

            // 手球が入った
            if (pocketInBall == Balls.CueBall)
                _ruleBlackBoard.IsPocketInCueBall = true;
        }
    }
}