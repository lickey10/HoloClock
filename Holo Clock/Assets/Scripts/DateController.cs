using UnityEngine;
using System.Collections;
using TMPro;

public class DateController : MonoBehaviour {

	//-- set start time 00:00
    public int day = 0;
    public int month = 0;
	public int year = 0;
	public bool realTime=true;
    public bool displayDate = true;
	
	public GameObject pointerSeconds;
    public GameObject pointerMinutes;
    public GameObject pointerHours;

    //-- internal vars
    float msecs=0;
    TextMeshPro tm;
    int counter = 0;

void Start() 
{
    tm = GetComponent<TextMeshPro>();

	//-- set real time
	if (realTime)
	{
		month=System.DateTime.Now.Month;
		day=System.DateTime.Now.Day;
		year=System.DateTime.Now.Year;
	}
}

    void Update()
    {
        if (realTime)
        {
            counter++;

            if (counter > 1000000)//refresh values every so often
            {
                month = System.DateTime.Now.Month;
                day = System.DateTime.Now.Day;
                year = System.DateTime.Now.Year;

                counter = 0;
            }
        }

        if (displayDate)
        {
            tm.text = month + "/" + day +"/"+ year;
        }
        else
        {
            //-- calculate pointer angles
            float rotationSeconds = (360.0f / 60.0f) * year;
            float rotationMinutes = (360.0f / 60.0f) * day;
            float rotationHours = ((360.0f / 12.0f) * month) + ((360.0f / (60.0f * 12.0f)) * day);

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
