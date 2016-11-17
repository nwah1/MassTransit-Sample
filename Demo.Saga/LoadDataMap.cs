using MassTransit.EntityFrameworkIntegration;

public class LoadDataMap : SagaClassMapping<LoadData>
{
    public LoadDataMap()
    {
        Property(x => x.CurrentState.ToString()).IsRequired();

        Property(x => x.Id).IsRequired();

        Property(x => x.TimeOfRequest).IsRequired();
    }
}