using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public abstract class TypedEcho<T> : Echo
    {
        public override void ApplyState (object state)
        {
            ApplyState((T) state);
        }

        protected abstract void ApplyState (T state);
    }
}
