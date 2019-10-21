using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GestureManagerEditor
{
    //Add a new menu item under an existing menu
    [MenuItem("Kinect/Create Kinect Gesture Manager %#g")]
    private static void MenuOption()
    {
        CreateKinectGestureManager();
    }

    //Add a new menu item in hierarchy menu
    [MenuItem("GameObject/Create Other/Kinect Gesture Manager")]
    private static void ContextOption()
    {
        CreateKinectGestureManager();
    }

    //Add the gesture manager in the scene
    private static void CreateKinectGestureManager()
    {
        if (!Object.FindObjectOfType<GestureManager>())
        {
            var go = new GameObject(typeof(GestureManager).Name);
            go.AddComponent<GestureManager>();
            Debug.Log("Gesture Manager created.");
        }
        else
        {
            Debug.LogError("Gesture Manager already exists in scene " + SceneManager.GetActiveScene().name);
        }
    }
}