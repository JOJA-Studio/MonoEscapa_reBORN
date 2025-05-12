using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        text.text = $"Cards: {Card.count} / {Card.maxCount}";
    }

    void Update()
    {
        text.text = $"Cards: {Card.count} / {Card.maxCount}";
    }
}
