using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBonus : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] private GameObject[] bonuses;
    //public float DestroyDuration;

    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag != "Player") { return; }
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

        //Transform tr = GetComponentsInChildren<Transform>()[0];
        Transform tr = transform.Find("30_0");
        Animator rd = tr.GetComponent<Animator>();
        //Animator rd = GetComponentsInChildren<Animator>()[0];
        rd.Play("30");
        tr.parent = null;
        Destroy(gameObject);
        Destroy(tr.gameObject, rd.GetCurrentAnimatorStateInfo(0).length);
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