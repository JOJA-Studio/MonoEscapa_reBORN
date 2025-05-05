using System.Collections;
using System.Collections.Generic;
using SA;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    Controller playerController;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
            playerController = GameObject.FindObjectOfType<Controller>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        InitializeControllerForScene(LevelManager.singleton.playerSpawnposition);
    }

    public void LoadTargetScene(string stringName)
    { 
        StartCoroutine(LoadScene(stringName));
    }

    IEnumerator LoadScene(string sceneName)
    { 
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        if (LevelManager.singleton != null)
        {
            InitializeControllerForScene(LevelManager.singleton.playerSpawnposition);
        }
    }

    void InitializeControllerForScene(Transform spawnPosition)
    {
        if (playerController == null)
        { 
            GameObject go = Instantiate(ResourcesManager.singleton.playerPrefab) as GameObject;
            playerController = go.GetComponentInChildren<Controller>();
        }

        playerController.mtransform.position = spawnPosition.position;
        playerController.mtransform.transform.rotation = spawnPosition.rotation;
    }
}
