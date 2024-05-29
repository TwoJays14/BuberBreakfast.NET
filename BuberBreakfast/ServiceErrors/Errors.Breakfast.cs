namespace BuberBreakfast.ServiceErrors;
using ErrorOr;

public static class Errors
{
    public static class Breakfast
    {
        public static Error NotFound => Error.NotFound(
            "Breakfast.NotFound",
            "Breakfast Not found");
    }
}