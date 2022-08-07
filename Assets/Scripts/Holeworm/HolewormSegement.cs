using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112.Holeworms
{
    public class HolewormSegement : MonoBehaviour
    {
        public Rigidbody2D Rigidbody;

        public void SetLocation (HolewormState.Entry location, bool teleport)
        {
            location.ApplyToRigidbody(Rigidbody, teleport);
        }
    }
}
