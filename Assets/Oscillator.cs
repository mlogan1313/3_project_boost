using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 _movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] private float _period = 2f;

    private float _movementFactor; // 0 = none, 1 fully moved

    private Vector3 _startingPos;

	// Use this for initialization
	void Start ()
	{
	    _startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Math.Abs(_period) <= Mathf.Epsilon) { return; }
	    float cycles = Time.time / _period;

	    const float tau = Mathf.PI * 2;
	    float rawSinWave = Mathf.Sin(cycles * tau);

	    _movementFactor = rawSinWave / 2f + 0.5f;
	    var offset = _movementVector * _movementFactor;
	    transform.position = _startingPos + offset;
	}
}
