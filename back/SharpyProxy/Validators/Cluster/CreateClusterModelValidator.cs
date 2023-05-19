using FluentValidation;
using SharpyProxy.Models.Cluster;
using SharpyProxy.Models.Cluster.Destination;

namespace SharpyProxy.Validators.Cluster;

public class CreateClusterModelValidator : AbstractValidator<CreateClusterModel>
{
    public CreateClusterModelValidator(IValidator<CreateClusterDestinationModel> destinationValidator)
    {
        RuleFor(model => model.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(model => model.Destinations)
            .NotNull()
            .NotEmpty();

        RuleForEach(model => model.Destinations).SetValidator(destinationValidator);
    }
}