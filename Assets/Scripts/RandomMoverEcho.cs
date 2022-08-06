using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public class RandomMoverEcho : TypedEcho<RandomMoverState>
    {
        protected override void ApplyState (RandomMoverState state)
        {
            transform.SetPositionAndRotation(state.Position, state.Rotation);
            transform.localScale = state.LocalScale;
        }
    }
}
