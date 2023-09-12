public class Research : GAction {

    public override bool PrePerform() {

        // Get a free office
        target = GWorld.Instance.GetQueue("offices").RemoveResource();
        // Check that we did indeed get an office
        if (target == null)
            // No office so return false
            return false;

        // Add it to the inventory
        inventory.AddItem(target);
        // Make the office unavailable to other doctors
        GWorld.Instance.GetWorld().ModifyState("FreeOffice", -1);
        // Debug.Log("Research Started");
        // All good
        return true;
    }

    public override bool PostPerform() {

        // Add the office back to the pool
        GWorld.Instance.GetQueue("offices").AddResource(target);
        // Remove the office from the list
        inventory.RemoveItem(target);
        // Give the office back to the world
        GWorld.Instance.GetWorld().ModifyState("FreeOffice", 1);
        // Debug.Log("Research Finished");
        // All good
        return true;
    }
}
