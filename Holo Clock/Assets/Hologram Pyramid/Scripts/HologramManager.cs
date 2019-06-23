using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HologramManager : MonoBehaviour {
    [Header("Customization")]
	public Image Square;
	private Color SquareColor = new Color (1f, 1f, 1f, 1f);
    private bool HideSquare = false;
    public GameObject TapButton;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void TapToHide()
    {
        HideSquare = true;
        TapButton.SetActive(false);
    }

    void Update()
    {
        if (HideSquare)
        {
            if (SquareColor.a != 0f)
            {
                SquareColor.a = SquareColor.a - 0.002f;
                Square.color = new Color(1f, 1f, 1f, SquareColor.a);
            }
        }
    }
}
