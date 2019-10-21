using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 5.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we 
    public float heightDamping = 2.0f;

    // Place the script in the Camera-Control group in the component menu
    [AddComponentMenu("Camera-Control/Smooth Follow")]

    void Awake()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (!playerObject)
        {
            Debug.LogError("Not found player in the scene");
        }
        else
        {
            target = playerObject.transform;
        }
    }

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target) return;

        // Calculate the current rotation angles
        //float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;
        
        float currentHeight = transform.position.y;
        
        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        
        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;
        transform.position -=  Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the target
        transform.LookAt(target);
    }
}