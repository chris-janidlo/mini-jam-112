using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public class RandomMoverEcho : Echo
    {
        public override void ApplyState (object state)
        {
            var castState = (ValueTuple<Vector3, Quaternion, Vector3>) state;

            transform.SetPositionAndRotation(castState.Item1, castState.Item2);
            transform.localScale = castState.Item3;
        }
    }
}
