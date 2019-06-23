using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField]
    Gradient[] gradient;
    [SerializeField]
    float duration = 10f;
    float t = 0f;
    public Color ColorCurrent;
    bool colorGoingUp = true; //the direction we are going through the gradient
    int gradientIndex = 0;

    void Start()
    {
        

        //color lerp example
        //color1 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public void StartColorChange(Gradient[] newGradient)
    {
        if (newGradient == null)
        {
            var colorkeys = new GradientColorKey[2];
            var alphakeys = new GradientAlphaKey[2];

            colorkeys[0].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            colorkeys[0].time = 0f;
            colorkeys[1].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            colorkeys[1].time = 1f;

            alphakeys[0].alpha = 1f;
            alphakeys[0].time = 0f;
            alphakeys[1].alpha = 1f;
            alphakeys[1].time = 1f;

            gradient[0].SetKeys(colorkeys, alphakeys);
        }
        else
        {
            gradient = newGradient;

            //t = 0;
        }
    }

    public Color GetColor()
    {
        float value = Mathf.Lerp(0f, 1f, t);

        if (colorGoingUp)
            t += Time.deltaTime / duration;
        else
            t -= Time.deltaTime / duration;

        Color tempColor = gradient[gradientIndex].Evaluate(value);

        if (colorGoingUp && tempColor == gradient[gradientIndex].colorKeys[gradient[gradientIndex].colorKeys.Length - 1].color)//we have cycled all the way through the gradient
            colorGoingUp = false;

        if (!colorGoingUp && tempColor == gradient[gradientIndex].colorKeys[0].color)
            ChangeGradient();

        return tempColor;
    }

    private void ChangeGradient()
    {
        gradientIndex++;

        if (gradientIndex > gradient.Length - 1)
            gradientIndex = 0;

        colorGoingUp = true;
    }
}
