using UnityEngine;

namespace Nitou.ObservableUI
{
    /// <summary>
    /// <see cref="FloatReactiveInputField"/>にステップ操作（増減）を加えたコンポーネント．
    /// <see cref="MoveNext"/>／<see cref="MovePrevious"/>をButton等から呼び出して使用する．
    /// </summary>
    public class FloatReactiveInputFieldStepper : FloatReactiveInputField, IReactiveInputFieldStepper<float>
    {
        [Header("Stepper")]
        [SerializeField] private float _delta = 1f;
        [SerializeField] private bool _useClamp = false;
        [SerializeField] private float _min = float.NegativeInfinity;
        [SerializeField] private float _max = float.PositiveInfinity;


        public float Delta => _delta;

        public bool CanMovePrevious() => !_useClamp || _property.Value > _min;
        public bool CanMoveNext() => !_useClamp || _property.Value < _max;

        public void MovePrevious()
        {
            if (!CanMovePrevious()) return;
            _property.Value = Clamp(_property.Value - _delta);
        }

        public void MoveNext()
        {
            if (!CanMoveNext()) return;
            _property.Value = Clamp(_property.Value + _delta);
        }


        private float Clamp(float value) => _useClamp ? Mathf.Clamp(value, _min, _max) : value;
    }
}
