using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private AudioSource _rocketSound;

    // Use this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rocketSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddRelativeForce(Vector3.up);
            if (!_rocketSound.isPlaying) _rocketSound.Play();
        }
        else
        {
            _rocketSound.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
    }
}
