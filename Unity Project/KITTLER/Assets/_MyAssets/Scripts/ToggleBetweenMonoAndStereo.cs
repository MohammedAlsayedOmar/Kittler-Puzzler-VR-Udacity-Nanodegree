using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR; 


public class ToggleBetweenMonoAndStereo : MonoBehaviour {

    Transform cam;

    void Start()
    {
        cam =  Camera.main.GetComponent<Transform>();
        Debug.Log(VRSettings.supportedDevices[0].ToString());
        StartCoroutine(LoadDevice("cardboard"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (!VRSettings.enabled)
        {
            //UpdateHeadTrackingForVRCameras();
            cam.localRotation = UnityEngine.VR.InputTracking.GetLocalRotation(VRNode.CenterEye);
        }
    }

    private void UpdateHeadTrackingForVRCameras()
    {
        Quaternion pose =
          InputTracking.GetLocalRotation(VRSettings.enabled ? VRNode.Head : VRNode.CenterEye);
        Camera[] cams = Camera.allCameras;
        for (int i = 0; i < cams.Length; i++)
        {
            Camera cam = cams[i];
            if (cam.targetTexture == null && cam.cullingMask != 0)
            {
                cam.GetComponent<Transform>().localRotation = pose;
            }
        }
    }


    public static IEnumerator LoadDevice(string newDevice)
    {
        VRSettings.LoadDeviceByName("cardboard");
        if (newDevice == "Magic Window")
        {
            yield return null;
            VRSettings.enabled = false;
        }
        else
        {

            yield return null;
            VRSettings.enabled = true;
        }
    }
}
