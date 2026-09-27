# Documento de Arquitetura

Etapa 19 do Roteiro de Execução (Fase 3). Consolida as classes de domínio, o algoritmo de análise e a estrutura de dados definidos nas etapas 14 a 17. As decisões arquiteturais que fundamentam este documento estão registradas separadamente no ADR.

---

## 1. Classes de Domínio — Regras de Negócio

Etapa 14 do Roteiro de Execução (Fase 2). Lista apenas as classes que representam algo do problema em si — sem interfaces de repositório e sem classes de coordenação/serviço (essas ficam para quando a arquitetura em camadas for desenhada, Etapa 15).

### 1.1 ConteudoRelevante

Conhece o remetente e os hyperlinks de um e-mail recebido, e diz se contém algum hyperlink.

- `ConteudoBruto` (uma cópia do conteúdo bruto)
- `EndereçoRemetente` (endereço de correio eletrônico)
- `EmpresaRemetente` (nome da suposta empresa remetente)
- `Hyperlinks` (lista de `Hyperlink`)

### 1.2 Inconsistencia

Descreve um motivo específico pelo qual uma verificação falhou.

- `Tipo`
- `Descricao`

### 1.3 RelatorioInconsistencias

Reúne as inconsistências encontradas em uma mensagem e registra quando foi gerado.

- `Conteudo` (referência a `ConteudoRelevante.ConteudoBruto`)
- `Inconsistencias` (lista de `Inconsistencia`)
- `DataGeracao`

### 1.4 Classes fora de domínio — implementações sólidas

`Capturador`: (captura o correio eletrônico para uma fila e filtra o conteúdo, entregando um `ConteudoRelevante` apenas quando um hyperlink é encontrado) 
`MongoDBInterface`: (uma interface generica na database MongoDB para facilitar as consultas do `Algoritmo de Analise`)

---

## 2. Algoritmo de Análise

### 2.1 Objetivo

Analisar os correios eletrônicos recebidos e identificar aqueles que apresentam inconsistências nos hyperlinks.

### 2.2 Processamento

```text
CORREIO ELETRÔNICO recebido é capturado

hyperlinks ← verificar existência de hyperlinks
             no conteúdo do correio eletrônico

SE hyperlinks estiver vazio:
    ignorar o correio eletrônico

PARA CADA hyperlink:

    verificar domínio e padrão do hyperlink
    no Registro de domínios de encurtamento

empresa ← identificar empresa sendo impersonada
           no conteúdo do correio eletrônico
           utilizando o Registro de empresas

template ← consultar o Template específico
           da empresa identificada

PARA CADA hyperlink:

    domínio ← extrair domínio do hyperlink
    endereço ← extrair endereço de correio eletrônico

    comparar domínio e endereço com todos os
    domínios oficiais registrados no template

SE houver alguma inconsistência:
    marcar o correio eletrônico como suspeito
    registrar as inconsistências
    gerar relatório
    notificar o usuário

CASO CONTRÁRIO:
    não marcar o correio eletrônico como suspeito
```

### 2.3 Etapas

#### 2.3.1 Captura

O sistema monitora a caixa de entrada principal e captura os correios eletrônicos recebidos.

A captura não impede o recebimento do correio eletrônico na caixa de entrada principal.

#### 2.3.2 Filtragem

O conteudo do correio eletronico e verificado para existencia de hyperlinks

Correios eletrônicos sem hyperlinks são ignorados.

#### 2.3.3 Verificação de encurtamento

Cada hyperlink capturado é verificado contra o Registro de domínios de encurtamento de links.

A verificação considera os domínios registrados e os padrões de links encurtados associados a eles.

#### 2.3.4 Identificação da empresa

O nome da empresa sendo impersonada dentro do conteudo do correio eletronico é capturado referenciando o Index de empresas.

#### 2.3.5 Consulta do template

Após a identificação da empresa, o Template específico registrado no banco, da empresa é consultado para obter seus domínios oficiais.

#### 2.3.6 Verificação de domínios oficiais

O domínio de cada hyperlink e endereço de correio eletronico e comparado com todos os domínios oficiais registrados no template da empresa identificada.

#### 2.3.7 Resultado

Caso uma das verificações apresente uma inconsistência:

- o correio eletrônico é marcado como suspeito;
- as inconsistências identificadas são registradas;
- é gerado um relatório;
- o usuário é notificado.

Caso não sejam encontradas inconsistências, o correio eletrônico não é marcado como suspeito.

### 2.4 Operações principais

Lista de entidades x operaçao dominante no algoritmo

1. Registro de domínios de encurtamento de links: Consulta
2. Index de empresas: Consulta
3. Templates específicos de uma empresa: Consulta

---

## 3. Estrutura de Dados

### 3.1 Banco de dados

A PoC utilizará MongoDB como banco de dados documental. A base será composta por três tipos de documentos:

1. Registro de domínios de encurtamento de links;
2. Index de empresas;
3. Templates específicos de uma empresa.

A base será hospedada localmente.

#### 3.1.1 Justificação

A equipe, consistindo de uma unica pessoa com os papeis de projetista, pesquisador e executor, esta acostumado com essa tecnologia, e nenhuma necessidade dentro desse PoC descorda com o uso dessa tecnologia.

### 3.2 Registro de domínios de encurtamento de links

Esse documento registra um domínio conhecido de serviço de encurtamento de links e os padrões utilizados pelos links encurtados.

```json
{
  "list": [
	{
		"domain": "bit.ly",
		"patterns": [
			"bit.ly/*"
			...
		]
	},
	...
  ]
}
```

### 3.3 Index de empresas

Esse documento registra uma empresa e é utilizado para procurar o nome da empresa sendo impersonada no correio eletrônico.

```json
{
	companies = [
		EmpresaExemplo1
		EmpresaExemplo2
		...
	]
}
```

Após a identificação da empresa, o registro de template específico correspondente será consultado.

### 3.4 Template de padrao de Registro Empresas

Cada documento seguindo esse padrao, registra uma certa empresa e seus domínios oficiais.

```json
{
  "company": "Empresa Exemplo",
  "official_domains": [
    "empresa.com.br",
    "www.empresa.com.br"
  ]
}
```

Os domínios dos hyperlinks encontrados no correio eletrônico serão comparados com todos os domínios oficiais registrados no template da empresa identificada.

### 3.5 Fluxo de consulta

```text
Correio eletrônico
       │
       ▼
Index de empresas
       │
       │ empresa identificada
       ▼
Registro de template específico
       │
       │ domínios oficiais
       ▼
Comparação com os hyperlinks
```

O Registro de domínios de encurtamento de links é consultado para verificar se os hyperlinks correspondem a domínios ou padrões conhecidos de encurtamento.

### 3.6 Persistência do relatório de inconsistências

O `RelatorioInconsistencias` é o único dado efetivamente gravado pela aplicação, é salvo localmente como arquivo `.txt`, na pasta padrão de documentos do usuário, dentro de uma subpasta `Phishing Reports`.
As demais estruturas descritas neste documento (domínios de encurtamento, index de empresas, templates de empresa) são somente consultadas pela aplicação. 
Qualquer adição, remoção ou atualização nelas é feita manualmente por um operador diretamente na base — a aplicação não possui nenhuma operação de escrita sobre essas coleções
