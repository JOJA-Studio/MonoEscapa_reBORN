using System.Collections;
using System.Collections.Generic;
using SA;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    InputHandler playerController;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
            playerController = GameObject.FindObjectOfType<InputHandler>();
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

    public void LoadTargetScene(string stringName, Transform targetTrigger)
    {
        StartCoroutine(LoadSceneAndMovePlayer(stringName, targetTrigger));
    }

    Vector3 targetDirection;
    bool movePlayer;

    private void Update()
    {
        if (movePlayer)
        {
            playerController.transform.position += playerController.transform.forward / Time.deltaTime * 2;
            playerController.controller.animator.SetFloat("movement", 1);
        }
    }

    IEnumerator LoadSceneAndMovePlayer(string sceneName, Transform target)
    {
        playerController.enabled = false;
        targetDirection = target.forward;
        playerController.transform.rotation = target.rotation;

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        if (LevelManager.singleton != null)
        {
            InitializeControllerForScene(LevelManager.singleton.playerSpawnposition);
        }
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
            playerController = go.GetComponentInChildren<InputHandler>();
        }

        playerController.transform.position = spawnPosition.position;
        playerController.transform.transform.rotation = spawnPosition.rotation;
    }
}
