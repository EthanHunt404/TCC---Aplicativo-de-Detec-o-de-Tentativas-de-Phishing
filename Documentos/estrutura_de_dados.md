# Estrutura de Dados

## 1. Banco de dados

A PoC utilizará MongoDB como banco de dados documental. A base será composta por três tipos de documentos:

1. Registro de domínios de encurtamento de links;
2. Index de empresas;
3. Templates específicos de uma empresa.

A base será hospedada localmente.

## 1.1 Justificaçao

A equipe, consistindo de uma unica pessoa com os papeis de projetista, pesquisador e executor, esta acostumado com essa tecnologia, e nenhuma necessidade dentro desse PoC descorda com o uso dessa tecnologia.

## 2. Registro de domínios de encurtamento de links

Cada documento registra um domínio conhecido de serviço de encurtamento de links e os padrões utilizados pelos links encurtados.

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

## 3. Index de empresas

Cada documento registra uma empresa e é utilizado para procurar o nome da empresa sendo impersonada no correio eletrônico.

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

## 4. Padrao de registro de template específico da empresa

Cada documento registra um template específico de uma empresa e seus domínios oficiais.

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

## 5. Fluxo de consulta

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
