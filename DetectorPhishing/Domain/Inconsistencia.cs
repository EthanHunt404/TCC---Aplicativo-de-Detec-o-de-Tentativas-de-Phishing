namespace DetectorPhishing.Domain;

/// <summary>
/// Descreve um motivo especifico pelo qual uma verificacao falhou
/// (Documento de Arquitetura, 1.2).
/// </summary>
public class Inconsistencia
{
    public string Tipo { get; }
    public string Descricao { get; }

    public Inconsistencia(string tipo, string descricao)
    {
        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("Tipo e obrigatorio", nameof(tipo));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descricao e obrigatoria", nameof(descricao));

        Tipo = tipo;
        Descricao = descricao;
    }
}
