using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniRx
{
    public static partial class ImageExtensions
    {
        /// ----------------------------------------------------------------------------
        #region Binding

        /// <summary>
        /// <see cref="Image"/>.fillAmountへのバインディング．
        /// </summary>
        public static IDisposable SubscribeToImageFillAmount(this IObservable<float> source, Image image)
        {
            return source.SubscribeWithState(image, (x, i) => i.fillAmount = x);
        }

        /// <summary>
        /// <see cref="Image"/>.colorへのバインディング．
        /// </summary>
        public static IDisposable SubscribeToImageColor(this IObservable<Color> source, Image image)
        {
            return source.SubscribeWithState(image, (x, i) => i.color = x);
        }
        #endregion
    }
}
