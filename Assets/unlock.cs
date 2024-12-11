using UnityEngine;

public class UnlockCursorOnSceneLoad : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
