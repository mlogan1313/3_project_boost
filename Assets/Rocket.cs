using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    [SerializeField] float _rcsThrust = 100f;
    [SerializeField] float _mainThrust = 100f;

    enum Scenes
    {
        LevelOne = 0, LevelTwo = 1
    }

    enum State { Alive, Dying, Transcending }
    private State _state = State.Alive;

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
            Thrust();
            Rotate();
        }

        if (_state != State.Alive)
        {
            _audioSource.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_state != State.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                _state = State.Transcending;
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                _state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene((int) Scenes.LevelTwo);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene((int)Scenes.LevelOne);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddRelativeForce(Vector3.up * _mainThrust);
            if (!_audioSource.isPlaying) _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }
    }

    private void Rotate()
    {
        _rigidBody.freezeRotation = true;

        var rotationThisFrame = _rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        _rigidBody.freezeRotation = false;
    }

   
}
