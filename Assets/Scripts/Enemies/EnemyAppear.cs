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
    public class EnemyAppear : MonoBehaviour, IInteractable, ICancelInteractable
    {
        public GameObject art;
        public Object fx;
        public LayerMask layerMask;
        bool active = false;
        EnemyAI aiScript;

        GameObject player;

        private void Start()
        {
            art.SetActive(false);
            aiScript = GetComponent<EnemyAI>();
        }

        public void Interacted(GameObject playerRef)
        {
            player = playerRef;
        }

        public void CancelInteraction(GameObject playerRef)
        {
            if (playerRef == player)
            {
                Debug.Log("Cancel interaction");

                player = null;
                Deactivate();
            }
        }

        private void Update()
        {
            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                Debug.Log(player.name);
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, targetDirection, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.Log("Collided with: " + hit.collider.gameObject);
                    if (hit.collider.gameObject == player)
                    {
                        if (!active)
                        {
                            Debug.Log("Interacted - Unearth and activate AI");
                            //Unearth and activate AI
                            art.SetActive(true);
                            aiScript.ActivateAI(player);
                            if (fx != null)
                            {
                                Instantiate(fx, this.gameObject.transform);
                                fx = null;
                            }

                            active = true;
                        }
                        else
                        {
                            //Already active
                        }
                    }

                    Debug.DrawRay(transform.position, targetDirection, Color.yellow);
                    Debug.Log("Did Hit");
                }
                else
                {
                    Debug.DrawRay(transform.position, targetDirection, Color.white);
                    Debug.Log("Did not Hit " + targetDirection);
                }
            }
        }

        void Deactivate()
        {
            if (active)
            {
                Debug.Log("Deactivate AI");
                //Unearth and activate AI
                //art.SetActive(true);
                aiScript.DeactivateAI();

                active = false;
            }
            else
            {
                //Already active
            }
        }
    }
}