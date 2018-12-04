using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour {

    [SerializeField] public int isLight = 0;
    public Light torchLight;
    public Transform flameTransform;
    [SerializeField] private int extend = 5;

    private IEnumerator TorchDown()
    {
        for (int i = 14 * extend; i > 0; i--)
        {
            torchLight.range = i / extend;
            flameTransform.localScale -= new Vector3(0.1f / extend, 0.1f / extend);
            flameTransform.position -= new Vector3(0, 0.14f / extend);
            yield return null;
        }
        flameTransform.gameObject.SetActive(false);
        isLight = 2;

    }
	
	// Update is called once per frame
	public void LightDone()
    {
		if (isLight == 1)
        {
            StartCoroutine(TorchDown());
            isLight = 1;
        }
	}
}
