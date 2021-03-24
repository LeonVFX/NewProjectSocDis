using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OpenDoor : MonoBehaviour
{
    //   float distancetoTarget;
    bool open = false;
    bool broke = false;
    bool change = true;
    float timer = 0.0f;

    //the door
    [SerializeField] GameObject Door;
    void Update()
    {
        //once true the door will move
        if (open == true)
        {
            //After Interact used moves door away
            if (Input.GetButtonDown("Interact"))
            {                               
                    change = !change;
                   Door.SetActive(change);                                  
            }
        }

        if (broke == true)
        {
            if (Input.GetButtonDown("Interact"))
            {
                GameObject.Destroy(Door);
            }
        }
    }

   

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherParent = (other.transform.parent != null) ? other.transform.parent.gameObject : null;

        if (otherParent == null)
            return;

        if (otherParent.tag == "Researcher" || otherParent.tag == "Creature")
        {
            Debug.Log("Near door");
            PhotonView playerView = otherParent.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;
            open = true;
        }

        if (otherParent.tag == "Creature" && GameManager.gm.currentStage == GameManager.GameStage.Stage2)
        {
            open = false;
            broke = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherParent = (other.transform.parent != null) ? other.transform.parent.gameObject : null;

        if (otherParent == null)
            return;

        if (otherParent.tag == "Researcher" || otherParent.tag == "Creature")
        {
            PhotonView playerView = otherParent.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;
            open = false;
        }
        if (other.tag == "Creature" && GameManager.gm.currentStage == GameManager.GameStage.Stage2)
        {
            open = false;
            broke = false;
        }
    }   
}
  