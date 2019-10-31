using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{

    protected List<GameObject> insideTrap;

    protected Animation animations;

    protected bool canActivate;

    [SerializeField] protected float damage;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        insideTrap = new List<GameObject>();
        animations = GetComponent<Animation>();
        canActivate = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        for (var i = insideTrap.Count - 1; i > -1; i--)
        {
            if (insideTrap[i] == null)
                insideTrap.RemoveAt(i);
        }
    }

    protected abstract IEnumerator Activate();
    protected abstract IEnumerator Deactivate();

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(other.gameObject.tag == "Enemy")
        {
            insideTrap.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            insideTrap.Remove(other.gameObject);
        }
    }

    protected void HurtEntities()
    {
        try
        {
            foreach (GameObject obj in insideTrap)
            {
                if (obj != null)
                    obj.SendMessage("TakeDamage", gameObject.name + "_" + damage, SendMessageOptions.DontRequireReceiver);
            }
        }catch (System.InvalidOperationException){ }
    }

    public void removeFromTrap(GameObject go)
    {
        insideTrap.Remove(go);
    }

    public List<GameObject> getInsideTrap()
    {
        return insideTrap;
    }

}
