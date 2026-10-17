using System.Collections.Generic;
using Epic.OnlineServices.Sessions;

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
        FishNetRoomAttributeManager.SetSessionAttribute(stackAttibutes);
    }
}