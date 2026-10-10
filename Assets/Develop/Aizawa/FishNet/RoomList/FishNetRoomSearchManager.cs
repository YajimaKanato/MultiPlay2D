using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Sessions;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;

/// <summary>
/// セッション検索を管理する静的クラス
/// </summary>
public static class FishNetRoomSearchManager
{
    //衝突回避のため、FishNet検証終了後にFishNetCallbackへ移動
    /// <summary>
    /// セッション検索が完了した際のコールバック
    /// </summary>
    /// <param name="info">セッション検索処理結果</param>
    public static void OnSessionFind(ref SessionSearchFindCallbackInfo info)
    {
        if(info.ResultCode == Result.Success)
        {
            Debug.Log("セッション検索に成功しました。");

            var resultCountOptions = new SessionSearchGetSearchResultCountOptions();
            var findCount = FishNetHandles.SessionSearchHandle.GetHandle().GetSearchResultCount(ref resultCountOptions);

            _findedSessions.Clear();

            if(findCount == 0)
            {
                Debug.LogError("セッションが見つかりませんでした。");
                FishNetHandles.SessionSearchHandle.Release();
                return;
            }

            var sessionDetails = FishNetHandles.SessionDetailsHandle;

            if(sessionDetails.GetHandle() != null)
            {
                for(int i = 0; i < findCount; i++)
                {
                    sessionDetails.SetOptions(new(){ SessionIndex = (uint)i });
                    _findedSessions.Add(new(sessionDetails.GetHandle()));
                    sessionDetails.Release();
                }

                Debug.Log("属性読み取りに成功しました。");
            }
        }
        else
        {
            Debug.LogError($"セッション検索に失敗しました。 {info.ResultCode}");
        }

        FishNetHandles.SessionSearchHandle.Release();
    }

    private static List<SessionAttributes> _findedSessions = new();
    public static List<SessionAttributes> FindedSessions => _findedSessions;

    /// <summary>
    /// セッション検索を行う
    /// </summary>
    public static void SearchSession()
    {
        var sessionSearchHandle = FishNetHandles.SessionSearchHandle;

        if(sessionSearchHandle.GetHandle() != null)
        {
            AddSearchParameter(sessionSearchHandle.GetHandle(), new(){ Key = AttributeTypes.RoomName.ToString(), Value = "NewRoom" });

            var findOptions = new SessionSearchFindOptions
            {
                LocalUserId = EOSManager.Instance.GetProductUserId()
            };

            sessionSearchHandle.GetHandle().Find(ref findOptions, null, OnSessionFind);
        }
    }

    /// <summary>
    /// セッション検索条件を追加する
    /// </summary>
    /// <param name="sessionSearchHandle">検索用ハンドル</param>
    /// <param name="attribute">照合するデータ</param>
    private static void AddSearchParameter(SessionSearch sessionSearchHandle, AttributeData attribute)
    {
        var setSessionParameterOptions = new SessionSearchSetParameterOptions
        {
            Parameter = attribute,
            ComparisonOp = ComparisonOp.Equal
        };

        Result result = sessionSearchHandle.SetParameter(ref setSessionParameterOptions);

        if (result == Result.Success)
        {

            Debug.Log("検索条件の設定に成功");
        }
        else
        {
            Debug.LogError($"検索条件の設定に失敗: {result}");
        }
    }

    /// <summary>
    /// 検索対象のセッションIDを指定する。
    /// </summary>
    /// <param name="sessionSearchHandle">検索用ハンドル</param>
    private static void SetSearchId(SessionSearch sessionSearchHandle)
    {
        var setSessionIdOptions = new SessionSearchSetSessionIdOptions
        {
            SessionId = GetSessionInfo()?.SessionId
        };

        Result result = sessionSearchHandle.SetSessionId(ref setSessionIdOptions);

        if (result == Result.Success)
        {

            Debug.Log("SessionIDの設定に成功");
        }
        else
        {
            Debug.LogError($"SessionIDの設定に失敗: {result}");
        }
    }

    /// <summary>
    /// 現在いるセッションの情報を取得する
    /// </summary>
    /// <returns>セッション情報</returns>
    public static SessionDetailsInfo? GetSessionInfo()
    {
        if(EOSManager.Instance.GetProductId() == null)
        {
            Debug.LogError("ログインしてください。");
            return null;
        }

        var sessionHandle = FishNetHandles.ActiveSessionHandle;

        if(sessionHandle.GetHandle() != null)
        {
            var copySessionDetailsOptions = new ActiveSessionCopyInfoOptions();
            Result detailsResult = sessionHandle.GetHandle().CopyInfo(ref copySessionDetailsOptions, out ActiveSessionInfo? sessionDetails);

            if (detailsResult == Result.Success && sessionDetails != null)
            {
                Debug.Log("アクティブセッションの取得に成功しました。");

                return sessionDetails.Value.SessionDetails.Value;
            }
            else
            {
                Debug.LogError("SessionDetailsInfoの取得に失敗しました。");
            }

            sessionHandle.Release();
        }

        return null;
    }

    /// <summary>
    /// セッションから読み取ったデータの保管用構造体
    /// </summary>
    public struct SessionAttributes
    {
        public Dictionary<AttributeTypes, AttributeDataValue> attributes;

        public SessionAttributes(SessionDetails sessionDetails)
        {
            attributes = new();

            foreach(AttributeTypes type in Enum.GetValues(typeof(AttributeTypes)))
            {
                var copySessionAttributeByKeyOptions = new SessionDetailsCopySessionAttributeByKeyOptions
                {
                    AttrKey = type.ToString()
                };
                
                Result result = sessionDetails.CopySessionAttributeByKey(ref copySessionAttributeByKeyOptions, out SessionDetailsAttribute? attribute);

                if(result == Result.Success && attribute != null)
                {
                    attributes.Add(type, attribute.Value.Data.Value.Value);
                }
                else
                {
                    Debug.LogError($"{type}の読み込みに失敗しました。");
                }
            }
        }
    }
}
