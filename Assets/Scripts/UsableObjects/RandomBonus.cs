using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBonus : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] private GameObject[] bonuses;
    [SerializeField] private GameObject animationPrefab;
    private GameObject animationInstance;
    private AudioSource pickupSound;
    //public float DestroyDuration;

    void Start()
    {
        this.animationInstance = Instantiate(animationPrefab, transform.position, Quaternion.identity);
        this.animationInstance.transform.parent = this.transform;
        this.animationInstance.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        pickupSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag != "Player") { return; }
        if (bonuses.Length <= 0)
            Debug.Log("Error, there is no bonuses.");
        else
        {
            Transform tr = animationInstance.transform;
            Animator rd = tr.GetComponent<Animator>();
            rd.Play("30");
            GetComponent<Renderer>().enabled = false;
            GameObject go = Instantiate(bonuses[Random.Range(0, bonuses.Length)], transform.position, Quaternion.identity);
            CollectableObject bonus = go.GetComponent<CollectableObject>();
            bonus.UseEffect(entity.gameObject);
            DestroyAnim(tr);
            StartCoroutine(ShowMessage(bonus.description, entity.gameObject, 2.5f));
            return;
        }
        DestroyBonus();
    }

    private IEnumerator ShowMessage(string message, GameObject player, float delay)
    {
        Debug.Log(player.name);
        PlayerBonusText pbt = player.GetComponent<PlayerBonusText>();
        pbt.SetBonusText(message);
        yield return new WaitForSeconds(delay);
        Debug.Log("Reseting");
        pbt.SetBonusText("");
        DestroyBonus();
    }

    private void DestroyAnim(Transform tr)
    {
        if (tr)
        {
            pickupSound.Play();
            tr.parent = null;
            Destroy(tr.gameObject, tr.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }

    private void DestroyBonus()
    {
        Destroy(gameObject);
    }
}
