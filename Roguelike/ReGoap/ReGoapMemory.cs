using ReGoap.Core;

namespace ReGoap
{
    internal class ReGoapMemory<T, W> : IReGoapMemory<T, W>
    {
        protected ReGoapState<T, W> state;

        public ReGoapMemory() : base()
        {
            state = ReGoapState<T, W>.Instantiate();
        }

        public virtual void Destroy()
        {
            state.Recycle();
        }

        //#region UnityFunctions
        //protected virtual void Awake()
        //{
        //    state = ReGoapState<T, W>.Instantiate();
        //}

        //protected virtual void OnDestroy()
        //{
        //    state.Recycle();
        //}

        //protected virtual void Start()
        //{
        //}
        //#endregion

        public virtual ReGoapState<T, W> GetWorldState()
        {
            return state;
        }
    }
}
