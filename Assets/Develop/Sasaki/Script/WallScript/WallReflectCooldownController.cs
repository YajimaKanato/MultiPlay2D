using UnityEngine;

public class WallReflectCooldownController : MonoBehaviour
{
    [Header("反射設定")]
    [Tooltip("反射可否の判定処理")]
    [SerializeField] private bool _canReflect = true;
    private const float _reflectCoolDownTime = 0.05f;　//次の反射までのクールダウン時間 
    private float _reflectCoolDownTimer;

    public bool CanReflect => _canReflect;

    void Update()
    {
        UpdateReflectCoolDownTime();
    }
    /// <summary>
    /// 球の反射クールダウン処理
    /// </summary>
    private void UpdateReflectCoolDownTime()
    {
        if (_canReflect) return;

        _reflectCoolDownTimer -= Time.deltaTime;

        if (_reflectCoolDownTimer < 0)
        {
            _canReflect = true;
        }
    }

    /// <summary>
    /// 壁の衝突判定
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && _canReflect)
        {
            _canReflect = false;
            _reflectCoolDownTimer = _reflectCoolDownTime;
        }
    }
}
