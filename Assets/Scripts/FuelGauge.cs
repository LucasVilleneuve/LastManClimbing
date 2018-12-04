using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelGauge : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] private GameObject fuelGaugeImage;

    /* Private fields */
    private float fuelLeft = 1.0f;
    private Image fuelImg;

    private void Start()
    {
        if (fuelGaugeImage)
            fuelImg = fuelGaugeImage.GetComponent<Image>();
    }

    private void UpdateFuelImage()
    {
        fuelImg.fillAmount = fuelLeft;
    }

    public void ConsumeFuel(float amount)
    {
        if (fuelLeft <= 0.0f || amount <= 0.0f) return;

        fuelLeft -= amount;
        if (fuelLeft < 0.0f) fuelLeft = 0.0f;

        UpdateFuelImage();
    }

    public void RefillFuel(float amount)
    {
        if (fuelLeft >= 1.0f || amount <= 0.0f) return;

        fuelLeft += amount;
        if (fuelLeft > 1.0f) fuelLeft = 1.0f;

        UpdateFuelImage();
    }

    public float GetFuelLeft()
    {
        return fuelLeft;
    }
}