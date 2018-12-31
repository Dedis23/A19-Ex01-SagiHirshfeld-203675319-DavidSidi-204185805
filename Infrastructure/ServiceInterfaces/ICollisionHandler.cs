namespace Infrastructure.ServiceInterfaces
{
    public interface ICollisionHandler
    {
        void HandleCollision(ICollidable i_CollidableA, ICollidable i_CollidableB);
    }
}