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
