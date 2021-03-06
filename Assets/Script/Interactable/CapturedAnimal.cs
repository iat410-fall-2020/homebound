﻿using UnityEngine;

public class CapturedAnimal : Interactable
{
	public NetBullet bullet;

    protected override void Interact ()
    {
    	ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
	    ++playerController.currentResource;

	    if (bullet != null) {
	    	bullet.capturedAnimals.Remove(gameObject);
	    }

	    playerController.focus = null;
	    Destroy(gameObject);
    }
}
