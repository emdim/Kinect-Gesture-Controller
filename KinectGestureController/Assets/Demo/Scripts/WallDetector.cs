using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using GesturesInput;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _controller;

    [SerializeField] private float Speed = 1.0f;
    [SerializeField] private float Gravity = 20.0f;
    [SerializeField] private float RaycastDistance = 1.0f;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _fwd;
    private float _horizontal;
    private bool _stopped = true;
    private KinectSensor _sensor;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _sensor = KinectSensor.GetDefault();
        _fwd = transform.TransformDirection(Camera.main.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (_sensor.IsAvailable && _sensor.IsOpen)
        {
            InputHandlerKinect();
        }
        else
        {
            InputHandler();
        }
    }

    void FixedUpdate()
    {
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, _fwd, RaycastDistance) && !_stopped)
        {
            _stopped = true;
            //print("There is something in front of the object!");
            _horizontal = 0.0f;
            _animator.SetInteger("moving", 0); //idle

        }
        else if (_stopped && Mathf.Abs(_horizontal) > 0)
        {
            _stopped = false;
            _animator.SetInteger("moving", 1); //walk
        }

        MovePlayer(_horizontal);

    }

    void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetDirection(0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetDirection(90); ;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SetDirection(270);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetDirection(180);
        }
    }

    void InputHandlerKinect()
    {
        if (GestureManager.GetGestureStatus(GestureId.SwipeUpLeftHand) ||
            GestureManager.GetGestureStatus(GestureId.SwipeUpRightHand))
        {
            Debug.Log("MOVING 0");
            SetDirection(0);
        }
        else if (GestureManager.GetGestureStatus(GestureId.SwipeRightWithLeftHand) ||
                 GestureManager.GetGestureStatus(GestureId.SwipeRightWithRightHand))
        {
            Debug.Log("MOVING 90");
            SetDirection(90); ;
        }
        else if (GestureManager.GetGestureStatus(GestureId.SwipeLeftWithRightHand) ||
                 GestureManager.GetGestureStatus(GestureId.SwipeLeftWithLeftHand))
        {
            Debug.Log("MOVING 270");
            SetDirection(270);
        }
        else if (GestureManager.GetGestureStatus(GestureId.SwipeDownLeftHand) ||
                 GestureManager.GetGestureStatus(GestureId.SwipeDownRightHand))
        {
            Debug.Log("MOVING 180");
            SetDirection(180);
        }
    }

    void SetDirection(int rotateDegrees)
    {
        _horizontal = 1.0f;
        RotatePlayer(rotateDegrees);
        _fwd = transform.TransformDirection(Vector3.forward);
    }

    void RotatePlayer(float degrees)
    {
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + degrees, 0);
        transform.eulerAngles = new Vector3(0, degrees, 0);
    }

    void MovePlayer(float h)
    {
        if (_controller.isGrounded)
        {
            _moveDirection = transform.forward * h * Speed;
            _animator.speed = Speed;
        }

        _controller.Move(_moveDirection * Time.deltaTime);
        _moveDirection.y -= Gravity * Time.deltaTime;
    }
}
