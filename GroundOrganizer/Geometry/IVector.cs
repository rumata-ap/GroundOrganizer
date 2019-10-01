namespace Geo
{
    public interface IVector
    {
        int N { get; }

        double[] ToArray();
    }
}