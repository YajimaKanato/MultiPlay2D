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
    private static bool _attributeAdded;

    //衝突回避のため、FishNet検証終了後にFishNetCallbackへ移動
    /// <summary>
    /// セッション情報の更新が完了した際のコールバック
    /// </summary>
    /// <param name="info">セッション更新処理結果</param>
    public static void OnUpdateSession(ref UpdateSessionCallbackInfo info)
    {
        if(info.ResultCode == Result.Success)
        {
            _attributeAdded = true;
            Debug.Log($"セッションを更新しました。");
        }
        else
        {
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
        if(FishNetRoomSearchManager.FindedSessions.Count <= index)
        {
            Debug.LogError($"セッションリストにインデックス{index}番は存在しません。");
            return new();
        }

        return FishNetRoomSearchManager.FindedSessions[index].attributes[attribute];
    }

    /// <summary>
    /// データ埋め込み用の構造体の取得
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
        if(!_attributeAdded)
        {
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

    /// <summary>
    /// セッションに埋め込むデータの保管用構造体
    /// </summary>
    public struct AttributeStack
    {
        public List<AttributeData> stackAttibutes;

        /// <summary>
        /// 文字列型のデータを生成する
        /// </summary>
        /// <param name="attribute">データの種類</param>
        /// <param name="value">データ内容</param>
        /// <returns>メソッドチェーン用自己返却</returns>
        public AttributeStack AddStringAttribute(AttributeTypes attribute, string value)
        {
            stackAttibutes ??= new();

            stackAttibutes.Add(
                new AttributeData
                {
                    Key = attribute.ToString(),
                    Value = new AttributeDataValue
                    {
                        AsUtf8 = value,
                    }
                }
            );

            return this;
        }

        /// <summary>
        /// long型のデータを生成する
        /// </summary>
        /// <param name="attribute">データの種類</param>
        /// <param name="value">データ内容</param>
        /// <returns>メソッドチェーン用自己返却</returns>
        public AttributeStack AddLongAttribute(AttributeTypes attribute, long value)
        {
            stackAttibutes ??= new();

            stackAttibutes.Add(
                new AttributeData
                {
                    Key = attribute.ToString(),
                    Value = new AttributeDataValue
                    {
                        AsInt64 = value,
                    }
                }
            );

            return this;
        }

        /// <summary>
        /// bool型のデータを生成する
        /// </summary>
        /// <param name="attribute">データの種類</param>
        /// <param name="value">データ内容</param>
        /// <returns>メソッドチェーン用自己返却</returns>
        public AttributeStack AddBoolAttribute(AttributeTypes attribute, bool value)
        {
            stackAttibutes ??= new();

            stackAttibutes.Add(
                new AttributeData
                {
                    Key = attribute.ToString(),
                    Value = new AttributeDataValue
                    {
                        AsBool = value,
                    }
                }
            );

            return this;
        }

        /// <summary>
        /// 溜めたデータを全てセッションに埋め込む
        /// </summary>
        public void SetAttributes()
        {
            SetSessionAttribute(stackAttibutes);
        }
    }
}
