using System;
using System.Globalization;
using UnityEngine;
using TMPro;
using R3;

namespace Nitou.ObservableUI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class IntReactiveInputField : ReactiveInputField<int>
    {
        [SerializeField] protected TMP_InputField _inputField;


        public override bool IsInteractable
        {
            get => _inputField.interactable;
            set => _inputField.interactable = value;
        }


        /// ----------------------------------------------------------------------------
        // Public Method
        public void SetTextColor(Color32 value)
        {
            _inputField.GetComponentInChildren<TextMeshProUGUI>().color = value;
        }


        /// ----------------------------------------------------------------------------
        // Protected Method
        protected override bool TryParseFromView(out int value)
        {
            return int.TryParse(_inputField.text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        protected override void SetToView(int value)
        {
            _inputField.text = value.ToString(CultureInfo.InvariantCulture);
        }

        protected override Observable<Unit> ObserveEndEditEvent()
        {
            return _inputField.OnEndEditAsObservable().AsUnitObservable();
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_inputField == null)
                _inputField = GetComponent<TMP_InputField>();
        }
#endif
    }
}