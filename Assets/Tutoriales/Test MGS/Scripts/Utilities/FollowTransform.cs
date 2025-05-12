using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform targetTransform;
    Transform mTransform;

    private void Start()
    {
        mTransform = this.transform;
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            this.enabled = false;
            return;
        }

        mTransform.position = transform.position;
    }
}
