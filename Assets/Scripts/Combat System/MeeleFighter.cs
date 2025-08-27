using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeeleFighter : MonoBehaviour
{
    [SerializeField] private GameObject sword;

    [SerializeField] private AttackDate[] attackDates;
    private int combatCount = 0;
    private bool doCombat = false;
    public bool InAction { get; private set; } = false;

    private Animator anim;
    private BoxCollider swordColl;
    private AttackState attackState;
    public enum AttackState
    { 
        Idle,
        Windup,
        Impact,
        Cooldown,
    };

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        if(sword!=null)
        {
            swordColl = sword.GetComponent<BoxCollider>();
            swordColl.enabled = false;
        }
    }


    public void TryToAttack()
    {
        if(!InAction)
        {
            StartCoroutine(Attack());
        }
        else if(attackState==AttackState.Cooldown)
        {
            doCombat = true;
        }
    }

    IEnumerator Attack()
    {
        InAction = true;

        anim.CrossFade(attackDates[combatCount].AttackAnimName,0.2f);
        yield return null;

        var animState = anim.GetNextAnimatorStateInfo(1);

        float normalizedTime = 0f;
        float elaspedTime = 0f;
        attackState = AttackState.Windup;
        while(elaspedTime<animState.length)
        {
            elaspedTime += Time.deltaTime;
            normalizedTime = elaspedTime / animState.length;

            if(attackState==AttackState.Windup)
            {
                if (normalizedTime >= attackDates[combatCount].ImpactStartTime)
                {
                    attackState = AttackState.Impact;
                    swordColl.enabled = true;
                }
            }
            else if(attackState==AttackState.Impact)
            {
                if(normalizedTime>= attackDates[combatCount].ImpactEndTime)
                {
                    attackState = AttackState.Cooldown;
                    swordColl.enabled = false;
                }
            }
            else if(attackState==AttackState.Cooldown)
            {
                if(doCombat)
                {
                    Debug.Log("11");
                    doCombat = false;
                    combatCount = (combatCount + 1) % attackDates.Length;
                    StartCoroutine(Attack());

                    yield break;
                }
            }
            yield return null;
        }
        attackState = AttackState.Idle;
        combatCount = 0;
        InAction = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHitbox" && !InAction)
        {
            StartCoroutine(Reaction());
        }
    }
    IEnumerator Reaction()
    {
        InAction = true;
        anim.CrossFade("Reaction", 0.2f);
        yield return null;

        var animState = anim.GetNextAnimatorStateInfo(1);

        yield return new WaitForSeconds(animState.length*0.7f);
        InAction = false;
    }
}
