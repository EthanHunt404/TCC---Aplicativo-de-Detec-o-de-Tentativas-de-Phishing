using System.Reflection;
using DetectorPhishing.Testes;
using Xunit;

namespace DetectorPhishing.Application;

/// <summary>
/// Ponto de entrada da PoC. A entrega atual e a propria suite de testes:
/// executa todos os metodos [Fact] de EmailAnalistTests (dados dummy,
/// sem Capturador nem MongoDB) e reporta o resultado no console.
/// Retorna 0 se tudo passou, 1 se houve falha.
/// </summary>
public static class Program
{
    public static int Main()
    {
        var testes = typeof(EmailAnalistTests)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => m.GetCustomAttribute<FactAttribute>() != null)
            .OrderBy(m => m.MetadataToken)
            .ToList();

        var falhas = 0;

        foreach (var teste in testes)
        {
            try
            {
                teste.Invoke(null, null);
                Console.WriteLine($"[OK]    {teste.Name}");
            }
            catch (TargetInvocationException ex)
            {
                falhas++;
                Console.WriteLine($"[FALHA] {teste.Name}");
                Console.WriteLine($"        {ex.InnerException?.Message}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"{testes.Count - falhas} de {testes.Count} testes passaram.");

        return falhas == 0 ? 0 : 1;
    }
}
