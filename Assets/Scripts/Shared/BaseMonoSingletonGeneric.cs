using UnityEngine;

namespace PKPL.DiamondRush
{
    public class BaseMonoSingletonGeneric<T> : MonoBehaviour where T : BaseMonoSingletonGeneric<T>
    {
        private static T instance;

        public static T Instance { get { return instance; } }

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
