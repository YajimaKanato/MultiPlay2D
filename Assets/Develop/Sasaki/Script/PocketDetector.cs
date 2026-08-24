using System;
using UnityEngine;

public class PocketDetector : MonoBehaviour
{
    public static event Action<GameObject> OnBallPocketed;

    /// <summary>
    /// 球がポケットに入ったことを通知し、イベントを呼び出す処理
    /// </summary>
    /// <param name="other">落下した球</param>
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Ball"))
        {
            OnBallPocketed?.Invoke(other.gameObject);
        }
        
    }
}
