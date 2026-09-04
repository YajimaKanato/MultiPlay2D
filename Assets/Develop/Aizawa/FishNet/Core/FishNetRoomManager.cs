using Epic.OnlineServices.Auth;
using FishNet;
using FishNet.Transporting.FishyEOSPlugin;
using ParrelSync;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;

/// <summary>
/// FishNetによるネットワーク接続を管理するコンポーネント
/// </summary>
public class FishNetRoomManager : MonoBehaviour
{
    void Awake()
    {
        if(FindObjectsByType<FishNetRoomManager>(FindObjectsSortMode.None).Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        if(ClonesManager.IsClone())
        {
            LoginByDeviceID();
        }
        else
        {
            LoginByEpicAccount();
        }
    }

    /// <summary>
    /// エピックアカウントなしでEOSへのログインを行う
    /// </summary>
    private async void LoginByDeviceID()
    {   
        EOSManager.Instance.StartConnectLoginWithDeviceToken(
            "Player2",
            FishNetCallback.OnConnectLoginComplete
        );
    }

    /// <summary>
    /// エピックアカウントを使用してEOSへのログインを行う
    /// </summary>
    private void LoginByEpicAccount()
    {
        EOSManager.Instance.StartLoginWithLoginTypeAndToken(
            LoginCredentialType.AccountPortal,
            null,
            "Player1",
            FishNetCallback.OnAuthLoginComplete
        );
    }

    /// <summary>
    /// ホストとして部屋を立てる
    /// </summary>
    public void StartRoom()
    {
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();

        InstanceFinder.ClientManager.OnClientConnectionState += FishNetCallback.OnChangeClientConnectionState;
    }

    /// <summary>
    /// クライアントとして部屋に合流する
    /// </summary>
    /// <param name="puid">P2P接続先のプレイヤーのPUID</param>
    public void JoinRoom(string puid)
    {
        if (InstanceFinder.NetworkManager.TransportManager.Transport is FishyEOS transport)
        {
            transport.RemoteProductUserId = puid;
            InstanceFinder.ClientManager.StartConnection();

            InstanceFinder.ClientManager.OnClientConnectionState += FishNetCallback.OnChangeClientConnectionState;
        }
        else
        {
            GameDebug.LogError("TransportがFishyEOSではありません。");
        }
    }
}
