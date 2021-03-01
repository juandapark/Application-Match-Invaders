/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic setting the difficulty of the game.
 * ******************************************/
using UnityEngine;

[CreateAssetMenu]
public class GameDifficulty : ScriptableObject
{
    [Header("--Do not go crazy with this!!, the movement is based in units not actual speed, See enemy manager--")]
    public float EnemySpeedInUnits;
    public float EnemySpeedInUnitsDown;
    [Header("--------------------")]

    public float EnemyBulletSpeed;
    public float PlayerBulletSpeed;
    public int NumberOfLives;
    public int ProtectorLives;
    public int EnemyShootMaxDelay;

}
