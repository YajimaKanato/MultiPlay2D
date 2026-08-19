using UnityEngine;


public class WallController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 衝突したボールの反射処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        //BallBase ball = collision.gameObject.GetComponent<BallBase>();

        //if (ball == null) return;

        // 衝突点の法線（壁の向き）
        Vector3 wallNormal = collision.contacts[0].normal;

        // 球に反射ベクトルを渡す
        //CalculateReflectionVelocity(wallNormal, collision.gameObject.);
    }

    /// <summary>
    /// 壁反射処理と壁の速度更新処理
    /// </summary>
    /// <param name="wallNormal"></param>
    /// <param name="addForce"></param>
    private void CalculateReflectionVelocity(Vector3 wallNormal)
    {
        //if (!_canReflect) return;

        //// 現在の速度を内部変数に統一
        //_velocity = _rb.linearVelocity;
        //Debug.Log(_velocity);

        //// 完全反射
        //_velocity = Vector3.Reflect(_velocity, wallNormal);

        // Rigidbody に反映

    }
}
