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

        float pos = Mathf.Floor(transform.position.x / 20.0f);
        float posRelatedToScrWidth = Screen.width / 4 * pos; // Set x position depending on player position
        float startingOffset = -Screen.width / 2 + (Screen.width / 4 / 2);
        newPosition.x = startingOffset + posRelatedToScrWidth;

        tr.anchoredPosition = newPosition; // Set new position
    }

    public void SetBonusText(string message)
    {
        textMPBonus.GetComponent<TextMeshProUGUI>().text = message;
    }
}