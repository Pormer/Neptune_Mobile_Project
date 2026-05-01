using Unity.Cinemachine;
using UnityEngine;

namespace Code.Core
{
    public class CameraSizeSetter : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinema;
        [SerializeField] private float defaultHeight = 10;
        
        private void Awake()
        {
            float currentAspect = (float)Screen.width / (float)Screen.height;
            float height = Mathf.Clamp(defaultHeight / currentAspect, 18f, float.MaxValue);
            
            cinema.Lens.OrthographicSize = height / 2f;
            
        }
    }
}