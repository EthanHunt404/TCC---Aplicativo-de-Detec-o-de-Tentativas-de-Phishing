# Classes de Domínio — Regras de Negócio

Etapa 14 do Roteiro de Execução (Fase 2). Lista apenas as classes que representam algo do problema em si — sem interfaces de repositório e sem classes de coordenação/serviço (essas ficam para quando a arquitetura em camadas for desenhada, Etapa 15).

Cada classe passa no teste dos 10 segundos: existiria no papel, o solicitante reconhece pelo nome, sobrevive à troca de banco/protocolo/tela.

---

## ConteudoRelevante

Conhece o remetente e os hyperlinks de um e-mail recebido, e diz se contém algum hyperlink.

- `ConteudoBruto` (uma cópia do conteúdo bruto)
- `EndereçoRemetente` (endereço de correio eletrônico)
- `EmpresaRemetente` (nome da suposta empresa remetente)
- `Hyperlinks` (lista de `Hyperlink`)

## Inconsistencia

Descreve um motivo específico pelo qual uma verificação falhou.

- `Tipo`
- `Descricao`

## RelatorioInconsistencias

Reúne as inconsistências encontradas em uma mensagem e registra quando foi gerado.

- `Conteudo` (referência a `ConteudoRelevante.ConteudoBruto`)
- `Inconsistencias` (lista de `Inconsistencia`)
- `DataGeracao`

---

# Classes Fora de Domínio - Implementaçoes Solidas
`Capturador`: (captura o correio eletrônico para uma fila e filtra o conteúdo, entregando um `ConteudoRelevante` apenas quando um hyperlink é encontrado) 
`MongoDBInterface`: (uma interface generica na database MongoDB para facilitar as consultas do `Algoritmo de Analise`)
