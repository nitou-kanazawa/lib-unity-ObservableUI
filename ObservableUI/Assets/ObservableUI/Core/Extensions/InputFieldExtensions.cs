using System;
using TMPro;

namespace UniRx
{
    /// <summary>
    /// <see cref="TMP_InputField"/>の拡張メソッド群．
    /// </summary>
    public static partial class InputFieldExtensions
    {
        /// ----------------------------------------------------------------------------
        #region Observable

        /// <summary>
        /// Observe onEndEdit(Submit) event.
        /// </summary>
        public static IObservable<string> OnEndEditAsObservable(this TMP_InputField inputField)
        {
            return inputField.onEndEdit.AsObservable();
        }

        /// <summary>
        /// Observe onValueChanged with current `text` value on subscribe.
        /// </summary>
        public static IObservable<string> OnValueChangedAsObservable(this TMP_InputField inputField, bool withCurrentValue = true)
        {
            return Observable.CreateWithState<string, TMP_InputField>(inputField, (i, observer) =>
            {
                if (withCurrentValue)
                    observer.OnNext(i.text);

                return i.onValueChanged.AsObservable().Subscribe(observer);
            });
        }
        #endregion


        /// ----------------------------------------------------------------------------
        #region Binding

        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this IReactiveProperty<string> property, TMP_InputField inputField)
        {
            // Model → View
            var d1 = property.SubscribeWithState(inputField, (x, i) => i.text = x);
            // View → Model
            var d2 = inputField.OnEndEditAsObservable().SubscribeWithState(property, (x, p) => p.Value = x);

            return StableCompositeDisposable.Create(d1, d2);
        }

        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this IReactiveProperty<int> property, TMP_InputField inputField,
            int defaultValue = 0)
        {
            return property.BindToInputField(inputField,
                value => int.TryParse(value, out int result) ? result : defaultValue,
                value => value.ToString());
        }

        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this IReactiveProperty<float> property, TMP_InputField inputField,
            string format = "F2", float defaultValue = 0f)
        {
            return property.BindToInputField(inputField,
                value => float.TryParse(value, out float result) ? result : defaultValue,
                value => value.ToString(format));
        }

        /// <summary>
        /// 変換処理を指定した<see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField<T>(
            this IReactiveProperty<T> property, TMP_InputField inputField,
            Func<string, T> parseFunc, Func<T, string> formatFunc)
        {

            // Model → View
            var d1 = property.SubscribeWithState2(inputField, formatFunc, (x, i, f) => i.text = f(x));
            // View → Model
            var d2 = inputField.OnEndEditAsObservable()
                .SubscribeWithState3(property, formatFunc, inputField,
                    (value, p, f, i) =>
                    {
                        try
                        {
                            p.Value = parseFunc(value);
                        }
                        catch
                        {
                            // 変換失敗時に入力フィールドをリセット
                            i.text = f(p.Value);
                        }
                    });

            return StableCompositeDisposable.Create(d1, d2);
        }
        #endregion
    }
}
