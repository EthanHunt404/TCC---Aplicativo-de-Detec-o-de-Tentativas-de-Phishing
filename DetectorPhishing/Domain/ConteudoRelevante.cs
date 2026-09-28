namespace DetectorPhishing.Domain;

/// <summary>
/// Conhece o remetente e os hyperlinks de um e-mail recebido,
/// e diz se contém algum hyperlink (Documento de Arquitetura, 1.1).
/// </summary>
public class ConteudoRelevante
{
    public string ConteudoBruto { get; }
    public string EnderecoRemetente { get; }
    public string EmpresaRemetente { get; }
    public IReadOnlyList<string> Hyperlinks { get; }

    public ConteudoRelevante(
        string conteudoBruto,
        string enderecoRemetente,
        string empresaRemetente,
        IEnumerable<string> hyperlinks)
    {
        if (string.IsNullOrWhiteSpace(conteudoBruto))
            throw new ArgumentException("Conteudo bruto e obrigatorio", nameof(conteudoBruto));

        if (string.IsNullOrWhiteSpace(enderecoRemetente))
            throw new ArgumentException("Endereco do remetente e obrigatorio", nameof(enderecoRemetente));

        ConteudoBruto = conteudoBruto;
        EnderecoRemetente = enderecoRemetente;
        EmpresaRemetente = empresaRemetente ?? string.Empty;
        Hyperlinks = (hyperlinks ?? Enumerable.Empty<string>()).ToList();
    }

    public bool ContemHyperlink() => Hyperlinks.Count > 0;
}
