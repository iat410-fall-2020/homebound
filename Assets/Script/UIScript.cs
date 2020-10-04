using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
	public ThirdPersonController player;

	public EnergyBarScript energyBar;
	public CollectedResource collectedResource;
    public WeaponUISection weaponUI;

	public GameObject Minimap;

	bool minimapStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        energyBar.SetMaxEnergy(player.maxEnergy);
        Minimap.SetActive(minimapStatus);
        weaponUI.ChangeWeapon(player.currentWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        // update energy bar
        energyBar.SetEnergy(player.currentEnergy);

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
        weaponUI.ChangeWeapon(player.currentWeapon);
    }
}
