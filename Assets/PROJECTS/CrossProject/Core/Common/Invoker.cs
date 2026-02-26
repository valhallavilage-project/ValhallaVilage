using System;

public struct Invoker
{
    public string Id;

    public static Invoker New()
    {
        return new Invoker { Id = Guid.NewGuid().ToString() };
    }
}