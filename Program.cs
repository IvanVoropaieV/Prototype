using System;

public interface IMyCloneable<out T>
{
    T MyClone();
}

public abstract class Entity
{
    public Guid Id { get; }
    public string Name { get; }

    protected Entity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    protected Entity(Entity other)
    {
        Id = other.Id;
        Name = other.Name;
    }

    public override string ToString() => $"{GetType().Name}(Id={Id}, Name={Name})";
}

public abstract class Vehicle : Entity
{
    public int MaxSpeed { get; }

    protected Vehicle(Guid id, string name, int maxSpeed) : base(id, name)
    {
        MaxSpeed = maxSpeed;
    }

    protected Vehicle(Vehicle other) : base(other)
    {
        MaxSpeed = other.MaxSpeed;
    }

    public override string ToString() => $"{GetType().Name}(Id={Id}, Name={Name}, MaxSpeed={MaxSpeed})";
}

public sealed class Car : Vehicle, IMyCloneable<Car>, ICloneable
{
    public int Seats { get; }

    public Car(Guid id, string name, int maxSpeed, int seats) : base(id, name, maxSpeed)
    {
        Seats = seats;
    }

    private Car(Car other) : base(other)
    {
        Seats = other.Seats;
    }

    public Car MyClone() => new Car(this);

    object ICloneable.Clone() => MyClone();

    public override string ToString() => $"Car(Id={Id}, Name={Name}, MaxSpeed={MaxSpeed}, Seats={Seats})";
}

public sealed class Truck : Vehicle, IMyCloneable<Truck>, ICloneable
{
    public int CapacityKg { get; }

    public Truck(Guid id, string name, int maxSpeed, int capacityKg) : base(id, name, maxSpeed)
    {
        CapacityKg = capacityKg;
    }

    private Truck(Truck other) : base(other)
    {
        CapacityKg = other.CapacityKg;
    }

    public Truck MyClone() => new Truck(this);

    object ICloneable.Clone() => MyClone();

    public override string ToString() => $"Truck(Id={Id}, Name={Name}, MaxSpeed={MaxSpeed}, CapacityKg={CapacityKg})";
}

class Program
{
    static void Main()
    {
        var car = new Car(Guid.NewGuid(), "Coupe", 240, 4);
        var carCloneTyped = ((IMyCloneable<Car>)car).MyClone();
        var carCloneStd = (Car)((ICloneable)car).Clone();

        Console.WriteLine("Car Original:   " + car);
        Console.WriteLine("Car MyClone<T>: " + carCloneTyped);
        Console.WriteLine("Car ICloneable: " + carCloneStd);
        Console.WriteLine("Same ref typed: " + ReferenceEquals(car, carCloneTyped));
        Console.WriteLine("Same ref std:   " + ReferenceEquals(car, carCloneStd));
        Console.WriteLine();

        var truck = new Truck(Guid.NewGuid(), "Hauler", 140, 12000);
        var truckCloneTyped = ((IMyCloneable<Truck>)truck).MyClone();
        var truckCloneStd = (Truck)((ICloneable)truck).Clone();

        Console.WriteLine("Truck Original:   " + truck);
        Console.WriteLine("Truck MyClone<T>: " + truckCloneTyped);
        Console.WriteLine("Truck ICloneable: " + truckCloneStd);
        Console.WriteLine("Same ref typed: " + ReferenceEquals(truck, truckCloneTyped));
        Console.WriteLine("Same ref std:   " + ReferenceEquals(truck, truckCloneStd));
        Console.WriteLine();

        Console.WriteLine("IMyCloneable<T> advantages: type-safe return T, no casts, clear contract.");
        Console.WriteLine("IMyCloneable<T> disadvantages: not standard, libraries don't expect it.");
        Console.WriteLine("ICloneable advantages: standard .NET interface, sometimes expected by code.");
        Console.WriteLine("ICloneable disadvantages: returns object, requires casts; unclear shallow/deep contract.");
    }
}
