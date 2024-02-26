using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerCamera : MonoBehaviour
    {
        private float _mouseInputX;
        private float _mouseInputY;
        private float _xRotation;
        private float _yRotation;
        private Vector3 _playerSpeed;
        private float _timer;
        private Rigidbody _playerRb;
        private float _defaultPosY;
        private static float _mouseSens = 2f;
        [SerializeField] private GameObject playerBody;
        [SerializeField] private GameObject camHolder;
        [SerializeField] private float bobSpeed;
        [SerializeField] private float bobHeight;

        // Start is called before the first frame update
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _playerRb = playerBody.GetComponent<Rigidbody>();
            _defaultPosY = camHolder.transform.localPosition.y;
        }

        private void Update()
        {
            if (_playerRb.velocity.magnitude > 0.1f)
            {
                _timer += Time.deltaTime * bobSpeed * _playerRb.velocity.magnitude;
                camHolder.transform.localPosition = new Vector3(transform.localPosition.x,
                    _defaultPosY + Mathf.Sin(_timer) * (bobHeight * _playerRb.velocity.magnitude), transform.localPosition.z);
            }
            else
            {
                _timer = 0;
            }
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            if (GameManager.Instance.isPaused || GameplayManager.Instance.IsGameOver()) return;
            _yRotation += _mouseInputX * Time.deltaTime * _mouseSens;
            _xRotation -= _mouseInputY * Time.deltaTime * _mouseSens;
            _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        
            camHolder.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            playerBody.transform.localRotation = Quaternion.Euler(0, _yRotation, 0);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _mouseInputX = context.ReadValue<Vector2>().x;
            _mouseInputY = context.ReadValue<Vector2>().y;
        }

        public static float GetMouseSens()
        {
            return _mouseSens;
        }

        public static void SetMouseSens(float value)
        {
            _mouseSens = value;
        }
    }
}
