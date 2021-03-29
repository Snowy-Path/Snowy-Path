using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for Ennemy Controllers.
/// </summary>
public interface IEnnemyController {

    /// <summary>
    /// Manage different processing of a single hit depending on the Ennemy Controller implementation.
    /// </summary>
    /// <param name="toolType">The type of tool that called this method. Used to differentiate between Pistol and Torch weapons.</param>
    /// <param name="attackDamage">The damage value to be dealt.</param>
    void Hit(EToolType toolType, int attackDamage);

}
