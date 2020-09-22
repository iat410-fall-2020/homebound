using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    bool isFocus = false;
    protected GameObject player;

    bool hasInteracted = false;
    bool looking = false;

    public GameObject text;

    public virtual void Interact () 
    {
    	 // this method is meant to override
    	Debug.Log("Interacting with " + transform.name);
    }

    void Update () 
    {
    	if (looking) {
    		text.SetActive(true);
    	}
    	else {
    		text.SetActive(false);
    	}

    	if (isFocus && !hasInteracted) 
    	{
    		Interact();
    		hasInteracted = true;
    	}

    	looking = false;
    }

    public void OnFocused (GameObject Player) 
    {
    	isFocus = true;
    	player = Player;
    	hasInteracted = false;
    }

    public void OnDefocused ()
    {
    	isFocus = false;
    	player = null;
    	hasInteracted = false;
    }

    public bool distanceCheck(Transform playerTransform) 
    {	

    	float distance = Vector3.Distance(playerTransform.position, transform.position);

    	bool result = (distance <= radius);
    	if (result) 
    	{
    		looking = true;
    	}

		return result;
    }

    void OnDrawGizmosSelected() 
    {
    	Gizmos.color = Color.yellow;
    	Gizmos.DrawWireSphere(transform.position, radius);
    }

}
