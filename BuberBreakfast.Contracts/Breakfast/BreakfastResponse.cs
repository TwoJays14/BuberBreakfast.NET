namespace BuberBreakfast.Contracts.Breakfast;

public record BreakfastResponse(  
    Guid Id,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    DateTime LastModifiedTime,
    List<string> Savoury,
    List<string> Sweet
);