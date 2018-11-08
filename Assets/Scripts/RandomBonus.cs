using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBonus : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] private GameObject[] bonuses;

    private void OnTriggerEnter2D(Collider2D entity)
    {
        Debug.Log("Collision");
        if (bonuses.Length <= 0)
            Debug.Log("Error, there is no bonuses.");
        else
        {
            GameObject go = Instantiate(bonuses[Random.Range(0, bonuses.Length)], transform.position, Quaternion.identity);
            go.GetComponent<CollectableObject>().UseEffect(entity.gameObject);
        }
        Destroy(gameObject);
    }
}