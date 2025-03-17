using Unity.VisualScripting;
using UnityEngine;

public class HidingCameraSwitcher : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera[] hidingCameras;
    private int currentCameraIndex = 0;
    [SerializeField] GameObject Cam1;
    [SerializeField] GameObject Cam2;
    [SerializeField] GameObject Cam3;
    [SerializeField] GameObject CamMain;

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
           

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SwitchCamera(1);
                Cam1.SetActive(true);
                Cam2.SetActive(false);
                Cam3.SetActive(false);
                CamMain.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchCamera(2);
                Cam2.SetActive(true);
                Cam1.SetActive(false);
                Cam3.SetActive(false);
                CamMain.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SwitchToMainCamera();
                CamMain.SetActive(true);
                Cam2.SetActive(false);
                Cam1.SetActive(false);
                Cam3.SetActive(false);
                
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) 
            {
                SwitchCamera(3);
                Cam3.SetActive(true);
                Cam2.SetActive(false);
                Cam1.SetActive(false);
                CamMain.SetActive(false);

            }
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