using System;
using System.Collections;
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
        private Texture2D _blackTexture;
        private float _alpha = 0f;
        [SerializeField] private float waitForFadeTime = 6f;
        [SerializeField] private GameObject playerBody;
        [SerializeField] private GameObject camHolder;
        [SerializeField] private float bobSpeed;
        [SerializeField] private float bobHeight;

        private void Awake()
        {
            //create new black texture
            _blackTexture = new Texture2D(1, 1);
            _blackTexture.SetPixel(0, 0, Color.black);
            _blackTexture.Apply();
        }

        private void OnGUI()
        {
            //place new texture on screen
            GUI.color = new Color(0, 0, 0, _alpha);
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), _blackTexture);
        }
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
            //headbob based on sinus wave
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
            //rotate camera and playerbody based on mouse movement
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

        public void FadeCamera()
        {
            StartCoroutine(FadeInAndOut());
        }

        private IEnumerator FadeInAndOut()
        {
            float fadeDuration = 3f;
            float elapsedTime = 0f;
            
            //screen to black
            _alpha = 1;

            yield return new WaitForSeconds(waitForFadeTime);
            
            //fade to game
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                _alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
                yield return null;
            }

            _alpha = 0;

        }
        
    }
}
