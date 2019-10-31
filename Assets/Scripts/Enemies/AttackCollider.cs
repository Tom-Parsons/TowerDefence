using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{

    private List<GameObject> insideCollider;

    // Start is called before the first frame update
    void Start()
    {
        insideCollider = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Statue" || other.tag == "Enemy")
        {
            AddToList(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Statue" || other.tag == "Enemy")
        {
            RemoveFromList(other.gameObject);
        }
    }

    public void AddToList(GameObject go)
    {
        insideCollider.Add(go);
    }

    public void RemoveFromList(GameObject go)
    {
        insideCollider.Remove(go);
    }

    public List<GameObject> GetList()
    {
        return insideCollider;
    }

}
