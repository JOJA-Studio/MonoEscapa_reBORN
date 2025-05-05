using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform playerSpawnposition;

    public static LevelManager singleton;
    private void Awake()
    {
        singleton = this;
    }
}