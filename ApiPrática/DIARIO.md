### 2026-10-08 — Injeção de dependência

- O que estudei: Injeção de dependência: Scoped vs Transient vs Singleton (e por que o DbContext é Scoped)
- O que entendi (com minhas palavras): Utilizar menos New e utilizar a injeção de dependencia.

> ## Conceitos Básicos
>
> Antes de mergulharmos nos tipos específicos, é importante entender o fluxo básico da DI no ASP.NET Core:
>
> 1. **Registro de Serviços** : No método ConfigureServices (ou no Program.cs em .NET 6+), você registra serviços usando o IServiceCollection. (lá nos tempos de Asp.Net Framework, haviam pacotes para gerenciar a injeção de Dependência, como Ninject, Autofac e Castle Windosor).
> 2. **Resolução de Dependências** : O contêiner resolve dependências automaticamente ao injetar instâncias via construtores, métodos ou propriedades.
> 3. **Ciclo de Vida** : O tipo de registro determina o ciclo de vida da instância.

Singleton: Cria uma unica instancia que é compartilhada, globalmente, pique Logger

Scoped: instancia por escopo, usado ali para banco de dados.

Transient: Cria instancia toda vez que o serviço é solicitado. pique Calculadora.

DbContext utiliza scoped por que trabalhamos com escopo, e cada escopo precisamos injetar a dependencia. Não guarde estados nas dependencias

Tem que fazer essa injeção no program.cs

- O que ainda não ficou claro: Em que momento utilizar na vdd, como saber de vdd qual das 3 é importante
  O que devo criar alem das classes ? Interfaces ? CAso tenha duvidas utilize o Transient, implementação facil. Quase nunca é utilizado o Singleton
- Onde apliquei no código (arquivo/commit): Scoped utilizei no controller

### 2026-10-09 — Patch

- O que estudei: Patch
- O que entendi (com minhas palavras): O patch faz com que somente 1 propriedade seja alterada, sem precisar alterar todas
- O que ainda não ficou claro: As vezes dá alguns erros de nullable la na classe, porém não implementei o DTO ainda, estão vou buscar estudar isso
- Onde apliquei no código (arquivo/commit): Apliquei o patch no controller
