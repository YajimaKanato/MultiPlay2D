using System;
using System.Linq;
using UnityEngine;

namespace Yajima
{
    public partial class BilliardManager : MonoBehaviour
    {
        [SerializeField, Tooltip("ゲームの状態を定義したクラス")] RuleBlackBoard _blackBoard;
        [SerializeField, Tooltip("盤面のマネージャークラス")] TableManager _tableManager;
        [SerializeField, Tooltip("初期フェーズ")] PhaseType _initialPhase;
        [SerializeField, Tooltip("ルールに関するデータ")] RuleData[] _ruleDataArray;
        RuleStateMachine _ruleStateMachine;

        static BilliardManager _instance;
        public static BilliardManager Instance => _instance;

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="ruleType">ルール</param>
        public void Init(RuleType ruleType = RuleType.NineBall)
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            // ブラックボード初期化
            _blackBoard = new();

            // StateMachine初期化
            _ruleStateMachine = new(_blackBoard, _initialPhase);

            // テーブル初期化
            var ruleData = _ruleDataArray.FirstOrDefault(rule => rule.RuleType == ruleType);
            _tableManager.Init(ruleData.RackAsset, _blackBoard);
        }

        void OnDestroy()
        {
            _instance = null;
        }

        void Update()
        {
            _ruleStateMachine?.Update(Time.deltaTime);
        }

        /// <summary>ルールに関する情報をもつクラス</summary>
        [Serializable]
        class RuleData
        {
            [SerializeField] RuleType _ruleType = RuleType.NineBall;
            [SerializeField] RackAsset _rackAsset;

            public RuleType RuleType => _ruleType;
            public RackAsset RackAsset => _rackAsset;
        }
    }
}