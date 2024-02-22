using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    private float _interval = 1;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _interval)
        {
            lightSource.enabled = !lightSource.enabled;
            _interval = Random.Range(0f, 1f);
            _timer = 0;
        }
    }
}
