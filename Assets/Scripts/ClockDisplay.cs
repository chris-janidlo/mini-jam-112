using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public class ClockDisplay : MonoBehaviour, IClockFollower
    {
        public RectTransform Hand;

        void Start ()
        {
            Clock.Instance.Register(this);
        }

        void OnDestroy ()
        {
            Clock.Instance.Deregister(this);
        }

        public void TimedUpdate ()
        {
            Hand.eulerAngles = Clock.Instance.LoopTime * 360 * Vector3.back;
        }
    }
}
