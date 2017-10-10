namespace Info.MartinDupuis.DomainLanguage.Time
{
    /// <summary>
    /// Some usefull converstion factors.
    /// </summary>
    public enum TimeUnitConversionFactors
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        millisecondsPerSecond =                    1000,
        millisecondsPerMinute =               60 * 1000,
        millisecondsPerHour   =          60 * 60 * 1000,
        millisecondsPerDay    =     24 * 60 * 60 * 1000,
        millisecondsPerWeek   = 7 * 24 * 60 * 60 * 1000,
        monthsPerQuarter = 3,
        monthsPerYear = 12
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    };
}
