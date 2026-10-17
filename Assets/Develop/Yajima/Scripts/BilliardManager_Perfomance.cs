using UnityEngine;

namespace Yajima
{
    // 演出面のメソッドをこのファイルに書く
    // staticインスタンス経由で呼び出される想定
    public partial class BilliardManager
    {
        public void ResetRack()
        {
            _tableManager?.CreateRack();
        }
    }
}