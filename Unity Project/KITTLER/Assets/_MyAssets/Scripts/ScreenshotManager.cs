using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour {

    static int screenShotCounter = 0;
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Application.CaptureScreenshot("Screenshot" + (screenShotCounter++).ToString() + ".png");
            Debug.Log("Screenshot" + (screenShotCounter).ToString() + ".png SAVED");
        }
	}
}
