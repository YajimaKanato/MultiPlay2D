namespace Yajima
{
    /// <summary>ステートの基底クラス</summary>
    public abstract class PhaseBase : IPhase
    {
        protected RuleBlackBoard _ruleBlackBoard;
        readonly PhaseType _ownState;

        public PhaseType OwnState => _ownState;

        public PhaseBase(RuleBlackBoard ruleBlackBoard, PhaseType ownState)
        {
            _ruleBlackBoard = ruleBlackBoard;
            _ownState = ownState;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract PhaseType Update(float deltaTime);
    }
}
