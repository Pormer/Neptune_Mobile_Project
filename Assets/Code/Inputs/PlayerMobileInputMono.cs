using System;
using UnityEngine;

namespace Code.Inputs
{
    public class PlayerMobileInputMono : MonoBehaviour
    {
        [SerializeField] private PlayerMobileInputSO playerInput;

        private void Update()
        {
            playerInput.InputUpdate();
        }

        private void OnDestroy()
        {
            playerInput.ClearInputAction();
        }
    }
}