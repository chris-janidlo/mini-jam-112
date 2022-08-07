using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

namespace mj112.Holeworms
{
    public class LiveHoleworm : PrefabEchoable<EchoHoleworm, HolewormState>
    {
        public HolewormHead Head;

        float input;

        protected override HolewormState GetTypedState ()
        {
            var state = Head.NewStateFrom(input);
            Head.ApplyState(state, false);
            return state;
        }

        public void OnHorizontalInput (InputAction.CallbackContext context)
        {
            input = context.ReadValue<float>();
        }
    }
}
