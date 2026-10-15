namespace Yajima
{
    /// <summary>ショットフェーズ</summary>
    public sealed class ShotPhase : PhaseBase
    {
        float _elapsedTime = 0f;

        public ShotPhase(RuleBlackBoard ruleBlackBoard, PhaseType ownPhase)
        : base(ruleBlackBoard, ownPhase)
        {
        }

        public override void Enter()
        {
            _elapsedTime = 0f;
            GameDebug.Log("ShotPhase Enter");
        }

        public override void Exit()
        {
            GameDebug.Log("ShotPhase Exit");
        }

        public override PhaseType Update(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime >= _ruleBlackBoard.ShotLimitTime)
            {
                return PhaseType.DraftPhase;
            }

            if (_ruleBlackBoard.IsShot)
            {
                return PhaseType.AfterShotPhase;
            }
            
            return OwnState;
        }
    }
}
