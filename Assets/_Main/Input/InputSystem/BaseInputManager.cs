using Scripts.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Input.InputSystem
{
    [DefaultExecutionOrder(-2)]
    public class BaseInputManager : SingletonMonoBehaviour<BaseInputManager>
    {
        [SerializeField] private ScopeMode _scopeMode = ScopeMode.Hold;

        private GameInput _input;

        // --- Hold Handlers ---
        public HoldInputHandler Fire { get; private set; }
        public HoldInputHandler AltFire { get; private set; }
        public HoldInputHandler Jump { get; private set; }

        // --- Toggle/Hold (ayara göre) ---
        public InputActionHandler Scope { get; private set; }
        public TapInputHandler Interact { get; private set; }

        // --- Raw Value Readers ---
        public Vector2 MoveInput => _input.Player.Move.ReadValue<Vector2>();
        public Vector2 LookInput => _input.Player.Look.ReadValue<Vector2>();

        protected override void Awake()
        {
            base.Awake();
            _input = new GameInput();

            Fire = new HoldInputHandler(_input.Player.Fire);
            AltFire = new HoldInputHandler(_input.Player.AltFire);
            Jump = new HoldInputHandler(_input.Player.Jump);

            SetScopeMode(_scopeMode);
            SetInteractionAction();
        }

        public void SetScopeMode(ScopeMode mode)
        {
            _scopeMode = mode;
            Scope = mode == ScopeMode.Hold
                ? new HoldInputHandler(_input.Player.AltFire)
                : new ToggleInputHandler(_input.Player.AltFire);
        }

        private void SetInteractionAction()
        {
            Interact = new TapInputHandler(_input.Player.Interact);
        }

        public void EnablePlayerMap() => _input.Player.Enable();
        public void DisablePlayerMap() => _input.Player.Disable();

        private void OnDestroy() => _input.Player.Disable();
    }

    public class TapInputHandler : InputActionHandler
    {
        public TapInputHandler(InputAction action)
        {
            action.performed += _ => SetActive(true);
            action.canceled  += _ => SetActive(false);
        }
    }

    public class HoldInputHandler : InputActionHandler
    {
        public HoldInputHandler(InputAction action)
        {
            action.started += _ => SetActive(true);
            action.canceled += _ => SetActive(false);
        }
    }

    public class ToggleInputHandler : InputActionHandler
    {
        public ToggleInputHandler(InputAction action)
        {
            action.performed += _ => SetActive(!IsActive);
        }
    }

    public enum ScopeMode
    {
        Hold,
        Toggle
    }
}