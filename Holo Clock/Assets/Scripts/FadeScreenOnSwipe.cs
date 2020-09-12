using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenOnSwipe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DimmScreen(bool dimmScreen)
    {
        if (Lean.Touch.LeanTouch.Fingers.Count > 1)//dimm / brighten the screen
        {
            float swipeDistance = LeanTouch.Fingers[0].SwipeScreenDelta.magnitude;

            while (swipeDistance > 1)
                swipeDistance = swipeDistance / 10;

            if (dimmScreen)
                Gamekit3D.ScreenFader.SetAlpha(Gamekit3D.ScreenFader.Instance.faderCanvasGroup.alpha + swipeDistance);
            else
                Gamekit3D.ScreenFader.SetAlpha(Gamekit3D.ScreenFader.Instance.faderCanvasGroup.alpha - swipeDistance);
        }
    }
}
