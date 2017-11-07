using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBase : MonoBehaviour
{
    public RoomData Data;
    
	void Start ()
    {
		
	}

    public void Flip()
    {
        this.Data.IsFlipped = true;
        this.transform.localScale = new Vector3(-1, 1, 1);
    }
}
