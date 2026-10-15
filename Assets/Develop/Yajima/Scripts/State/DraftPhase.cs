namespace Yajima
{
    public sealed class DraftPhase : PhaseBase
    {
        float _elapsedTime = 0f;

        public DraftPhase(RuleBlackBoard ruleBlackBoard, PhaseType ownState)
        : base(ruleBlackBoard, ownState)
        {
        }

        public override void Enter()
        {
            _elapsedTime = 0f;
            GameDebug.Log("DraftPhase Enter");
        }

        public override void Exit()
        {
            GameDebug.Log("DraftPhase Exit");
        }

        public override PhaseType Update(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime >= _ruleBlackBoard.DraftLimitTime)
            {
                return PhaseType.BreakShotPhase;
            }

            return OwnState;
        }
    }
}