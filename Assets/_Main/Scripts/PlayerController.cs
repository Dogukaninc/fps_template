using UnityEngine;
using UnityEngine.InputSystem; // Namespace'i eklemeyi unutma

public class PlayerController : MonoBehaviour
{
    private GameInput _input;
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 10f;

    public Transform playerCamera;
    private float _xRotation = 0f;

    private void Awake()
    {
        _input = new GameInput();
        
        _input.Player.Fire.performed += ctx => OnLeftClick();
        _input.Player.AltFire.performed += ctx => OnRightClick();
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
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _input.Player.Move.ReadValue<Vector2>();

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }

    private void HandleLook()
    {
        // 'Look' action'ından mouse delta verisini oku
        Vector2 lookInput = _input.Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); // Boyun kırma sınırı

        playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnLeftClick()
    {
        Debug.Log("Mouse 0 (Sol Tık) algılandı!");
    }

    private void OnRightClick()
    {
        Debug.Log("Mouse 1 (Sağ Tık) algılandı!");
    }
}