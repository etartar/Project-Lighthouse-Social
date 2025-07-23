using LighthouseSocial.Domain.Common;

namespace LighthouseSocial.Domain.Enumerations;

public sealed class PhotoCategory : EnumerationBase
{
    private PhotoCategory(int id, string name) : base(id, name) { }

    public static readonly PhotoCategory Sunset = new(1, "Sunset");
    public static readonly PhotoCategory Historical = new(2, "Historical");
    public static readonly PhotoCategory Storm = new(3, "Storm");
    public static readonly PhotoCategory Sundown = new(4, "Sundown");

    public static IEnumerable<PhotoCategory> List() => [Sunset, Sundown, Historical, Storm];

    public static PhotoCategory FromId(int id) =>
        List().FirstOrDefault(x => x.Id == id)
        ?? throw new ArgumentOutOfRangeException(nameof(id));
}