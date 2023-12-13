using MsEmail.API.Controllers;

namespace MsEmail.Tests.Architecture;
public class ApiTests
{
    private static readonly Assembly ApiAssembly = typeof(EmailController).Assembly;

    [Test]
    public void Api_ShouldNotDepend_OfPrepareEmail()
    {
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace(nameof(MSEmail.API))
            .ShouldNot()
            .HaveDependencyOnAny(nameof(MSEmail.PrepareEmail))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
