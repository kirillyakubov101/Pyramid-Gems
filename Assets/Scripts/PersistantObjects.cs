using UnityEngine;

public class PersistantObjects : MonoBehaviour
{
    private void Awake()
    {
        var containers = FindObjectsOfType<PersistantObjects>();
        if(containers.Length > 1)
        {
            for(int i = 1; i < containers.Length; i++)
            {
                Destroy(containers[i].gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

    }
}
