using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceQueue {

    public Queue<GameObject> que = new Queue<GameObject>();
    public string tag;
    public string modState;

    public ResourceQueue(string t, string ms, WorldStates w) {

        tag = t;
        modState = ms;
        if (tag != "") {

            GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject r in resources) {

                que.Enqueue(r);
            }
        }

        if (modState != "") {

            w.ModifyState(modState, que.Count);
        }
    }

    // Add the resource
    public void AddResource(GameObject r) {

        que.Enqueue(r);
    }


    // Remove the resource
    public GameObject RemoveResource() {

        if (que.Count == 0) return null;

        return que.Dequeue();
    }

    // Overloaded RemoveResource
    public void RemoveResource(GameObject r) {

        // Put everything in a new queue except 'r' and copy it back to que
        que = new Queue<GameObject>(que.Where(p => p != r));
    }
}

public sealed class GWorld {

    // Our GWorld instance
    private static readonly GWorld instance = new GWorld();
    // Our world states
    private static WorldStates world;
    // Queue of patients
    private static ResourceQueue patients;
    // Queue of cubicles
    private static ResourceQueue cubicles;
    // Queue of offices
    private static ResourceQueue offices;
    // Queue of toilets
    private static ResourceQueue toilets;
    // Queue for the puddles
    private static ResourceQueue puddles;

    // Storage for all
    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>();

    static GWorld() {

        // Create our world
        world = new WorldStates();
        // Create patients array
        patients = new ResourceQueue("", "", world);
        // Add to the resources Dictionary
        resources.Add("patients", patients);
        // Create cubicles array
        cubicles = new ResourceQueue("Cubicle", "FreeCubicle", world);
        // Add to the resources Dictionary
        resources.Add("cubicles", cubicles);
        // Create offices array
        offices = new ResourceQueue("Office", "FreeOffice", world);
        // Add to the resources Dictionary
        resources.Add("offices", offices);
        // Create toilet array
        toilets = new ResourceQueue("Toilet", "FreeToilet", world);
        // Add to the resources Dictionary
        resources.Add("toilets", toilets);
        // Create puddles array
        puddles = new ResourceQueue("Puddle", "FreePuddle", world);
        // Add to the resources Dictionary
        resources.Add("puddles", puddles);

        // Set the time scale in Unity
        Time.timeScale = 5.0f;
    }

    public ResourceQueue GetQueue(string type) {

        return resources[type];
    }

    private GWorld() {

    }

    public static GWorld Instance {

        get { return instance; }
    }

    public WorldStates GetWorld() {

        return world;
    }
}
