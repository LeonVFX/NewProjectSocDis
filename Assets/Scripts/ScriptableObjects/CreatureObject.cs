using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creature")]
public class CreatureObject : ScriptableObject
{
    [Header("Creature Basic Stats")]
    public int killStunTime;
    [Header("Creature Sprint Stats")]
    public int trackInterval;
    public float speedDifference;
    public GameObject footprintPrefab;
    [Header("Creature Hinting Data")]
    public int goopInterval;
    public GameObject goopParticlePrefab;
    public float goopStartTime;
    public float goopBufferTime;
}