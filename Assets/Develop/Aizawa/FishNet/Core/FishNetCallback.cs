using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using FishNet;
using FishNet.Transporting;
using PlayEveryWare.EpicOnlineServices;

/// <summary>
/// <para>FishNetにおけるコールバック処理をまとめたクラス</para>
/// <para>引数にrefを求められる場合がありラムダ式で対応しきれなかったため総じて分割しました</para>
/// </summary>
public class FishNetCallback
{
    /// <summary>
    /// エピックアカウントなしでのログイン処理が終了した際のコールバック
    /// </summary>
    /// <param name="callbackInfo">ログイン処理結果</param>
    public static void OnConnectLoginComplete(LoginCallbackInfo callbackInfo)
    {
        if (callbackInfo.ResultCode == Result.Success)
        {
            var puid = callbackInfo.LocalUserId;
            GameDebug.Log($"ログイン成功。PUID: {puid}");
        }
        else
        {
            GameDebug.LogError($"ログイン失敗: {callbackInfo.ResultCode}");
        }
    }

    /// <summary>
    /// エピックアカウントでのログイン処理が終了した際のコールバック
    /// </summary>
    /// <param name="callbackInfo">ログイン処理結果</param>
    public static void OnAuthLoginComplete(Epic.OnlineServices.Auth.LoginCallbackInfo callbackInfo)
    {
        if (callbackInfo.ResultCode == Result.Success)
        {
            GameDebug.Log($"認証成功");

            EOSManager.Instance.StartConnectLoginWithEpicAccount(
                callbackInfo.LocalUserId,
                OnConnectLoginComplete
            );
        }
        else
        {
            GameDebug.LogError($"認証失敗: {callbackInfo.ResultCode}");
        }
    }

    /// <summary>
    /// セッションへの接続処理が終了した際のコールバック
    /// </summary>
    /// <param name="connectionStateArgs">セッション接続処理結果</param>
    public static void OnChangeClientConnectionState(ClientConnectionStateArgs connectionStateArgs)
    {
        if(connectionStateArgs.ConnectionState == LocalConnectionState.Started)
        {
            GameDebug.Log("ルームに接続しました。");
        }
        else
        {
            GameDebug.LogError("ルーム接続に失敗しました。");
        }

        InstanceFinder.ClientManager.OnClientConnectionState -= OnChangeClientConnectionState;
    }
}
