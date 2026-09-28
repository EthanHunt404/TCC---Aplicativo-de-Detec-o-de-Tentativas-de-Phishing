namespace DetectorPhishing;

/// <summary>
/// Reune as inconsistencias encontradas em uma mensagem e registra
/// quando foi gerado (Documento de Arquitetura, 1.3).
/// So existe quando ha pelo menos uma inconsistencia - um relatorio
/// vazio nao faz sentido como conceito de dominio.
/// </summary>
public class RelatorioInconsistencias
{
    public string Conteudo { get; }
    public IReadOnlyList<Inconsistencia> Inconsistencias { get; }
    public DateTime DataGeracao { get; }

    public RelatorioInconsistencias(
        string conteudo,
        IEnumerable<Inconsistencia> inconsistencias,
        DateTime dataGeracao)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new ArgumentException("Conteudo e obrigatorio", nameof(conteudo));

        var lista = (inconsistencias ?? Enumerable.Empty<Inconsistencia>()).ToList();
        if (lista.Count == 0)
            throw new ArgumentException("Relatorio precisa de ao menos uma inconsistencia", nameof(inconsistencias));

        Conteudo = conteudo;
        Inconsistencias = lista;
        DataGeracao = dataGeracao;
    }
}
