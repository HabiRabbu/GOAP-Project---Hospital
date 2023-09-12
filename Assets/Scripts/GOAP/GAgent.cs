using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubGoal {
    // Dictionary to store our goals
    public Dictionary<string, int> sGoals;
    // Bool to store if goal should be removed after it has been achieved
    public bool remove;
    // Constructor
    public SubGoal(string s, int i, bool r) {

        sGoals = new Dictionary<string, int>();
        sGoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour {
    // Store our list of actions
    public List<GAction> actions = new List<GAction>();
    // Dictionary of subgoals
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    // Our inventory
    public GInventory inventory = new GInventory();
    // Our beliefs
    public WorldStates beliefs = new WorldStates();

    // Access the planner
    GPlanner planner;
    // Action Queue
    Queue<GAction> actionQueue;
    // Our current action
    public GAction currentAction;
    // Our subgoal
    SubGoal currentGoal;

    // Out target destination for the office
    Vector3 destination = Vector3.zero;

    // Start is called before the first frame update
    public void Start() {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
            actions.Add(a);
    }


    bool invoked = false;
    //an invoked method to allow an agent to be performing a task
    //for a set location
    void CompleteAction() {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate() {
        //if there's a current action and it is still running
        if (currentAction != null && currentAction.running) {
            // Find the distance to the target
            float distanceToTarget = Vector3.Distance(destination, this.transform.position);
            //Debug.Log(currentAction.agent.hasPath + "   " + distanceToTarget);
            // Check the agent has a goal and has reached that goal
            if (distanceToTarget < 2f)//currentAction.agent.remainingDistance < 0.5f)
            {
                // Debug.Log("Distance to Goal: " + currentAction.agent.remainingDistance);
                if (!invoked) {
                    //if the action movement is complete wait
                    //a certain duration for it to be completed
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        // Check we have a planner and an actionQueue
        if (planner == null || actionQueue == null) {
            planner = new GPlanner();

            // Sort the goals in descending order and store them in sortedGoals
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            //look through each goal to find one that has an achievable plan
            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals) {
                actionQueue = planner.plan(actions, sg.Key.sGoals, beliefs);
                // If actionQueue is not = null then we must have a plan
                if (actionQueue != null) {
                    // Set the current goal
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        // Have we an actionQueue
        if (actionQueue != null && actionQueue.Count == 0) {
            // Check if currentGoal is removable
            if (currentGoal.remove) {
                // Remove it
                goals.Remove(currentGoal);
            }
            // Set planner = null so it will trigger a new one
            planner = null;
        }

        // Do we still have actions
        if (actionQueue != null && actionQueue.Count > 0) {
            // Remove the top action of the queue and put it in currentAction
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform()) {
                // Get our current object
                if (currentAction.target == null && currentAction.targetTag != "")
                    // Activate the current action
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if (currentAction.target != null) {
                    // Activate the current action
                    currentAction.running = true;
                    // Pass in the office then look for its cube
                    destination = currentAction.target.transform.position;
                    Transform dest = currentAction.target.transform.Find("Destination");
                    // Check we got it
                    if (dest != null)
                        destination = dest.position;

                    // Pass Unities AI the destination for the agent
                    currentAction.agent.SetDestination(destination);
                }
            } else {
                // Force a new plan
                actionQueue = null;
            }

        }

    }
}
