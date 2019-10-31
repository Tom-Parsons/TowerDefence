using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemySpawnController : MonoBehaviour
{

    public static EnemySpawnController instance;

    [SerializeField] public List<GameObject> SpawnPoints;
    private List<GameObject> activatedSpawns;
    private List<int> activatedSpawnsIndex;

    [SerializeField] GameObject skeletonObject;
    [SerializeField] GameObject spiderObject;
    [SerializeField] GameObject mageObject;

    public List<GameObject> enemies;
    int wave;
    [SerializeField] float maxWaveTimer;
    float waveTimer;
    private bool newWave;
    private bool inWave;

    private int activatedTowers;
    private int skeletonNumber;
    private int spiderNumber;
    private int mageNumber;

    [SerializeField] private Text waveTimerText;
    [SerializeField] private Text waveText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        activatedSpawns = new List<GameObject>();
        activatedSpawnsIndex = new List<int>();
        enemies = new List<GameObject>();
        waveTimer = maxWaveTimer;
        wave = 0;
        activatedTowers = 1;
        skeletonNumber = 4;
        spiderNumber = 0;
        mageNumber = 0;
        foreach(GameObject obj in SpawnPoints)
        {
            obj.transform.GetChild(0).GetComponent<Light>().enabled = false;
        }
        StartCoroutine(NewWave());
    }

    // Update is called once per frame
    void Update()
    {
        waveText.text = "Wave " + wave;
        if(enemies.Count <= 0)
        {
            inWave = false;
            if (!newWave) StartCoroutine(NewWave());
            waveTimerText.text = "Time Until Next Wave: " + Mathf.RoundToInt(waveTimer).ToString();
        }
        else
        {
            inWave = true;
        }
    }

    IEnumerator NewWave()
    {
        newWave = true;

        MusicController.instance.TransitionToBuild();

        if ((wave + 1) % 10 == 0 && activatedTowers < 4) activatedTowers++;

        activatedSpawns = new List<GameObject>();
        activatedSpawnsIndex = new List<int>();

        for(int i = 0; i < activatedTowers; i++)
        {
            int spawnIndex;
            do
            {
                spawnIndex = Random.Range(0, 3);
                print(spawnIndex);
            } while (activatedSpawns.Contains(SpawnPoints[spawnIndex]));
            activatedSpawns.Add(SpawnPoints[spawnIndex]);
            activatedSpawnsIndex.Add(spawnIndex);
        }
        print(activatedTowers);
        foreach (GameObject obj in SpawnPoints)
        {
            obj.transform.GetChild(0).GetComponent<Light>().enabled = false;
        }
        foreach (GameObject obj in activatedSpawns)
        {
            print(obj.name);
            obj.transform.GetChild(0).GetComponent<Light>().enabled = true;
        }

        while(waveTimer > 0)
        {
            yield return new WaitForEndOfFrame();
            waveTimer -= Time.deltaTime;
        }

        wave++;

        skeletonNumber++;

        if(wave % 3 == 0)
        {
            spiderNumber++;
        }
        if(wave % 6 == 0)
        {
            mageNumber++;
        }

        List<GameObject> enemiesToSpawn = new List<GameObject>();
        for(int i = 0; i < skeletonNumber; i++)
        {
            enemiesToSpawn.Add(skeletonObject);
        }
        for (int i = 0; i < spiderNumber; i++)
        {
            enemiesToSpawn.Add(spiderObject);
        }
        for (int i = 0; i < mageNumber; i++)
        {
            //enemiesToSpawn.Add(mageObject);
        }

        foreach (GameObject obj in activatedSpawns)
        {
            StartCoroutine(SpawnEnemies(obj, enemiesToSpawn));
        }

        MusicController.instance.TransitionToBattle();

        waveTimer = maxWaveTimer;
    }

    IEnumerator SpawnEnemies(GameObject spawn, List<GameObject> enemiesToSpawn)
    {
        List<GameObject> newEnemies = new List<GameObject>();
        foreach (GameObject obj in enemiesToSpawn)
            newEnemies.Add(obj);
        yield return new WaitForSeconds(1);

        GameObject enemy = Instantiate(newEnemies[0]);
        enemy.transform.position = spawn.transform.position;
        enemy.GetComponent<NavMeshAgent>().enabled = true;
        enemies.Add(enemy);
        newEnemies.RemoveAt(0);
        if (newEnemies.Count > 0)
        {
            StartCoroutine(SpawnEnemies(spawn, newEnemies));
            newWave = true;
        }
        else
        {
            newWave = false;
        }
    }

    public List<int> getActivatedSpawns()
    {
        return activatedSpawnsIndex;
    }

    public bool isInWave()
    {
        return inWave;
    }

}
