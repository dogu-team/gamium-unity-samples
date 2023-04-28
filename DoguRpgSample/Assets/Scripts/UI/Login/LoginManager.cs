using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Static;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Login
{
    public class LoginManager : MonoBehaviour
    {
        public GameObject startCurtain;
        public GameObject loginPanel;
        public GameObject registerPanel;
        public InputField registerInputField;
        public Text registerWarningText;
        private bool isLoginDone;

        void Awake()
        {
            Refresh();
        }

        void Refresh()
        {
            if (PlayerDataController.Instance.data.isLogin)
            {
                OnLoginDone();
            }
            else
            {
                startCurtain.SetActive(true);
                loginPanel.SetActive(true);
                registerPanel.SetActive(false);
            }
        }

        public void OnGuestLoginClicked()
        {
            startCurtain.SetActive(true);
            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
        }

        public void OnRegisterClicked()
        {
            // check if username has space
            if (registerInputField.text.Contains(" "))
            {
                registerWarningText.text = "Username cannot contain space";
                return;
            }

            // check if username is empty
            if (registerInputField.text == string.Empty)
            {
                registerWarningText.text = "Username cannot be empty";
                return;
            }

            PlayerDataController.Instance.SetPlayerData(new PlayerData
            {
                isLogin = true,
                nickName = registerInputField.text,
            });
            InventoryData.Instance.IncrementItem(ItemInfoList.Instance.GetCoin(), 1000);

            OnLoginDone();
        }

        public void OnDeleteAccountClicked()
        {
            PlayerDataController.Instance.SetPlayerData(new PlayerData
            {
                isLogin = false,
                nickName = "Deleted",
            });
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            PlayerDataController.Instance.Reload();
            Refresh();
        }

        public void OnStartClicked()
        {
            if (!isLoginDone)
            {
                return;
            }

            SceneManager.LoadScene("Lobby");
        }


        void OnLoginDone()
        {
            startCurtain.SetActive(false);
            loginPanel.SetActive(false);
            registerPanel.SetActive(false);

            isLoginDone = true;
        }
    }
}