// csharp
using _Main.Scripts.InteractionSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private GameInput _input;
        private InteractionController _interactionController;

        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float mouseSensitivity = 10f;

        [Header("Jump / Gravity")]
        public float jumpForce = 0.4f;
        public float gravity = -9.81f;

        [Header("References")]
        public Transform playerCamera;

        private float _xRotation = 0f;
        private CharacterController _characterController;
        private float _verticalVelocity;

        private void Awake()
        {
            _input = new GameInput();
            _characterController = GetComponent<CharacterController>();
            if (_characterController == null)
                Debug.LogWarning("[PlayerController] CharacterController component missing on the player.");

            _interactionController = GetComponent<InteractionController>();

            _input.Player.Fire.performed += ctx => OnLeftClick();
            _input.Player.AltFire.performed += ctx => OnRightClick();
            _input.Player.Interact.performed += ctx => _interactionController?.Interact();
        }

        private void OnEnable()
        {
            _input.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            _input.Player.Disable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            HandleLook();
            HandleMovementAndJump();
        }

        private void HandleMovementAndJump()
        {
            if (_characterController == null) return;

            // Yatay hareket
            Vector2 moveInput = _input.Player.Move.ReadValue<Vector2>();
            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            Vector3 horizontal = move * moveSpeed;

            // Grounded kontrolü via CharacterController
            bool isGrounded = _characterController.isGrounded;
            if (isGrounded && _verticalVelocity < 0f)
            {
                // Küçük negatif hız ile zemine yapışma sağlanır
                _verticalVelocity = -4f;
            }

            // Zıplama isteği
            if (_input.Player.Jump.triggered && isGrounded)
            {
                _verticalVelocity = jumpForce;
            }

            // Yerçekimi uygulanması
            _verticalVelocity += gravity * Time.deltaTime;

            Vector3 velocity = horizontal + Vector3.up * _verticalVelocity;

            // CharacterController hareketi
            _characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleLook()
        {
            Vector2 lookInput = _input.Player.Look.ReadValue<Vector2>();

            float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            if (playerCamera != null)
                playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            transform.Rotate(Vector3.up * mouseX);
        }

        private bool IsGrounded()
        {
            return _characterController != null && _characterController.isGrounded;
        }

        private void OnLeftClick()
        {
            Debug.Log("Mouse 0 (Sol Tık) algılandı!");
        }

        private void OnRightClick()
        {
            Debug.Log("Mouse 1 (Sağ Tık) algılandı!");
        }

        private void OnDrawGizmosSelected()
        {
            if (_characterController != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position + Vector3.down * (_characterController.height / 2f), 0.1f);
            }
        }
    }
}
