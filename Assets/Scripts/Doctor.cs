using UnityEngine;

public class Doctor : GAgent {

    new void Start() {

        // Call base Start method
        base.Start();
        // Set goal so that it can't be removed so the Doctoe can repeat this action
        SubGoal s1 = new SubGoal("research", 1, false);
        goals.Add(s1, 1);

        // Toilet goal
        SubGoal s2 = new SubGoal("relief", 1, false);
        goals.Add(s2, 2);

        // Resting goal
        SubGoal s3 = new SubGoal("rested", 1, false);
        goals.Add(s3, 3);

        // Call the GetTired() method for the first time
        Invoke("GetTired", Random.Range(2.0f, 5.0f));
        // Call the NeedRelief() methd for the first time
        Invoke("NeedRelief", Random.Range(2.0f, 5.0f));
    }

    void GetTired() {

        beliefs.ModifyState("exhausted", 0);
        // Call the get tired method over and over at random times to make the agent
        // get tired again
        Invoke("GetTired", Random.Range(0.0f, 20.0f));
    }

    void NeedRelief() {

        beliefs.ModifyState("busting", 0);
        // Call the get NeedRelief method over and over at random times to make the agent
        // go to the loo again
        Invoke("NeedRelief", Random.Range(2.0f, 5.0f));
    }

}