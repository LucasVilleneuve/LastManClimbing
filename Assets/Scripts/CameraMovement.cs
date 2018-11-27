using System.Collections;
using UnityEngine;
using TMPro;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float[] speedUpLevels = { 70.0f, 80.0f, 90.0f, 100.0f, 105.0f, 110.0f, 115.0f, 120.0f, 125.0f, 150.0f };
    [SerializeField] private float levelLength = 20.0f;
    [SerializeField] private float timeBeforeStarting = 3.0f;
    [SerializeField] private GameObject textSpeedUpLevel;
    [SerializeField] private GameObject textCounterSpeedUp;
    [SerializeField] private GameObject textStartCounter;

    // Debug
    [SerializeField] private float cameraSpeed;

    private bool cameraMovementEnable = true;
    private int wave = 0;
    private float timeLeft = 0;
    private TextMeshProUGUI textMPCounterSU;
    private TextMeshProUGUI textMPStartCounter;
    private float coolDown = 0.0f;

    private void Start()
    {
        textSpeedUpLevel.SetActive(false);
        textMPCounterSU = textCounterSpeedUp.GetComponent<TextMeshProUGUI>();
        textMPStartCounter = textStartCounter.GetComponent<TextMeshProUGUI>();

        timeLeft = levelLength;
        TriggerSpeedUp();
    }

    public void SetCameraSpeed(float newSpeed)
    {
        cameraSpeed = newSpeed;
    }

    public float GetCameraSpeed()
    {
        return cameraSpeed;
    }

    private void Update()
    {
        if (cameraMovementEnable)
        {
            if (timeBeforeStarting > 0.0f)
            {
                timeBeforeStarting -= Time.deltaTime;

                int timeCounter = (int)Mathf.Ceil(timeBeforeStarting);

                if (timeCounter > 0)
                {
                    textMPStartCounter.text = timeCounter.ToString();
                }
                else
                {
                    StartCoroutine(ShowGoOnStartCounter(1.0f));
                }
            }
            else
            {
                UpdateTime();

                if (timeLeft < 0.0f)
                {
                    TriggerSpeedUp();
                    timeLeft = levelLength;
                }

                transform.position = transform.position + new Vector3(0, cameraSpeed / 1000.0f, 0);
            }
        }
    }

    private void UpdateTime()
    {
        timeLeft -= Time.deltaTime;
        coolDown -= Time.deltaTime;

        if (timeLeft > 0.0f)
        {
            if (timeLeft < 3.0f)
            {
                textSpeedUpLevel.SetActive(true);
                textMPCounterSU.text = Mathf.Ceil(timeLeft).ToString();
            }
        }
        else
        {
            // Deactivate
            textSpeedUpLevel.SetActive(false);
            textMPCounterSU.text = "";
        }
        string textCounter = textMPCounterSU.text;
    }

    private void TriggerSpeedUp()
    {
        if (speedUpLevels.Length == 0 || wave >= speedUpLevels.Length)
        {
            return; // No more waves to do
        }

        cameraSpeed = speedUpLevels[wave];
        ++wave;
    }

    public void EnableMovement(bool enable)
    {
        cameraMovementEnable = enable;
    }

    public void SpeedUp()
    {
        if (coolDown > 0.0f) // Cooldown not over
            return;

        if (timeLeft > 3.0f)
        {
            timeLeft = 3.0f; // Activate next wave in 3 seconds
        }

        coolDown = 5.0f; // Set cooldown to not spam PlayNextWave
    }

    private IEnumerator ShowGoOnStartCounter(float timeToShow)
    {
        textMPStartCounter.text = "GO";
        yield return new WaitForSeconds(timeToShow);
        textMPStartCounter.text = "";
    }
}