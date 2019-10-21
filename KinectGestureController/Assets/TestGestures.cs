using Windows.Kinect;
using GesturesInput;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGestures : MonoBehaviour
{
    private GameObject _sprite;
    private GameObject _cube;

    // Use this for initialization
    void Start()
    {
        _sprite = FindObjectOfType<Canvas>().transform.GetChild(0).gameObject;
        _cube = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(Camera.main.nearClipPlane);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("DemoScene");
        }

        WaveAndLasso();

        if (GestureManager.GetGestureStatus(GestureId.SwipeUpRightHand))
        {
            Debug.Log("Swipe up right hand!");
        }

        if (GestureManager.GetGestureStatus(GestureId.SwipeUpLeftHand))
        {
            Debug.Log("Swipe up left hand!");
        }

        //if (GestureManager.GetGestureStatus(GestureId.LEFTHAND_LEFTKNEE))
        //{
        //    Debug.Log("Left hand touched left knee!");
        //}

        //===========hand sprite test===============
        //var handpos = GestureManager.GetHandPointer();
        //Debug.Log(handpos.x + " " + handpos.y);
        //if (handpos != Vector2.zero) _sprite.transform.position = Vector2.Lerp(_sprite.transform.position, handpos, 0.05f);

        //============hand world test===============
        var handposWorld = GestureManager.GetHandPointerWorldPos();
        //Debug.Log(handposWorld);
        _cube.transform.position = handposWorld;
    }

    private void WaveAndLasso(int personId = 1)
    {
        if (GestureManager.GetGestureStatus(GestureId.WaveRightHand, personId) &&
            GestureManager.GetHandState(Person.Hand.LEFT, personId) == HandState.Lasso)
        {
            Debug.Log(string.Format("Player {0} waved!", personId));
        }
    }
}
