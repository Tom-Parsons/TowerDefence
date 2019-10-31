using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Name: " + other.name + " Tag: " + other.tag);
        if(other.tag != "TempTrap") TrapPlacement.instance.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "TempTrap") TrapPlacement.instance.Remove(other.gameObject);
    }

}
