using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace mj112.Holeworms
{
    public class LiveHoleworm : PrefabEchoable<EchoHoleworm, HolewormState>
    {
        public HolewormHead Head;
        public FloatVariable HorizontalInput;

        protected override HolewormState GetTypedState ()
        {
            var state = Head.NewStateFrom(HorizontalInput.Value);
            Head.ApplyState(state, false);
            return state;
        }
    }
}
