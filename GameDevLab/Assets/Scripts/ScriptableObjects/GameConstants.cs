using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
    // lives
    public int maxLives;

    // Mario's movement
    public int speed;
    public int maxSpeed;
    public int upSpeed;
    public int deathImpulse;

    public float maxFallSpeed = 10;
    public float defaultGravity = 1;
    public float fallGravity;
    public Vector3 marioStartingPosition;


    // Goomba's movement
    public float goombaPatrolTime;
    public float goombaMaxOffset;
    internal float flickerInterval;
}