using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterTrap : Trap
{

    bool canHurt;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        canHurt = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (gameObject.tag != "TempTrap")
        {
            if (insideTrap.Count > 0 && canActivate)
            {
                StartCoroutine(Activate());
            }
            else if (canActivate)
            {
                animations.Play("Anim_TrapCutter_Stop");
            }
        }
        if (canActivate == false && canHurt)
        {
            StartCoroutine(Hurt());
        }
    }

    IEnumerator Hurt()
    {
        canHurt = false;

        HurtEntities();

        yield return new WaitForSeconds(0.6f);

        canHurt = true;
    }

    protected override IEnumerator Activate()
    {
        canActivate = false;

        animations.Play("Anim_TrapCutter_Play");
        HurtEntities();

        yield return new WaitForSeconds(2);

        StartCoroutine(Deactivate());
    }

    protected override IEnumerator Deactivate()
    {
        animations.Play("Anim_TrapCutter_Stop");

        yield return new WaitForSeconds(1f);

        canActivate = true;
    }
}
