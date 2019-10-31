using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMode : MonoBehaviour
{

    public static BuildMode instance;

    [SerializeField] public Camera PlayerCamera;
    [SerializeField] public Camera BuildCamera;

    private bool buildMode;
    private bool canActivatebuildMode;

    private float heightOffset;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        buildMode = false;
        canActivatebuildMode = true;

        PlayerCamera.enabled = true;
        BuildCamera.enabled = false;

        heightOffset = 6;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canActivatebuildMode)
        {
            StartCoroutine(ActivateBuildMode());
        }
        if (!buildMode)
        {
            BuildCamera.transform.position = new Vector3(PlayerController.instance.gameObject.transform.position.x, BuildCamera.transform.position.y, PlayerController.instance.gameObject.transform.position.z);
            //BuildCamera.transform.rotation = new Quaternion(BuildCamera.transform.rotation.x, BuildCamera.transform.rotation.y, -PlayerController.instance.gameObject.transform.rotation.y, BuildCamera.transform.rotation.w);
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(BuildCamera.transform.position, Vector3.down, out hit))
            {
                BuildCamera.transform.position = Vector3.Lerp(BuildCamera.transform.position, new Vector3(hit.point.x, hit.point.y + heightOffset, hit.point.z), Time.deltaTime);
            }
        }

        if(Input.mouseScrollDelta.y < 0)
        {
            if (heightOffset < 15) heightOffset += 1.25f * (Input.mouseScrollDelta.y * -1);
        }else if(Input.mouseScrollDelta.y > 0)
        {
            if(heightOffset > 4) heightOffset -= 1.25f * (Input.mouseScrollDelta.y);
        }
    }

    IEnumerator ActivateBuildMode()
    {
        canActivatebuildMode = false;

        buildMode = !buildMode;

        if (buildMode)
        {
            PlayerCamera.enabled = false;
            BuildCamera.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            PlayerCamera.enabled = true;
            BuildCamera.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        yield return new WaitForSeconds(2);

        canActivatebuildMode = true;
    }

    public bool isBuildMode()
    {
        return buildMode;
    }

}
