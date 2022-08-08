using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace mj112
{
    public class Paradox : MonoBehaviour
    {
        public float SpawnAnimationTime, DeathAnimationTime;
        public Ease SpawnAnimationEase, DeathAnimationEase;

        public Collider2D Collider;

        IEnumerator Start ()
        {
            yield return sizeAnimation(true);
        }

        public void Kill ()
        {
            Collider.enabled = false;

            _scaleTween?.Kill();
            StopAllCoroutines();

            StartCoroutine(sizeAnimation(false));
        }

        Tween _scaleTween;
        IEnumerator sizeAnimation (bool spawn)
        {
            transform.localScale = spawn ? Vector3.zero : Vector3.one;

            float time = spawn ? SpawnAnimationTime : DeathAnimationTime;
            Ease ease = spawn ? SpawnAnimationEase : DeathAnimationEase;

            _scaleTween = transform.DOScale(spawn ? 1 : 0, time)
                .SetEase(ease);

            yield return _scaleTween.WaitForCompletion();

            if (!spawn) Destroy(gameObject);
        }
    }
}
