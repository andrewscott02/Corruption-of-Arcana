using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SaveSceneManager : MonoBehaviour
    {
        //C:\Users\as243879\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        //C:\Users\mr232432\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled

        #region Singleton
        //Code from last year

        public static SaveSceneManager instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                gameObject.transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Singleton();
            SaveManager.instance.saveAllData += SaveSceneData;
            SaveManager.instance.saveAllBaseData += SaveBaseQuestData;
            SaveManager.instance.overideAllBaseData += OverideBaseQuestData;
            SaveManager.instance.loadAllData += LoadSceneData;
            SaveManager.instance.loadAllBaseData += LoadBaseQuestData;
        }

        [ContextMenu("Save Quest Data")]
        public void SaveSceneData()
        {
            //SaveScene
            if (LoadingScene.instance != null)
                LoadingScene.instance.SaveScene();



            // Saving stuff

            // Save scene name, pos, rot
            SavingLoading.instance.Save();

            // Save player position


            // Save player rotation
        }

        [ContextMenu("Save Base Quest Data")]
        public void SaveBaseQuestData()
        {
            //SaveBaseSceneData
            if (LoadingScene.instance != null)
                LoadingScene.instance.SaveScene();
        }

        [ContextMenu("Overide Base Quest Data")]
        public void OverideBaseQuestData()
        {
            if (LoadingScene.instance != null)
                LoadingScene.instance.SaveScene();
        }

        [ContextMenu("Load Scene Data")]
        public void LoadSceneData()
        {
            //LoadingScene.instance.loadScene = GetData();
            SavingLoading.instance.Load();

        }

        [ContextMenu("Load Base Quest Data")]
        public void LoadBaseQuestData()
        {
            //LoadingScene.instance.loadScene = GetData();
        }

        [ContextMenu("Reset Quest Data")]
        public void ResetQuestData()
        {
            //LoadingScene.instance.loadScene = GetData();
        }
    }
}
