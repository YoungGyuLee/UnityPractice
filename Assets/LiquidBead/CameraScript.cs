using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera firstCamera;
    public Camera secondCamera;

    // Call this function to disable FPS camera,
    // and enable overhead camera.
    public void Update()
    {
        if(Input.GetKey(KeyCode.A)){
            ShowFirstCamera();
        }
        else
        {
            ShowSecondCamera();
        }
    }

    public void ShowFirstCamera()
    {
        firstCamera.enabled = true;
        secondCamera.enabled = false;
    }

    // Call this function to enable FPS camera,
    // and disable overhead camera.
    public void ShowSecondCamera()
    {
        firstCamera.enabled = false;
        secondCamera.enabled = true;
    }
}
