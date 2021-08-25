using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Windows.WebCam;

public class NewBehaviourScript : MonoBehaviour
{

    public Button btn;
    public InputField inputCamName;

    string Camname = "FHD Camera";

    public RawImage display;
    WebCamTexture camTexture;
    private int currentIndex = 0;

    void InputCam()
    {
        Camname = inputCamName.text;

        camTexture = new WebCamTexture(Camname);
        display.texture = camTexture;
        camTexture.Play();
    }

    private void Start()
    {
        btn.onClick.AddListener(InputCam);

        if (camTexture != null)
        {
            display.texture = null;
            camTexture.Stop();
            camTexture = null;
        }
        WebCamDevice device = WebCamTexture.devices[currentIndex];
        // camTexture = new WebCamTexture("FHD Camera");
        camTexture = new WebCamTexture(Camname);
        display.texture = camTexture;
        camTexture.Play();
    }
}