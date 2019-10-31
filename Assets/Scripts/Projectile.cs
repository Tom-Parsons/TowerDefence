using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //EXPLODE
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

}
