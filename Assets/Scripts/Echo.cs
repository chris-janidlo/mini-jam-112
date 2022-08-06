using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public abstract class Echo : MonoBehaviour
    {
        public Guid Guid { get; private set; }

        public abstract void ApplyState (object state);

        public virtual void Initialize () { }

        public virtual void AssignGuid (Guid guid)
        {
            Guid = guid;
            name += "[" + guid.ToString() + "]";
        }

        public virtual void SetActive (bool value)
        {
            gameObject.SetActive(value);
        }

        public virtual void Kill ()
        {
            Destroy(gameObject);
        }
    }
}
