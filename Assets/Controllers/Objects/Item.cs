using UnityEngine;

namespace Assets.Controllers.Objects
{
    [System.Serializable]
    public class Item
    {
        [SerializeField]
        public int type = -1;
        [SerializeField]
        public int amount = 0;
    }
}
