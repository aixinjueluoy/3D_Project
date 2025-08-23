using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleFighter : MonoBehaviour
{
    public bool InAction { get; private set; } = false;

    private Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
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

        yield return new WaitForSeconds(animState.length);
        Debug.Log(animState.length);
        InAction = false;
    }
}
