using ProductsSolution.Features.Products.V1.CreateProduct;
using Xunit;

public class CreateProductValidatorTests
{
    private readonly CreateProductValidator createProductValidator;

    public CreateProductValidatorTests()
    {
        createProductValidator = new CreateProductValidator();
    }

    [Fact]
    public void Validate_ValidRequest_ShouldPass()
    {
        var request = new CreateProductRequest
        {
            Name = "Phone",
            Price = 100,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("ab")]
    public void Validate_InvalidName_ShouldFail(string name)
    {
        var request = new CreateProductRequest
        {
            Name = name,
            Price = 100,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.Contains(result.Errors, e => e.PropertyName == "Name"); //Check if there is at least one validation error for the Name field
    }

    [Theory]
    [InlineData(10)]
    [InlineData(5)]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidPrice_ShouldFail(decimal price)
    {
        var request = new CreateProductRequest
        {
            Name = "Phone",
            Price = price,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.Contains(result.Errors, e => e.PropertyName == "Price");
    }

    [Theory]
    [InlineData("Phone")]
    [InlineData("Laptop")]
    [InlineData("Tablet")]
    public void Validate_ValidNames_ShouldPass(string name)
    {
        var request = new CreateProductRequest
        {
            Name = name,
            Price = 100,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "Name");
    }
}