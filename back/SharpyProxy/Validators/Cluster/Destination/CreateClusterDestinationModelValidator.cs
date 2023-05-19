using FluentValidation;
using SharpyProxy.Models.Cluster.Destination;

namespace SharpyProxy.Validators.Cluster.Destination;

public class CreateClusterDestinationModelValidator : AbstractValidator<CreateClusterDestinationModel>
{
    public CreateClusterDestinationModelValidator()
    {
        RuleFor(destination => destination.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(destination => destination.Address)
            .NotNull()
            .NotEmpty();
    }
}