namespace MarkedUp.OpenMetrics
{
    /// <summary>
    /// Computes the value of a <see cref="ICounter"/> metric described by its related <see cref="IMetricType"/>
    /// </summary>
    public interface IMetricCalculator
    {
        IMetricType MetricType { get; }
    }
}