using MsEmail.Infra.Context;

namespace MsEmail.Tests.Architecture;
public class InfraTests
{
    private static readonly Assembly InfraAssembly = typeof(AppDbContext).Assembly;
    
    [Test]
    public void InfraAssembly_ShouldDepend_OfDomainAndCommon()
    {
        var result = Types.InAssembly(InfraAssembly)
            .That()
            .ResideInNamespace(nameof(MSEmail.Infra))
            .Should()
            .HaveDependencyOtherThan(
                nameof(MSEmail.Domain),
                nameof(MSEmail.Common))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void Repositories_ShouldHaveNameEnding_WithRepository()
    {
        var result = Types.InAssembly(InfraAssembly)
            .That()
            .ResideInNamespace(nameof(MSEmail.Infra.Repository))
            .Should()
            .HaveNameEndingWith("Repository")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
