namespace TVR.Service.Core.Math.Filters
{
    public interface IFilter
    {
        float Value { get; }

        void Push(float value);
    }
}
