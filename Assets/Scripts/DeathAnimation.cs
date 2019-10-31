using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{

    public static DeathAnimation instance;

    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Camera BuildCamera;
    [SerializeField] private Camera DeathCamera;

    private const int pointsInRadius = 25;
    private bool animating;
    private List<Vector3> points;
    private int nextPoint;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        points = new List<Vector3>();
        nextPoint = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            print((DeathCamera.transform.position - points[nextPoint]).magnitude);
            DeathCamera.transform.LookAt(CentreStatue.instance.transform);
            if ((DeathCamera.transform.position - points[nextPoint]).magnitude > 0.1)
            {
                DeathCamera.transform.position = Vector3.MoveTowards(DeathCamera.transform.position, points[nextPoint], Time.deltaTime * 8);
            }
            else
            {
                nextPoint++;
                if (nextPoint >= 100) nextPoint = 0;
            }
        }
    }

    public void BeginAnimation()
    {
        animating = true;

        PlayerCamera.enabled = false;
        BuildCamera.enabled = false;
        DeathCamera.enabled = true;

        for (int i = 0; i < pointsInRadius; i++)
        {
            float angle = i * Mathf.PI * 2f / pointsInRadius;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * 19 + CentreStatue.instance.gameObject.transform.position.x, CentreStatue.instance.gameObject.transform.position.y, Mathf.Sin(angle) * 16 + CentreStatue.instance.gameObject.transform.position.z);
            points.Add(newPos);
            print("added " + newPos + " " + i);
        }

        DeathCamera.transform.position = points[0];

    }

}
