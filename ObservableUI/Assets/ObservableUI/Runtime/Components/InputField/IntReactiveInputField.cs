using System;
using UnityEngine;
using TMPro;
using UniRx;

namespace Nitou.ObservableUI {

	[RequireComponent(typeof(TMP_InputField))]
	public class IntReactiveInputField : ReactiveInputField<int> {

		[SerializeField] protected TMP_InputField _inputField;


		public override bool IsIntaractable {
			get => _inputField.interactable;
			set => _inputField.interactable = value;
		}


		/// ----------------------------------------------------------------------------
		// Public Method

		public void SetTextColor(Color32 value) {
			_inputField.GetComponentInChildren<TextMeshProUGUI>().color = value;
		}


		/// ----------------------------------------------------------------------------
		// Protected Method

		protected override bool TryParseFromView(out int value) {
			return int.TryParse(_inputField.text, out value);
		}

		protected override void SetToView(int value) {
			_inputField.text = value.ToString();
		}

		protected override IObservable<Unit> ObserveEndEditEvent() {
			return _inputField.OnEndEditAsObservable().AsUnitObservable();
		}


		/// ----------------------------------------------------------------------------
#if UNITY_EDITOR
		private void OnValidate() {
			if (_inputField == null)
				_inputField = GetComponent<TMP_InputField>();
		}
#endif
	}
}
