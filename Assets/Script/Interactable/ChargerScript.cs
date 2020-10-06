using UnityEngine;

public class ChargerScript : Interactable
{
    protected override void Interact ()
    {
    	ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
	    playerController.currentEnergy += 20;

	    playerController.currentEnergy = Mathf.Min(playerController.currentEnergy, playerController.maxEnergy);
    }
}
