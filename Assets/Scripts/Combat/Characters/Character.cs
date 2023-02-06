using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class Character : MonoBehaviour
    {
        #region Setup

        public CharacterStats stats;
        protected TeamManager teamManager; public TeamManager GetManager() { return teamManager; }
        protected CharacterHealth health; public CharacterHealth GetHealth() { return health; }
        protected StatusManager statusManager; public StatusManager GetStatusManager() { return statusManager; }

        [HideInInspector]
        public EnemySpawner spawner; //Unused for the player right now, but it might be used in the future

        Deck2D deck;

        protected virtual void Start()
        {
            SetupReferences();
        }

        [ContextMenu("Setup References")]
        public virtual void SetupReferences()
        {
            health = GetComponent<CharacterHealth>();
            deck = GetComponentInChildren<Deck2D>();
            teamManager = GetComponentInParent<TeamManager>();
            if (teamManager != null)
                teamManager.Add(this);
            simulateValues = GetComponentInChildren<SimulateValues>();

            statusManager = GetComponentInChildren<StatusManager>();
        }

        #endregion

        #region Taking Turn

        public virtual void StartTurn()
        {
            damageTakenLastTurn = damageTakenThisTurn;
            damageTakenThisTurn = 0;
            CheckOverlay();
            health.StartTurn();
        }

        public virtual CombatHelperFunctions.SpellUtility PrepareSpell()
        {
            //Overwritten by children
            return new CombatHelperFunctions.SpellUtility();
        }

        [HideInInspector]
        public int damageTakenThisTurn;
        int damageTakenLastTurn; public int GetDamageTakenThisTurn() { return damageTakenLastTurn; }

        public void CheckOverlay()
        {
            deck.CheckOverlay();
        }

        #endregion

        #region Health Checks

        /// <summary>
        /// Checks if the character should be dying
        /// </summary>
        public void CheckHealth()
        {
            if (health.dying)
            {
                StartCoroutine(IDelayDeath(0.01f));
            }
        }

        public IEnumerator IDelayDeath(float delay)
        {
            yield return new WaitForSeconds(delay);
            health.PlayDeathSound();
            CombatManager.instance.CharacterDied(this);
            teamManager.team.Remove(this);
            if (spawner != null)
            {
                spawner.filled = false;
            }
            Destroy(gameObject);
        }

        #endregion

        #region Statuses

        //Positive Statuses
        [HideInInspector]
        public bool enlightened;
        //Neutral Statuses
        [HideInInspector]
        public bool empowerDeck, weakenDeck;
        //Negative Statuses
        [HideInInspector]
        public bool banish, charm, silence, stun, curse, confuse;

        /// <summary>
        /// Apply or remove a status effect from the character
        /// </summary>
        /// <param name="apply">Whether or not the status will be applied or removed</param>
        /// <param name="status">The data status effect</param>
        public void ApplyStatus(bool apply, E_Statuses status)
        {
            switch (status)
            {
                //Positive Effects
                case E_Statuses.Reflect:
                    if (deck != null)
                    {
                        deck.GetComponentInChildren<EmpowerWeakenManager>().DisplayReflect(apply);
                    }
                    break;
                case E_Statuses.Enlightened:
                    enlightened = apply;
                    break;
                //Neutral Effects
                case E_Statuses.EmpowerDeck:
                    empowerDeck = apply;
                    if (deck != null)
                    {
                        deck.GetComponentInChildren<EmpowerWeakenManager>().DisplayEmpower(apply);
                    }
                    break;
                case E_Statuses.WeakenDeck:
                    weakenDeck = apply;
                    if (deck != null)
                    {
                        deck.GetComponentInChildren<EmpowerWeakenManager>().DisplayWeaken(apply);
                    }
                    break;
                //Negative Effects
                case E_Statuses.Banish:
                    banish = apply;
                    health.ActivateArt(!apply);
                    break;
                case E_Statuses.Charm:
                    charm = apply;
                    break;
                case E_Statuses.Silence:
                    silence = apply;
                    Silence();
                    break;
                case E_Statuses.Stun:
                    stun = apply;
                    break;
                case E_Statuses.Curse:
                    curse = apply;
                    health.CheckCurseHealth();
                    break;
                case E_Statuses.Redirect:
                    if (apply)
                        CombatManager.instance.redirectedCharacter = this;
                    else
                        CombatManager.instance.redirectedCharacter = null;
                    break;
                case E_Statuses.Confuse:
                    confuse = apply;
                    break;
                default:
                    break;
            }

            CheckOverlay();
        }

        /// <summary>
        /// Reduce the player's max arcana count
        /// </summary>
        protected virtual void Silence()
        {
            ArcanaManager arcanaManager = Timeline.instance.GetArcanaManager();
            if (silence)
            {
                if (arcanaManager.silenced == false)
                {
                    arcanaManager.silenced = true;
                    arcanaManager.AdjustArcanaMax(-2);
                }
            }
            else
            {
                arcanaManager.silenced = false;
                Timeline.instance.GetArcanaManager().AdjustArcanaMax(2);
            }
        }

        public bool CanCast()
        {
            bool canCast = true;

            if (banish || health.dying || stun)
            {
                canCast = false;
            }

            return canCast;
        }

        public bool CanBeTargetted()
        {
            bool canTarget = true;

            if (banish || health.GetHealth() < 0 || health.dying)
            {
                canTarget = false;
            }

            return canTarget;
        }

        #endregion

        #region Simulating Turn

        int damage = 0, healing = 0, shield = 0;
        float highestExecute = 0;
        SimulateValues simulateValues;

        public void SimulateValues(int newDamage, int newHealing, int newShield, float newExecute)
        {
            damage += newDamage;
            healing += newHealing;
            shield += newShield;

            //Save only the highest execute value
            if (newExecute > highestExecute)
            {
                highestExecute = newExecute;
            }
            
            PreviewValues();
        }

        void PreviewValues()
        {
            bool kills = damage >= health.GetHealth() + healing + shield ||
                        health.GetHealthPercentageFromDamage(damage - shield) < highestExecute;
            
            int damagePreview = damage;
            int healingPreview = healing;
            int shieldPreview = shield;

            if (kills)
            {
                damagePreview = Mathf.Abs(Mathf.Clamp(damage, 0, health.GetHealth() - damage));
                healingPreview = 0;
                shieldPreview = 0;
            }

            simulateValues.DisplayValues(damagePreview, healingPreview, shieldPreview, kills);
        }

        public void ResetValues()
        {
            damage = 0; healing = 0; shield = 0; highestExecute = 0;
            PreviewValues();
        }

        #endregion
    }
}