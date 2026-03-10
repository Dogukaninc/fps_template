using _Main.Input.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.InteractionSystem
{
    public class InteractionController : MonoBehaviour
    {
        public float interactionRange = 3f;
        public LayerMask interactableLayer;
        Camera _mainCamera;

        private BaseInputManager Inputs => BaseInputManager.Instance;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            Inputs.Interact.OnPerformed += Interact;
        }

        private void OnDisable()
        {
            Inputs.Interact.OnPerformed -= Interact;
        }

        public void Interact()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
}