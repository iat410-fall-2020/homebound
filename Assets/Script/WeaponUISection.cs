using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUISection : MonoBehaviour
{
	public List<Graphic> weapons = new List<Graphic>();

	public Text currentAmmo;
	public Text totalAmmo;

    public void ChangeWeapon(string tag) {

        foreach (Graphic g in weapons) {
            if (g.tag == tag) {
                g.color = Color.blue;
            }
            else {
                g.color = Color.white;
            }
        }  	

    }

    public void AmmoDisplay(int current, int total) {
    	currentAmmo.text = current.ToString();
    	totalAmmo.text = total.ToString();
    }
}
