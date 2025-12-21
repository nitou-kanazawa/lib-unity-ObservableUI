using System;
using UnityEngine;
using TMPro;
using R3;

namespace Nitou.ObservableUI
{
    public class Vector2ReactiveInputField : ReactiveInputField<Vector2>
    {
        [SerializeField] TMP_InputField _inputFieldX;
        [SerializeField] TMP_InputField _inputFieldY;


        public override bool IsIntaractable
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
            throw new NotImplementedException();
        }

        protected override void SetToView(Vector2 value)
        {
            _inputFieldX.text = value.x.ToString("F2");
            _inputFieldY.text = value.y.ToString("F2");
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