using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

namespace mj112
{
    public class InputReader : MonoBehaviour
    {
        public FloatVariable HorizontalInput;

        public void OnHorizontalInput (InputAction.CallbackContext context)
        {
            HorizontalInput.Value = context.ReadValue<float>();
        }
    }
}
