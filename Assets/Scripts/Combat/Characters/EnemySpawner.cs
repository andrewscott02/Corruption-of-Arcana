using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EnemySpawner : MonoBehaviour
    {
        public int order = 0;
        public bool filled = false;

        public void SpawnEnemy(Object enemyRef)
        {
            Instantiate(enemyRef, transform);
        }
    }
}