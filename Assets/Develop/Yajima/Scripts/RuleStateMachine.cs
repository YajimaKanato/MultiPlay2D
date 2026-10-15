using System;
using System.Collections.Generic;

namespace Yajima
{
    public sealed class RuleStateMachine
    {
        /// <summary>ルールのブラックボード</summary>
        RuleBlackBoard _blackBoard;

        /// <summary>現在のステートのインスタンス</summary>
        IPhase _currentState;

        /// <summary>ステートのキャッシュをするコレクション</summary>
        Dictionary<PhaseType, IPhase> _stateCaches = new();

        public RuleStateMachine(RuleBlackBoard blackBoard, PhaseType initialPhase)
        {
            _blackBoard = blackBoard ?? throw new ArgumentNullException(nameof(blackBoard));

            foreach (PhaseType phaseType in Enum.GetValues(typeof(PhaseType)))
            {
                var state = GenerateState(phaseType);
                _stateCaches[phaseType] = state;
            }

            ChangeState(initialPhase);
        }

        /// <summary>
        /// 毎フレーム呼び出すメソッド
        /// </summary>
        /// <param name="deltaTime">前フレームからの経過時間</param>
        public void Update(float deltaTime)
        {
            if (_currentState == null) return;

            var nextPhase = _currentState.Update(deltaTime);

            if (_currentState.OwnState != nextPhase)
            {
                ChangeState(nextPhase);
            }
        }

        /// <summary>
        /// ステートを変更するメソッド
        /// </summary>
        /// <param name="nextPhase">変更するステートの種類</param>
        void ChangeState(PhaseType nextPhase)
        {
            // 現在のステートから抜ける
            _currentState?.Exit();

            // キャッシュから次のステートを探す
            // または新たに生成する
            if (!_stateCaches.TryGetValue(nextPhase, out var nextState))
            {
                nextState = GenerateState(nextPhase);
                _stateCaches[nextPhase] = nextState;
            }
            // 現在のステートを更新
            _currentState = nextState;

            // 次のステートに入る
            _currentState?.Enter();
        }

        /// <summary>
        /// ステートを生成するメソッド
        /// </summary>
        /// <param name="phaseType">生成するステートの種類</param>
        /// <returns>生成されたステート</returns>
        /// <exception cref="InvalidOperationException">ステートの種類が不正</exception>
        IPhase GenerateState(PhaseType phaseType)
        {
            IPhase state = phaseType switch
            {
                PhaseType.BreakShotPhase => new BreakShotPhase(_blackBoard, phaseType),
                PhaseType.DraftPhase => new DraftPhase(_blackBoard, phaseType),
                PhaseType.ShotPhase => new ShotPhase(_blackBoard, phaseType),
                PhaseType.AfterShotPhase => new AfterShotPhase(_blackBoard, phaseType),
                PhaseType.ResultPhase => new ResultPhase(_blackBoard, phaseType),
                _ => throw new InvalidOperationException($"ステートの種類が不正です。phaseType: {phaseType}")
            };

            return state;
        }
    }
}