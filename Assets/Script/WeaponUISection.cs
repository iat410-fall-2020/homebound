using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUISection : MonoBehaviour
{
	public List<Graphic> weapons = new List<Graphic>();
	int currentWeapon = 0;

	public Text currentAmmo;
	public Text totalAmmo;

    public void ChangeWeapon(int n) {
    	weapons[currentWeapon].color = Color.white;
    	weapons[n].color = Color.blue;

    	currentWeapon = n;
    }

    public void AmmoDisplay(int current, int total) {
    	currentAmmo.text = current.ToString();
    	totalAmmo.text = total.ToString();
    }
}
