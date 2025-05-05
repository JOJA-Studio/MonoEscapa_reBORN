using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class score : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        text.text = $"coins en inventario: {Card.count} / {Card.maxCount}";
    }

    void Update()
    {
        text.text = $"coins en inventario: {Card.count} / {Card.maxCount}";
    }
}
