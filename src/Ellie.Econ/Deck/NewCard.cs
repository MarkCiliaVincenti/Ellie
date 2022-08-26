namespace Ellie.Econ;

public abstract record class NewCard<TSuit, TValue>(TSuit suit, TValue value)
    where TSuit : struct, Enum
    where TValue : struct, Enum;