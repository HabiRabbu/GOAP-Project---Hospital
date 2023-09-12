public class GoToToilet : GAction {

    public override bool PrePerform() {

        // Get a free toilet
        target = GWorld.Instance.GetQueue("toilets").RemoveResource();
        // Check we got a toilet
        if (target == null) return false;
        // Add it to the inventory
        inventory.AddItem(target);
        // Remove it's availability from the world
        GWorld.Instance.GetWorld().ModifyState("FreeToilet", -1);
        return true;
    }

    public override bool PostPerform() {

        // Return the toilet to the pool
        GWorld.Instance.GetQueue("toilets").AddResource(target);
        // Remove the toilet from the list
        inventory.RemoveItem(target);
        // Give the toilet back to the world
        GWorld.Instance.GetWorld().ModifyState("FreeToilet", 1);
        // Remove the busting belief so it won't keep trying the action until it's invoked again
        beliefs.RemoveState("busting");
        return true;
    }
}
