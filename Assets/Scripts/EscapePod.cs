using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapePod : MonoBehaviour
{
    [SerializeField] GameObject EscapePod1;
    private int spaceleft = 2;
    private int countdown = 5;
    private bool enterEscapePod = false;
    private bool canEnter = false;
    private bool canEscape = false;
    private bool infectedEscaped = false;
    private bool launchPod = false;


   /* private void PlayerEscape()
    {
        This Player Position set to Escape Pod Position
         if(spaceleft == 0)
            pop up for players to either leave now or wait for a 3 player to enter then auto launch
            countdown will decrease - countdown -= Time.deltaTime
        if(Key.GetButtonDown("Interact")
            Launch escape pod with players and will win as long as not infected
        if(countdown == 0)
           escape will wait for another player
        if(player is infected)
           Everyone in escape pod will lose
    }*/

    void Update()
    {
        //If a researcher is near an escape and has space then the can enter
        if(canEnter == true && spaceleft > 0)
        {
            Debug.Log("Player has reached escape pod");
            //PlayerEscape();
            spaceleft = spaceleft - 1;
            //this.GetComponent<Player>(Player);
        }
    }
    //Will recognise the player as either a researcher or monster
    private void OnTriggerEnter(Collider player)
    {
        
        if (player.tag == "Researcher")
        {
            Debug.Log("Escape Pod in reach");
            canEnter = true;
            
        }
       /* if (player.tag == "Infected")
        {
            Debug.Log("Escape Pod in reach");
            canEscape = true;

        }*/
        if (player.tag == "Creature")
        {
            Debug.Log("Unable to use Escape Pod");

        }
    }
    private void OnTriggerExit(Collider player)
    {
        if (player.tag == "Researcher")
        {
            Debug.Log("Player Out of Range");
            canEnter = false;

        }

        if (player.tag == "Creature")
        {
            Debug.Log("Creature near Escape Pods");

        }
    }
}

//MIGHT BE WORTH CHECKING FOR RESEARCHER IS INFECTED INSTEAD OF CHECKING TAGS