using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public abstract class TypedEchoable<T> : Echoable
    {
        public override Echo CreateEcho ()
        {
            return CreateTypedEcho();
        }

        public override object GetState ()
        {
            return GetTypedState();
        }

        protected abstract TypedEcho<T> CreateTypedEcho ();

        protected abstract T GetTypedState ();
    }
}
