using System;
using UnityEngine;
using UnityEngine.UI;

namespace R3
{
    /// <summary>
    /// <see cref="Image"/>の拡張メソッド群．
    /// </summary>
    public static partial class ImageExtensions
    {
        #region Binding

        /// <summary>
        /// <see cref="Image"/>.fillAmountへのバインディング．
        /// </summary>
        public static IDisposable SubscribeToImageFillAmount(this Observable<float> source, Image image)
        {
            return source.Subscribe(x => image.fillAmount = x);
        }

        /// <summary>
        /// <see cref="Image"/>.colorへのバインディング．
        /// </summary>
        public static IDisposable SubscribeToImageColor(this Observable<Color> source, Image image)
        {
            return source.Subscribe(x => image.color = x);
        }

        #endregion
    }
}