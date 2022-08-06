using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public abstract class PrefabEchoable : Echoable
    {
        public Echo EchoPrefab;

        public override Echo CreateEcho ()
        {
            Echo echo = Instantiate(EchoPrefab);
            echo.Initialize();
            return echo;
        }
    }
}
