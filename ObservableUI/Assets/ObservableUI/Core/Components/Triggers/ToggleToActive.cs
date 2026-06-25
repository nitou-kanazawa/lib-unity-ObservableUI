using UnityEngine;
using UnityEngine.UI;
using R3;

namespace Nitou.ObservableUI
{
    /// <summary>
    /// <see cref="Toggle"/>の状態（Trigger）に応じて，対象<see cref="GameObject"/>の
    /// アクティブ状態（Action）を切り替える簡易コンポーネント．
    /// <para>命名規約: [Trigger] To [Action]</para>
    /// </summary>
    [AddComponentMenu("Observable UI/Triggers/Toggle To Active")]
    [DisallowMultipleComponent]
    public sealed class ToggleToActive : MonoBehaviour
    {
        [Header("Trigger")]
        [SerializeField] private Toggle _toggle;

        [Header("Action")]
        [Tooltip("アクティブ状態を切り替える対象")]
        [SerializeField] private GameObject _target;

        [Tooltip("ONで非アクティブ化する（ToggleToInactive相当）")]
        [SerializeField] private bool _invert = false;


        /// ----------------------------------------------------------------------------
        // LifeCycle Events
        private void Reset()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void Awake()
        {
            if (_toggle == null)
                _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            if (_toggle == null || _target == null)
            {
                Debug.LogWarning($"[{nameof(ToggleToActive)}] Toggle/Targetが未設定のため動作しません．", this);
                return;
            }

            // 初期状態を同期
            Apply(_toggle.isOn);

            // 以降の変化を購読（onValueChangedは購読時に初期値を流さないため上で明示的に同期）
            _toggle.onValueChanged.AsObservable()
                   .Subscribe(Apply)
                   .AddTo(this);
        }


        /// ----------------------------------------------------------------------------
        // Private Method
        private void Apply(bool isOn)
        {
            _target.SetActive(_invert ? !isOn : isOn);
        }
    }
}
