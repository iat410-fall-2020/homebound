using UnityEngine;

public class ChargerScript : Interactable
{	
	bool refuel = false;

    protected override void Interact ()
    {	
    	ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
	    playerController.currentEnergy += 20;

	    playerController.currentEnergy = Mathf.Min(playerController.currentEnergy, playerController.maxEnergy);

	    refuel = true;
    }

    protected virtual void constantUpdate() 
    {
    	if (refuel && isFocus) {

    		if (Input.GetKeyDown(KeyCode.F)) {
    			ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
			    playerController.currentEnergy += Time.deltaTime * (playerController.maxEnergy / 10);

			    playerController.currentEnergy = Mathf.Min(playerController.currentEnergy, playerController.maxEnergy);
    		}
    		else {
    			refuel = false;
    		}
    	}
    }

}
