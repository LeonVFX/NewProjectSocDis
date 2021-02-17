using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
 //   float distancetoTarget;
    GameObject Door;
    GameObject Door2;
    bool open = false;
    //bool broke = false;
    int change = 0;
    float currentposx = 4.39f;
    float currentposy = 0.92f;
    float closedposx =  1000f;
    float closedposy = 1000f;
    float timer = 0.0f;

    //the door
    [SerializeField] GameObject DoorOpen = null;

    void Update()
    {
        timer += Time.deltaTime;
        //once true the door will move
        if (open == true)
        {
            //After Interact used moves door away
            if (Input.GetButtonDown("Interact"))
            {
                timer = 0f;
               // Debug.Log("Move");
                DoorOpen.transform.position = new Vector2(closedposx, closedposy);
                change = change + 1;
                //timer += Time.deltaTime;
            }
         
        }
        //resets door
        if (timer >= 5f)
        {
            DoorOpen.transform.position = new Vector2(currentposx, currentposy);
            timer = 0f;
            //   timer = 0;
            //    Debug.Log("has changed");
            // timer += Time.deltaTime;
        }
        // else
        // {
        //     open = false;
        // }
    }

    /*    if(broke == true)
        {
            GameObject.Destroy(Dooropen);
        }*/
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Researcher")
        {
            open = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Researcher")
        {
            open = false;
        }
    }

    /*  private void OnTriggerEnter2D(Collider2D other)
      {
          if (other.tag == "Creature")
          {
              broke = true;
          }
      }

      private void OnTriggerExit2D(Collider2D other)
      {
          if (other.tag == "Creature")
          {
              broke = false;
          }
      }*/

    //DONT USE CODE

    /*  if (timer >= 5)
           {
               Dooropen.transform.position = new Vector2(currentposx, currentposy);
               //   timer = 0;
               //    Debug.Log("has changed");
               // timer += Time.deltaTime;
           }*/

    // if(timer >= 5)
    //  {
    /*  if ((change % 2) == 0)
      {
          Debug.Log("Just Work");
          //change = 0;
          Dooropen.transform.position = new Vector2(currentposx, currentposy);
      }*/

    //   }
}
