using UnityEngine;

public class CaputredAnimal : Interactable
{
	public NetBullet bullet;

    protected override void Interact ()
    {
    	ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
	    ++playerController.currentResource;

	    bullet.capturedAnimals.Remove(gameObject);

	    playerController.focus = null;
	    Destroy(gameObject);
    }
}
