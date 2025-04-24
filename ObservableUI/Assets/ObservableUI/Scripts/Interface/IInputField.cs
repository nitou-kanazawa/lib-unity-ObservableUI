using System;
using System.Collections.Generic;
using UniRx;

namespace Nitou.ObservableUI {

    /// <summary>
    /// <see cref="T"/>型のカスタムInputField．
    /// </summary>
    public interface IInputField<T> {

        IObservable<T> OnValueChangedAsObservable { get; }

        T GetValue();

        void SetValue(T newValue);

        void ResetValue();
    }


    public static class InputFieldExtensions {

        /// <summary>
        /// <see cref="IInputField{T}"/>との双方向バインディング．
        /// </summary>
        public static void BindToInputField<T>(this IReactiveProperty<T> property, IInputField<T> inputField, ICollection<IDisposable> disposables) {
            property.SubscribeWithState(inputField, (x, i) => i.SetValue(x)).AddTo(disposables);
            inputField.OnValueChangedAsObservable.SubscribeWithState(property, (x, p) => p.Value = x).AddTo(disposables);
        }
    }
}
