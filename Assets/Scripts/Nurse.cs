using UnityEngine;

public class Nurse : GAgent {

    new void Start() {

        // Call base Start method
        base.Start();
        // Set goal so that it can't be removed so the nurse can repeat this action
        SubGoal s1 = new SubGoal("treatPatient", 1, false);
        goals.Add(s1, 3);

        // Resting goal
        SubGoal s2 = new SubGoal("rested", 1, false);
        goals.Add(s2, 1);

        // Toilet goal
        SubGoal s3 = new SubGoal("relief", 1, false);
        goals.Add(s3, 5);

        // Call the GetTired() method for the first time
        Invoke("GetTired", Random.Range(10.0f, 20.0f));
    }

    void GetTired() {

        beliefs.ModifyState("exhausted", 0);
        //call the get tired method over and over at random times to make the nurse
        //get tired again
        Invoke("GetTired", Random.Range(10.0f, 20.0f));
        // Call the NeedRelief() methd for the first time
        Invoke("NeedRelief", Random.Range(10.0f, 20.0f));
    }

    void NeedRelief() {

        beliefs.ModifyState("busting", 0);
        // Call the get NeedRelief method over and over at random times to make the agent
        // go to the loo again
        Invoke("NeedRelief", Random.Range(10.0f, 20.0f));
    }

}