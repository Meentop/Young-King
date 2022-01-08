using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureEffector : MonoBehaviour
{
    Creature creature;

    [SerializeField] EffectCell effectCell;

    [SerializeField] List<Sprite> effectImages;

    [SerializeField] float distanceBetweenCells;

    private void Awake()
    {
        creature = transform.parent.parent.GetComponent<Creature>();
    }

    public void UpdateEffectsPanel()
    {
        if (gameObject != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < creature.effects.Count; i++)
            {
                Vector3 pos = transform.position + (transform.right * i * distanceBetweenCells);
                effectCell.SetImageAndNumber(effectImages[(int)creature.effects[i].effect], creature.effects[i].duration);
                Instantiate(effectCell.gameObject, pos, transform.rotation, transform);
            }
        }
    }
}

public enum Effect
{
    stunned
}