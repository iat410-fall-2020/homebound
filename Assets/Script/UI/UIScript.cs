using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
	public ThirdPersonController player;

	public BarScript energyBar;
    public BarScript rocketBar;
	public CollectedResource collectedResource;
    public WeaponUISection weaponUI;

    public GameObject reloadBar;

	public GameObject Minimap;

	bool minimapStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        energyBar.SetMax(player.maxEnergy);
        rocketBar.SetMax(player.maxOverHeatTime);

        Minimap.SetActive(minimapStatus);
        ChangeWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        // update energy bar
        energyBar.SetCurrent(player.currentEnergy);

        // update rocket bar
        rocketBar.SetCurrent(player.overHeatTimer);

        // update collected resource
        collectedResource.SetText(player.currentResource);

        // upadate ammo amount
        Weapons playerWeapon = player.weapons[player.currentWeapon].GetComponent<Weapons>();
        weaponUI.AmmoDisplay(playerWeapon.currentMag, playerWeapon.totalAmmo);

        // reload bar
        if (playerWeapon.isReloading()) {
            reloadBar.SetActive(true);
            reloadBar.GetComponent<ReloadBar>().BarUpdate(playerWeapon.reloadCountDown / playerWeapon.reloadTime, playerWeapon.reloadCountDown);
        }
        else {
            reloadBar.SetActive(false);
        }
    }

    public void AffectMinimap() {
    	minimapStatus = !minimapStatus;
    	Minimap.SetActive(minimapStatus);
    }

    public void ChangeWeapon() {
        weaponUI.ChangeWeapon(player.weapons[player.currentWeapon].tag);
    }
}
