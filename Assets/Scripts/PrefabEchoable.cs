using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public abstract class PrefabEchoable<TEcho, TState> : TypedEchoable<TState>
        where TEcho : TypedEcho<TState>
    {
        public TEcho EchoPrefab;

        protected override TypedEcho<TState> CreateTypedEcho ()
        {
            var echo = Instantiate(EchoPrefab);
            echo.Initialize();
            return echo;
        }
    }
}
