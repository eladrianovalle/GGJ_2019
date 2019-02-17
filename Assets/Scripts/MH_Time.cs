using UnityEngine;

public class MH_Time : MonoBehaviour {

    public static float timestep;
    public static float fixedTimestep;
    public static float unscaledtimestep;
    public static float unscaledFixedTimestep;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }

    private void Update () 
    {
        timestep = Time.deltaTime / 0.01666f;
        unscaledtimestep = Time.unscaledDeltaTime / 0.01666f;
	}

    private void FixedUpdate()
    {
        fixedTimestep = Time.fixedDeltaTime / 0.01666f;
        unscaledFixedTimestep = Time.fixedUnscaledDeltaTime / 0.01666f;
    }
}
