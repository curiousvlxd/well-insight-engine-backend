namespace WellInsightEngine.Core.Entities;

public sealed class WellParameter
{
    public Guid WellId { get; set; }
    public Well? Well { get; set; }

    public Guid ParameterId { get; set; }
    public Parameter? Parameter { get; set; }
}