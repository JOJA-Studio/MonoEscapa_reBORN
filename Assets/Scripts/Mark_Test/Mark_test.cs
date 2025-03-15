using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mark_test : MonoBehaviour
{
    private int _executeCount;
    [SerializeField] private List<GameObject> _markedEnemies;
    [SerializeField] int maxMarked = 3;

    // Update is called once per frame
    void Update()
    {
        Mark();
    }

    private void Mark()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 5, Color.green, 0.5f);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 3))
            {
                Debug.Log("Enemy Hitted");
                enemyRestarVidaTest enemy = hitInfo.transform.GetComponent<enemyRestarVidaTest>();
                if (AlreadyMarked(hitInfo.transform.gameObject) == false)
                {
                    if (_markedEnemies.Count < maxMarked)
                    {
                        _markedEnemies.Add(hitInfo.transform.gameObject);
                        if (enemy != null)
                            enemy.ToggleMark();
                    }
                    else
                        Debug.Log("Hit max amount of marks");
                }
                else
                { 
                    _markedEnemies.Remove(hitInfo.transform.gameObject);
                    if (enemy != null)
                        enemy.ToggleMark();
                }
            }
        }
    }

    private bool AlreadyMarked(GameObject enemyTarget)
    {
        foreach (var enemy in _markedEnemies)
        {
            if (enemyTarget.transform.gameObject == enemy)
            {
                return true;
            }
        }

        return false;
    }
}
