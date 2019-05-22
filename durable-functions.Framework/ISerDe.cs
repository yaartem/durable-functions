namespace Dodo.Bus
{
    public interface ISerDe<T> {
        byte[] Serialize(T element);
        T Deserialize(byte[] message);
    }
}