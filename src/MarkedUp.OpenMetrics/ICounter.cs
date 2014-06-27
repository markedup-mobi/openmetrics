namespace MarkedUp.OpenMetrics
{
    /// <summary>
    /// Describes a global interface for counter implementations. 
    /// 
    /// Includes real-time snapshot counters (i.e. what is the current value right now?) and 
    /// historionic time-series (i.e. what were all of the values over the past 30 minutes?)
    /// 
    /// Allows a user to specify:
    /// 
    /// 1. The unique id of an entity - this is an arbitrary value, but it should
    /// ultimately be unique inside a given system.
    /// 
    /// 2. The dimensions of the counter - these are traits that you want to be able to
    /// filter and aggregate metrics on in the future. If your entity was "all users" then
    /// you might want to include dimensions like "country", "language", "browser", etc...
    /// 
    /// 3. The metrics this counter should compute for this entity across all dimensions,
    /// such as "total counted", "total summed", "difference", and more.
    /// </summary>
    public interface ICounter
    {
        /// <summary>
        /// The unique system-wide identity for this counter. Should be high cardinality.
        /// </summary>
        string EntityId { get; }

        /// <summary>
        /// The dimensions for this counter - these are aspects that we can filter over during reports
        /// </summary>
        string[] Dimensions { get; }

        /// <summary>
        /// A set of metrics to 
        /// </summary>
        string[] Metrics { get; }
    }
}
