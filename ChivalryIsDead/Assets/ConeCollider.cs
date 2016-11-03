using UnityEngine;
using System.Collections.Generic;

public class ConeCollider : MonoBehaviour {


    /// <summary>
    /// used to hold all enemies that are inside the collider radius
    /// </summary>
    public static List<Collider> CollidersInside = new List<Collider>();
 
    //called when something enters the trigger
    void OnTriggerEnter(Collider other)
    {
        //if the object is not already in the list
        if (!CollidersInside.Contains(other))
        {

            //add the object to the list
            CollidersInside.Add(other);
        }
    }

    //called when something exits the trigger
    void OnTriggerExit(Collider other)
    {
        //if the object is in the list
        if (CollidersInside.Contains(other))
        {
            //remove it from the list
            CollidersInside.Remove(other);
        }
    
	}
	
}
