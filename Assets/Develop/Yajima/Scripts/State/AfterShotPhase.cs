namespace Yajima
{
    /// <summary>ショット後のフェーズ</summary>
    public sealed class AfterShotPhase : PhaseBase
    {
        public AfterShotPhase(RuleBlackBoard ruleBlackBoard, PhaseType ownState) : base(ruleBlackBoard, ownState)
        {
        }

        public override void Enter()
        {
            GameDebug.Log("AfterShotPhase Enter");
        }

        public override void Exit()
        {
            GameDebug.Log("AfterShotPhase Exit");
        }

        public override PhaseType Update(float deltaTime)
        {
            if (_ruleBlackBoard.IsAllBallStop)
            {
                if (_ruleBlackBoard.IsWinCondition)
                    return PhaseType.ResultPhase;
                else
                    return PhaseType.DraftPhase;
            }

            return OwnState;
        }
    }
}
