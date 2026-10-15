using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachineGenerator : MonoBehaviour
{
    [SerializeField] private GameState _startState = GameState.ReadyPhase;

    private GameStateMachine _stateMachine;

    private readonly Dictionary<GameState, IGameState> _cacheStates = new();

    /// <summary>ステートマシンを取得する</summary>
    public GameStateMachine StateMachine => _stateMachine;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        _stateMachine?.Update();
    }

    /// <summary>ステートマシンを初期化する</summary>
    private void Initialize()
    {
        _stateMachine = new GameStateMachine();

        CacheStates();

        if (!_cacheStates.TryGetValue(_startState, out IGameState startPhase))
        {
            Debug.LogError(
                $"開始ステート {_startState} が登録されていません。");
            return;
        }

        _stateMachine.ChangeState(startPhase);
    }

    /// <summary>各ステートを生成してキャッシュする</summary>
    private void CacheStates()
    {
        _cacheStates.Clear();

        //RegisterState(
        //    GameState.DraftPhase,
        //    new DraftPhase(_stateMachine, ));

        //RegisterState(
        //    GameState.ShotPhase,
        //    new ShotPhase(_stateMachine, ));
    }

    /// <summary>ステートを登録する</summary>
    private void RegisterState(GameState type, IGameState state)
    {
        if (!_cacheStates.TryAdd(type, state))
        {
            Debug.LogError($"ステート {type} は既に登録されています。");
        }
    }

    /// <summary>登録済みのステートを取得する</summary>
    public IGameState GetState(GameState type)
    {
        if (_cacheStates.TryGetValue(type, out IGameState state))
        {
            return state;
        }

        Debug.LogError($"ステート {type} が登録されていません。");
        return null;
    }

    
}
