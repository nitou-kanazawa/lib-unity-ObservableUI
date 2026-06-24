using System;
using UnityEngine;

namespace R3
{
    /// <summary>
    /// <see cref="CanvasGroup"/>の拡張メソッド群．
    /// </summary>
    public static partial class CanvasGroupExtensions
    {
        #region Binding

        /// <summary>
        /// <see cref="CanvasGroup.alpha"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToAlpha(this Observable<float> source, CanvasGroup canvasGroup)
        {
            return source.Subscribe(x => canvasGroup.alpha = x);
        }

        /// <summary>
        /// <see cref="CanvasGroup.interactable"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToInteractable(this Observable<bool> source, CanvasGroup canvasGroup)
        {
            return source.Subscribe(x => canvasGroup.interactable = x);
        }

        /// <summary>
        /// <see cref="CanvasGroup.blocksRaycasts"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToBlocksRaycasts(this Observable<bool> source, CanvasGroup canvasGroup)
        {
            return source.Subscribe(x => canvasGroup.blocksRaycasts = x);
        }

        /// <summary>
        /// 表示状態（<see cref="CanvasGroup.alpha"/>／<see cref="CanvasGroup.interactable"/>／
        /// <see cref="CanvasGroup.blocksRaycasts"/>）をまとめて切り替える単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToVisible(this Observable<bool> source, CanvasGroup canvasGroup)
        {
            return source.Subscribe(x =>
            {
                canvasGroup.alpha = x ? 1f : 0f;
                canvasGroup.interactable = x;
                canvasGroup.blocksRaycasts = x;
            });
        }

        #endregion
    }
}
