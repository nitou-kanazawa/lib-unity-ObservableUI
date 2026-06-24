using System;
using UnityEngine;
using UnityEngine.UI;

namespace R3
{
    /// <summary>
    /// <see cref="RawImage"/>の拡張メソッド群．
    /// </summary>
    public static partial class RawImageExtensions
    {
        #region Binding

        /// <summary>
        /// <see cref="RawImage"/>.textureへのバインディング．
        /// </summary>
        public static IDisposable SubscribeToRawImageTexture(this Observable<Texture> source, RawImage rawImage)
        {
            return source.Subscribe(x => rawImage.texture = x);
        }

        /// <summary>
        /// <see cref="RawImage"/>.colorへのバインディング．
        /// </summary>
        public static IDisposable SubscribeToRawImageColor(this Observable<Color> source, RawImage rawImage)
        {
            return source.Subscribe(x => rawImage.color = x);
        }

        #endregion
    }
}
