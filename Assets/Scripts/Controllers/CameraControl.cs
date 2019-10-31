using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField] GameObject desiredCameraPosition;
    [SerializeField] GameObject desiredPlayerPosition;

    Vector3 startPosition;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, desiredPlayerPosition.transform.position - transform.position, out hit))
        {
            Debug.DrawRay(transform.position, desiredPlayerPosition.transform.position - transform.position);
            if(hit.collider.name != "Player")
            {
                float i = 0.01f;
                bool valid = false;
                while (!valid)
                {
                    Vector3 v3Pos = desiredCameraPosition.transform.position + (desiredPlayerPosition.transform.position - desiredCameraPosition.transform.position) * i;
                    if (Physics.Raycast(v3Pos, desiredPlayerPosition.transform.position - v3Pos, out hit))
                    {
                        if (hit.collider.name == "Player")
                        {
                            position = v3Pos;
                            valid = true;
                        }
                    }
                    if(i > 15)
                    {
                        valid = true;
                        position = desiredCameraPosition.transform.position;
                    }
                    i += 0.01f;
                }
            }
            else
            {
                if (Physics.Raycast(desiredCameraPosition.transform.position, desiredPlayerPosition.transform.position - desiredCameraPosition.transform.position, out hit))
                {
                    if (hit.collider.name == "Player")
                    {
                        position = desiredCameraPosition.transform.position;
                    }
                }
            }
        }
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 20);
    }
}
