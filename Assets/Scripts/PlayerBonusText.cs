using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBonusText : MonoBehaviour
{
    [SerializeField] private GameObject textMPBonus;

    private void Start()
    {
        RectTransform tr = textMPBonus.GetComponent<RectTransform>();
        Vector3 newPosition = tr.anchoredPosition;

        float pos = transform.position.x * (Screen.width / 2.0f) / 40.0f;
        newPosition.x = -980 + pos;
        tr.anchoredPosition = newPosition; // Set new position
    }

    public void SetBonusText(string message)
    {
        textMPBonus.GetComponent<TextMeshProUGUI>().text = message;
    }
}