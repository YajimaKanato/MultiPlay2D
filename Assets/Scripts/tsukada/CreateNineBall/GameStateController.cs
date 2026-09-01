using System.Xml;
using UnityEngine;

/// <summary>  9ボールで、ゲームの進行状態を管理するクラス </summary>
public class GameStateController : MonoBehaviour
{
    private GameStateMachine _stateMachine;

    private GameStateController _gameStateController;

    [SerializeField] private TurnController _turnController;

    [SerializeField] private CollideBalls _collideBalls;

    /// <summary> ショットしたかのフラグ。ターン移行毎にfalseに戻す。 </summary>
    private bool _isShotted = false;

    /// <summary> 全ての球が停止したかのフラグ </summary>
    private bool _hadAllBallsStop = false;

    /// <summary> 何もポケットに落ちなかったかどうかのフラグ </summary>
    private bool _hadPocketAnyBall = false;

    /// <summary> ファールしたかどうかのフラグ </summary>
    private bool _hadFoul = false;

    /// <summary> クリア(合法的に9ボールがポケットに落ちたかどうかのフラグ </summary>
    private bool _isGameClear = false;

    /// <summary> 結果を表示したかのフラグ </summary>
    private bool _isResultDisplayed = false;

    //フラグのプロパティ
    public bool IsShotted => _isShotted;
    public bool HadAllBallsStop => _hadAllBallsStop;
    public bool HadPocketAnyBall => _hadPocketAnyBall;
    public bool HadFoul => _hadFoul;
    public bool IsGameClear => _isGameClear;
    public bool IsResultDisplayed => _isResultDisplayed;

    private void Awake()
    {
        _stateMachine = new GameStateMachine();
    }

    private void Start()
    {
        _stateMachine.ChangeState(new ReadyFase(_stateMachine, _gameStateController));
    }

    private void Update()
    {
        _stateMachine.Update();
    }


    //-----------------------------
    //↓↓↓フラグ管理系メソッド↓↓↓
    ///----------------------------

    /// <summary> ゲームクリア条件を満たしたらに呼ばれ、クリアフラグを立てるメソッド </summary>
    public void MeetConditionOfGameClear()
    {
        _isGameClear = true;
    }

    /// <summary> 何かしらのボールがポケットに落ちたかどうかのフラグを切り替えるメソッド </summary>
    public void SwitchFlagOfPocketAnyBall()
    {
        _hadPocketAnyBall = !_hadPocketAnyBall;
    }

    /// <summary> ショットしたかどうかのフラグを切り替えるメソッド </summary>
    public void SwitchFlagOfShotted()
    {
        _isShotted = !_isShotted;
    }

    /// <summary> ファールフラグを切り替えるメソッド </summary>
    public void SwitchFlagofFouled()
    {
        _hadFoul = !_hadFoul;
    }

    /// <summary> 全ての球が停止したフラグを切り替えるメソッド </summary>
    public void SwitchFlagOfAllBallsHadStop()
    {
        _hadAllBallsStop = !_hadAllBallsStop;
    }
}
