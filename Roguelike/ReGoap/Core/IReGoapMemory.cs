namespace ReGoap.Core
{
    internal interface IReGoapMemory<T, W>
    {
        ReGoapState<T, W> GetWorldState();
    }
}