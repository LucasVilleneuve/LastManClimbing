using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 0.0f;
    [SerializeField] private float speedIncreaseByTick = 0.0f;

    private bool cameraMovementEnable = true;

    private void Awake()
    {
        Screen.SetResolution(480, 1080, false);
    }

    private void Update()
    {
        if (cameraMovementEnable)
        {
            transform.position = transform.position + new Vector3(0, cameraSpeed / 1000.0f, 0);
            cameraSpeed += (speedIncreaseByTick / 1000.0f);
        }
    }

    public void EnableMovement(bool enable)
    {
        cameraMovementEnable = enable;
    }
}