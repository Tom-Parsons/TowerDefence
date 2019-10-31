using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : Trap
{
    private ParticleSystem ps;
    bool canHurt;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ps = GetComponentInChildren<ParticleSystem>();
        canHurt = true;
        ps.Stop();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (gameObject.tag != "TempTrap")
        {
            if (insideTrap.Count > 0 && canActivate)
            {
                StartCoroutine(Activate());
            }else if(insideTrap.Count == 0 && !canActivate)
            {
                StartCoroutine(Deactivate());
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

        ps.Play();
        HurtEntities();

        yield return new WaitForSeconds(2);

    }

    protected override IEnumerator Deactivate()
    {
        ps.Stop();

        yield return new WaitForSeconds(1f);

        canActivate = true;
    }
}
