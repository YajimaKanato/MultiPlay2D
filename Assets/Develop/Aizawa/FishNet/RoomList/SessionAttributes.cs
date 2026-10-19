using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Sessions;
using UnityEngine;

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