using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OpenDoor : MonoBehaviour
{
   // public event System.Action DoorChange;
   // public event System.Action DoorDestroyed;
    private PhotonView doorView;
    public static OpenDoor od;
    //   float distancetoTarget;
    bool open = false;
    bool broke = false;
    bool change = true;
    //float timer = 0.0f;
    private bool isButtonPressed = false;

   // private List<OpenDoor> doorList;
   // private int numberOfDoors = 13;
   // public List<OpenDoor> DoorRange;

    //the door
    [SerializeField] GameObject Door;

    private void Start()
    {
       // DoorChange += Changed;
       // DoorDestroyed += Destroyed;
        doorView = GetComponent<PhotonView>();

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            RegisterPlayer(player);
        }
        
    }
    private void Update()
    {
        //once true the door will move
        if (open == true)
        {
            //After Interact used moves door away
            if (isButtonPressed)
            {                         
                    //Debug.Log("Door state changing");                
                     change = !change;
                     Door.SetActive(change);
                    // Changed();                     
                     doorView.RPC("RPC_DoorChange", RpcTarget.All);
            }
        }

        if (broke == true)
        {
            if (isButtonPressed)
            {
                GameObject.Destroy(Door);
                //doorView.RPC("RPC_DoorDestroy", RpcTarget.All);
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
            Debug.Log("Not near door");
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
   /* public void Changed()
    {
        DoorChange?.Invoke();       
    }*/

    [PunRPC]
    private void RPC_DoorChange()
    {
        Debug.Log("Door changed");
        change = !change;
        Door.SetActive(change);
    }

/*    private void RPC_SetUpDoors()
    {
        doorList = new List<OpenDoor>(FindObjectsOfType<OpenDoor>());
        foreach (Player player in PlayerManager.pm.playerList)
        {
            if (!player.playerView.IsMine)
                continue;

            foreach (OpenDoor door in doorList)
            {
               
            }
        }
    }*/
    /*public void Destroyed()
    {
        DoorDestroyed?.Invoke();
    }

    [PunRPC]
    private void RPC_DoorDestroy()
    {
        Debug.Log("Door destroyed");
        GameObject.Destroy(Door);
    }*/



















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
