using System;
using UnityEngine;

namespace Yajima
{
    /// <summary>
    /// <para>ラックの位置番号を表す列挙型</para>
    /// <para>格段左から数える</para>
    /// <para>0オリジンとしてインデックス指定に使えるようにする</para>
    /// </summary>
    public enum RackPositionNumber : Byte
    {
        /// <summary>ラックの位置番号1</summary>
        [InspectorName("三角形のトップ")] Position1 = 0,

        /// <summary>ラックの位置番号2</summary>
        [InspectorName("２段目左")] Position2 = 1,

        /// <summary>ラックの位置番号3</summary>
        [InspectorName("２段目右")] Position3 = 2,

        /// <summary>ラックの位置番号4</summary>
        [InspectorName("３段目左")]Position4 = 3,

        /// <summary>ラックの位置番号5</summary>
        [InspectorName("３段目真ん中")]Position5 = 4,

        /// <summary>ラックの位置番号6</summary>
        [InspectorName("３段目右")]Position6 = 5,

        /// <summary>ラックの位置番号7</summary>
        [InspectorName("４段目左端")]Position7 = 6,

        /// <summary>ラックの位置番号8</summary>
        [InspectorName("４段目左寄り")]Position8 = 7,

        /// <summary>ラックの位置番号9</summary>
        [InspectorName("４段目右寄り")]Position9 = 8,

        /// <summary>ラックの位置番号10</summary>
        [InspectorName("４段目右端")]Position10 = 9,

        /// <summary>ラックの位置番号11</summary>
        [InspectorName("５段目左端")]Position11 = 10,

        /// <summary>ラックの位置番号12</summary>
        [InspectorName("５段目左寄り")]Position12 = 11,

        /// <summary>ラックの位置番号13</summary>
        [InspectorName("５段目中央")]Position13 = 12,

        /// <summary>ラックの位置番号14</summary>
        [InspectorName("５段目右寄り")]Position14 = 13,

        /// <summary>ラックの位置番号15</summary>
        [InspectorName("５段目右端")]Position15 = 14
    }
}
