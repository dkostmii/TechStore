using FluentValidation;
using System.Text.RegularExpressions;

namespace TechStore.Web.Mappers;

public class ShopProfileOptionsValidator : AbstractValidator<ShopProfileOptions>
{
    public ShopProfileOptionsValidator()
    {
        RuleFor(x => x.ProductImagesEndpoint)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("ProductImagesEndpoint must be provided")
            .Must(x => x!.IsAbsoluteUri)
            .WithMessage("ProductImagesEndpoint must be asbsolute URI")
            .ChildRules(v =>
            {
                v.RuleFor(x => x!.Scheme)
                .Must(scheme => scheme == "http" || scheme == "https")
                .WithMessage("ProductImagesEndpoint scheme should be either HTTP or HTTPS");

                var endpointSegmentRegex = new Regex(@"^[A-Za-z]+/?$");

                v.RuleFor(x => x!.Segments)
                    .Must(segments => segments.Length == 2)
                    .Must(segments => segments[0] == "/")
                    .WithMessage("ProductImagesEndpoint must have exactly one endpoint");


                v.RuleFor(x => x!.Segments[1])
                    .NotEmpty()
                    .Must(endpoint => endpointSegmentRegex.IsMatch(endpoint))
                    .WithMessage("Endpoint must consist of alphanumeric characters");
            })
            .WithMessage("ProductImagesEndpoints must be valid endpoint address");
    }
}
