using System;
using UnityEngine;

namespace Code.Inputs
{
    [CreateAssetMenu(fileName = "MobileInput", menuName = "SO/Input/Mobile", order = 0)]
    public class PlayerMobileInputSO : ScriptableObject
    {
        ///터치
        public event Action OnTapPressedEvent;
        
        ///회면에서 손을 땠을때
        public event Action OnTapReleasedEvent;

        ///더블 터치
        public event Action OnDoubleTapEvent;

        ///홀드(홀드하는 동안 계속 호출)
        public event Action OnHoldEvent;

        ///스와이프(스와이프 방향과 같이 넘김)
        public event Action<Vector2> OnSwipeEvent;

        ///핀치(확대: +1, 축소: -1)
        public event Action<int> OnPinchEvent;

        [SerializeField] private float doubleTapCheckTime;
        [SerializeField] private float holdCheckTime;
        
        private Vector2 _startTouchScreenPosition;
        private Vector3 _prevTouchWorldPosition;
        private float _prevPinchValue;
        
        public Vector2 GetTouchWorldPosition()
        {
            _prevTouchWorldPosition = Camera.main.ScreenToWorldPoint(_startTouchScreenPosition);
            
            return _prevTouchWorldPosition;
        }
        public Vector3 GetTouchWorldPosition(LayerMask targetLayer)
        {
            Ray ray = Camera.main.ScreenPointToRay(_startTouchScreenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit,  Camera.main.farClipPlane, targetLayer))
            {
                _prevTouchWorldPosition = hit.point;
            }

            return _prevTouchWorldPosition;
        }

        public void InputUpdate()
        {
            if (Input.touchCount <= 0) return;
            if (Input.touchCount > 1 && CheckDoubleClickInput()) return;

            Touch touchInfo = Input.GetTouch(0);

            _startTouchScreenPosition = touchInfo.position;

            switch (touchInfo.phase)
            {
                case TouchPhase.Began:
                    OnTapPressedEvent?.Invoke();
                    break;
                case TouchPhase.Moved:
                    Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint(_startTouchScreenPosition);
                    Vector2 prevPosition = _startTouchScreenPosition - touchInfo.deltaPosition;
                    Vector3 previousWorldPoint = Camera.main.ScreenToWorldPoint(prevPosition);
                    
                    Vector3 direction = (currentWorldPoint - previousWorldPoint).normalized;
                    OnSwipeEvent?.Invoke(direction);
                    break;
                case TouchPhase.Stationary:
                    OnHoldEvent?.Invoke();
                    break;
                //더블 탭과 취소
                case TouchPhase.Ended:
                    if(touchInfo.tapCount > 1) OnDoubleTapEvent?.Invoke();
                    OnTapReleasedEvent?.Invoke();
                    break;
                case TouchPhase.Canceled:
                    OnTapReleasedEvent?.Invoke();
                    break;
            }
            
        }
        
        private bool CheckDoubleClickInput()
        {
            var first = Input.GetTouch(0);
            var second = Input.GetTouch(1);
                
            if(first.phase == TouchPhase.Moved || second.phase == TouchPhase.Moved)
            {
                DoubleFingerTouch(first, second);
                return true;
            }

            return false;
        }

        private void DoubleFingerTouch(Touch first,  Touch second)
        {
            float distance = Vector2.Distance(first.position, second.position);
            OnPinchEvent?.Invoke((int)Mathf.Sign(distance - _prevPinchValue));
            _prevPinchValue = distance;
        }

        public void ClearInputAction()
        {
            OnTapPressedEvent = null;
            OnTapReleasedEvent = null;
            OnDoubleTapEvent = null;
            OnHoldEvent = null;
            OnSwipeEvent = null;
            OnPinchEvent = null;
        }
    }
}