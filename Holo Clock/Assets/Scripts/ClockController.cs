using UnityEngine;
using System.Collections;
using TMPro;

public class ClockController : MonoBehaviour {

	//-- set start time 00:00
    public int minutes = 0;
    public int hour = 0;
	public int seconds = 0;
	public bool realTime=true;
    public bool displayTime = true;
    public bool displaySeconds = false;
    [SerializeField]
    Gradient[] gradient;

    public GameObject pointerSeconds;
    public GameObject pointerMinutes;
    public GameObject pointerHours;
    
    //-- time speed factor
    public float clockSpeed = 1.0f;     // 1.0f = realtime, < 1.0f = slower, > 1.0f = faster

    //-- internal vars
    float msecs=0;
    TextMeshPro tm;
    string tempMinutes = "";
    string tempHour = "";
    string tempSeconds = "";
    string amPM = "am";
    ColorChange colorChange = new ColorChange();

void Start() 
{
    tm = GetComponent<TextMeshPro>();

	//-- set real time
	if (realTime)
	{
		hour=System.DateTime.Now.Hour;
		minutes=System.DateTime.Now.Minute;
		seconds=System.DateTime.Now.Second;
	}

    colorChange.StartColorChange(gradient);
        //tm.colorGradient = new VertexGradient(Color.red, Color.cyan, Color.blue, Color.green);
    }

    void Update()
    {
        tm.color = colorChange.GetColor();

        //if(tm.color == gradient.colorKeys[gradient.colorKeys.Length-1].color)
        //    colorChange.StartColorChange(gradient);

        //-- calculate time
        msecs += Time.deltaTime * clockSpeed;
        if (msecs >= 1.0f)
        {
            msecs -= 1.0f;
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if (minutes > 60)
                {
                    minutes = 0;
                    hour++;
                    if (hour >= 24)
                        hour = 0;
                }
            }
        }

        if (displayTime)
        {
            tempHour = hour.ToString();
            tempMinutes = minutes.ToString();
            tempSeconds = seconds.ToString();

            if (hour > 12)
            {
                tempHour = (hour - 12).ToString();
                amPM = "pm";
            }
            else
            {
                tempHour = hour.ToString();

                amPM = "am";
            }

            if (minutes < 10)
                tempMinutes = "0" + minutes.ToString();
            else
                tempMinutes = minutes.ToString();

            if (seconds < 10)
                tempSeconds = "0" + seconds.ToString();
            else
                tempSeconds = seconds.ToString();

            tm.text = tempHour + ":" + tempMinutes;

            if (displaySeconds)
                tm.text += ":" + tempSeconds;

            tm.text += " " + amPM;
        }
        else
        {
            //-- calculate pointer angles
            float rotationSeconds = (360.0f / 60.0f) * seconds;
            float rotationMinutes = (360.0f / 60.0f) * minutes;
            float rotationHours = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

            //-- draw pointers
            pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
            pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
            pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationHours);
        }
    }

    void OnGUI()
    {
        //if(displayTime)
        //    GUI.Label(new Rect(Screen.width / 2 - 50, 3, 100, 50), hour +":" + minutes + ":"+ seconds, "box");
    }
}
