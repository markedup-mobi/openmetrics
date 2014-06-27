namespace MarkedUp.OpenMetrics
{
    /// <summary>
    /// Describes a type of metric that can be computed by a <see cref="ICounter"/>
    /// </summary>
    public interface IMetricType
    {
        /// <summary>
        /// Instaniates an instance of the <see cref="IMetricCalculator"/> described by this <see cref="IMetricType"/>.
        /// </summary>
        IMetricCalculator Create();
    }
}