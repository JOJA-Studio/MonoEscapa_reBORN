using SA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneTrigger : MonoBehaviour
{
    public string targetScene;

    private void OnTriggerEnter(Collider other)
    {
        Controller controller = other.transform.GetComponentInParent<Controller>();
        if(controller != null)
        {
            GameManager.singleton.LoadTargetScene(targetScene, this.transform);
        }
    }
}
