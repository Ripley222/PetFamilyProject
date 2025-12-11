namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

public record FilePath
{
    public string Value { get; }

    private FilePath(string value)
    {
        Value = value;
    }

    public static FilePath Create(Guid path, string extension)
    {
        return new FilePath(path + extension);
    }
    
    public static FilePath ParseOrGenerate(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return new FilePath(Guid.NewGuid().ToString());
        
        return new FilePath(Path.GetFileNameWithoutExtension(fileName) + Path.GetExtension(fileName));
    }
}