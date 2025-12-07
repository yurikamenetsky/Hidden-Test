using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class EndLevelPanelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;

        public void Init(Action onRestart, Action onExit)
        {
            restartButton.onClick.AddListener(() => onRestart?.Invoke());
            exitButton.onClick.AddListener(() => onExit?.Invoke());
        }

        public void Show(bool isWin)
        {
            gameObject.SetActive(true);
            text.text = isWin ? "You won!" : "You Lose (((";
            text.color = isWin ? Color.green : Color.red;
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}
