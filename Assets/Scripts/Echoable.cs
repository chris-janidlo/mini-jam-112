using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public abstract class Echoable : MonoBehaviour
    {
        public abstract object GetState ();

        public abstract Echo CreateEcho ();
    }
}
