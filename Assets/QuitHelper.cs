using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSS
{
    public class QuitHelper : MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit();
        }

        public void Restart()
        {
            GameManager.instance.RestartGame();
        }
    }
}