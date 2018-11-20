using UnityEngine;
using TMPro;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float[] wavesSpeed = { 70.0f, 80.0f, 90.0f, 100.0f, 105.0f, 110.0f, 115.0f, 120.0f, 125.0f, 150.0f };
    [SerializeField] private float waveLength = 20.0f;
    [SerializeField] private float timeBeforeStarting = 3.0f;
    [SerializeField] private GameObject textNextWave;
    [SerializeField] private GameObject textCounterWave;

    // Debug
    [SerializeField] private float cameraSpeed;

    private bool cameraMovementEnable = true;
    private int wave = 0;
    private float timeLeft = 0;
    private TextMeshProUGUI textMPCounterWave;

    private void Start()
    {
        textNextWave.SetActive(false);
        textMPCounterWave = textCounterWave.GetComponent<TextMeshProUGUI>();

        timeLeft = waveLength;
        ActivateNextWave();
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
            }
            else
            {
                UpdateTime();

                if (timeLeft < 0.0f)
                {
                    ActivateNextWave();
                    timeLeft = waveLength;
                }

                transform.position = transform.position + new Vector3(0, cameraSpeed / 1000.0f, 0);
            }
        }
    }

    private void UpdateTime()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft > 0.0f)
        {
            if (timeLeft < 3.0f)
            {
                textNextWave.SetActive(true);
                textMPCounterWave.text = Mathf.Ceil(timeLeft).ToString();
            }
        }
        else
        {
            // Deactivate
            textNextWave.SetActive(false);
            textMPCounterWave.text = "";
        }
        string textCounter = textMPCounterWave.text;
    }

    private void ActivateNextWave()
    {
        if (wavesSpeed.Length == 0 || wave > wavesSpeed.Length)
        {
            return; // No more waves to do
        }

        cameraSpeed = wavesSpeed[wave];
        ++wave;
    }

    public void EnableMovement(bool enable)
    {
        cameraMovementEnable = enable;
    }
}