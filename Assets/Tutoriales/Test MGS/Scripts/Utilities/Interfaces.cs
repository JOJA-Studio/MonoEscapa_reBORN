using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interfaces : MonoBehaviour
{
    public interface IShootable {
        void OnHit();
        string GetHitFx();
    }

    public interface IPointOfInterest
    {
        bool OnDetect(AIController aIController);
        Transform GetTransform();   
    }
}
