using UnityEngine;

public class HidingCameraSwitcher : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera[] hidingCameras;
    private int currentCameraIndex = 0;

    private PlayerController2 player; // Reference to PlayerController2

    void Start()
    {
        player = FindObjectOfType<PlayerController2>();

        // Ensure main camera starts active
        SwitchToMainCamera();
    }

    void Update()
    {
        if (player == null) return;

        if (player.isHiding)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCamera(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCamera(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCamera(3);
        }
        else
        {
            SwitchToMainCamera();
        }
    }

    void SwitchCamera(int cameraIndex)
    {
        if (cameraIndex < 1 || cameraIndex > hidingCameras.Length) return;

        // Disable all cameras
        mainCamera.gameObject.SetActive(false);
        foreach (var cam in hidingCameras)
        {
            cam.gameObject.SetActive(false);
        }

        // Activate the selected hiding camera
        hidingCameras[cameraIndex - 1].gameObject.SetActive(true);
        currentCameraIndex = cameraIndex;
    }

    void SwitchToMainCamera()
    {
        foreach (var cam in hidingCameras)
        {
            cam.gameObject.SetActive(false);
        }
        mainCamera.gameObject.SetActive(true);
        currentCameraIndex = 0;
    }
}