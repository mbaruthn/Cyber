using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    private void Start()
    {
        // Force the game to start in LandscapeLeft mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Update()
    {
        // Continuously check the screen orientation
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            Debug.LogWarning("The game only supports landscape mode.");
            ForceLandscape();
        }
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            Debug.LogWarning("The game only supports landscape mode. Please rotate your device.");
            Time.timeScale = 0; // Oyunu duraklat
        }
        else
        {
            Time.timeScale = 1; // Oyunu devam ettir
        }
    }

    private void ForceLandscape()
    {
        // Force the screen orientation back to LandscapeLeft
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
