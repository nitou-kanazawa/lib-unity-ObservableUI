using System;
using UniRx;
using UnityEngine;

namespace Nitou.ObservableUI {

	/// <summary>
	/// 
	/// </summary>
	public abstract class ReactiveInputField<T> : MonoBehaviour, IReactiveInputField<T> {

		protected readonly ReactiveProperty<T> _reactiveProperty = new();

		public IReactiveProperty<T> ReactiveProperty => _reactiveProperty;

		public abstract bool IsIntaractable { get; set; }


		/// ----------------------------------------------------------------------------
		// LifeCycle Events

		private void Awake() {
			_reactiveProperty.Subscribe(SetToView).AddTo(this);

			ObserveEndEditEvent()
				.Subscribe(_ => {
					if (TryParseFromView(out var value))
						_reactiveProperty.Value = value;
					else
						_reactiveProperty.SetValueAndForceNotify(value);
				})
				.AddTo(this);
		}

		private void OnDestroy() {
			_reactiveProperty.Dispose();
		}


		/// ----------------------------------------------------------------------------
		// Protected Method

		// Getter
		protected abstract bool TryParseFromView(out T value);

		// Setter
		protected abstract void SetToView(T value);

		protected abstract IObservable<Unit> ObserveEndEditEvent();


		/// ----------------------------------------------------------------------------
		// Private Method
	}
}
