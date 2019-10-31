using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    bool alive;

    private Rigidbody rb;

    Animator anim;

    private float forwardSpeed = 300;
    private float sideSpeed = 270;

    private bool canAttack;
    private bool isDead;

    private Vector3 mousePoint;

    [SerializeField] protected Image healthImage;

    [Header("STATS")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected int coins;

    [Header("MAGIC")]
    [SerializeField] protected GameObject staffPosition;
    [SerializeField] protected GameObject fireball;

    [Header("UI")]
    [SerializeField] protected Text coinsText;

    [SerializeField] GameObject coin;

    [SerializeField] Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        alive = true;
        canAttack = true;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        coinsText.text = "Coins: " + coins;
        healthImage.rectTransform.sizeDelta = new Vector2(health * 4f, healthImage.rectTransform.sizeDelta.y);
        if (alive && !BuildMode.instance.isBuildMode())
        {
            if(((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0)) && canAttack)
            {
                rb.velocity = ((rb.transform.forward * forwardSpeed * Time.deltaTime * Input.GetAxis("Vertical")) + (rb.transform.right * sideSpeed * Time.deltaTime * Input.GetAxis("Horizontal")));
                anim.SetBool("IsWalking", true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                anim.SetBool("IsWalking", false);
            }
            
            if(Input.GetAxis("Mouse X") != 0 && canAttack)
            {
                gameObject.transform.Rotate(0, Input.GetAxis("Mouse X") * 100 * Time.deltaTime, 0);
            }

            if (Input.GetButtonUp("Fire1"))
            {
                StartCoroutine(Attack());
            }
            //if(Input.GetButtonDown("Fire1") && !isAiming){
            //    isAiming = true;
            //}else if (Input.GetButtonUp("Fire1") && isAiming)
            //{
            //    isAiming = false;
            //    StartCoroutine(Attack());
            //}

            if (Input.GetKey(KeyCode.F))
            {
                GameObject go = Instantiate(coin, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z), new Quaternion(0, 0, 0, 0));
                //go.transform.position = gameObject.transform.position;
            }

            if (Input.GetKey(KeyCode.N))
            {
                DeathAnimation.instance.BeginAnimation();
            }

        }
        else if(alive && BuildMode.instance.isBuildMode())
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
            {
                Camera cam = BuildMode.instance.BuildCamera;

                Vector3 desired = ((cam.transform.up * Input.GetAxis("Vertical") * -1) + (cam.transform.right * Input.GetAxis("Horizontal") * -1));

                Debug.DrawLine(cam.transform.position, desired, Color.blue);
                cam.transform.position = Vector3.Lerp(cam.transform.position, cam.transform.position - desired, Time.deltaTime * 10);
            }
        } 
    }

    IEnumerator Attack()
    {
        if (canAttack)
        {
            canAttack = false;

            StartCoroutine(Fire());
            anim.SetTrigger("Attack");

            yield return new WaitForSeconds(2);

            canAttack = true;
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(1f);

        //lr.enabled = false;

        foreach (GameObject go in gameObject.transform.GetChild(0).GetComponent<AttackCollider>().GetList())
        {
            if (go != null)
            {
                if (go.tag == "Enemy")
                {
                    go.SendMessage("TakeDamage", "Fireball_" + damage, SendMessageOptions.DontRequireReceiver);
                    GameObject ball = Instantiate(fireball);
                    ball.transform.position = staffPosition.transform.position;
                    ball.transform.LookAt(go.transform.position);
                }
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Trap" || collision.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    public void TakeDamage(string data)
    {
        string[] dataSplit = data.Split('_');
        health -= float.Parse(dataSplit[1]);
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        //RESPAWN
        gameObject.transform.position = new Vector3(CentreStatue.instance.gameObject.transform.position.x, CentreStatue.instance.gameObject.transform.position.y - 2, CentreStatue.instance.gameObject.transform.position.z + 2.5f);
        MessageBroadcaster.instance.Broadcast("You died and so have been resurrected using the power of the statue!", 3);
        health = maxHealth;
        CentreStatue.instance.TakeDamage("Resurrect_25");
    }

    public void AddCoin()
    {
        coins += 10;
    }

    public void SpendCoins(int amount)
    {
        coins -= amount;
    }

    public int getCoins()
    {
        return coins;
    }

    public bool isPlayerDead()
    {
        return isDead;
    }

}
