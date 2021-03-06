﻿/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the interface fot the enemy logic.
 * ******************************************/
using UnityEngine;

public interface IEnemy
{
    /// <summary>
    /// Set the column for each enemy, this is very important to be able to set the shooter and direction setter.
    /// </summary>
    int ColumnID { get;}

    /// <summary>
    /// Set the Row for each enemy, this is very important to be able to set the shooter and direction setter.
    /// </summary>
    int RowID { get;}

    /// <summary>
    /// Checks if the enemy can shoot.
    /// </summary>
    bool CanShoot { get;}

    /// <summary>
    /// Checks if the enemy can shoot.
    /// </summary>
    bool SetsDirection { get; }

    /// <summary>
    /// Sets the amount of units to move the enamy.
    /// </summary>
    float MoveUnits { get;}

    /// <summary>
    /// Sets Die animation and resets Values.
    /// </summary>
    void Die();

    /// <summary>
    /// Shoots a bullet from the bullet pool.
    /// </summary>
    /// <param name="bullet"></param>
    void Shoot(GameObject bullet);

    /// <summary>
    /// Resets values to start new game without re loading the scene.
    /// </summary>
    void Reset();

    /// <summary>
    /// Starts the movement of the enemy.
    /// </summary>
    void StartMovement();

    /// <summary>
    /// Moves the Enemies Down.
    /// </summary>
    void MoveDown();

    /// <summary>
    /// Sets up the enemy row, column, canShoot etc.
    /// </summary>
    void SetUpEnemy(int row, int column, bool canShoot, bool setsDirection);

}
