using System;
using FishNet;
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

    /// <summary>
    /// ホストとして部屋を立てる
    /// </summary>
    public void StartRoom()
    {
        Login(() =>
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();

            var eosManager = EOSManager.Instance;
            if (eosManager != null)
            {
                string puid = eosManager.GetProductUserId().ToString();
                GameDebug.Log($"PUID: {puid}");
            }
        });
    }

    /// <summary>
    /// EOSにログインする
    /// </summary>
    /// <param name="whenLogined">ログイン成功後の処理</param>
    private void Login(Action whenLogined)
    {
        var eosManager = EOSManager.Instance;
        if(eosManager.GetProductUserId() != null)
        {
            whenLogined();
        }
        else
        {
            eosManager.StartConnectLoginWithDeviceToken(
                $"Player_{UnityEngine.Random.Range(1000, 9999)}",
                loginInfo =>
                {
                    if (loginInfo.ResultCode == Epic.OnlineServices.Result.Success)
                    {
                        Debug.Log("ログインに成功しました。");
                        whenLogined();
                    }
                    else
                    {
                        Debug.LogError($"ログインに失敗しました: {loginInfo.ResultCode}");
                    }
                }
            );
        }
    }
}
