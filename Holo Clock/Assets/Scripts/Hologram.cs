using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hologram : MonoBehaviour
{
    public GameObject[] Holograms;
    public UILabel HologramNumDisplay;
    int selectedHologramIndex = 0;
    GameObject currentHologram = null;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate selected hologram in this gameobjects location
        currentHologram = Instantiate(Holograms[selectedHologramIndex], transform.position, Quaternion.identity);

        HologramNumDisplay.text = (selectedHologramIndex + 1).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HologramNext()
    {
        Destroy(currentHologram);

        int tempIndex = selectedHologramIndex + 1;

        if (tempIndex <= Holograms.Length - 1)
            selectedHologramIndex = tempIndex;
        else
            selectedHologramIndex = 0;

        //Instantiate selected hologram in this gameobjects location
        currentHologram = Instantiate(Holograms[selectedHologramIndex], transform.position, Quaternion.identity);

        HologramNumDisplay.text = (selectedHologramIndex + 1).ToString();
    }

    public void HologramPrev()
    {
        Destroy(currentHologram);

        int tempIndex = selectedHologramIndex - 1;

        if (tempIndex >= 0)
            selectedHologramIndex = tempIndex;
        else
            selectedHologramIndex = Holograms.Length - 1;

        //Instantiate selected hologram in this gameobjects location
        currentHologram = Instantiate(Holograms[selectedHologramIndex], transform.position, Quaternion.identity);

        HologramNumDisplay.text = (selectedHologramIndex + 1).ToString();
    }


}
