using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeeleFighter : MonoBehaviour
{
    [SerializeField] private GameObject sword;
    public bool InAction { get; private set; } = false;

    private Animator anim;

    private BoxCollider swordColl;

    private AttackState attackState;
    public enum AttackState
    { 
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
    }

    IEnumerator Attack()
    {
        InAction = true;

        anim.CrossFade("Slash",0.2f);
        yield return null;

        var animState = anim.GetNextAnimatorStateInfo(1);

        float impactStartTime = 0.3f;
        float impactEndTime = 0.5f;
        float normalizedTime = 0f;
        float elaspedTime = 0f;
        attackState = AttackState.Windup;
        while(elaspedTime<animState.length)
        {
            elaspedTime += Time.deltaTime;
            normalizedTime = elaspedTime / animState.length;

            if(attackState==AttackState.Windup)
            {
                if(normalizedTime>=impactStartTime)
                {
                    attackState = AttackState.Impact;
                    swordColl.enabled = true;
                }
            }
            else if(attackState==AttackState.Impact)
            {
                if(normalizedTime>=impactEndTime)
                {
                    attackState = AttackState.Cooldown;
                    swordColl.enabled = false;
                }
            }
            else if(attackState==AttackState.Cooldown)
            {
                //combat
            }
            yield return null;
        }
        InAction = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("reaction");
        if (other.tag == "PlayerHitbox" && !InAction)
        {
            StartCoroutine(Reaction());
        }
    }
    IEnumerator Reaction()
    {
        InAction = true;
        Debug.Log("dd");
        anim.CrossFade("Reaction", 0.2f);
        yield return null;

        var animState = anim.GetNextAnimatorStateInfo(1);

        yield return new WaitForSeconds(animState.length);
        InAction = false;
    }
}
