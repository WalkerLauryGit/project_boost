using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 5f;
    Rigidbody rigidbody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
       
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly Collision");
                break;
            case "Finish":
                state = State.Transcending;
                print("Got Finish");
                Invoke("LoadNextScene", 3.0f); //parameterize time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstScene", 2.0f);
                break;
        }
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
        state = State.Alive;
    }

    private void Thrust()
    {
       
            if (Input.GetKey(KeyCode.Space))
            {
                rigidbody.AddRelativeForce(Vector3.up * mainThrust);
                if (!audioSource.isPlaying)
                {
                if (state == State.Dying || state == State.Transcending)
                {
                    audioSource.Stop();
                }
                else
                {


                    audioSource.Play();
                }
                }

            }
            else
            {
                audioSource.Stop();
            }
        
        

    }

    private void Rotate()
    {
       
            rigidbody.freezeRotation = true; //Manual control of rotation


            float rotationThisFrame = rcsThrust * Time.deltaTime;

            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {

                transform.Rotate(Vector3.forward * rotationThisFrame);
            }
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {

                transform.Rotate(-Vector3.forward * rotationThisFrame);
            }
            rigidbody.freezeRotation = false;
        
    }
}
