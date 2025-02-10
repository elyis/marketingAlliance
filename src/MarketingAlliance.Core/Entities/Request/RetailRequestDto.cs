namespace MarketingAlliance.Core.Entities.Request
{
    public class RetailRequestDto
    {
        public Guid[] RetailTypeGuids { get; set; }
        public Guid[] RetailGuids { get; set; }
        public Guid[] DepartmentGuids { get; set; }
        public Guid[] DivisionGuids { get; set; }
        public Guid[] OrganizationGuids { get; set; }
        public bool IsNotNullDivision { get; set; }
        public Guid[] ExpressCalculationProtocolGuids { get; set; }
        public Guid[] DepartmentRegionGuids { get; set; }
        public Guid[] StatusGuids { get; set; }
    }
}