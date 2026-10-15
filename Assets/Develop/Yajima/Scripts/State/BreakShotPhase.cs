namespace Yajima
{
    /// <summary>ブレイクショットフェーズ</summary>
    public sealed class BreakShotPhase : PhaseBase
    {
        float _elapsedTime = 0f;

        public BreakShotPhase(RuleBlackBoard ruleBlackBoard, PhaseType ownState)
        : base(ruleBlackBoard, ownState)
        {
        }

        public override void Enter()
        {
            _elapsedTime = 0f;
            GameDebug.Log("BreakShotPhase Enter");
        }

        public override void Exit()
        {
            GameDebug.Log("BreakShotPhase Exit");
        }

        public override PhaseType Update(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime >= _ruleBlackBoard.BreakShotLimitTime)
            {
                return PhaseType.DraftPhase;
            }

            if(_ruleBlackBoard.IsShot)
            {
                return PhaseType.AfterShotPhase;
            }

            return OwnState;
        }
    }
}
