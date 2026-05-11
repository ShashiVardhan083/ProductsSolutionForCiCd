using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductsSolution.Features.Products.V1.CreateProduct;
using System.ComponentModel.DataAnnotations;
using System.Linq;

[TestClass]
public class CreateProductValidatorTests
{
    private CreateProductValidator createProductValidator = null!; //ensuring to the compiler initialize this before use

    [TestInitialize]
    public void Setup()
    {
        createProductValidator = new CreateProductValidator();
    }

    // Single scenario
    [TestMethod]
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

    // Parameterized test
    [DataTestMethod]
    [DataRow("")]
    [DataRow("a")]
    [DataRow("ab")]
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

    //Multiple invalid prices
    [DataTestMethod]
    [DataRow(10)]
    [DataRow(5)]
    [DataRow(0)]
    [DataRow(-1)] //ms test can not implicitly convert int to decimal
    public void Validate_InvalidPrice_ShouldFail(double price)
    {
        var request = new CreateProductRequest
        {
            Name = "Phone",
            Price = (decimal)price,
            IsAvailable = true
        };

        var result = createProductValidator.Validate(request);

        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Price"));
    }

    // Valid names
    [DataTestMethod]
    [DataRow("Phone")]
    [DataRow("Laptop")]
    [DataRow("Tablet")]
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