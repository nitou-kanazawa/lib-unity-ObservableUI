using System;
using UnityEngine;

namespace R3
{
    /// <summary>
    /// <see cref="GameObject"/>の拡張メソッド群．
    /// </summary>
    public static partial class GameObjectExtensions
    {
        #region Binding

        /// <summary>
        /// <see cref="GameObject.SetActive(bool)"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToActive(this Observable<bool> source, GameObject gameObject)
        {
            return source.Subscribe(x => gameObject.SetActive(x));
        }

        #endregion
    }
}
