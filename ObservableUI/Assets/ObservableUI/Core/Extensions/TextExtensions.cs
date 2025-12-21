using System;
using TMPro;

namespace R3
{
    /// <summary>
    /// <see cref="TextMeshProUGUI"/>の拡張メソッド群．
    /// </summary>
    public static partial class TextExtensions
    {
        #region Binding

        /// <summary>
        ///
        /// </summary>
        public static IDisposable SubscribeToText(this Observable<string> source, TextMeshProUGUI text)
        {
            return source.Subscribe(x => text.text = x);
        }

        /// <summary>
        ///
        /// </summary>
        public static IDisposable SubscribeToText<T>(this Observable<T> source, TextMeshProUGUI text)
        {
            return source.Subscribe(x => text.text = x.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        public static IDisposable SubscribeToText<T>(this Observable<T> source, TextMeshProUGUI text, Func<T, string> selector)
        {
            return source.Subscribe(x => text.text = selector(x));
        }

        #endregion
    }
}