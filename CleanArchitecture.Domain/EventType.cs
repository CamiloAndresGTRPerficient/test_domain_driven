namespace CleanArchitecture.Domain;

using SafeFleet.MediaManagement.Domain.SharedKernel;

public class EventType : StringEnumeration
{
    public static readonly EventType HistoricalCoCMigrated = new("026", "Historic");
    public static readonly EventType DefaultValue = new("000", "NotMapped");

    private EventType(string id, string name)
        : base(id, name)
    {
    }
}
