using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112.Holeworms
{
    public class EchoHoleworm : TypedEcho<HolewormState>
    {
        public HolewormHead Head;

        protected override void ApplyState (HolewormState state)
        {
            Head.ApplyState(state, Clock.Instance.JumpedThisFrame);
        }
    }
}
