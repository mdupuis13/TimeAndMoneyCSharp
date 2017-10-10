using System;

namespace Info.MartinDupuis.DomainLanguage.Base
{
    /// <summary>
    /// This enum is there for completedness, but it is not used.
    /// <para>To simplify coding, the C# MidpointRounding enum is used instead.</para>
    /// <para>I am thinking it could be used as a reference point to guide implementations 
    /// from clients. The <see cref="Math"/> namespace, <see cref="decimal"/> and 
    /// <see cref="double"/> has some functions that could be used to emulate the java's 
    /// implementation of this enum.</para>
    /// </summary>
    public enum Rounding
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Ceiling,
        Down,
        Floor,
        HalfDown,
        HalfEven,
        HalfUp,
        Unnecessary,
        Up
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
