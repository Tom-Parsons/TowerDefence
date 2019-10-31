using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : EnemyController
{

    ParticleSystem ps;
    bool onFire;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ps = GetComponentInChildren<ParticleSystem>();
        target = PlayerController.instance.gameObject;
        positionRadius = 2;
        onFire = false;
        ps.Stop();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isDead)
        {
            return;
        }

        if (agent.path == null || (agent.remainingDistance < 0.3 && (transform.position - target.transform.position).magnitude > 3))
        {
            List<Vector3> positions = PositionsAroundTarget(positionRadius, 20, false);
            Vector3 pos = positions[Random.Range(0, positions.Count - 1)];
            Move(pos);
        }

        if ((transform.position - target.transform.position).magnitude < 4.5f)
        {
            LookAtTarget();
        }

        if (agent.remainingDistance > 0.4)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            if (canAttack)
            {
                Attack();
            }
        }

    }

    public override void TakeDamage(string data)
    {
        print(data);
        if (data.Contains("Fire") && !onFire)
        {
            onFire = true;
            ps.Play();
            StartCoroutine(FireDamage());
        }
    }

    IEnumerator FireDamage()
    {
        yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));

        health -= 1;
        if(health > 0)
        {
            StartCoroutine(FireDamage());
        }
        else
        {
            isDead = true;
            base.Die();
        }
    }

    protected override void Attack()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(AttackDelay());
    }

    protected override IEnumerator AttackDelay()
    {
        canAttack = false;

        //ATTACK
        foreach(GameObject go in gameObject.transform.GetChild(0).GetComponent<AttackCollider>().GetList())
        {
            if (go.tag == "Player" || go.tag == "Statue") go.SendMessage("TakeDamage", "Spider_" + damage, SendMessageOptions.DontRequireReceiver);
        }

        yield return new WaitForSeconds(1);

        canAttack = true;
    }

    protected override void Move(Vector3 position)
    {
        agent.SetDestination(position);
    }

}
