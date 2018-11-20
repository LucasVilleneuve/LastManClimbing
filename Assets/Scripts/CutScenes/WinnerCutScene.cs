using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerCutScene : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] private GameObject endingCanvas;
    [SerializeField] private GameObject winnerTextMeshPro;
    [SerializeField] private AudioSource mainThemeAudioSource;
    //[SerializeField] private float speedToGetToCenter = 25.0f;

    /* Private fields */
    private TextMeshProUGUI textMP;

    private void Awake()
    {
        textMP = winnerTextMeshPro.GetComponent<TextMeshProUGUI>();
    }

    public void Activate(GameObject winner)
    {
        endingCanvas.SetActive(true);
        winnerTextMeshPro.SetActive(true);
        textMP.text = winner.name + " won !";
        mainThemeAudioSource.Stop();
        //Vector3 cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //cameraCenter.z = -1;
        //StartCoroutine(MoveToPosition(winner.transform, winner.transform.position, cameraCenter, speedToGetToCenter));
    }

    private IEnumerator MoveToPosition(Transform tr, Vector3 from, Vector3 to, float speed)
    {
        float step = (speed / (from - to).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            tr.position = Vector3.Lerp(from, to, t); // Move tr closer to position
            yield return new WaitForFixedUpdate(); // Leave the routine and return here in the next frame
        }
        tr.position = to;
    }
}