namespace Yajima
{
    /// <summary>ステートのインターフェース</summary>
    public interface IPhase
    {
        /// <summary>自身のフェーズの種類を返すプロパティ</summary>
        PhaseType OwnState { get; }

        /// <summary>ステートに入った時に呼ばれるメソッド</summary>
        void Enter();

        /// <summary>毎フレーム呼ばれるメソッド</summary>
        /// <param name="deltaTime">前フレームからの経過時間</param>
        /// <returns>次のフェーズの種類</returns>
        PhaseType Update(float deltaTime);

        /// <summary>ステートから出る時に呼ばれるメソッド</summary>
        void Exit();
    }
}
