using UnityEngine;

/// <summary>  9ボールで、ゲームの進行状態を管理するクラス </summary>
public class GameStateController : MonoBehaviour
{
    // <summary> 現在のゲームフェーズ </summary>
    [SerializeField] GameState _currentGameState = GameState.ReadyFase;       //ReadyFaseから開始する

    [Tooltip("ドラフトフェーズの制限時間")]
    [SerializeField] private float _draftFaseTime = 30.0f;

    [Tooltip("ショットフェーズの制限時間")]
    [SerializeField] private float _shotFaseTime = 30.0f;

    //タイマー用の変数
    private float _timer = 0.0f;

    [SerializeField] private TurnController _turnController = null;
    [SerializeField] private CollideBalls _collideBalls = null;

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

    //プロパティ
    public float DraftFaseTimer { get { return _draftFaseTime; } private set { _draftFaseTime = value; } }
    public float ShotFaseTimer { get { return _shotFaseTime; } private set { _shotFaseTime = value; } }


    void Start()
    {
        GameDebug.Log("ゲーム開始前のReadyFaseから始まります");
    }

    void Update()
    {
        //フェーズごとに異なるUpdate処理を行う
        switch (_currentGameState)
        {
            case GameState.ReadyFase: UpdateReadyFase(); break;
            case GameState.BreakeShotFase: UpdateBreakeShotFase(); break;
            case GameState.DraftFase: UpdateDraftFase(); break;
            case GameState.ShotFase: UpdateShotFase(); break;
            case GameState.FoulFase: UpdateFoulFase(); break;
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
    }

    /// <summary> ブレイクショットフェーズ中のUpdate処理 </summary>
    void UpdateBreakeShotFase()
    {
        //撃ったらドラフトフェーズ開始
        //if (ブレイクショットが終了(全ての球が停止)したら)
        if (_isShotted && _hadAllBallsStop)
        {
            ChangeGameState(GameState.DraftFase);
        }
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
    }

    /// <summary> ショットフェーズ中のUpdate処理。結果の確認も含む。 </summary>
    void UpdateShotFase()
    {
        if (_timer >= _shotFaseTime)        //(ショット結果が収束する、もしくは)制限時間超えたらフェーズ移行
        {
            ChangeGameState(GameState.DraftFase);
        }

        //if(ショット後、9ボールがポケットに落ちたら)
        if (_isShotted && !_hadFoul && _isGameClear)
        {
            ChangeGameState(GameState.ResultFase);
        }

        if (_isShotted) return;     //まだショットしてなければ、タイマーを進める
        _timer += Time.deltaTime;
    }

    /// <summary> ファールフェーズ中のUpdate処理 </summary>
    void UpdateFoulFase()
    {
        //_collideBallsの_pocketBallsを参照し、含まれるボールを初期位置に戻すメソッドを呼び出す
        //if(手球を好きな場所へ配置した場合(配置した通知を受け取った場合))
        ChangeGameState(GameState.DraftFase);
    }

    
    /// <summary> 結果フェーズ中のUpdate処理 </summary>
    void UpdateResultFase()
    {
        if(!_isResultDisplayed)     //最初の一度だけ実行
        {
            //試合結果、スコアを表示
            GameDebug.Log($"ゲームが終了しました。 {_turnController.CurrentTurn} が勝利しました。");
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

        //ドラフトフェーズへの移行時
        if (newGameState == GameState.DraftFase)
        {
            _collideBalls.RemoveObjectBallNum();        //

            if (!_hadPocketAnyBall)         //なにもポケットに入らなかった場合(ファールも起きず、合法的にポケットに的球を落としすらしなかった場合)
            {
                _turnController.ChangeTurn();
            }
        }
        else if (newGameState == GameState.FoulFase)
        {
            _turnController.ChangeTurn();
        }
        else if (newGameState == GameState.ShotFase)
        {
            _collideBalls.CheckMinObjectBallNum();      //存在する的球の中の最小値を求める
        }

        _hadFoul = false;
        _isShotted = false;
        _hadPocketAnyBall = false;
        _isGameClear = false;
        _timer = 0f;
        _collideBalls.ResetCollideObjectBallFlag();      //最小値の的球に衝突したかどうかのフラグをリセット
        _collideBalls.ResetMemoryOfPocketBalls();        //ポケットに落ちたボールの記憶をリセット
    }


    /// <summary> ゲームクリア条件を満たしたらに呼ばれ、クリアフラグを立てるメソッド </summary>
    public void MeetConditionOfGameClear()
    {
        _isGameClear = true;
    }

    /// <summary> 何かしらのボールがポケットに落ちたことを通知するメソッド </summary>
    public void NotifyPocketedAnyBall()
    {
        _hadPocketAnyBall = true;
    }

    /// <summary> ショットしたら呼ばれ、ショット済みフラグを立てるメソッド </summary>
    public void NotifyShotted()
    {
        _isShotted = true;
    }

    /// <summary> ファールしたら、ファールフラグを立てるメソッド </summary>
    public void NotifyFouled()
    {
        _hadFoul = true;
    }

    /// <summary> 全ての球が停止したら呼ばれ、停止済みフラグを立てるメソッド </summary>
    public void NotifyAllBallsHadStop()
    {
        _hadAllBallsStop = true;
    }
}
