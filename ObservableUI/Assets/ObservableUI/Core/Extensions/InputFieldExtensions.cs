using System;
using TMPro;

namespace R3
{
    /// <summary>
    /// <see cref="TMP_InputField"/>の拡張メソッド群．
    /// </summary>
    public static partial class InputFieldExtensions
    {
        #region Observable

        /// <summary>
        /// Observe onValueChanged with current `text` value on subscribe.
        /// </summary>
        public static Observable<string> OnValueChangedAsObservable(this TMP_InputField inputField, bool withCurrentValue = true)
        {
            return Observable.Create<string>(observer =>
            {
                if (withCurrentValue)
                    observer.OnNext(inputField.text);

                return inputField.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        #endregion


        #region Binding

        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this ReactiveProperty<string> property, TMP_InputField inputField)
        {
            // Model → View
            var d1 = property.Subscribe(x => inputField.text = x);
            // View → Model
            var d2 = inputField.OnEndEditAsObservable().Subscribe(x => property.Value = x);

            return Disposable.Combine(d1, d2);
        }

        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this ReactiveProperty<int> property, TMP_InputField inputField,
                                                   int defaultValue = 0)
        {
            return property.BindToInputField(inputField,
                value => int.TryParse(value, out int result) ? result : defaultValue,
                value => value.ToString());
        }

        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this ReactiveProperty<float> property, TMP_InputField inputField,
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
            this ReactiveProperty<T> property, TMP_InputField inputField,
            Func<string, T> parseFunc, Func<T, string> formatFunc)
        {
            // Model → View
            var d1 = property.Subscribe(x => inputField.text = formatFunc(x));
            // View → Model
            var d2 = inputField.OnEndEditAsObservable()
                               .Subscribe(value =>
                               {
                                   try
                                   {
                                       property.Value = parseFunc(value);
                                   }
                                   catch
                                   {
                                       // 変換失敗時に入力フィールドをリセット
                                       inputField.text = formatFunc(property.Value);
                                   }
                               });

            return Disposable.Combine(d1, d2);
        }

        #endregion
    }
}