using MechanicShop.Domain.Common;

namespace MechanicShop.Domain.common;

public abstract class Entity
{

    private readonly List<DomainEvent> _domainEvents = [];
    public Guid Id { get; }

    protected Entity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }

    protected Entity()
    {
    }

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }  
    public void clearDomainEvents()
    {
        _domainEvents.Clear();
    }

}