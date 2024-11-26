using UnityEngine;

public class CameraFrustumLoader : MonoBehaviour
{
    public Camera mainCamera; // Reference to the camera (assign in the Inspector)
    public float loadRadius = 50f; // Max distance to load objects
    public float loadAngle = 60f; // Angle in which objects are considered in front of the camera
    public GameObject[] objectsToManage; // Array of objects to load/unload

    void Update()
    {
        foreach (GameObject obj in objectsToManage)
        {
            Vector3 directionToObject = obj.transform.position - mainCamera.transform.position;
            float distanceToObject = directionToObject.magnitude;

            // Check if the object is within range
            if (distanceToObject < loadRadius)
            {
                // Check if the object is within the camera's field of view (angle)
                float angleToObject = Vector3.Angle(mainCamera.transform.forward, directionToObject);

                if (angleToObject < loadAngle)
                {
                    // Object is within the camera's view, load it
                    LoadObject(obj);
                }
                else
                {
                    // Object is out of the camera's view, unload it
                    UnloadObject(obj);
                }
            }
            else
            {
                // Object is too far from the camera, unload it
                UnloadObject(obj);
            }
        }
    }

    // Method to activate objects (load them into the scene)
    void LoadObject(GameObject obj)
    {
        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true); // Activate the object if it's not already active
        }
    }

    // Method to deactivate objects (unload them from memory)
    void UnloadObject(GameObject obj)
    {
        if (obj.activeInHierarchy)
        {
            obj.SetActive(false); // Deactivate the object to unload it
        }
    }
}
