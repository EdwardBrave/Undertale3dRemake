using UnityEngine;

namespace Tools
{
    public class DontDestroyOnLoad: MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}