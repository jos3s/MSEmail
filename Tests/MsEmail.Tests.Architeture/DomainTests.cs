using MsEmail.Infra.Context;

namespace MsEmail.Tests.Architecture;
public class DomainTests
{
    private static readonly Assembly DomainAssembly = typeof(AppDbContext).Assembly;

    [Test]
    public void Domain_ShouldDepend_OnlySystem()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ResideInNamespaceContaining(nameof(MSEmail.Domain))
            .ShouldNot()
            .HaveDependencyOtherThan(nameof(System))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
