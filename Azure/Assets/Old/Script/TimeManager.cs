using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.25f;
    public float slowdownLength = 2f;

    //private bool isSlowMotion = false;

    //private void Update()
    //{
    //    if (isSlowMotion)
    //    {
    //        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
    //        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    //    }
    //}

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor ;
        Time.fixedDeltaTime = Time.timeScale * 0.05f;

        //isSlowMotion = true;
        //Time.timeScale = slowdownFactor;

        //Physics.autoSimulation = false;
    }

    public void ReCoverNormol()
    {
        //isSlowMotion = false;

        //Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        //Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        Time.timeScale = 1f;
        //Invoke("NormelTime",0.2f);
        //Time.fixedDeltaTime = 1;

        //Physics.autoSimulation = true;
    }

    void NormelTime()
    {
        Time.timeScale = 1;
    }

}
