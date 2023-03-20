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
    public static class QuestQuickReset
    {
        public static List<string> restartStatic = new List<string>();
        public static List<string> resetStatic = new List<string>();

        public static void QuestResetStatic()
        {
            restartStatic.Add("C1M");

            resetStatic.Add("S1M-Apothecary");
            resetStatic.Add("S2M-Apothecary");

            Quest[] allQuests = Resources.FindObjectsOfTypeAll<Quest>();

            foreach (var item in allQuests)
            {
                if (restartStatic.Contains(item.name))
                {
                    item.ForceRestartQuest();
                }
                if (resetStatic.Contains(item.name))
                {
                    item.ForceResetQuest();
                }
            }
        }
    }
}
