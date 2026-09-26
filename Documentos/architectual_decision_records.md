Registro de Decisões Arquiteturais (ADR)

Aplicativo de Detecção de Tentativas de Phishing por Correio Eletrônico. Documento único, addendado a cada nova decisão — não um arquivo por ADR.

Nº	Decisão	Status
0001	Linguagem e plataforma: C# / .NET	Aceita
0002	Banco de dados: MongoDB	Aceita
0003	Biblioteca de acesso IMAP: MailKit	Aceita
0004	Autenticação: OAuth2 (Google/Microsoft)	Aceita
0005	Arquitetura em camadas (Domain / Application / Infrastructure / Presentation)	Aceita

ADR 0001 — Linguagem e plataforma: C# / .NET

Data: 25/09/2026 
Status: Aceita
Contexto: É necessário escolher a linguagem/plataforma de implementação do aplicativo.
Decisão: C# sobre .NET.
Justificativa: A equipe esta mais familiar com C# e o ambiente .NET

ADR 0002 — Banco de dados: MongoDB

Data: 25/09/2026 
Status: Aceita
Contexto: A base de templates de empresa e a lista de domínios de encurtamento precisam de um meio de armazenamento local (RNF-04).
Decisão: MongoDB, hospedado localmente (localhost), com duas coleções (Templates de empresa; Domínios de encurtamento).
Justificativa: O projeto usa dados nao estruturados e essa database em specifico a equipe possui boa familiaridade

ADR 0003 — Biblioteca de acesso IMAP: MailKit

Data: 25/09/2026 
Status: Aceita
Contexto: É necessário monitorar a caixa de entrada via IMAP sobre SSL/TLS (RF-04).
Decisão: Biblioteca MailKit (C#).
Justificativa: Biblioteca padrão de mercado para IMAP em C#, dento do ambiente .NET, alem da equipe possuir alta familiaridade com a biblioteca em questao

ADR 0004 — Autenticação: OAuth2 (Google/Microsoft)

Data: 25/09/2026 
Status: Aceita
Contexto: O acesso à caixa de entrada real do usuário precisa de autorização, sem armazenar senha (RNF-02).
Decisão: Fluxo OAuth2 padrão dos provedores (Google/Microsoft).
Justificativa: Exigência do próprio provedor para qualquer aplicação de terceiro acessar IMAP; não há alternativa dentro do escopo.

ADR 0005 — Arquitetura em camadas (Domain / Application / Infrastructure / Presentation)

Data: 25/09/2026 
Status: Aceita
Contexto: O roteiro de execução (Etapa 15) e os critérios C10–C12 do AV1 exigem separação de camadas com as dependências apontando para o domínio.
Decisão: Quatro camadas — Domain (regras de negócio, sem infraestrutura), Application (casos de uso, orquestra domínio + repositórios), Infrastructure (implementações concretas: Mongo, MailKit, OAuth2), Presentation (bandeja do sistema, notificações nativas).
Justificativa: Exigência do roteiro e da avaliação. As interfaces de repositório ficam declaradas no Domain (inversão de dependência), implementadas no Infrastructure.