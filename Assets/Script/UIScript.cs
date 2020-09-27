using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
	public ThirdPersonController player;

	public EnergyBarScript energyBar;
	public CollectedResource collectedResource;


	public GameObject Minimap;

	bool minimapStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        energyBar.SetMaxEnergy(player.maxEnergy);
        Minimap.SetActive(minimapStatus);
    }

    // Update is called once per frame
    void Update()
    {
        // update energy bar
        energyBar.SetEnergy(player.currentEnergy);

        // update collected resource
        collectedResource.SetText(player.currentResource);
    }

    public void AffectMinimap() {
    	minimapStatus = !minimapStatus;
    	Minimap.SetActive(minimapStatus);
    }
}
