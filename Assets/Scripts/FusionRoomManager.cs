using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;
using System.Collections.Generic;

public class FusionRoomManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] int _maxPlayerCount = 4;
    [SerializeField] SceneIndex _startSceneIndex = 0;

    NetworkRunner _runner;
    public NetworkRunner Runner => _runner;

    static FusionRoomManager _instance;
    public static FusionRoomManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void CreateRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            GameDebug.LogError("部屋の名前を指定してください");
            return;
        }

        CreateRunner();

        var startScene = new NetworkSceneInfo();
        startScene.AddSceneRef(SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex), LoadSceneMode.Single);

        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = roomName,
            Scene = startScene,
            PlayerCount = _maxPlayerCount,
            SceneManager = _runner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (!result.Ok)
        {
            GameDebug.LogError($"ルームの作成に失敗しました: {result.ShutdownReason}");
            return;
        }

        GameDebug.Log($"ルームを作成しました: {roomName}");
        await _runner.LoadScene(SceneRef.FromIndex((int)SceneIndex.Lobby));
    }

    public async void SearchRoom()
    {
        CreateRunner();

        var result = await _runner.JoinSessionLobby(SessionLobby.ClientServer);

        if (!result.Ok)
        {
            GameDebug.LogError($"ルームの検索に失敗しました: {result.ShutdownReason}");
            return;
        }

        GameDebug.Log("ルームの検索に成功しました");
    }

    public async void JoinRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            GameDebug.LogError("部屋の名前を指定してください");
            return;
        }

        CreateRunner();

        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = roomName,
            SceneManager = _runner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (!result.Ok)
        {
            GameDebug.LogError($"ルームへの参加に失敗しました: {result.ShutdownReason}");
            return;
        }

        GameDebug.Log($"ルームに参加しました: {roomName}");
    }

    public async void StartGame()
    {
        if (_runner == null || !_runner.IsRunning)
        {
            GameDebug.LogError("Fusionに接続されていません");
            return;
        }

        if (!_runner.IsServer)
        {
            GameDebug.LogError("ゲームを開始できるのはホストのみです");
            return;
        }

        await _runner.LoadScene(SceneRef.FromIndex((int)SceneIndex.InGame));
    }

    public async void LeaveRoom()
    {
        if (_runner == null || !_runner.IsRunning)
        {
            GameDebug.LogError("Fusionに接続されていません");
            return;
        }

        await _runner.Shutdown();
    }
    
    void CreateRunner()
    {
        if (_runner != null)
            return;

        var runnerObject = new GameObject("NetworkRunner");
        runnerObject.transform.SetParent(transform);

        _runner = runnerObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);

        runnerObject.AddComponent<NetworkSceneManagerDefault>();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        GameDebug.Log($"プレイヤーが参加しました : {player}\n現在のプレイヤー数 : {runner.ActivePlayers.Count()}");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        GameDebug.Log($"プレイヤーが退出しました : {player}\n現在のプレイヤー数 : {runner.ActivePlayers.Count()}");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        GameDebug.Log($"Fusionから切断されました : {shutdownReason}");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        GameDebug.Log($"ルームリストが更新されました : {sessionList.Count}件のルームが見つかりました");
    }

#region Fusionのコールバックたち
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        // throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        // throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        // throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        // throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        // throw new NotImplementedException();
    }
    #endregion
}
