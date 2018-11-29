using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public enum CollectableObjectType
    {
        BONUS,
        MALUS,
    }

    // entity will be the player GameObject.
    protected delegate void Delegate(GameObject entity);

    protected Delegate actionOnTriggerEnter = null;
    protected Delegate actionBeforeDestruction = null;
    protected Delegate actionInNextFrame = null;

    // Time in seconds before destroy gameObject after OnTriggerEnter2D call
    // Set to -1 to avoid the destruction.
    protected int realtimeBeforeDestruction = 0;

    protected CollectableObjectType type = CollectableObjectType.BONUS;

    [SerializeField] public string description = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject entity = collision.gameObject;

        if (entity.tag == "Player")
        {
            if (this.realtimeBeforeDestruction != -1)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            this.UseEffect(entity);
            StartCoroutine(WaitAndDestroy(entity));
        }
    }

    private IEnumerator WaitAndDestroy(GameObject entity)
    {
        if (this.realtimeBeforeDestruction > 0)
            yield return (new WaitForSecondsRealtime(this.realtimeBeforeDestruction));
        if (this.actionBeforeDestruction != null)
            this.actionBeforeDestruction(entity);
        if (this.actionInNextFrame != null)
        {
            yield return (null);
            this.actionInNextFrame(entity);
        }
        if (this.realtimeBeforeDestruction != -1)
            Destroy(gameObject);
    }

    public void UseEffect(GameObject entity)
    {
        if (this.actionOnTriggerEnter != null && this.EffectIsUsableOnTheTarget(entity))
            this.actionOnTriggerEnter(entity);
    }

    bool EffectIsUsableOnTheTarget(GameObject entity)
    {
        PlayerMovement target = entity.GetComponent<PlayerMovement>();

        return (!(this.type == CollectableObjectType.MALUS && target.Invicible));
    }
}