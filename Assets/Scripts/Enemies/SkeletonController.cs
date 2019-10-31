using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : EnemyController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        positionRadius = 1.5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isDead)
        {
            return;
        }

        if(agent.path == null || (transform.position - target.transform.position).magnitude > 4)
        {
            Vector3 offsetTarget = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z + positionRadius);
            Move(offsetTarget);
        }
        else if((agent.remainingDistance < 0.3 && (transform.position - target.transform.position).magnitude <= 4))
        {
            List<Vector3> positions = PositionsAroundTarget(positionRadius, 20, true);
            Vector3 pos = positions[Random.Range(0, positions.Count - 1)];
            Move(pos);
        }

        Debug.DrawLine(transform.position, agent.destination);
        DrawNearTarget(positionRadius, 20);
        if ((transform.position - target.transform.position).magnitude < 4.5f)
        {
            LookAtTarget();
        }

        if (agent.remainingDistance > 0.3)
        {
            animator.SetBool("IsWalking", true);
        }
        else if((transform.position - target.transform.position).magnitude < 4.5f)
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
        base.TakeDamage(data);

        animator.SetTrigger("Hit");
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
        foreach (GameObject go in gameObject.transform.GetChild(0).GetComponent<AttackCollider>().GetList())
        {
            if(go.tag == "Player" || go.tag == "Statue") go.SendMessage("TakeDamage", "Skeleton_" + damage, SendMessageOptions.DontRequireReceiver);
        }

        yield return new WaitForSeconds(2.5f);

        canAttack = true;
    }

    protected override void Move(Vector3 position)
    {
        agent.SetDestination(position);
    }

}
