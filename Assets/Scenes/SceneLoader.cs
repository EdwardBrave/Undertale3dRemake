﻿using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        public SceneAsset nextScene;

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        
        public void LoadSelectedScene()
        {
            SceneManager.LoadScene(nextScene.name);
        }
    
        public void LoadSceneById(int index)
        {
            SceneManager.LoadScene(index);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("SceneDoor"))
                SceneManager.LoadScene(nextScene.name);
        }
    }
}
