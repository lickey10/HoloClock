using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public UILabel WidthDisplay;
    public UILabel HeightDisplay;
    public UILabel CompletionDisplay;
    public Material[] Skyboxes;

    int minWidth = 3;
    int maxWidth = 20;
    int minHeight = 3;
    int maxHeight = 20;
    int currentSkyboxIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(Skyboxes != null && Skyboxes.Length > 0)
            RenderSettings.skybox = Skyboxes[currentSkyboxIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkyboxesNext()
    {
        if (Skyboxes != null && Skyboxes.Length > 0)
        {
            int tempIndex = currentSkyboxIndex + 1;

            if (tempIndex >= Skyboxes.Length)
                tempIndex = 0;

            currentSkyboxIndex = tempIndex;

            RenderSettings.skybox = Skyboxes[currentSkyboxIndex];
        }
    }

    public void SkyboxesPrev()
    {
        if (Skyboxes != null && Skyboxes.Length > 0)
        {
            int tempIndex = currentSkyboxIndex - 1;

            if (tempIndex < 0)
                tempIndex = Skyboxes.Length - 1;

            currentSkyboxIndex = tempIndex;

            RenderSettings.skybox = Skyboxes[currentSkyboxIndex];
        }
    }

    
}
