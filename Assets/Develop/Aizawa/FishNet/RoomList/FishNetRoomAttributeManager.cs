using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Sessions;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;

/// <summary>
/// セッションの埋め込みデータを管理する静的クラス
/// </summary>
public static class FishNetRoomAttributeManager
{
    private static AttributeInitializeState _attributeInitializeState;
    /// <summary> セッションデータの埋め込み進行状況 </summary>
    public static AttributeInitializeState AttributeInitializeState => _attributeInitializeState;

    //衝突回避のため、FishNet検証終了後にFishNetCallbackへ移動
    /// <summary>
    /// セッション情報の更新が完了した際のコールバック
    /// </summary>
    /// <param name="info">セッション更新処理結果</param>
    public static void OnUpdateSession(ref UpdateSessionCallbackInfo info)
    {
        //セッション情報の更新が無事に完了したか確認
        if(info.ResultCode == Result.Success)
        {
            _attributeInitializeState = AttributeInitializeState.Done;
            Debug.Log($"セッションを更新しました。");
        }
        else
        {
            _attributeInitializeState = AttributeInitializeState.NotDone;
            Debug.LogError("セッションの更新に失敗しました。");
        }
    }

    /// <summary>
    /// セッションから文字列型のデータを読み取る
    /// </summary>
    /// <param name="index">読み取り先のセッションのインデックス</param>
    /// <param name="attribute">読み取るデータの種類</param>
    /// <returns>読み取り結果</returns>
    public static string ReadStringAttribute(int index, AttributeTypes attribute)
    {
        //データをstring(Utf8String)に変換する
        //AttributeDataValueはUtf8型しか対応していない模様
        return ReadAttribute(index, attribute).AsUtf8;
    }

    /// <summary>
    /// セッションからlong型のデータを読み取る
    /// </summary>
    /// <param name="index">読み取り先のセッションのインデックス</param>
    /// <param name="attribute">読み取るデータの種類</param>
    /// <returns>読み取り結果</returns>
    public static long ReadLongAttribute(int index, AttributeTypes attribute)
    {
        //データをlong(Int64)型に変換する
        //null許容型なのでValueを取る
        return ReadAttribute(index, attribute).AsInt64.Value;
    }

    /// <summary>
    /// セッションからbool型のデータを読み取る
    /// </summary>
    /// <param name="index">読み取り先のセッションのインデックス</param>
    /// <param name="attribute">読み取るデータの種類</param>
    /// <returns>読み取り結果</returns>
    public static bool ReadBoolAttribute(int index, AttributeTypes attribute)
    {
        //データをbool型に変換する
        //null許容型なのでValueを取る
        return ReadAttribute(index, attribute).AsBool.Value;
    }

    /// <summary>
    /// セッションからデータを読み取る
    /// </summary>
    /// <param name="index">読み取り先のセッションのインデックス</param>
    /// <param name="attribute">読み取るデータの種類</param>
    /// <returns>読み取り結果</returns>
    private static AttributeDataValue ReadAttribute(int index, AttributeTypes attribute)
    {
        //存在していないインデックスを指定していないか、
        //検索が完了していない状況で読もうとしていないかを同時に確認する
        if(FishNetRoomSearchManager.FoundSessions.Count <= index)
        {
            Debug.LogError($"セッションリストにインデックス{index}番は存在しません。");
            Debug.LogError($"セッション検索が完了していないか、インデックスが不正です。");
            return new();
        }

        //データの値を返す
        //型が専用の特殊なものになっているため、取得後にそれぞれ元の型に戻す必要がある
        return FishNetRoomSearchManager.FoundSessions[index].attributes[attribute];
    }

    /// <summary>
    /// 変更データ内容をまとめて記憶するホルダーを作成
    /// </summary>
    public static AttributeStack GetAttributeStack()
    {
        return new();
    }

    /// <summary>
    /// セッションにデータを埋め込む
    /// </summary>
    /// <param name="attributes">埋め込むデータのリスト</param>
    public static void SetSessionAttribute(List<AttributeData> attributes)
    {
        //UpdateSession後はCreateSessionModificationHandleを取得しようとするとエラーになる
        //よって、部屋立て前に1度だけ初期化として実行する
        if(_attributeInitializeState == AttributeInitializeState.NotDone)
        {
            _attributeInitializeState = AttributeInitializeState.Progress;

            //セッション操作用のインターフェースを取得する
            var sessionsInterface = EOSManager.Instance.GetEOSSessionsInterface();

            //セッション初期化用のハンドルを取得する
            var modificationHandle = FishNetHandles.CreateSessionModificationHandle;

            if (modificationHandle.GetHandle() != null)
            {
                //全データを埋め込む
                attributes.ForEach(attribute =>
                {
                    var addAttributeOptions = new SessionModificationAddAttributeOptions
                    {
                        SessionAttribute = attribute,
                        //Advertiseにしないと外部から参照できなくなる
                        AdvertisementType = SessionAttributeAdvertisementType.Advertise
                    };

                    //ハンドルに変更内容を送る
                    modificationHandle.GetHandle().AddAttribute(ref addAttributeOptions);
                });

                var updateOptions = new UpdateSessionOptions
                {
                    SessionModificationHandle = modificationHandle.GetHandle()
                };

                //変更に基づいてセッション情報を更新する
                sessionsInterface.UpdateSession(ref updateOptions, null, OnUpdateSession);

                modificationHandle.Release();
            }
        }
        else
        {
            Debug.LogError("データは初期化済みです。");
        }
    }
}
