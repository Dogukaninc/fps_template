using UnityEngine;

namespace _Main.Scripts.InteractionSystem
{
    public abstract class BaseAbstractInteractable : MonoBehaviour, IInteractable
    {
        public virtual void Interact()
        {
            
        }
    }
}