using DetectorPhishing;
using DetectorPhishing.Domain;
using DetectorPhishing.Application;
using DetectorPhishing.Infrastructure;
using Xunit;

namespace DetectorPhishing.Testes;

public static class EmailAnalistTests
{
    [Fact]
    public static void EmailSemHyperlinkEhIgnorado()
    {
        IConsulta consulta = new ConsultaFake();
        var analista = new EmailAnalist(consulta);
        var conteudo = new ConteudoRelevante(
            "Sem links aqui", "remetente@empresa.com", "Empresa", Array.Empty<string>());

        var relatorio = analista.Analisar(conteudo);

        Assert.Null(relatorio);
    }

    [Fact]
    public static void HyperlinkEncurtadoGeraInconsistencia()
    {
        var consulta = new ConsultaFake
        {
            DominiosEncurtamento = new[] { "bit.ly" },
        };
        var analista = new EmailAnalist(consulta);
        var conteudo = new ConteudoRelevante(
            "conteudo qualquer", "remetente@empresa.com", "Empresa",
            new[] { "http://bit.ly/xyz" });

        var relatorio = analista.Analisar(conteudo);

        Assert.NotNull(relatorio);
        Assert.Contains(relatorio!.Inconsistencias, i => i.Tipo == "LinkEncurtado");
    }

    [Fact]
    public static void EnderecoRemetenteForaDoDominioOficialGeraInconsistencia()
    {
        var consulta = new ConsultaFake
        {
            EmpresasIdentificadas = new[] { "Banco Exemplo" },
            DominioOficial = "banco-exemplo.com.br",
        };
        var analista = new EmailAnalist(consulta);
        var conteudo = new ConteudoRelevante(
            "Banco Exemplo: confirme seus dados", "atendimento@banco-exemplo-fraude.com", "Banco Exemplo",
            new[] { "http://banco-exemplo.com.br/pagina" });

        var relatorio = analista.Analisar(conteudo);

        Assert.NotNull(relatorio);
        Assert.Contains(relatorio!.Inconsistencias, i => i.Tipo == "EnderecoRemetenteDivergente");
    }

    [Fact]
    public static void HyperlinkForaDoDominioOficialGeraInconsistencia()
    {
        var consulta = new ConsultaFake
        {
            EmpresasIdentificadas = new[] { "Banco Exemplo" },
            DominioOficial = "banco-exemplo.com.br",
        };
        var analista = new EmailAnalist(consulta);
        var conteudo = new ConteudoRelevante(
            "Banco Exemplo: confirme seus dados", "atendimento@banco-exemplo.com.br", "Banco Exemplo",
            new[] { "http://banco-exemplo-fraude.com/login" });

        var relatorio = analista.Analisar(conteudo);

        Assert.NotNull(relatorio);
        Assert.Contains(relatorio!.Inconsistencias, i => i.Tipo == "HyperlinkDivergente");
    }

    [Fact]
    public static void SubdominioDoDominioOficialNaoGeraInconsistencia()
    {
        var consulta = new ConsultaFake
        {
            EmpresasIdentificadas = new[] { "Banco Exemplo" },
            DominioOficial = "banco-exemplo.com.br",
        };
        var analista = new EmailAnalist(consulta);
        var conteudo = new ConteudoRelevante(
            "Banco Exemplo: sua fatura chegou", "faturas@contas.banco-exemplo.com.br", "Banco Exemplo",
            new[] { "http://contas.banco-exemplo.com.br/fatura" });

        var relatorio = analista.Analisar(conteudo);

        Assert.Null(relatorio);
    }

    [Fact]
    public static void EmailLegitimoNaoGeraRelatorio()
    {
        var consulta = new ConsultaFake
        {
            EmpresasIdentificadas = new[] { "Banco Exemplo" },
            DominioOficial = "banco-exemplo.com.br",
        };
        var analista = new EmailAnalist(consulta);
        var conteudo = new ConteudoRelevante(
            "Banco Exemplo: sua fatura chegou", "faturas@banco-exemplo.com.br", "Banco Exemplo",
            new[] { "http://banco-exemplo.com.br/fatura" });

        var relatorio = analista.Analisar(conteudo);

        Assert.Null(relatorio);
    }

    [Fact]
    public static void EmpresaNaoIdentificadaSemOutrasInconsistenciasNaoGeraRelatorio()
    {
        var consulta = new ConsultaFake
        {
            EmpresasIdentificadas = Array.Empty<string>(),
        };
        var analista = new EmailAnalist(consulta);
        var conteudo = new ConteudoRelevante(
            "conteudo sem empresa reconhecida", "remetente@desconhecido.com", string.Empty,
            new[] { "http://desconhecido.com/pagina" });

        var relatorio = analista.Analisar(conteudo);

        Assert.Null(relatorio);
    }

    [Fact]
    public static void EmpresaSemTemplateRegistradoNaoGeraInconsistenciaDeDominio()
    {
        var consulta = new ConsultaFake
        {
            EmpresasIdentificadas = new[] { "Empresa Sem Template" },
            DominioOficial = string.Empty,
        };
        var analista = new EmailAnalist(consulta);
        var conteudo = new ConteudoRelevante(
            "Empresa Sem Template avisa algo", "contato@empresa-sem-template.com", "Empresa Sem Template",
            new[] { "http://empresa-sem-template.com/pagina" });

        var relatorio = analista.Analisar(conteudo);

        Assert.Null(relatorio);
    }

    [Fact]
    public static void ConteudoNuloEhIgnorado()
    {
        var analista = new EmailAnalist(new ConsultaFake());

        Assert.Null(analista.Analisar(null!));
    }

    // ---- Ponta a ponta com dados dummy (ConsultaDummy) ----

    [Fact]
    public static void DummyEmailDePhishingGeraRelatorioComTodasAsInconsistencias()
    {
        var analista = new EmailAnalist(new ConsultaDummy());
        var conteudo = new ConteudoRelevante(
            "Prezado cliente, seu Banco Exemplo precisa de confirmacao urgente. Acesse: http://bit.ly/abc123",
            "atendimento@banco-exemplo-seguro.com", "Banco Exemplo",
            new[] { "http://bit.ly/abc123", "http://banco-exemplo-seguro.com/login" });

        var relatorio = analista.Analisar(conteudo);

        Assert.NotNull(relatorio);
        Assert.Contains(relatorio!.Inconsistencias, i => i.Tipo == "LinkEncurtado");
        Assert.Contains(relatorio.Inconsistencias, i => i.Tipo == "EnderecoRemetenteDivergente");
        Assert.Contains(relatorio.Inconsistencias, i => i.Tipo == "HyperlinkDivergente");
    }

    [Fact]
    public static void DummyEmailLegitimoNaoGeraRelatorio()
    {
        var analista = new EmailAnalist(new ConsultaDummy());
        var conteudo = new ConteudoRelevante(
            "Banco Exemplo: sua fatura esta disponivel.",
            "faturas@banco-exemplo.com.br", "Banco Exemplo",
            new[] { "http://banco-exemplo.com.br/faturas" });

        Assert.Null(analista.Analisar(conteudo));
    }

    [Fact]
    public static void DummyEmailSemHyperlinkEhIgnorado()
    {
        var analista = new EmailAnalist(new ConsultaDummy());
        var conteudo = new ConteudoRelevante(
            "Reuniao de equipe amanha as 10h.",
            "colega@empresa-interna.com", string.Empty, Array.Empty<string>());

        Assert.Null(analista.Analisar(conteudo));
    }
}
