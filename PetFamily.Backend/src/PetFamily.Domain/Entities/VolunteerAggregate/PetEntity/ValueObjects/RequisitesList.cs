namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record RequisitesList
{
    public IEnumerable<Requisites> Requisites { get; }

    //ef core ctor
    public RequisitesList()
    {
    }

    private RequisitesList(IEnumerable<Requisites> requisites)
    {
        Requisites = requisites;
    }

    public static RequisitesList Create(IEnumerable<Requisites> requisites)
    {
        return new RequisitesList(requisites);
    }
}