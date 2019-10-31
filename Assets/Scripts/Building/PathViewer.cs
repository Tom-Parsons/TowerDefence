using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathViewer : MonoBehaviour
{

    [SerializeField] private GameObject spawnPoint1;
    [SerializeField] private GameObject pathPoint1;
    private List<Vector3> path1Corners;

    [SerializeField] private GameObject spawnPoint2;
    [SerializeField] private GameObject pathPoint2;
    private List<Vector3> path2Corners;

    [SerializeField] private GameObject spawnPoint3;
    [SerializeField] private GameObject pathPoint3;
    private List<Vector3> path3Corners;

    [SerializeField] private GameObject spawnPoint4;
    [SerializeField] private GameObject pathPoint4;
    private List<Vector3> path4Corners;

    // Start is called before the first frame update
    void Start()
    {
        path1Corners = new List<Vector3>();
        path2Corners = new List<Vector3>();
        path3Corners = new List<Vector3>();
        path4Corners = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        //PATH 1

        if (!EnemySpawnController.instance.isInWave() && EnemySpawnController.instance.getActivatedSpawns().Contains(0))
        {
            if (path1Corners.Count == 0)
            {
                Vector3 pointOffset = new Vector3(spawnPoint1.transform.position.x, spawnPoint1.transform.position.y + 1, spawnPoint1.transform.position.z);
                pathPoint1.transform.position = spawnPoint1.transform.position;
                NavMeshPath path = new NavMeshPath();
                Vector3 dest = CentreStatue.instance.gameObject.transform.position;
                dest.z += 1;
                dest.y -= 1;
                if (NavMesh.CalculatePath(spawnPoint1.transform.position, dest, NavMesh.AllAreas, path))
                {
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        path1Corners.Add(path.corners[i]);
                    }
                    //foreach (Vector3 corner in path.corners) path1Corners.Add(corner);
                }
            }

            if ((pathPoint1.transform.position - new Vector3(path1Corners[0].x, path1Corners[0].y + 1, path1Corners[0].z)).magnitude > 0.1)
            {
                Debug.DrawLine(pathPoint1.transform.position, path1Corners[0], Color.blue);
                Vector3 pointOffset = new Vector3(path1Corners[0].x, path1Corners[0].y + 1, path1Corners[0].z);
                pathPoint1.transform.position = Vector3.MoveTowards(pathPoint1.transform.position, pointOffset, Time.deltaTime * 3);
            }
            else
            {
                path1Corners.RemoveAt(0);
            }
        }
        else
        {
            pathPoint1.transform.position = new Vector3(spawnPoint1.transform.position.x, spawnPoint1.transform.position.y - 5, spawnPoint1.transform.position.z);
        }

        //PATH 2

        if (!EnemySpawnController.instance.isInWave() && EnemySpawnController.instance.getActivatedSpawns().Contains(1))
        {
            if (path2Corners.Count == 0)
            {
                Vector3 pointOffset = new Vector3(spawnPoint2.transform.position.x, spawnPoint2.transform.position.y + 1, spawnPoint2.transform.position.z);
                pathPoint2.transform.position = spawnPoint2.transform.position;
                NavMeshPath path = new NavMeshPath();
                Vector3 dest = CentreStatue.instance.gameObject.transform.position;
                dest.z += 1;
                dest.y -= 1;
                if (NavMesh.CalculatePath(spawnPoint2.transform.position, dest, NavMesh.AllAreas, path))
                {
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        path2Corners.Add(path.corners[i]);
                    }
                    //foreach (Vector3 corner in path.corners) path2Corners.Add(corner);
                }
            }

            if ((pathPoint2.transform.position - new Vector3(path2Corners[0].x, path2Corners[0].y + 1, path2Corners[0].z)).magnitude > 0.1)
            {
                Debug.DrawLine(pathPoint2.transform.position, path2Corners[0], Color.blue);
                Vector3 pointOffset = new Vector3(path2Corners[0].x, path2Corners[0].y + 1, path2Corners[0].z);
                pathPoint2.transform.position = Vector3.MoveTowards(pathPoint2.transform.position, pointOffset, Time.deltaTime * 3);
            }
            else
            {
                path2Corners.RemoveAt(0);
            }
        }
        else
        {
            pathPoint2.transform.position = new Vector3(spawnPoint2.transform.position.x, spawnPoint2.transform.position.y - 5, spawnPoint2.transform.position.z);
        }

        //PATH 3

        if (!EnemySpawnController.instance.isInWave() && EnemySpawnController.instance.getActivatedSpawns().Contains(2))
        {
            if (path3Corners.Count == 0)
            {
                Vector3 pointOffset = new Vector3(spawnPoint3.transform.position.x, spawnPoint3.transform.position.y + 1, spawnPoint3.transform.position.z);
                pathPoint3.transform.position = spawnPoint3.transform.position;
                NavMeshPath path = new NavMeshPath();
                Vector3 dest = CentreStatue.instance.gameObject.transform.position;
                dest.z += 1;
                dest.y -= 1;
                if (NavMesh.CalculatePath(spawnPoint3.transform.position, dest, NavMesh.AllAreas, path))
                {
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        path3Corners.Add(path.corners[i]);
                    }
                    //foreach (Vector3 corner in path.corners) path3Corners.Add(corner);
                }
            }

            if ((pathPoint3.transform.position - new Vector3(path3Corners[0].x, path3Corners[0].y + 1, path3Corners[0].z)).magnitude > 0.1)
            {
                Debug.DrawLine(pathPoint3.transform.position, path3Corners[0], Color.blue);
                Vector3 pointOffset = new Vector3(path3Corners[0].x, path3Corners[0].y + 1, path3Corners[0].z);
                pathPoint3.transform.position = Vector3.MoveTowards(pathPoint3.transform.position, pointOffset, Time.deltaTime * 3);
            }
            else
            {
                path3Corners.RemoveAt(0);
            }
        }
        else
        {
            pathPoint3.transform.position = new Vector3(spawnPoint3.transform.position.x, spawnPoint3.transform.position.y - 5, spawnPoint3.transform.position.z);
        }

        //PATH 4

        if (!EnemySpawnController.instance.isInWave() && EnemySpawnController.instance.getActivatedSpawns().Contains(3))
        {
            if (path4Corners.Count == 0)
            {
                Vector3 pointOffset = new Vector3(spawnPoint4.transform.position.x, spawnPoint4.transform.position.y + 1, spawnPoint4.transform.position.z);
                pathPoint4.transform.position = spawnPoint4.transform.position;
                NavMeshPath path = new NavMeshPath();
                Vector3 dest = CentreStatue.instance.gameObject.transform.position;
                dest.z += 1;
                dest.y -= 1;
                if (NavMesh.CalculatePath(spawnPoint4.transform.position, dest, NavMesh.AllAreas, path))
                {
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        path4Corners.Add(path.corners[i]);
                    }
                    //foreach (Vector3 corner in path.corners) path4Corners.Add(corner);
                }
            }

            if ((pathPoint4.transform.position - new Vector3(path4Corners[0].x, path4Corners[0].y + 1, path4Corners[0].z)).magnitude > 0.1)
            {
                Debug.DrawLine(pathPoint4.transform.position, path4Corners[0], Color.blue);
                Vector3 pointOffset = new Vector3(path4Corners[0].x, path4Corners[0].y + 1, path4Corners[0].z);
                pathPoint4.transform.position = Vector3.MoveTowards(pathPoint4.transform.position, pointOffset, Time.deltaTime * 3);
            }
            else
            {
                path4Corners.RemoveAt(0);
            }
        }
        else
        {
            pathPoint4.transform.position = new Vector3(spawnPoint4.transform.position.x, spawnPoint4.transform.position.y - 5, spawnPoint4.transform.position.z);
        }
    }
}
