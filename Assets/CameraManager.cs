using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static Camera MainCam { get; private set; }

    void Awake()
    {
        MainCam = GetComponent<Camera>();
    }
}

