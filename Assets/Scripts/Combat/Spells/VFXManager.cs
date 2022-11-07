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
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager instance;
        public Vector2 middlePositionOffset;
        public float projectileSpeed = 0.4f;
        public float speedCalculationMultiplier = 700;

        private void Start()
        {
            instance = this;
        }

        public void AffectSelfDelay(Spell spellRef, Character caster, CombatHelperFunctions.SpellModule spell, Vector2 spawnPosition, float delay)
        {
            StartCoroutine(IDelayAffectSelf(spellRef, caster, spell, spawnPosition, delay));
        }

        IEnumerator IDelayAffectSelf(Spell spellRef, Character caster, CombatHelperFunctions.SpellModule spell, Vector2 spawnPosition, float delay)
        {
            yield return new WaitForSeconds(delay);
            float effectDelay = QueryTime(spawnPosition, caster.transform.position);
            VFXManager.instance.SpawnProjectile(spawnPosition, caster.transform.position, spellRef.projectileObject);
            yield return new WaitForSeconds(effectDelay);
            spellRef.AffectSelf(caster, spell);
        }

        public void AffectTargetDelay(Spell spellRef, Character caster, Character target, CombatHelperFunctions.SpellModule spell, Vector2 spawnPosition, float delay)
        {
            StartCoroutine(IDelayAffectTarget(spellRef, caster, target, spell, spawnPosition, delay));
        }

        IEnumerator IDelayAffectTarget(Spell spellRef, Character caster, Character target, CombatHelperFunctions.SpellModule spell, Vector2 spawnPosition, float delay)
        {
            yield return new WaitForSeconds(delay);
            float effectDelay = QueryTime(spawnPosition, target.transform.position);
            VFXManager.instance.SpawnProjectile(spawnPosition, target.transform.position, spellRef.projectileObject);
            yield return new WaitForSeconds(effectDelay);
            spellRef.AffectTarget(caster, target, spell);
        }

        public void SpawnProjectile(Vector2 spawnPosition, Vector2 targetPosition, Object projectileRef)
        {
            if (projectileRef == null)
            {
                Debug.Log("No reference to projectile");
                return;
            }

            //Spawn projectile at spawn position
            GameObject projectileObject;
            projectileObject = Instantiate(projectileRef, this.gameObject.transform) as GameObject;

            if (projectileObject == null)
            {
                Debug.Log("2nd attempt");
                projectileObject = Instantiate(projectileRef, this.gameObject.transform) as GameObject;
                //return;
            }

            if (projectileObject == null)
            {
                Debug.Log("No game object spawned, returning");
                return;
            }

            projectileObject.transform.position = spawnPosition;

            ProjectileMovement projectileMovement = projectileObject.GetComponent<ProjectileMovement>();

            List<Vector2> movementPositions = new List<Vector2>();
            movementPositions.Add(spawnPosition);
            movementPositions.Add(spawnPosition + middlePositionOffset);
            movementPositions.Add(targetPosition);

            projectileMovement.MoveToPositions(projectileSpeed, movementPositions);
        }

        public float QueryTime(Vector2 spawnPosition, Vector2 targetPosition)
        {
            List<Vector2> movementPositions = new List<Vector2>();
            movementPositions.Add(spawnPosition);
            movementPositions.Add(spawnPosition + middlePositionOffset);
            movementPositions.Add(targetPosition);

            //Calculate and return delay (T=D/S)
            float distance = 0;

            for (int i = 0; i < movementPositions.Count - 1; i++)
            {
                distance += Vector2.Distance(movementPositions[i], movementPositions[i + 1]);
            }

            float time = distance / (projectileSpeed * speedCalculationMultiplier);
            //Debug.Log(time);
            time = Time.fixedDeltaTime / projectileSpeed; //Instead of 1, use the time between frames
            //float fixedFrameTime = Time.fixedDeltaTime;
            return time;
        }
    }
}