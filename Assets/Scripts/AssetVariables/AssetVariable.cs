namespace AssetVariables
{
    public abstract class AssetVariable<T> : BaseAssetVariable
    {
        public T value;

        public static implicit operator T(AssetVariable<T> a)
        {
            return a.value;
        }
    }
}