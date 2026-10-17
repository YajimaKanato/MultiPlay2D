using System;
using UnityEngine;

namespace Yajima
{
    /// <summary>
    /// <para>ルールの種類を定義した列挙型</para>
    /// <para>0オリジンとしてインデックス指定に使えるようする</para>
    /// </summary>
    public enum RuleType : Byte
    {
        [InspectorName("ナインボール")] NineBall = 0,
        [InspectorName("隠し球")] SecretBall = 1
    }
}