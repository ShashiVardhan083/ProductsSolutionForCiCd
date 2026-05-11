using NUnit.Framework;
using ProductsSolution.Features.Products.V1.CreateProduct;
using System.Linq;

[TestFixture]
public class CreateProductValidatorTests
{
    private CreateProductValidator createProductValidator;

    [SetUp]
    public void Setup()
    {
        createProductValidator = new CreateProductValidator();
    }

    //Single scenario -> Test
    [Test]
    public void Validate_ValidRequest_ShouldPass()
    {
        var request = new CreateProductRequest
        {
            Name = "Phone",
            Price = 100,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.IsTrue(result.IsValid);
    }

    //Equivalent to xUnit Theory → TestCase
    [TestCase("")]
    [TestCase("a")]
    [TestCase("ab")]
    public void Validate_InvalidName_ShouldFail(string name)
    {
        var request = new CreateProductRequest
        {
            Name = name,
            Price = 100,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Name"));
    }

    // Multiple invalid prices
    [TestCase(10)]
    [TestCase(5)]
    [TestCase(0)]
    [TestCase(-1)]
    public void Validate_InvalidPrice_ShouldFail(decimal price)
    {
        var request = new CreateProductRequest
        {
            Name = "Phone",
            Price = price,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Price"));
    }

    // Valid names
    [TestCase("Phone")]
    [TestCase("Laptop")]
    [TestCase("Tablet")]
    public void Validate_ValidNames_ShouldPass(string name)
    {
        var request = new CreateProductRequest
        {
            Name = name,
            Price = 100,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "Name"));
    }
}