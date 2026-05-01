using System;
using UnityEngine;

namespace Code.Core
{
    public class FrameCounter : MonoBehaviour
    {
        [Range(30, 120)] [SerializeField] private int frame = 60;

        private void Awake()
        {
            Application.targetFrameRate = frame;
        }
    }
}