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
    [SerializeField] GameObject Cam4;
    [SerializeField] GameObject CamMain;


    void Start()
    {
        // Ensure main camera starts active
        SwitchToMainCamera();
    }

    void Update()
    {
        if (GameManager.instance.playerScript.isHiding)
        {
           

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SwitchCamera(1);
                Cam1.SetActive(true);
                Cam2.SetActive(false);
                Cam3.SetActive(false);
                CamMain.SetActive(false);
                Cam4.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchCamera(2);
                Cam2.SetActive(true);
                Cam1.SetActive(false);
                Cam3.SetActive(false);
                CamMain.SetActive(false);
                Cam4.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SwitchToMainCamera();
                CamMain.SetActive(true);
                Cam2.SetActive(false);
                Cam1.SetActive(false);
                Cam3.SetActive(false);
                Cam4.SetActive(false);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) 
            {
                SwitchCamera(3);
                Cam3.SetActive(true);
                Cam2.SetActive(false);
                Cam1.SetActive(false);
                CamMain.SetActive(false);
                Cam4.SetActive(false);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchCamera(4);
                Cam4.SetActive(true);
                Cam3.SetActive(false);
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
            Cam3.SetActive(false);
            Cam2.SetActive(false);
            Cam1.SetActive(false);
            CamMain.SetActive(false);
        }
        mainCamera.gameObject.SetActive(true);
        currentCameraIndex = 0;
       
    }
}