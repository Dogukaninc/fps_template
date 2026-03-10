using System;

namespace _Main.Input.InputSystem
{
    public abstract class InputActionHandler
    {
        public bool IsActive { get; protected set; }
        public event Action OnActivated;
        public event Action OnDeactivated;
        /// <summary>Sadece basıldığı frame'de IsActive true olur. Update'de kontrol edilir.</summary>
        public event Action OnPerformed;

        protected void SetActive(bool value)
        {
            if (IsActive == value)
                return;
            IsActive = value;

            if (value)
            {
                OnActivated?.Invoke();
                OnPerformed?.Invoke();
            }
            else
            {
                OnDeactivated?.Invoke();
            }
        }
    }
}