namespace Yajima
{
    public sealed class ResultPhase : PhaseBase
    {
        public ResultPhase(RuleBlackBoard ruleBlackBoard, PhaseType ownState) : base(ruleBlackBoard, ownState)
        {
        }

        public override void Enter()
        {
            GameDebug.Log("ResultPhase Enter");
        }

        public override void Exit()
        {
            GameDebug.Log("ResultPhase Exit");
        }

        public override PhaseType Update(float deltaTime)
        {
            return OwnState;
        }
    }
}