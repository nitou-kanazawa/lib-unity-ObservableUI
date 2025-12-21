using System;
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


        public override bool IsIntaractable
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
            throw new NotImplementedException();
            //return float.TryParse(_inputField.text, out value);
        }

        protected override void SetToView(Vector3 value)
        {
            _inputFieldX.text = value.x.ToString("F2");
            _inputFieldY.text = value.y.ToString("F2");
            _inputFieldZ.text = value.z.ToString("F2");
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