using UnityEngine;

/// <summary>
/// 壁の反射処理
/// </summary>
public class WallController : MonoBehaviour
{
    /// <summary>
    /// 衝突した球の判定、反射の計算処理
    /// </summary>
    /// <param name="collision">壁に衝突した球</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Rigidbody rigidbody = collision.rigidbody;
        BallBase ballBase = collision.gameObject.GetComponent<BallBase>();
        WallReflectCooldownController reflectController = collision.gameObject.GetComponent<WallReflectCooldownController>();

        if (rigidbody == null || ballBase == null || reflectController == null
            || reflectController.CanReflect == false) return;

        Vector3 velocity = rigidbody.linearVelocity;

        // 衝突点の法線（壁の向き）
        Vector3 wallNormal = collision.contacts[0].normal;

        // 完全反射
        velocity = Vector3.Reflect(velocity, wallNormal);

        ballBase.UpdateMoveVelocity(velocity);
    }
}
