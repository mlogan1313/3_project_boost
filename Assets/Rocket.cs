﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    [SerializeField] float _rcsThrust = 100f;
    [SerializeField] float _mainThrust = 100f;

    [SerializeField] AudioClip _mainEngine;
    [SerializeField] AudioClip _success;
    [SerializeField] AudioClip _death;

    [SerializeField] ParticleSystem _mainEngineParticles;
    [SerializeField] ParticleSystem _successParticles;
    [SerializeField] ParticleSystem _deathParticles;

    [SerializeField] float _levelLoadDelay = 1f;

    enum Scenes
    {
        LevelOne = 0, LevelTwo = 1
    }

    enum State { Alive, Dying, Transcending }
    private State _state = State.Alive;

    private bool _collisionsEnabled = true;

    // Use this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    void Update()
    {
        if (_state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        // if in debug mode
        if (Debug.isDebugBuild) RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _collisionsEnabled = !_collisionsEnabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_state != State.Alive || !_collisionsEnabled) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        _state = State.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_death);
        _deathParticles.Play();
        Invoke("LoadFirstLevel", _levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        _state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_success);
        _successParticles.Play();
        Invoke("LoadNextLevel", _levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        print("current index: " + currentSceneIndex);
        var nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            print("inside reset scene to 0");
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            _audioSource.Stop();
            _mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rigidBody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);

        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_mainEngine);
        }

        if (!_mainEngineParticles.isPlaying)
        {
            _mainEngineParticles.Play();
        }

    }

    private void RespondToRotateInput()
    {
        _rigidBody.freezeRotation = true;

        var rotationThisFrame = _rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        _rigidBody.freezeRotation = false;
    }
}
