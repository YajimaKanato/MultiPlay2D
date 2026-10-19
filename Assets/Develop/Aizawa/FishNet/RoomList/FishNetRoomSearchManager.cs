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
    public static void OnSessionFound(ref SessionSearchFindCallbackInfo info)
    {
        if(info.ResultCode == Result.Success)
        {
            Debug.Log("セッション検索に成功しました。");

            //検索結果の数を取得
            var resultCountOptions = new SessionSearchGetSearchResultCountOptions();
            var foundCount = FishNetHandles.SessionSearchHandle.GetHandle().GetSearchResultCount(ref resultCountOptions);

            _foundSessions.Clear();

            //結果が0ならSessionDetailsHandleを取得できないためメソッドを抜ける
            if(foundCount == 0)
            {
                Debug.Log("セッションが見つかりませんでした。");
                FishNetHandles.SessionSearchHandle.Release();
                return;
            }

            //SessionDetailsからデータ内容を抜き出し、専用の構造体に変換してリストに保管
            for(int i = 0; i < foundCount; i++)
            {
                //インデックスi番の検索結果ハンドルを取得
                var sessionDetails = FishNetHandles.SessionDetailsHandle;
                sessionDetails.SetOptions(new(){ SessionIndex = (uint)i });

                if(sessionDetails.GetHandle() != null)
                {
                    //ハンドルをずっと保持しているとメモリリークの恐れがあるため、
                    //内容だけを読み取って専用構造体に変換、ハンドル自体は即時開放する
                    _foundSessions.Add(new(sessionDetails.GetHandle()));
                    sessionDetails.Release();
                }
            }

            Debug.Log($"セッションを{foundCount}件見つけました。");
        }
        else
        {
            Debug.LogError($"セッション検索に失敗しました。 {info.ResultCode}");
        }

        //検索が完了したためハンドルを開放する
        FishNetHandles.SessionSearchHandle.Release();
    }

    private static List<SessionAttributes> _foundSessions = new();
    /// <summary> セッション検索結果一覧 </summary>
    public static List<SessionAttributes> FoundSessions => _foundSessions;

    /// <summary>
    /// セッション検索を行う
    /// </summary>
    public static void SearchSession()
    {
        //検索用のハンドルを取得
        //コールバックを受け取れなくなるため、コールバック内で解放を行う
        var sessionSearchHandle = FishNetHandles.SessionSearchHandle;

        if(sessionSearchHandle.GetHandle() != null)
        {
            //検索条件の設定
            AddSearchParameter(sessionSearchHandle.GetHandle(), new(){ Key = AttributeTypes.RoomName.ToString(), Value = "NewRoom" });

            //検索元を自身に設定
            var findOptions = new SessionSearchFindOptions
            {
                LocalUserId = EOSManager.Instance.GetProductUserId()
            };

            //セッション検索を行い、コールバックの受け取り先をOnSessionFoundに指定
            sessionSearchHandle.GetHandle().Find(ref findOptions, null, OnSessionFound);
        }
    }

    /// <summary>
    /// セッション検索条件を追加する
    /// </summary>
    /// <param name="sessionSearchHandle">検索用ハンドル</param>
    /// <param name="attribute">照合するデータ</param>
    private static void AddSearchParameter(SessionSearch sessionSearchHandle, AttributeData attribute)
    {
        //条件として指定するデータを設定に変換
        var setSessionParameterOptions = new SessionSearchSetParameterOptions
        {
            Parameter = attribute,
            //条件に対してどのような状態のセッションを認めるか
            //Equalの場合は完全一致
            ComparisonOp = ComparisonOp.Equal
        };

        //ハンドルに検索条件を入力
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
}
