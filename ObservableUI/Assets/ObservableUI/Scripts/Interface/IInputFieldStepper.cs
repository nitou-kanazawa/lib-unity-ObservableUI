using System;

namespace Nitou.ObservableUI {
    
    public interface IInputFieldStepper<T> : IInputField<T>�@{

        T Delta { get; }

        void MovePrevious();
        void MoveNext();

        bool CanMovePrevious();
        bool CanMoveNext();
    }
}
