using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSS
{
    public class ProgramPurchase : MonoBehaviour
    {
        public void Purchase(int program)
        {
            ProgramManager.instance.Purchase(program);
        }
    }
}
