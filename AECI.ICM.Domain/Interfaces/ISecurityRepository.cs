namespace AECI.ICM.Domain.Interfaces
{
    public interface ISecurityRepository<T>
    {
        T Login(T userDetails);
    }
}
