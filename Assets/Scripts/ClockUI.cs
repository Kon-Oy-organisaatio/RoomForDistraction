using UnityEngine;
using UnityEngine.UI;

public class ClockUI: MonoBehaviour
{
    public Image Background;
    public Image LeftClock;
    public Image RightClock;
    public Image LeftFill;
    public Image RightFill;
    

    public void Start()
    {
        Reset();
    }

    public void Reset()
    {
        RightFill.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        LeftFill.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        RightClock.enabled = true;
        RightFill.enabled = true;
        LeftClock.enabled = false;
        LeftFill.enabled = false;
    }

    public void UpdateClock(float currentTime, float totalTime)
    {
        float percent = currentTime / totalTime;
        float degrees = percent * 360f;
        if (degrees < 0f) degrees = 0f;
        if (degrees > 360f) degrees = 360f;
        if (degrees <= 180f)
        {
            RightFill.transform.rotation = Quaternion.Euler(0f, 0f, degrees);
        }
        else
        {
            RightClock.enabled = false;
            LeftClock.enabled = true;
            LeftFill.enabled = true;
            RightFill.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            LeftFill.transform.rotation = Quaternion.Euler(0f, 0f, degrees - 180f);
        }
    }
}