using System;
using System.Globalization;
using UnityEngine;
using TMPro;
using R3;

namespace Nitou.ObservableUI
{
    public class Vector2ReactiveInputField : ReactiveInputField<Vector2>
    {
        [SerializeField] TMP_InputField _inputFieldX;
        [SerializeField] TMP_InputField _inputFieldY;


        public override bool IsInteractable
        {
            get => _inputFieldX.interactable
                   && _inputFieldY.interactable;
            set
            {
                _inputFieldX.interactable = value;
                _inputFieldY.interactable = value;
            }
        }


        /// ----------------------------------------------------------------------------
        // Public Method
        public void SetTextColor(Color32 value)
        {
            _inputFieldX.GetComponentInChildren<TextMeshProUGUI>().color = value;
            _inputFieldY.GetComponentInChildren<TextMeshProUGUI>().color = value;
        }


        /// ----------------------------------------------------------------------------
        // Protected Method
        protected override bool TryParseFromView(out Vector2 value)
        {
            value = default;
            if (float.TryParse(_inputFieldX.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                float.TryParse(_inputFieldY.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
            {
                value = new Vector2(x, y);
                return true;
            }
            return false;
        }

        protected override void SetToView(Vector2 value)
        {
            _inputFieldX.text = value.x.ToString("F2", CultureInfo.InvariantCulture);
            _inputFieldY.text = value.y.ToString("F2", CultureInfo.InvariantCulture);
        }

        protected override Observable<Unit> ObserveEndEditEvent()
        {
            return Observable.Merge(
                _inputFieldX.OnEndEditAsObservable().AsUnitObservable(),
                _inputFieldY.OnEndEditAsObservable().AsUnitObservable()
            );
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnValidate() {
        }
#endif
    }
}