using System;
using System.Globalization;
using UnityEngine;
using TMPro;
using R3;

namespace Nitou.ObservableUI
{
    public class Vector3ReactiveInputField : ReactiveInputField<Vector3>
    {
        [SerializeField] TMP_InputField _inputFieldX;
        [SerializeField] TMP_InputField _inputFieldY;
        [SerializeField] TMP_InputField _inputFieldZ;


        public override bool IsInteractable
        {
            get => _inputFieldX.interactable
                   && _inputFieldY.interactable
                   && _inputFieldZ.interactable;
            set
            {
                _inputFieldX.interactable = value;
                _inputFieldY.interactable = value;
                _inputFieldZ.interactable = value;
            }
        }


        /// ----------------------------------------------------------------------------
        // Public Method
        public void SetTextColor(Color32 value)
        {
            _inputFieldX.GetComponentInChildren<TextMeshProUGUI>().color = value;
            _inputFieldY.GetComponentInChildren<TextMeshProUGUI>().color = value;
            _inputFieldZ.GetComponentInChildren<TextMeshProUGUI>().color = value;
        }


        /// ----------------------------------------------------------------------------
        // Protected Method
        protected override bool TryParseFromView(out Vector3 value)
        {
            value = default;
            if (float.TryParse(_inputFieldX.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                float.TryParse(_inputFieldY.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var y) &&
                float.TryParse(_inputFieldZ.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var z))
            {
                value = new Vector3(x, y, z);
                return true;
            }
            return false;
        }

        protected override void SetToView(Vector3 value)
        {
            _inputFieldX.text = value.x.ToString("F2", CultureInfo.InvariantCulture);
            _inputFieldY.text = value.y.ToString("F2", CultureInfo.InvariantCulture);
            _inputFieldZ.text = value.z.ToString("F2", CultureInfo.InvariantCulture);
        }

        protected override Observable<Unit> ObserveEndEditEvent()
        {
            return Observable.Merge(
                _inputFieldX.OnEndEditAsObservable().AsUnitObservable(),
                _inputFieldY.OnEndEditAsObservable().AsUnitObservable(),
                _inputFieldZ.OnEndEditAsObservable().AsUnitObservable());
        }


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnValidate() {
        }
#endif
    }
}