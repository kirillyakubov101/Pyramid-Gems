
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public void Leave()
    {
        SceneHandler.Instance.QuitGame();
    }

}