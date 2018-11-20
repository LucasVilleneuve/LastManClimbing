using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRandomBonus : MonoBehaviour {

    [SerializeField] private GameObject randomBonusPrefab;
    
    void Start () {
        if (randomBonusPrefab)
        {
            GameObject instance = Instantiate(randomBonusPrefab, transform.position, Quaternion.identity);

            instance.transform.parent = this.transform.parent;
            Destroy(gameObject);
        }
    }
	
	void Update () {
		
	}
}
