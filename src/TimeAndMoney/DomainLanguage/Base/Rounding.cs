using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace DomainLanguage.Base
{
    /// <summary>
    /// This enum is there for completedness, but it is not used.
    /// <para>To simplify coding, the C# MidpointRounding enum is used instead.</para>
    /// </summary>
    public enum Rounding
    {
        Ceiling,
        Down,
        Floor,
        HalfDown,
        HalfEven,
        HalfUp,
        Unnecessary,
        Up
    }
}
