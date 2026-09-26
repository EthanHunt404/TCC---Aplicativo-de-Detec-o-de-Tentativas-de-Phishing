# Algoritmo de Análise

## 1. Objetivo

Analisar os correios eletrônicos recebidos e identificar aqueles que apresentam inconsistências nos hyperlinks.

## 2. Processamento

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

## 3. Etapas

### 3.1 Captura

O sistema monitora a caixa de entrada principal e captura os correios eletrônicos recebidos.

A captura não impede o recebimento do correio eletrônico na caixa de entrada principal.

### 3.2 Filtragem

O conteudo do correio eletronico e verificado para existencia de hyperlinks

Correios eletrônicos sem hyperlinks são ignorados.

### 3.3 Verificação de encurtamento

Cada hyperlink capturado é verificado contra o Registro de domínios de encurtamento de links.

A verificação considera os domínios registrados e os padrões de links encurtados associados a eles.

### 3.4 Identificação da empresa

O nome da empresa sendo impersonada dentro do conteudo do correio eletronico é capturado referenciando o Index de empresas.

### 3.5 Consulta do template

Após a identificação da empresa, o Template específico registrado no banco, da empresa é consultado para obter seus domínios oficiais.

### 3.6 Verificação de domínios oficials

O domínio de cada hyperlink e endereço de correio eletronico e comparado com todos os domínios oficiais registrados no template da empresa identificada.

### 3.7 Resultado

Caso uma das verificações apresente uma inconsistência:

- o correio eletrônico é marcado como suspeito;
- as inconsistências identificadas são registradas;
- é gerado um relatório;
- o usuário é notificado.

Caso não sejam encontradas inconsistências, o correio eletrônico não é marcado como suspeito.

## 4 Operaçoes Principais 

Lista de entidades x operaçao dominante no algoritmo

1. Registro de domínios de encurtamento de links: Consulta
2. Index de empresas: Consulta
3. Templates específicos de uma empresa: Consulta


