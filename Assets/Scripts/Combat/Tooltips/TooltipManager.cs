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
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager instance;

        public GameObject toolTipObject;
        TooltipBox toolTip;

        public GameObject spellToolTipObject;
        SpellTooltip spellToolTip;

        private void Start()
        {
            instance = this;
            if (toolTipObject != null)
            {
                toolTip = toolTipObject.GetComponent<TooltipBox>();
                toolTipObject.SetActive(false);
            }

            if (spellToolTipObject)
            {
                spellToolTip = spellToolTipObject.GetComponent<SpellTooltip>();
                spellToolTipObject.SetActive(false);
            }
        }

        public void ShowTooltip(bool active, string titleText, string descText)
        {
            Debug.Log("show tooltip " + active);
            if (spellToolTipObject != null)
                spellToolTipObject.SetActive(false);

            if (toolTip == null || DragManager.instance.draggedCard != null)
            {
                toolTipObject.SetActive(false);
                return;
            }

            toolTipObject.SetActive(active);

            if (active)
            {
                toolTip.SetText(titleText, descText);
            }
        }

        public void ShowSpellTooltip(bool active, string titleText, Spell spell)
        {
            if (toolTipObject != null)
                toolTipObject.SetActive(false);

            if (spellToolTip == null || DragManager.instance.draggedCard != null)
            {
                spellToolTipObject.SetActive(false);
                return;
            }

            spellToolTipObject.SetActive(active);

            if (active)
            {
                spellToolTip.SetText(titleText, spell);
            }
        }

        public void EnableTooltips(bool active)
        {
            if (toolTipObject != null)
                toolTipObject.SetActive(false);

            TooltipInfo[] allTooltips = GameObject.FindObjectsOfType<TooltipInfo>();

            foreach (TooltipInfo item in allTooltips)
            {
                item.EnableRaycasting(active);
            }
        }
    }
}