using UnityEngine;

namespace Nitou.ObservableUI
{
    /// <summary>
    /// <see cref="IntReactiveInputField"/>にステップ操作（増減）を加えたコンポーネント．
    /// <see cref="MoveNext"/>／<see cref="MovePrevious"/>をButton等から呼び出して使用する．
    /// </summary>
    public class IntReactiveInputFieldStepper : IntReactiveInputField, IReactiveInputFieldStepper<int>
    {
        [Header("Stepper")]
        [SerializeField] private int _delta = 1;
        [SerializeField] private bool _useClamp = false;
        [SerializeField] private int _min = int.MinValue;
        [SerializeField] private int _max = int.MaxValue;


        public int Delta => _delta;

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


        private int Clamp(int value) => _useClamp ? Mathf.Clamp(value, _min, _max) : value;
    }
}
