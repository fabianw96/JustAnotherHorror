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
        private Vector3 _playerSpeed;
        private float _timer;
        private Rigidbody _playerRb;
        private float _defaultPosY;
        [SerializeField] private GameObject playerBody;
        [SerializeField] private GameObject camHolder;
        [SerializeField] private float mouseSens = 50f;
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
            Vector3 playerRotation = Vector3.up * (_mouseInputX * mouseSens);
            playerBody.transform.Rotate(playerRotation);
        
            _xRotation -= _mouseInputY * mouseSens;
            _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _mouseInputX = context.ReadValue<Vector2>().x;
            _mouseInputY = context.ReadValue<Vector2>().y;
        }
    }
}
