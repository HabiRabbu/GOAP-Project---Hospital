using UnityEngine;

public class GStateMonitor : MonoBehaviour {

    // State we are looking for (Busting)
    public string state;
    // State strength (How long can we hold it for)
    public float stateStrength;
    // State decay rate over time
    public float stateDecayRate;
    // Agents beliefs
    public WorldStates beliefs;
    // Resource we want to use (puddle)
    public GameObject resourcePrefab;
    // Queue name storage
    public string queueName;
    // WorldState that needs to be triggered
    public string worldState;
    // The related Action
    public GAction action;

    // bool for checking if state found
    private bool stateFound = false;
    // Float to reset the stateStrength to
    private float initialStrength;


    void Awake() {

        // Grab the agents beliefs
        beliefs = this.GetComponent<GAgent>().beliefs;
        // Reset the initial strength
        initialStrength = stateStrength;
    }


    void LateUpdate() {

        if (action.running) {

            stateFound = false;
            stateStrength = initialStrength;
        }

        if (!stateFound && beliefs.HasState(state)) {

            stateFound = true;
        }

        if (stateFound) {

            stateStrength -= stateDecayRate * Time.deltaTime;
            if (stateStrength <= 0.0f) {

                // Place the puddle at the agents location
                Vector3 location = new Vector3(this.transform.position.x, resourcePrefab.transform.position.y, this.transform.position.z);
                GameObject p = Instantiate(resourcePrefab, location, resourcePrefab.transform.rotation);
                stateFound = false; ;
                // Reset the stateStrength
                stateStrength = initialStrength;
                // Remove the belief
                beliefs.RemoveState(state);
                // Add the puddle to the queue
                GWorld.Instance.GetQueue(queueName).AddResource(p);
                GWorld.Instance.GetWorld().ModifyState(worldState, 1);
            }
        }
    }
}
