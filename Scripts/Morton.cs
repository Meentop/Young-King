using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Morton : Hero
{
    [SerializeField] int qSpellDamage;

    [SerializeField] float durationQSpellAnimation;

    public override void StartQSpell()
    {
        if (!spells[0].active && !spells[0].used && !wasToAttack)
        {
            spells[0].active = true;
            heroPanel.UpdateHeroPanel(this);
            OnAttackCells();
        }
        else
        {
            spells[0].active = false;
            heroPanel.UpdateHeroPanel(this);
            main.SetStandardAllCells();
            OnMoveCells();
        }
    }

    public override void StartAttack(Vector2 enemyPos)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, GetDirectionAngle(transform.position, enemyPos), transform.rotation.z);
        if (spells[0].active)
        {
            skinsAnimator.SetTrigger("QSpell");
            StartCoroutine(ToDamage(enemyPos));
        }
        else if(spells[2].active)
        {
            skinsAnimator.SetTrigger("ESpell");
            StartCoroutine(StunEnemy(enemyPos));
        }
        else
        {
            skinsAnimator.SetTrigger("Attack");
            StartCoroutine(ToDamage(enemyPos));
        }
    }

    protected override IEnumerator ToDamage(Vector2 enemyPos)
    {
        if (spells[0].active)
        {
            yield return new WaitForSeconds(durationQSpellAnimation);
            main.GetEnemyOnCell((int)enemyPos.x, (int)enemyPos.y).GetDamage(qSpellDamage);
            spells[0].active = false;
            spells[0].used = true;
        }
        else
        {
            yield return new WaitForSeconds(durationAnimationAttack);
            main.GetEnemyOnCell((int)enemyPos.x, (int)enemyPos.y).GetDamage(damage);
        }
        wasToAttack = true;
    }


    [SerializeField] Image shieldImg;
    [SerializeField] float durationWSpellAnimation;

    public override void StartWSpell()
    {
        StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        skinsAnimator.SetTrigger("WSpell");
        yield return new WaitForSeconds(durationWSpellAnimation);
        shieldImg.gameObject.SetActive(true);
        spells[1].used = true;
        heroPanel.UpdateHeroPanel(this);
        main.SetNoactiveAllHeroes();
        main.EndStep();
    }

    public override void GetDamage(int damage)
    {
        if(!shieldImg.gameObject.activeSelf)
            hp -= damage;
        else
            shieldImg.gameObject.SetActive(false);
        if (hp <= 0)
        {
            main.heroes.Remove(GetComponent<Hero>());
            Destroy(gameObject);
        }
        UpdateHPInfo();
    }

    [SerializeField] int stunDuration;

    [SerializeField] float durationESpellAnimation;

    public override void StartESpell()
    {
        if (!spells[2].active && !spells[2].used)
        {
            spells[2].active = true;
            heroPanel.UpdateHeroPanel(this);
            OnAttackCells();
        }
        else
        {
            spells[2].active = false;
            heroPanel.UpdateHeroPanel(this);
            main.SetStandardAllCells();
            OnMoveCells();
        }
    }

    IEnumerator StunEnemy(Vector2 enemyPos)
    {
        yield return new WaitForSeconds(durationESpellAnimation);
        main.GetEnemyOnCell((int)enemyPos.x, (int)enemyPos.y).AddEffect(Effect.stunned, stunDuration);
        spells[2].active = false;
        spells[2].used = true;
        main.EndStep();
        SetCurrentAllCells();
        main.SetNoactiveAllHeroes();
    }
}
