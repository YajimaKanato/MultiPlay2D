using System;
using UnityEngine;

public class PocketDetector : MonoBehaviour
{
    //球がポケットに入ったことを通知するイベント
    public static event Action<GameObject> OnBallPocketed;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Ball"))
        {
            GameDebug.Log("ボールが接触しました");
            OnBallPocketed?.Invoke(other.gameObject);
        }
        
    }
}
