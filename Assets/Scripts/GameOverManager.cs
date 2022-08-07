using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityAtoms.BaseAtoms;
using NaughtyAttributes;
using DG.Tweening;
using TMPro;
using crass;

namespace mj112
{
    public class GameOverManager : Singleton<GameOverManager>
    {
        public string TitleText, LeftSubtitleText, RightSubtitleText;

        [Scene]
        public string MainMenuScene, RestartScene;

        [Foldout("Animation Parameters")]
        public float CameraZoomTime, BackgroundFadeInTime, TextFadeTime, TitleShowDelay, SubtitleShowDelay;
        [Foldout("Animation Parameters")]
        public Ease CameraZoomEase, BackgroundFadeInEase, TextFadeEase;

        [Foldout("References")]
        public Image UiBackground;
        [Foldout("References")]
        public TextMeshProUGUI Title, LeftSubtitle, RightSubtitle;
        [Foldout("References")]
        public FloatVariable HorizontalInput;

        bool endingGame, acceptingInput;
        event Action gameOver;

        void Awake ()
        {
            SingletonOverwriteInstance(this);

            UiBackground.SetA(0);
            Title.alpha = 0;
            LeftSubtitle.alpha = 0;
            RightSubtitle.alpha = 0;
        }

        void Update ()
        {
            if (!acceptingInput) return;

            if (HorizontalInput.Value < 0) SceneManager.LoadScene(RestartScene);
            if (HorizontalInput.Value > 0) SceneManager.LoadScene(MainMenuScene);
        }

        public void Register (IGameOverListener listener)
        {
            gameOver += listener.OnGameOver;
        }

        public void EndGame ()
        {
            if (endingGame) return;

            endingGame = true;
            gameOver?.Invoke();
            StartCoroutine(endGameRoutine());
        }

        IEnumerator endGameRoutine ()
        {
            Clock.Instance.Stop();

            yield return DOTween.To(() => CameraCache.Main.orthographicSize, x => CameraCache.Main.orthographicSize = x, 0.001f, CameraZoomTime)
                .SetEase(CameraZoomEase)
                .WaitForCompletion();

            yield return UiBackground.DOFade(1, BackgroundFadeInTime)
                .SetEase(BackgroundFadeInEase)
                .WaitForCompletion();

            yield return new WaitForSeconds(TitleShowDelay);

            Title.text = TitleText;
            yield return Title.DOFade(1, TextFadeTime)
                .SetEase(TextFadeEase)
                .WaitForCompletion();

            yield return new WaitForSeconds(SubtitleShowDelay);

            LeftSubtitle.text = LeftSubtitleText;
            RightSubtitle.text = RightSubtitleText;
            var left = LeftSubtitle.DOFade(1, TextFadeTime)
                .SetEase(TextFadeEase)
                .WaitForCompletion();
            var right = RightSubtitle.DOFade(1, TextFadeTime)
                .SetEase(TextFadeEase)
                .WaitForCompletion();
            yield return left;
            yield return right;

            acceptingInput = true;
        }
    }
}
