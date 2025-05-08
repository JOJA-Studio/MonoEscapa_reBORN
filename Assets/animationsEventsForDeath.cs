using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class animationsEventsForDeath : MonoBehaviour
{
    [SerializeField] GameObject lifeItem;
    [SerializeField] Transform spawnHealthPosition;

    void SpawnLife()
    {
        Instantiate(lifeItem, spawnHealthPosition.position, spawnHealthPosition.rotation);
    }
}
