using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{

    protected GameObject crystal;
    protected GameObject target;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected float positionRadius;

    protected bool canAttack;
    protected bool canMove;
    protected bool isDead;

    [Header("STATS")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damage;

    [Header("COINS")]
    [SerializeField] protected GameObject coinPrefab;
    [SerializeField] protected int maxCoins = 2;
    [SerializeField] protected int minCoins = 1;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        crystal = CentreStatue.instance.gameObject;
        target = crystal;
        animator = GetComponent<Animator>();

        health = maxHealth;

        canAttack = true;
        canMove = true;
        isDead = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected abstract void Attack();
    protected abstract void Move(Vector3 position);

    protected abstract IEnumerator AttackDelay();

    public virtual void TakeDamage(string data)
    {
        string[] dataSplit = data.Split('_');
        health -= float.Parse(dataSplit[1]);
        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        if (EnemySpawnController.instance.enemies.Contains(gameObject))
            EnemySpawnController.instance.enemies.Remove(gameObject);

        int coinsOnDeath = Random.Range(minCoins, maxCoins + 1);

        for(int i = 0; i < coinsOnDeath; i++)
        {
            Instantiate(coinPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z), new Quaternion(0, 0, 0, 0));
        }

        foreach(GameObject go in TrapPlacement.instance.placedTraps)
        {
            if(go.GetComponent<Trap>() != null)
            {
                if (go.GetComponent<Trap>().getInsideTrap().Contains(gameObject))
                {
                    go.GetComponent<Trap>().removeFromTrap(gameObject);
                }
            }
        }

        agent.isStopped = true;
        animator.SetTrigger("Death");
    }

    public void DeathAnim()
    {
        print("animating death");
        StartCoroutine(DisappearAnim());
    }

    IEnumerator DisappearAnim()
    {
        yield return new WaitForSeconds(2);
        //Vector3 targetLoc = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);
        if(gameObject.GetComponent<CapsuleCollider>() != null) gameObject.GetComponent<CapsuleCollider>().enabled = false;
        if (gameObject.GetComponent<SphereCollider>() != null) gameObject.GetComponent<SphereCollider>().enabled = false;
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<NavMeshAgent>());

        float timer = 4;
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
            timer -= Time.deltaTime;
        }

        Destroy(gameObject);
    }

    protected List<Vector3> PositionsAroundTarget(float radius, int numberOfPositions, bool semiCircle)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < numberOfPositions; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfPositions;
            if (semiCircle)
            {
                angle /= 2f;
                angle *= -1;
            }
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius + target.transform.position.x, target.transform.position.y, Mathf.Sin(angle) * radius + target.transform.position.z);
            positions.Add(newPos);
        }
        return positions;
    }

    protected bool CanSeeTargetFromPosition(Vector3 position)
    {
        RaycastHit hit;
        Debug.DrawRay(position, target.gameObject.transform.position - position);
        if (Physics.Raycast(position, target.gameObject.transform.position - position, out hit))
        {
            if (hit.collider.transform.parent != null)
            {
                if (hit.collider.transform.parent.name == target.name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected bool CanSeeTarget()
    {
        return CanSeeTargetFromPosition(transform.position);
    }

    protected void DrawNearTarget(float radius, int numberOfPositions)
    {
        List<Vector3> positions = PositionsAroundTarget(radius, numberOfPositions, false);
        Vector3 prevv = Vector3.zero;
        foreach (Vector3 v in positions)
        {
            if (prevv != Vector3.zero)
            {
                Debug.DrawLine(prevv, v, Color.red);
            }
            prevv = v;
        }
    }

    protected bool LookAtTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, target.gameObject.transform.position - gameObject.transform.position, out hit))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position), Time.deltaTime * 30);
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

}
