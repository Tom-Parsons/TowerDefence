using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (gameObject.tag == "TempTrap")
        {
            animations.Play("Anim_TrapNeedle_Show");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (gameObject.tag != "TempTrap")
        {
            if (insideTrap.Count > 0 && canActivate)
            {
                StartCoroutine(Activate());
            }
            else if (canActivate)
            {
                animations.Play("Anim_TrapNeedle_Idle");
            }
        }
    }

    protected override IEnumerator Activate()
    {
        canActivate = false;

        animations.Play("Anim_TrapNeedle_Show");
        HurtEntities();

        yield return new WaitForSeconds(2);

        StartCoroutine(Deactivate());
    }

    protected override IEnumerator Deactivate()
    {
        animations.Play("Anim_TrapNeedle_Hide");

        yield return new WaitForSeconds(0.1f);

        canActivate = true;
    }

}
