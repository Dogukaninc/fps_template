using _Main.Input.InputSystem;
using _Main.Scripts.InteractionSystem;
using _Main.Scripts.WeaponSystem;
using UnityEngine;

namespace _Main.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
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

        [Header("Child References")]
        [SerializeField] private WeaponHolder weaponHolder;

        private BaseInputManager Inputs => BaseInputManager.Instance;

        private void Awake()
        {
            weaponHolder.Init();

            _characterController = GetComponent<CharacterController>();
            if (_characterController == null)
                Debug.LogWarning("[PlayerController] CharacterController component missing on the player.");

            _interactionController = GetComponent<InteractionController>();
        }

        private void OnEnable()
        {
            Inputs.EnablePlayerMap();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            Inputs.DisablePlayerMap();
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

            Vector2 moveInput = Inputs.MoveInput;
            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            Vector3 horizontal = move * moveSpeed;

            bool isGrounded = _characterController.isGrounded;
            if (isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = -4f;

            if (Inputs.Jump.IsActive && isGrounded)
                _verticalVelocity = jumpForce;

            _verticalVelocity += gravity * Time.deltaTime;
            Vector3 velocity = horizontal + Vector3.up * _verticalVelocity;
            _characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleLook()
        {
            Vector2 lookInput = Inputs.LookInput;

            float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            if (playerCamera != null)
                playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            transform.Rotate(Vector3.up * mouseX);
        }

        private void OnDrawGizmosSelected()
        {
            if (_characterController != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(
                    transform.position + Vector3.down * (_characterController.height / 2f), 0.1f);
            }
        }
    }
}
