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
    public class ProgressQuest : MonoBehaviour
    {
        public Quest quest;

        [ContextMenu("Progress Quest")]
        public void Progress()
        {
            quest.QuestProgress();
        }
    }
}
