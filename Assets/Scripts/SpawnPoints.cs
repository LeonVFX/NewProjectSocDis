using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 pos3;
    public Vector3 pos4;
    public Vector3 pos5;
    public Vector3 pos6;
    public Vector3 pos7;
    public Vector3 pos8;

        public Vector2 GetPosition(int playerNumber)
    {
        Vector3 tempPos = transform.position;

        switch (playerNumber)
        {
            case 1:
                tempPos += pos1;
                break;
            case 2:
                tempPos += pos2;
                break;
            case 3:
                tempPos += pos3;
                break;
            case 4:
                tempPos += pos4;
                break;
            case 5:
                tempPos += pos5;
                break;
            case 6:
                tempPos += pos6;
                break;
            case 7:
                tempPos += pos7;
                break;
            case 8:
                tempPos += pos8;
                break;
        }

        return tempPos;
    }
}