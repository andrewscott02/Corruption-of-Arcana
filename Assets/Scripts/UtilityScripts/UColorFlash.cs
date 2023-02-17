using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class UColorFlash : MonoBehaviour
    {
        MaterialInstance matInst;
        SpriteRenderer spriteRenderer;
        float revertTime = 0.1f;

        Color flashColour;
        float p = 0;

        private void Start()
        {
            matInst = GetComponent<MaterialInstance>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        #region Dissolve

        bool dissolving = false;

        public void ApplyDissolve(E_DamageTypes effectType)
        {
            dissolving = true;
            //Debug.Log("Dissolve");
            if (matInst != null)
                matInst.SetDissolveColor(ColourFromDamageType(effectType));

            InvokeRepeating("DissolveInvoke", 0f, 0.05f);
        }

        public void ReverseDissolve(E_DamageTypes effectType)
        {
            dissolving = true;
            //Debug.Log("Reverse Dissolve");
            if (matInst != null)
                matInst.SetDissolveColor(ColourFromDamageType(effectType));

            InvokeRepeating("ReverseDissolveInvoke", 0f, 0.05f);
        }

        float dissolveTime = 0.1f;
        float currentDissolve = 0;

        void DissolveInvoke()
        {
            currentDissolve += dissolveTime;

            if (currentDissolve >= 1)
            {
                CancelInvoke();
                currentDissolve = 1;
                dissolving = false;
            }

            if (matInst != null)
                matInst.SetDissolve(currentDissolve);
        }

        void ReverseDissolveInvoke()
        {
            currentDissolve -= dissolveTime;

            if (currentDissolve <= 0)
            {
                CancelInvoke();
                currentDissolve = 0;
                dissolving = false;
            }

            if (matInst != null)
                matInst.SetDissolve(currentDissolve);
        }

        #endregion

        #region Colour Flash

        public void Flash(E_DamageTypes effectType)
        {
            if (dissolving) return;

            //Debug.Log("Flash color");
            CancelInvoke();
            p = 0;

            flashColour = ColourFromDamageType(effectType);

            if (matInst != null)
                matInst.SetColour(flashColour);
            else
                spriteRenderer.color = flashColour;

            InvokeRepeating("RevertColour", 0f, 0.05f);
        }

        void RevertColour()
        {
            if (matInst != null)
                matInst.SetColour(LerpColour(p));
            else
                spriteRenderer.color = LerpColour(p);

            p += revertTime;

            if (p >= 1)
            {
                CancelInvoke();
                p = 0;
                if (matInst != null)
                    matInst.SetColour(normalColour);
                else
                    spriteRenderer.color = normalColour;
            }
        }

        Color LerpColour(float i)
        {
            Color lerpColour = new Color(0, 0, 0, 0);

            lerpColour.r = Mathf.Lerp(flashColour.r, normalColour.r, i);
            lerpColour.g = Mathf.Lerp(flashColour.g, normalColour.g, i);
            lerpColour.b = Mathf.Lerp(flashColour.b, normalColour.b, i);
            lerpColour.a = Mathf.Lerp(flashColour.a, normalColour.a, i);

            return lerpColour;
        }

        #endregion

        #region Edge Highlight

        public void Highlight(Color color)
        {
            if (dissolving) return;

            flashColour = color;
            if (matInst != null)
                matInst.SetColour(flashColour);
            else
                spriteRenderer.color = flashColour;
        }

        public void RemoveHighlightColour()
        {
            if (dissolving) return;

            if (matInst != null)
                matInst.SetColour(normalColour);
            else
                spriteRenderer.color = normalColour;
        }

        #endregion

        #region Colour

        [ColorUsage(true, true)]
        public Color normalColour = new Color(255, 255, 255, 255);

        [ColorUsage(true, true)]
        public Color physicalColour = new Color(255, 40, 40, 255);
        [ColorUsage(true, true)]
        public Color perforationColour = new Color(129, 16, 255, 103);
        [ColorUsage(true, true)]
        public Color septicColour = new Color(23, 132, 69, 255);
        [ColorUsage(true, true)]
        public Color bleakColour = new Color(90, 241, 255, 255);
        [ColorUsage(true, true)]
        public Color staticColour = new Color(255, 187, 81, 255);
        [ColorUsage(true, true)]
        public Color emberColour = new Color(250, 143, 86, 255);

        [ColorUsage(true, true)]
        public Color healColour = new Color(76, 255, 76, 255);
        [ColorUsage(true, true)]
        public Color shieldColour = new Color(76, 76, 185, 255);
        [ColorUsage(true, true)]
        public Color arcanaColour = new Color(255, 16, 255, 255);

        [ColorUsage(true, true)]
        public Color defaultColour = new Color(245, 75, 243, 255);

        Color ColourFromDamageType(E_DamageTypes damageType)
        {
            switch (damageType)
            {
                case E_DamageTypes.Physical:
                    return physicalColour;
                case E_DamageTypes.Perforation:
                    return perforationColour;
                case E_DamageTypes.Septic:
                    return septicColour;
                case E_DamageTypes.Bleak:
                    return bleakColour;
                case E_DamageTypes.Static:
                    return staticColour;
                case E_DamageTypes.Ember:
                    return emberColour;

                case E_DamageTypes.Healing:
                    return healColour;
                case E_DamageTypes.Shield:
                    return shieldColour;
                case E_DamageTypes.Arcana:
                    return arcanaColour;
                default:
                    return defaultColour;
            }
        }

        #endregion
    }
}
