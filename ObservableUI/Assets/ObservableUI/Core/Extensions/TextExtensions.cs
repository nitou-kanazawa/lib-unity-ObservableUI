using System;
using TMPro;

namespace UniRx
{
    /// <summary>
    /// <see cref="TextMeshProUGUI"/>の拡張メソッド群．
    /// </summary>
    public static partial class TextExtensions
    {

        /// ----------------------------------------------------------------------------
        #region Binding

        /// <summary>
        /// 
        /// </summary>
        public static IDisposable SubscribeToText(this IObservable<string> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x);
        }

        /// <summary>
        /// 
        /// </summary>
        public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI text, Func<T, string> selector)
        {
            return source.SubscribeWithState2(text, selector, (x, t, s) => t.text = s(x));
        }
        #endregion
    }

}
