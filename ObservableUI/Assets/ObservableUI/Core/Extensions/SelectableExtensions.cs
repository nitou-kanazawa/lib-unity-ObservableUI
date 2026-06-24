using System;
using UnityEngine.UI;

namespace R3
{
    /// <summary>
    /// <see cref="Selectable"/>（Button・Toggle等の基底）の拡張メソッド群．
    /// </summary>
    public static partial class SelectableExtensions
    {
        #region Binding

        /// <summary>
        /// <see cref="Selectable.interactable"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToInteractable(this Observable<bool> source, Selectable selectable)
        {
            return source.Subscribe(x => selectable.interactable = x);
        }

        #endregion
    }
}
