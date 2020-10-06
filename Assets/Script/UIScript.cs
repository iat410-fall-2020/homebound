﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
	public ThirdPersonController player;

	public BarScript energyBar;
    public BarScript rocketBar;
	public CollectedResource collectedResource;
    public WeaponUISection weaponUI;

	public GameObject Minimap;

	bool minimapStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        energyBar.SetMax(player.maxEnergy);
        rocketBar.SetMax(player.maxLiftingTime);

        Minimap.SetActive(minimapStatus);
        ChangeWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        // update energy bar
        energyBar.SetCurrent(player.currentEnergy);

        // update rocket bar
        rocketBar.SetCurrent(player.liftingtimer);

        // update collected resource
        collectedResource.SetText(player.currentResource);

        // upadate ammo amount
        Weapons playerWeapon = player.weapons[player.currentWeapon].GetComponent<Weapons>();
        weaponUI.AmmoDisplay(playerWeapon.currentMag, playerWeapon.totalAmmo);
    }

    public void AffectMinimap() {
    	minimapStatus = !minimapStatus;
    	Minimap.SetActive(minimapStatus);
    }

    public void ChangeWeapon() {
        weaponUI.ChangeWeapon(player.weapons[player.currentWeapon].tag);
    }
}
