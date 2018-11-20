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
        if (bonuses.Length <= 0)
            Debug.Log("Error, there is no bonuses.");
        else
        {
            Transform tr = transform.Find("30_0");
            Animator rd = tr.GetComponent<Animator>();
            rd.Play("30");
            GetComponent<Renderer>().enabled = false;
            GameObject go = Instantiate(bonuses[Random.Range(0, bonuses.Length)], transform.position, Quaternion.identity);
            CollectableObject bonus = go.GetComponent<CollectableObject>();
            bonus.UseEffect(entity.gameObject);
            StartCoroutine(ShowMessage(bonus.description, entity.gameObject, 2.5f, tr));
            return;
        }
        DestroyBonus(null);
    }

    private IEnumerator ShowMessage(string message, GameObject player, float delay, Transform tr)
    {
        Debug.Log(player.name);
        PlayerBonusText pbt = player.GetComponent<PlayerBonusText>();
        pbt.SetBonusText(message);
        yield return new WaitForSeconds(delay);
        Debug.Log("Reseting");
        pbt.SetBonusText("");
        DestroyBonus(tr);
    }

    private void DestroyBonus(Transform tr)
    {
        if (tr)
        {
            tr.parent = null;
            Destroy(tr.gameObject, tr.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(gameObject);
    }
}