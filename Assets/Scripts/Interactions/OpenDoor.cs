using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OpenDoor : MonoBehaviour
{
    private PhotonView doorView;
    bool open = false;
    bool broke = false;
    bool change = true;
    private bool isButtonPressed = false;
   
    //the door
    [SerializeField] GameObject Door;

    private void Start()
    {
        doorView = GetComponent<PhotonView>();

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            RegisterPlayer(player);
        }
        
    }
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        //once true the door will move
        if (open == true)
        {
            //After Interact used moves door away
            if (isButtonPressed)
            {
                     doorView.RPC("RPC_DoorChange", RpcTarget.All);
            }
        }

        if (broke == true)
        {
            if (isButtonPressed)
            {
                //GameObject.Destroy(Door);
                doorView.RPC("RPC_DoorDestroy", RpcTarget.All);
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
            Debug.Log("Breakable Door");
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

    private void RegisterPlayer(Player player)
    {
        player.PHUD.OnInteraction += PressButton;
    }
    private void PressButton(PhotonView playerView)
    {
        IEnumerator pressedButton = ButtonPressed();
        isButtonPressed = true;
        StartCoroutine(pressedButton);
    }

    private IEnumerator ButtonPressed()
    {
        yield return new WaitForEndOfFrame();
        isButtonPressed = false;
    }

    [PunRPC]
    private void RPC_DoorChange()
    {
        Debug.Log("Door changed");
        change = !change;
        Door.SetActive(change);
    }

    [PunRPC]
    private void RPC_DoorDestroy()
    {
        Debug.Log("Door destroyed");
        GameObject.Destroy(Door);
        //Door.SetActive(false);
        
    }



















    /* public void Changed()
     {
         DoorChange?.Invoke();
     }
     private void DoorHasChanged()
     {
         change = !change;
         Door.SetActive(change);
     }

     public void Destroyed()
     {
         DoorDestroyed?.Invoke();
     }
     private void DoorIsDestroyed()
     {

     }*/
}
