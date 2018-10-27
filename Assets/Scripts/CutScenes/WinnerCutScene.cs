using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerCutScene : MonoBehaviour {

	/* Serialized fields */
	[SerializeField] GameObject winnerTextMeshPro;


	/* Private fields */
	private TextMeshProUGUI textMP;

	private void Awake()
	{
		textMP = winnerTextMeshPro.GetComponent<TextMeshProUGUI>();
	}

	public void Activate(GameObject winner)
	{
		winnerTextMeshPro.SetActive(true);
		textMP.text = winner.name + " won !";
	}

}
