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
            CollectableObject bonus = go.GetComponent<CollectableObject>();
            StartCoroutine(ShowMessage(bonus.description, entity.gameObject, 2.5f));
            bonus.UseEffect(entity.gameObject);
        }
        Destroy(gameObject);
    }

    private IEnumerator ShowMessage(string message, GameObject player, float delay)
    {
        Debug.Log(player.name);
        PlayerBonusText pbt = player.GetComponent<PlayerBonusText>();
        pbt.SetBonusText(message);
        yield return new WaitForSeconds(delay);
        pbt.SetBonusText("");
    }
}