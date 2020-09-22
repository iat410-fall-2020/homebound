using UnityEngine;

public class Collectable : Interactable
{
    public override void Interact ()
    {
    	ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
	    ++playerController.currentResource;

	    playerController.focus = null;
	    Destroy(gameObject);
    }
}
