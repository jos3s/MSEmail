using MSEmail.PrepareEmail;

namespace MsEmail.Tests.Architecture.Microservices;
public class PrepareEmailTests
{
    private static readonly Assembly PrepareEmailAssembly = typeof(WorkerCreatedEmail).Assembly;

    [Test]
    public void InfraAssembly_ShouldDepend_OfDomainAndCommonAndInfra()
    {
        var result = Types.InAssembly(PrepareEmailAssembly)
            .That()
            .ResideInNamespace(nameof(MSEmail.PrepareEmail))
            .Should()
            .HaveDependencyOtherThan(

                nameof(MSEmail.Infra),
                nameof(MSEmail.Domain),
                nameof(MSEmail.Common))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
