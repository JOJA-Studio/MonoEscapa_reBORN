using System.Collections;
using UnityEngine;

public interface IICon
{
    GameObject GetObjectForIcon();
    Vector2 GetPivotPosition();
    void IconCreatedCallback(Sprite sprite);
}