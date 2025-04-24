using System;
using UnityEngine;
using TMPro;
using UniRx;

namespace Nitou.ObservableUI {

	public class FloatReactiveInputField: MonoBehaviour, IReactiveInputField<float> {

		[SerializeField] TMP_InputField _InputField;

		private readonly ReactiveProperty<float> _valueRP = new(0);


		public IReactiveProperty<float> ReactiveProperty => _valueRP;


		public bool IsIntaractable {
			get => _InputField.interactable;
			set => _InputField.interactable = value;
		}


		/// ----------------------------------------------------------------------------
		// LifeCycle Events

		private void Awake() {
			// ViewModelの監視
			//_valueRP.Subscribe(v => ApplyValue(v)).AddTo(this);

			//// Viewの監視
			//_InputField.onValueChanged
			//.Subscribe(_ => UpdateVectorValue())
			//.AddTo(this);
		}

		private void OnDestroy() {
			_valueRP.Dispose();
		}


		/// ----------------------------------------------------------------------------
		// Public Method

		public float GetValue() => _valueRP.Value;

		public void ResetValue() => _valueRP.Value = default;

		public void SetValue(float value) {
			_valueRP.Value = value;
		}

		public void SetInteractable(bool value) {
			_InputField.interactable = value;
		}

		public void SetTextColor(Color32 value) {
			_InputField.GetComponentInChildren<TextMeshProUGUI>().color = value;
		}


		/// ----------------------------------------------------------------------------
		// Private Method




		/// ----------------------------------------------------------------------------
#if UNITY_EDITOR
		private void OnValidate() {

		}
#endif
	}
}
