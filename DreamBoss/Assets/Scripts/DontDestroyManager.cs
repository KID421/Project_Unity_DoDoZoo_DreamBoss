using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DontDestroyManager : MonoBehaviour
{
    public static GameObject instance;

    [Header("在哪些場景不刪除")]
    public string[] scenesDontDestroy;

    private void Start()
    {
        if (instance == null) instance = gameObject;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        string nameScene = SceneManager.GetActiveScene().name;

        var nameSame = scenesDontDestroy.Where(x => x == nameScene);

        if (nameSame.ToList().Count > 0) return;
        else Destroy(gameObject);
    }
}
