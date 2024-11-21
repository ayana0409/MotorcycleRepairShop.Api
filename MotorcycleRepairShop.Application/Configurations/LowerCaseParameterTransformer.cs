using Microsoft.AspNetCore.Routing;

namespace MotorcycleRepairShop.Application.Configurations
{
    public class LowerCaseParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            return value?.ToString()?.ToLowerInvariant();
        }
    }
}
