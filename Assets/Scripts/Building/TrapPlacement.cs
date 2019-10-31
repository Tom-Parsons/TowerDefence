using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TrapPlacement : MonoBehaviour
{

    public static TrapPlacement instance;

    [SerializeField] private GameObject plane;

    [SerializeField] private List<GameObject> traps;
    private int selected;

    [Header("MATERIALS")]
    [SerializeField] Material correctMat;
    [SerializeField] Material incorrectMat;
    [SerializeField] Material waitingMat;

    [Header("UI")]
    [SerializeField] public Text previousText;
    [SerializeField] public Text nextText;
    [SerializeField] public Text currentTrapText;

    private GameObject preview;
    private List<GameObject> objects;

    private bool canPlaceTrap;
    private bool canRotateTrap;
    private bool canChangeSelection;

    private float rotateAmount;

    public List<GameObject> placedTraps;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        canPlaceTrap = true;
        canRotateTrap = true;
        canChangeSelection = true;
        objects = new List<GameObject>();
        placedTraps = new List<GameObject>();
        rotateAmount = 0;
    }

    NavMeshPath path = null;

    // Update is called once per frame
    void Update()
    {
        if(path != null)
        {
            Vector3 prevv = Vector3.zero;
            foreach(Vector3 v in path.corners)
            {
                if(prevv != Vector3.zero)
                {
                    Debug.DrawLine(prevv, v, Color.red);
                }
                prevv = v;
            }
        }

        if(preview == null && BuildMode.instance.isBuildMode())
        {
            CreatePreview(traps[selected]);
        }else if(preview != null && !BuildMode.instance.isBuildMode())
        {
            DestroyPreview();
        }
        if (!BuildMode.instance.isBuildMode())
        {
            nextText.enabled = false;
            previousText.enabled = false;
            currentTrapText.enabled = false;
            return;
        }

        if(selected > 0)
        {
            previousText.enabled = true;
        }
        else
        {
            previousText.enabled = false;
        }
        if(selected < traps.Count-1)
        {
            nextText.enabled = true;
        }
        else
        {
            nextText.enabled = false;
        }

        if(preview != null)
        {
            currentTrapText.enabled = true;
            currentTrapText.text = traps[selected].name + "\n" + traps[selected].GetComponent<TrapCost>().getCost() + " coins";
        }
        else
        {
            currentTrapText.enabled = false;
        }

        RaycastHit hit;
        if(Physics.Raycast(BuildMode.instance.BuildCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Vector3 point = hit.point;
            preview.transform.position = new Vector3(point.x, point.y + 0.01f, point.z);
        }

        if (Physics.Raycast(preview.transform.position, Vector3.down, out hit))
        {
            
        }
        //preview.transform.rotation = new Quaternion(preview.transform.rotation.x, rotateAmount, preview.transform.rotation.z, preview.transform.rotation.w);

        //preview.transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0, rotateAmount, 0), Time.deltaTime);
        preview.transform.eulerAngles = new Vector3(preview.transform.eulerAngles.x, rotateAmount, preview.transform.eulerAngles.z);

        if (objects != null)
        {
            if (canPlaceTrap)
            {
                if (hit.collider != null)
                {
                    if (objects.Count > 0 || hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
                    {
                        try
                        {
                            preview.transform.GetChild(0).GetComponent<Renderer>().material = incorrectMat;
                            preview.transform.GetChild(1).GetComponent<Renderer>().material = incorrectMat;
                        }
                        catch (UnityException) { }
                        try
                        {
                            preview.transform.GetComponent<Renderer>().material = incorrectMat;
                        }
                        catch (MissingComponentException) { }
                    }
                    else
                    {
                        try
                        {
                            preview.transform.GetChild(0).GetComponent<Renderer>().material = correctMat;
                            preview.transform.GetChild(1).GetComponent<Renderer>().material = correctMat;
                        }
                        catch (UnityException) { }
                        try
                        {
                            preview.transform.GetComponent<Renderer>().material = correctMat;
                        }
                        catch (MissingComponentException) { }
                    }
                }
            }
            else
            {
                try { 
                    preview.transform.GetChild(0).GetComponent<Renderer>().material = waitingMat;
                    preview.transform.GetChild(1).GetComponent<Renderer>().material = waitingMat;
                }
                catch (UnityException) { }
                try
                {
                    preview.transform.GetComponent<Renderer>().material = waitingMat;
                }
                catch (MissingComponentException) { }
            }
        }
        else
        {
            objects = new List<GameObject>();
        }



        if (Input.GetButton("Fire1") && canPlaceTrap && objects.Count == 0 && hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
        {
            //PLACE TRAP
            bool validPosition = true;
            foreach(GameObject obj in EnemySpawnController.instance.SpawnPoints)
            {
                NavMeshPath path = new NavMeshPath();
                Vector3 dest = CentreStatue.instance.gameObject.transform.position;
                dest.z += 1;
                dest.y -= 1;
                Debug.DrawLine(obj.transform.position, dest);
                if (!NavMesh.CalculatePath(obj.transform.position, dest, NavMesh.AllAreas, path))
                {
                    validPosition = false;
                }
                else
                {
                    if(path.status == NavMeshPathStatus.PathPartial)
                    {
                        validPosition = false;
                    }
                    this.path = path;
                }
            }
            if (validPosition)
            {
                if (PlayerController.instance.getCoins() >= traps[selected].GetComponent<TrapCost>().getCost())
                {
                    StartCoroutine(PlaceTrap(hit.point));
                }
                else
                {
                    MessageBroadcaster.instance.Broadcast("You don't have enough coins for this trap!", 2);
                }
            }
            else
            {
                MessageBroadcaster.instance.Broadcast("Placing this will block the enemies' path!", 2);
            }
        }

        if (Input.GetKey(KeyCode.R) && canRotateTrap)
        {
            StartCoroutine(RotateTrap());
        }

        if (Input.GetKey(KeyCode.Q) && selected > 0 && canChangeSelection)
        {
            StartCoroutine(ChangeSelection(-1));
        }
        if (Input.GetKey(KeyCode.E) && selected < traps.Count - 1 && canChangeSelection)
        {
            StartCoroutine(ChangeSelection(1));
        }

    }

    IEnumerator DrawLINE(NavMeshPath path)
    {
        Vector3 prev = path.corners[0];
        foreach (Vector3 vec in path.corners)
        {
            Debug.DrawLine(prev, vec);
            prev = vec;
        }
        yield return new WaitForEndOfFrame();
        DrawLINE(path);
    }

    IEnumerator PlaceTrap(Vector3 position)
    {
        canPlaceTrap = false;

        GameObject newTrap = Instantiate(traps[selected]);

        float height = new Bounds(newTrap.transform.position, Vector3.zero).size.y;

        if (newTrap.name.Contains("Rubble"))
            newTrap.transform.position = new Vector3(position.x, position.y - height / 2, position.z);
        else
            newTrap.transform.position = position;
        //newTrap.transform.DetachChildren();
        newTrap.transform.eulerAngles = new Vector3(preview.transform.eulerAngles.x, rotateAmount, preview.transform.eulerAngles.z);

        PlayerController.instance.SpendCoins(traps[selected].GetComponent<TrapCost>().getCost());

        placedTraps.Add(newTrap);

        yield return new WaitForSeconds(1);

        canPlaceTrap = true;
    }

    IEnumerator RotateTrap()
    {
        canRotateTrap = false;

        rotateAmount += 1;
        if (rotateAmount > 360) rotateAmount = 0;

        yield return new WaitForEndOfFrame();

        canRotateTrap = true;
    }

    IEnumerator ChangeSelection(int amount)
    {
        canChangeSelection = false;

        selected += amount;
        DestroyPreview();
        CreatePreview(traps[selected]);

        yield return new WaitForSeconds(0.5f);

        canChangeSelection = true;
    }

    void CreatePreview(GameObject obj)
    {
        preview = Instantiate(obj);
        preview.layer = 2;
        try
        {
            preview.transform.GetChild(0).gameObject.layer = 2;
        }
        catch(UnityException) { }
        preview.name = "TEMPORARY_TRAP";
        preview.tag = "TempTrap";
        try
        {
            preview.transform.GetChild(0).tag = "TempTrap";
        }
        catch (UnityException) { }
        try
        {
            preview.transform.GetChild(1).tag = "TempTrap";
        }
        catch (UnityException) { }
        //preview.transform.GetComponent<Renderer>().material = correctMat;
        try { 
            try { 
                preview.transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;
            }
            catch (MissingComponentException) { }
        }
        catch (UnityException) { }
        try
        {
            preview.GetComponent<BoxCollider>().isTrigger = true;
        }
        catch (MissingComponentException) { }
        try
        {
            preview.GetComponent<MeshCollider>().isTrigger = true;
        }
        catch (MissingComponentException) {
            try
            {
                preview.transform.GetChild(0).GetComponent<MeshCollider>().isTrigger = true;
                preview.transform.GetChild(0).gameObject.AddComponent<TrapTrigger>();
            }
            catch(MissingComponentException) { }
        }
        preview.AddComponent<TrapTrigger>();
        preview.AddComponent<Rigidbody>();
        preview.GetComponent<Rigidbody>().isKinematic = true;
        objects = new List<GameObject>();
        try { 
        preview.transform.GetChild(1).gameObject.layer = 2;
        }
        catch (UnityException) { }
    }



    void DestroyPreview()
    {
        Destroy(preview);
    }


    public void Add(GameObject go)
    {
        objects.Add(go);
    }
    public void Remove(GameObject go)
    {
        objects.Remove(go);
    }

}
