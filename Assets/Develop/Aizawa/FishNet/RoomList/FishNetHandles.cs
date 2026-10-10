using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Sessions;
using FishNet;
using FishNet.Transporting.FishyEOSPlugin;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;

/// <summary>
/// FishNetで要求されるハンドルを管理する静的クラス
/// </summary>
public static class FishNetHandles
{
    /// <summary> セッション設定を変更するハンドル </summary>
    public static HandleHolder<SessionModification, CreateSessionModificationOptions> CreateSessionModificationHandle { get; set; } =
    new((sessionsInterface, options) =>
        {
            var result = sessionsInterface.CreateSessionModification(ref options, out var handle);
            return (result, handle);
        },
        new()
        {
            SessionName = "EOSSession",
            MaxPlayers = 4,
            LocalUserId = EOSManager.Instance.GetProductUserId(),
            BucketId = InstanceFinder.NetworkManager.TransportManager.GetTransport<FishyEOS>().SocketName
        },
        handle => handle.Release()
    );

    /// <summary> セッション設定を変更するハンドル </summary>
    public static HandleHolder<SessionModification, UpdateSessionModificationOptions> UpdateSessionModificationHandle { get; set; } =
    new((sessionsInterface, options) =>
        {
            var result = sessionsInterface.UpdateSessionModification(ref options, out var handle);
            return (result, handle);
        },
        new()
        {
            SessionName = "EOSSession"
        },
        handle => handle.Release()
    );

    /// <summary> セッションを検索するハンドル </summary>
    public static HandleHolder<SessionSearch, CreateSessionSearchOptions> SessionSearchHandle { get; set; } =
    new((sessionsInterface, options) =>
        {
            var result = sessionsInterface.CreateSessionSearch(ref options, out var handle);
            return (result, handle);
        },
        new()
        {
            MaxSearchResults = 50
        },
        handle => handle.Release()
    );

    /// <summary> 検索結果からセッション情報を読み取るハンドル </summary>
    public static HandleHolder<SessionDetails, SessionSearchCopySearchResultByIndexOptions> SessionDetailsHandle { get; set; } =
    new((sessionsInterface, options) =>
        {
            var result = SessionSearchHandle.GetHandle().CopySearchResultByIndex(ref options, out var handle);
            return (result, handle);
        },
        new()
        {
            SessionIndex = 0
        },
        handle => handle.Release()
    );

    /// <summary> 現在のセッションを取得するハンドル </summary>
    public static HandleHolder<ActiveSession, CopyActiveSessionHandleOptions> ActiveSessionHandle { get; set; } =
    new((sessionsInterface, options) =>
        {
            var result = sessionsInterface.CopyActiveSessionHandle(ref options, out var handle);
            return (result, handle);
        },
        new()
        {
            SessionName = "EOSSession"
        },
        handle => handle.Release()
    );

    /// <summary>
    /// <para>各種ハンドルの生成・保持・解放を管理するクラス。</para>
    /// <para>ハンドルとその設定の型を渡す。</para>
    /// </summary>
    /// <typeparam name="THandle">ハンドルの型</typeparam>
    /// <typeparam name="TOptions">ハンドル設定の型</typeparam>
    public class HandleHolder<THandle, TOptions> where THandle : Handle where TOptions : struct
    {
        THandle holdHandle;
        TOptions holdOptions;
        Func<SessionsInterface, TOptions, (Result result, THandle handle)> createHandle;
        Action<THandle> releaseHandle;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="create">ハンドル生成用デリゲート</param>
        /// <param name="options">ハンドル設定</param>
        /// <param name="release">ハンドル解放用デリゲート</param>
        public HandleHolder(Func<SessionsInterface, TOptions, (Result, THandle)> create, TOptions options, Action<THandle> release)
        {
            holdHandle = null;
            holdOptions = options;
            createHandle = create;
            releaseHandle = release;
        }

        /// <summary>
        /// ハンドル取得
        /// </summary>
        /// <returns>保持ハンドル</returns>
        public THandle GetHandle()
        {
            holdHandle ??= SetHandle();

            return holdHandle;
        }

        /// <summary>
        /// ハンドル解放
        /// </summary>
        public void Release()
        {
            releaseHandle(holdHandle);
            holdHandle = null;
        }

        /// <summary>
        /// ハンドルの生成
        /// </summary>
        /// <returns>生成したハンドル</returns>
        private THandle SetHandle()
        {
            var sessionsInterface = EOSManager.Instance.GetEOSSessionsInterface();

            var (result, handle) = createHandle(sessionsInterface, holdOptions);

            if (result == Result.Success)
            {
                return handle;
            }
            else
            {
                Debug.LogError($"{typeof(THandle)}の取得に失敗しました。 {result}");
            }

            return null;
        }

        /// <summary>
        /// ハンドル設定の更新
        /// </summary>
        /// <param name="options">設定内容</param>
        public void SetOptions(TOptions options)
        {
            holdOptions = options;
        }
    }
}
