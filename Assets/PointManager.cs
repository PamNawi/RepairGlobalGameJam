using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            playerManager.CollectOrb();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(this.gameObject, 0.1f);
    }
}
