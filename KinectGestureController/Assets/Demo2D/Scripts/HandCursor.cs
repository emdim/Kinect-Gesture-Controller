using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using GesturesInput;
using UnityEngine;

public class HandCursor : MonoBehaviour
{
    public Sprite handOpen;
    public Sprite handClosed;


    private Transform _grabbedObject;
    private RaycastHit _hit;
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, GestureManager.GetHandPointerWorldPos(), 0.7f);

        if (GestureManager.CursorHandStateStart(HandState.Closed) || Input.GetKeyDown(KeyCode.A))
        {
            _spriteRenderer.sprite = handClosed;

            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, fwd, out _hit, 10))
            {
                //Debug.Log("There is something in front of the object!");
                if (_hit.transform.tag == "Ball")
                {
                    _grabbedObject = _hit.transform;
                }
            }
            //else
            //{
            //    Debug.Log("nothing found");
            //}
        }

        if (GestureManager.CursorHandStateEnd(HandState.Closed) || Input.GetKeyDown(KeyCode.D))
        {
            _spriteRenderer.sprite = handOpen;
            _grabbedObject = null;
        }

        if (_grabbedObject != null)
        {
            _grabbedObject.transform.position = new Vector3(transform.position.x, transform.position.y, _grabbedObject.transform.position.z);
        }
    }
}
