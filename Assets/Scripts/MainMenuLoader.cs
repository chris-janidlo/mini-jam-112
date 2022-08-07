using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

namespace mj112
{
    public class MainMenuLoader : MonoBehaviour
    {
        [Scene]
        public string GameScene;

        public void OnStartButtonPressed ()
        {
            SceneManager.LoadScene(GameScene);
        }
    }
}
