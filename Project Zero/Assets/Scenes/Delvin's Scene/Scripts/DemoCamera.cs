using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour

{
    [SerializeField] int sens = 200;
    [SerializeField] int lockVertMin = -80, lockVertMax = 80;
    [SerializeField] bool invertY;

    private float rotX;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        rotX += invertY ? mouseY : -mouseY;
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}