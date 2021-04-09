using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera component for following targets
/// </summary>
[RequireComponent(typeof(Camera))]
public class ActionCamera : MonoBehaviour
{
    public Bounds cameraBounds;
    private Camera cam;
    private Bounds viewBounds;
    public List<Transform> targets;
    public float overhang = 8f;
    public float minsize =10f;
    public float zOffset = -5f;

    public float followDelay = 0.5f;
    public float resizeSpeed = 0.5f;

    static public ActionCamera instance;

    private void Awake()
    {
        if (instance != null) Debug.LogError("Camera singleton has already assigned instance");
        instance = this;
    }

    private void Start()
    {
        //initialize local variables
        cam = GetComponent<Camera>();
        cameraBounds.center = new Vector3(cameraBounds.center.x,cameraBounds.center.y,0);
        viewBounds = new Bounds();


        UpdateBounds();
    }

    private void UpdateBounds() {

        viewBounds.center = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
        viewBounds.extents= new Vector3(cam.orthographicSize, cam.orthographicSize * cam.aspect,0);
    }

    private Bounds target;
    private Vector3 velocity;
    private void LateUpdate()
    {
        if (targets.Count > 0)
        {
            target = GetTargetBounds();

            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, target.center + (zOffset * Vector3.forward), ref velocity, followDelay);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Mathf.Min(target.extents.y,target.extents.x/cam.aspect), resizeSpeed*Time.deltaTime);
        }
    }

    private Bounds GetTargetBounds() {
        Bounds playerBounds = new Bounds() ;
        //encapsulating targets in common bounds
        foreach (Transform target in targets)
        {
            if (target != null)
            {
                playerBounds = new Bounds(target.position, Vector3.zero);
                break;
            }
        }
        foreach (Transform target in targets)
        {
            if (target != null) { 
                playerBounds.Encapsulate(target.position);
            }
        }

        //adding overhang
        playerBounds.Expand(overhang);

        //setting the playerBounds to fit camera aspect ratio
        float aspect = playerBounds.extents.x / playerBounds.extents.y;
        if (aspect < cam.aspect)
        {
            playerBounds.extents = new Vector3(playerBounds.extents.y * cam.aspect, playerBounds.extents.y, 0);
        }
        else
        {
            playerBounds.extents = new Vector3(playerBounds.extents.x, playerBounds.extents.x/cam.aspect, 0);
        }

        if (playerBounds.size.x < minsize) {
            playerBounds.extents*= minsize/playerBounds.size.x;
        }

        //fitting playebounds into outer cameraBounds
        //shifting
        playerBounds.center -= Vector3.Max(playerBounds.max - cameraBounds.max,Vector3.zero);
        playerBounds.center -= Vector3.Min(playerBounds.min - cameraBounds.min, Vector3.zero);
        //cutting if still too large
        playerBounds.SetMinMax(Vector3.Max(playerBounds.min, cameraBounds.min), Vector3.Min(playerBounds.max, cameraBounds.max));

        playerBounds.center -= playerBounds.center.z * Vector3.forward;
        return playerBounds;
    }
}
