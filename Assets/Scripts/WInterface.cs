using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class WInterface : MonoBehaviour {

    // Focus Object
    GameObject focusObj;
    // Resouce Data
    ResourceData fOData;
    // Prefab we are using.
    private GameObject newResourcePrefab;
    // Storage for all the resources.
    public GameObject[] allResources;
    // Holder for the Hospital GameObject
    public GameObject hospital;
    // Selected location
    Vector3 goalPos;
    // Holder for the NavMeshSurface
    public NavMeshSurface surface;
    // Store the offset
    Vector3 clickOffset = Vector3.zero;
    // Bool to test if we have calculate the offset yet?
    bool offsetCalc = false;
    // Should we are shouldn't we delete the resource
    bool deleteResource = false;

    void Start() {

    }

    // Method for checkingit we're over the trashcan icon
    public void MouseOnHoverTrash() {

        deleteResource = true;
    }

    // We've now left the area of the trashcan
    public void MouseOutHoverTrash() {

        deleteResource = false;
    }

    // Make the toilet the prefab
    public void ActivateToilet() {

        // Select the first item in the array which is our toilet
        newResourcePrefab = allResources[0];
    }

    // Make the cubicle the prefab
    public void ActivateCubicle() {

        // Select the first item in the array which is our cubicle
        newResourcePrefab = allResources[1];
    }

    // Update is called once per frame
    void Update() {

        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0)) {

            // Store the hit
            RaycastHit hit;
            // Convert to screen coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // If we didn't hit anything then or we are over the buttons just return
            if (!Physics.Raycast(ray, out hit) || EventSystem.current.IsPointerOverGameObject()) {

                return;
            }

            //if (EventSystem.current.IsPointerOverGameObject(-1)) {

            //    return;
            //}

            // Reset the offset has been calculated and the offset value
            offsetCalc = false;
            clickOffset = Vector3.zero;

            // Get the prefabs resource
            Resource r = hit.transform.gameObject.GetComponent<Resource>();

            // Check the resources isn't null
            if (r != null) {

                focusObj = hit.transform.gameObject;
                fOData = r.info;
            } else if (newResourcePrefab != null) {

                // Succes so get the position
                goalPos = hit.point;
                // Place the prefab where we clicked
                focusObj = Instantiate(newResourcePrefab, goalPos, newResourcePrefab.transform.rotation);
                // Set the focus data object
                fOData = focusObj.GetComponent<Resource>().info;
            }

            // Check we have a focus object
            if (focusObj) {

                // Get the collider and switch it off
                focusObj.GetComponent<Collider>().enabled = false;
            }

        } else if (focusObj && Input.GetMouseButtonUp(0)) {

            // Check if we want to delete the resource
            if (deleteResource) {

                // Remove from the queue
                GWorld.Instance.GetQueue(fOData.resourceQueue).RemoveResource(focusObj);
                // Modify the world state
                GWorld.Instance.GetWorld().ModifyState(fOData.resourceState, -1);
                // And get rid of the GameObject
                Destroy(focusObj);
            } else if (newResourcePrefab != null) {

                focusObj.transform.parent = hospital.transform;
                GWorld.Instance.GetQueue(fOData.resourceQueue).AddResource(focusObj);
                GWorld.Instance.GetWorld().ModifyState(fOData.resourceState, 1);
                // Turn the collider back on
                focusObj.GetComponent<Collider>().enabled = true;
            }

            surface.BuildNavMesh();
            focusObj = null;
        } else if (focusObj && Input.GetMouseButton(0)) {

            // Set up a layer mask
            int layerMask = 1 << 8;
            // Store the hit
            RaycastHit hitMove;
            // Convert to screen coordinates
            Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);

            // If we didn't hit anything then just return
            if (!Physics.Raycast(rayMove, out hitMove, Mathf.Infinity, layerMask)) {

                return;
            }

            // Have we calculated the offset yet?
            if (!offsetCalc) {

                // No.  So calculate it
                clickOffset = hitMove.point - focusObj.transform.position;
                // Now we have so set it to true
                offsetCalc = true;
            }

            // Success and also set tthe offset
            goalPos = hitMove.point - clickOffset;
            focusObj.transform.position = goalPos;
        }

        // Rotate the object left
        if (focusObj && Input.GetKeyDown(KeyCode.Less) || Input.GetKeyDown(KeyCode.Comma)) {

            focusObj.transform.Rotate(0.0f, 90.0f, 0.0f);
        }

        // Rotate the object right
        if (focusObj && Input.GetKeyDown(KeyCode.Greater) || Input.GetKeyDown(KeyCode.Period)) {

            focusObj.transform.Rotate(0.0f, -90.0f, 0.0f);
        }
    }
}
