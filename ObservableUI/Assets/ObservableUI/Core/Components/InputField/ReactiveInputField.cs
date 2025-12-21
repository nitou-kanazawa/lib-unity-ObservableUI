using System;
using R3;
using UnityEngine;

namespace Nitou.ObservableUI
{
    /// <summary>
    ///
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class ReactiveInputField<T> : MonoBehaviour, IReactiveInputField<T>
        where T : struct
    {
        protected readonly ReactiveProperty<T> _property = new();

        public ReactiveProperty<T> ReactiveProperty => _property;

        public abstract bool IsIntaractable { get; set; }


        /// ----------------------------------------------------------------------------
        // LifeCycle Events
        private void Awake()
        {
            // Init viewdata
            if (TryParseFromView(out var initValue))
                _property.Value = initValue;

            // viewdata -> view
            _property.Subscribe(SetToView).AddTo(this);

            // view -> viewdata
            ObserveEndEditEvent()
                .Subscribe(_ =>
                {
                    if (TryParseFromView(out var value))
                        _property.Value = value;
                    else
                        _property.OnNext(value);
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _property.Dispose();
        }


        /// ----------------------------------------------------------------------------
        // Protected Method
        protected abstract bool TryParseFromView(out T value);

        protected abstract void SetToView(T value);

        protected abstract Observable<Unit> ObserveEndEditEvent();
    }
}