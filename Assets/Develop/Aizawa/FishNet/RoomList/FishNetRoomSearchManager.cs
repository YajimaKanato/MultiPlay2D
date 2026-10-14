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

            //検索結果の数を取得
            var resultCountOptions = new SessionSearchGetSearchResultCountOptions();
            var findCount = FishNetHandles.SessionSearchHandle.GetHandle().GetSearchResultCount(ref resultCountOptions);

            _findedSessions.Clear();

            //結果が0ならSessionDetailsHandleを取得できないためメソッドを抜ける
            if(findCount == 0)
            {
                Debug.Log("セッションが見つかりませんでした。");
                FishNetHandles.SessionSearchHandle.Release();
                return;
            }

            var sessionDetails = FishNetHandles.SessionDetailsHandle;

            if(sessionDetails.GetHandle() != null)
            {
                //SessionDetailsからデータ内容を抜き出し、専用の構造体に変換してリストに保管
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
        //検索用のハンドルを取得
        //コールバックを受け取れなくなるため、コールバック内で解放を行う
        var sessionSearchHandle = FishNetHandles.SessionSearchHandle;

        if(sessionSearchHandle.GetHandle() != null)
        {
            //検索条件の設定
            AddSearchParameter(sessionSearchHandle.GetHandle(), new(){ Key = AttributeTypes.RoomName.ToString(), Value = "NewRoom" });

            var findOptions = new SessionSearchFindOptions
            {
                LocalUserId = EOSManager.Instance.GetProductUserId()
            };

            //セッション検索を行い、コールバックの受け取り先を指定
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
        //条件として指定するデータを設定に変換
        var setSessionParameterOptions = new SessionSearchSetParameterOptions
        {
            Parameter = attribute,
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

    /// <summary>
    /// セッションから読み取ったデータの保管用構造体
    /// </summary>
    public struct SessionAttributes
    {
        public Dictionary<AttributeTypes, AttributeDataValue> attributes;

        public SessionAttributes(SessionDetails sessionDetails)
        {
            attributes = new();

            //各データを取り出してディクショナリに保管
            foreach(AttributeTypes type in Enum.GetValues(typeof(AttributeTypes)))
            {
                //enumをキーに変換
                var copySessionAttributeByKeyOptions = new SessionDetailsCopySessionAttributeByKeyOptions
                {
                    AttrKey = type.ToString()
                };
                
                //セッション情報から特定のキーのデータを読み取る
                Result result = sessionDetails.CopySessionAttributeByKey(ref copySessionAttributeByKeyOptions, out SessionDetailsAttribute? attribute);

                if(result == Result.Success && attribute != null)
                {
                    //実際に扱う値のみ読み取ってディクショナリに追加
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
