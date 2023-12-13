using MSEmail.Sender;

namespace MsEmail.Tests.Architecture.Microservices;
public class SenderTests
{
    private static readonly Assembly SenderAssembly = typeof(Worker).Assembly;

    [Test]
    public void InfraAssembly_ShouldDepend_OfDomainAndCommonAndInfra()
    {
        var result = Types.InAssembly(SenderAssembly)
            .That()
            .ResideInNamespace(nameof(MSEmail.Sender))
            .Should()
            .HaveDependencyOtherThan(

                nameof(MSEmail.Infra),
                nameof(MSEmail.Domain),
                nameof(MSEmail.Common))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
