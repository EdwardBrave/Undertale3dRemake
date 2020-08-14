
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Utils
{
    public class OnCloseAnimEvent: SerializedMonoBehaviour
    {
        public bool isBackwards = false;
        [ListDrawerSettings(HideAddButton = true, DraggableItems = false)]
        public List<DOTweenAnimation> onClose = new List<DOTweenAnimation>();


        private bool _isClosed;
        public void Close()
        {
            _isClosed = true;
            foreach (var anim in onClose)
            {
                if (isBackwards)
                    anim.DOPlayBackwards();
                else
                    anim.DOPlay();
            }
        }

        private void Update()
        {
            if (_isClosed && onClose.All(anim => !anim.tween.IsPlaying()))
                SelfDestroy();
        }

        private void SelfDestroy()
        {
            _isClosed = false;
            foreach (var anim in onClose)
                anim.DOKill();
            Destroy(gameObject);
        }
    }
}