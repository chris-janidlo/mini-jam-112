using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public class ClockDisplay : MonoBehaviour, IClockFollower, IGameOverListener
    {
        public RectTransform Hand;

        void Start ()
        {
            Clock.Instance.Register(this);
            GameOverManager.Instance.Register(this);
        }

        public void TimedUpdate ()
        {
            Hand.eulerAngles = Clock.Instance.LoopTime * 360 * Vector3.back;
        }

        public void OnGameOver ()
        {
            gameObject.SetActive(false);
        }
    }
}
