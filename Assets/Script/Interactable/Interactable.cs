using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    bool isFocus = false;
    protected GameObject player;

    bool hasInteracted = false;
    bool looking = false;

    public GameObject text;

    protected virtual void Interact () 
    {
    	 // this method is meant to override
    	Debug.Log("Interacting with " + transform.name);
    }

    protected virtual void constantUpdate() 
    {

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

        constantUpdate();
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
    	hasInteracted = false;
    }

    public bool distanceCheck(Transform playerTransform, Transform camTransform) 
    {	
        Vector3 direction = (transform.position - camTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        text.transform.rotation = lookRotation;

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
