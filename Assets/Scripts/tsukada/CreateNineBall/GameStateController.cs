using UnityEngine;

/// <summary>
/// 9ボールで、ゲームの進行状態を管理するクラス
/// </summary>
public class GameStateController : MonoBehaviour
{

    public enum GameState
    {
        /// <summary>
        /// ゲーム開始前の準備フェーズ
        /// </summary>
        ReadyFase,
        /// <summary>
        /// ブレイクショットのフェーズ
        /// </summary>
        BreakeShotFase,
        /// <summary>
        /// ドラフトのフェーズ。ドラフトエフェクトの選択と使用の両方を含む
        /// </summary>
        DraftFase,
        /// <summary>
        /// 各プレイヤーのショットのフェーズ
        /// </summary>
        //FirstPlayerTurnFase, SecondPlayerTurnFase, ThirdPlayerTurnFase, FourthPlayerTurnFase,
        /// <summary>
        /// ショットのフェーズ。ショットの実行と結果の確認を含む
        /// </summary>
        ShotFase,
        /// <summary>
        /// ゲーム終了後のフェーズ。試合結果の確認や、「もう一度遊ぶ、ホームに戻る」などの選択を含む
        /// </summary>
        ResultFase,
    }

    [Tooltip("現在のゲーム状態")]
    GameState _currentGameState = GameState.ReadyFase;

    [Tooltip("ドラフトフェーズの制限時間")]
    [SerializeField] private float _draftFaseTime = 30.0f;

    [Tooltip("ショットフェーズの制限時間")]
    [SerializeField] private float _shotFaseTime = 30.0f;

    //タイマー用の変数
    private float _timer = 0.0f;

    [SerializeField] private TurnController _turnController = default;
    [SerializeField] private CollideBalls _collideBalls = default;

    /// <summary> ショットしたかどうかのフラグ。ターン移行毎にfalseに戻す。 </summary>
    private bool _isShotted = false;

    /// <summary> 合法的に9ボールがポケットに落ちたかどうかのフラグ </summary>
    private bool _isGameClear = false;

    /// <summary> 結果を表示したかのフラグ </summary>
    private bool _isResultDisplayed = false;

    //プロパティ
    public float DraftFaseTimer { get { return _draftFaseTime; } private set { _draftFaseTime = value; } }
    public float ShotFaseTimer { get { return _shotFaseTime; } private set { _shotFaseTime = value; } }


    void Start()
    {
        GameDebug.Log("ゲーム開始前のReadyFaseから始まります");
    }

    void Update()
    {
        switch (_currentGameState)
        {
            case GameState.ReadyFase: UpdateReadyFase(); break;
            case GameState.BreakeShotFase: UpdateBreakeShot(); break;
            case GameState.DraftFase: UpdateDraftFase(); break;
            case GameState.ShotFase: UpdateShotFase(); break;
            case GameState.ResultFase: UpdateResultFase(); break;
        }
    }
    
    /// <summary> ゲーム開始前の準備フェーズ中の処理 </summary>
    //例えば、プレイヤーの準備が整ったかどうかの確認など
    void UpdateReadyFase()
    {
        //if (プレイヤーの準備が整ったら)
        if (Input.GetKeyDown(KeyCode.Return))  //仮の条件
        {
            ChangeGameState(GameState.BreakeShotFase);
        }
        //ChangeGameState(GameState.BreakeShotFase);
    }

    /// <summary> ブレイクショットフェーズ中のUpdate処理 </summary>
    void UpdateBreakeShot()
    {
        //撃ったらドラフトフェーズ開始
        //if (ブレイクショットが終了(全ての球が停止)したら)
        if (Input.GetKeyDown(KeyCode.Return))   //仮の条件
        {
            ChangeGameState(GameState.DraftFase);
        }
        //ChangeGameState(GameState.DraftFase);
    }

    /// <summary> ドラフトフェーズ中のUpdate処理 </summary>
    void UpdateDraftFase()
    {
        _timer += Time.deltaTime;
        if (_timer >= _draftFaseTime)
        {
            ChangeGameState(GameState.ShotFase);
        }

        //if(全員の行動が完了したら)
        if (Input.GetKeyDown(KeyCode.Return))    //仮の条件
        {
            ChangeGameState(GameState.ShotFase);
        }
        //ChangeGameState(GameState.ShotFase);
    }

    /// <summary> ショットフェーズ中のUpdate処理。結果の確認も含む。 </summary>
    void UpdateShotFase()
    {

        
        if (_timer >= _shotFaseTime)        //(ショット結果が収束する、もしくは)制限時間超えたらフェーズ移行
        {
            ChangeGameState(GameState.DraftFase);
        }

        //if(ショット後、9ボールがポケットに落ちたら)
        if (_isShotted && _isGameClear)
        {
            ChangeGameState(GameState.ResultFase);
        }

        if (_isShotted) return;     //まだショットしてなければ、タイマーを進める
        _timer += Time.deltaTime;
    }

    
    /// <summary> 結果フェーズ中のUpdate処理 </summary>
    void UpdateResultFase()
    {
        if(!_isResultDisplayed)
        {
            //試合結果、スコアを表示
            GameDebug.Log($"ゲームが終了しました。<br> {_turnController.CurrentTurn} が勝ちました。");
            _isResultDisplayed = true;
        }

        //if(「もう一度遊ぶ」を選択したら)
        //if(「ロビーに戻る」を選択したら)
        //if(「ホームに戻る」を選択したら)
    }

    /// <summary> ゲーム状態を変更するメソッド </summary>
    public void ChangeGameState(GameState newGameState)
    {
        _currentGameState = newGameState;

        GameDebug.Log($"フェーズが {newGameState} に移行しました");

        //ドラフトフェーズへの移行時は、ターンを切り替える
        if (newGameState == GameState.DraftFase)
        {
            _turnController.ChangeTurn();
        }

        _collideBalls.CheckMinObjectBallNum();      //存在する的球の中の最小値を求める

        _isShotted = false;
        _timer = 0f;
        _collideBalls.ResetCollideObjectBallFlag();      //最小値の的球に衝突したかどうかのフラグをリセット
    }

    [ContextMenu("ショット後、DraftFaseに強制移行")]
    public void ForceChangeDraftFase()
    {
        ChangeGameState(GameState.DraftFase);
    }

    [ContextMenu("MeetConditionOfGameClear")]
    /// <summary> ゲームクリア条件を満たした場合に呼び出されるメソッド </summary>
    public void MeetConditionOfGameClear()
    {
        _isGameClear = true;
    }

    [ContextMenu("ショットしました")]
    public void NotifyShotted()
    {
        _isShotted = true;
    }

}
