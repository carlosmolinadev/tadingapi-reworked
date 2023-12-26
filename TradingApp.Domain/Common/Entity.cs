namespace TradingApp.Domain.Common;
public abstract class Entity<T>
{
    public T? Id { get; protected set; }

    protected Entity(T id)
    {
        Id = id;
    }

    protected Entity()
    {
        Id = default;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity<T> otherEntity)
        {
            return false;
        }
        return EqualityComparer<T>.Default.Equals(Id, otherEntity.Id);//return entity.Id.GetType() == Id.GetType();
    }

    public bool Equals(Entity<T> other)
    {
        if (other is null)
        {
            return false;
        }
        if (other.GetType() != GetType())
        {
            return false;
        }
        return other.Id.GetType() == Id.GetType();
    }

    public static bool operator ==(Entity<T?> first, Entity<T?> second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity<T?> first, Entity<T?> second)
    {
        return !(first == second);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 41;
    }
}