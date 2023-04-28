using System;
using MobileInput;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace UI
{
    public class MainTopBar : MonoBehaviour
    {
        private GameSceneView gameSceneView;
        private InventoryView inventoryView;
        private QuestView questView;

        private void Start()
        {
            gameSceneView = FindObjectOfType<GameSceneView>(true);
            inventoryView = FindObjectOfType<InventoryView>(true);
            questView = FindObjectOfType<QuestView>(true);
        }

        public void OnInventoryClicked()
        {
            gameSceneView.HideGameUI();
            inventoryView.onClosed = () => gameSceneView.ShowGameUI();
            inventoryView.gameObject.SetActive(true);
        }
        
        public void OnQuestClicked()
        {
            gameSceneView.HideGameUI();
            questView.onClosed = () => gameSceneView.ShowGameUI();
            questView.Refresh();
            questView.gameObject.SetActive(true);
        }

    }
}