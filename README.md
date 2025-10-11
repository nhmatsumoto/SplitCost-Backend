# SplitCosts – Documento de Requisitos Funcionais

## 📌 Visão Geral

O **SplitCosts** é um sistema de **gestão financeira colaborativa** desenvolvido para grupos de pessoas que compartilham uma mesma **residência**.  
Ele permite o registro de **ganhos, despesas e investimentos**, organizados em categorias **compartilhadas** e **individuais**, fornecendo transparência sobre a saúde financeira **coletiva** e **individual**.

---

## 🎯 Objetivos

- Facilitar o **controle de finanças compartilhadas** entre duas ou mais pessoas.
- Permitir que cada indivíduo também gerencie seus **gastos e investimentos pessoais**.
- Automatizar a **divisão de despesas compartilhadas** entre os membros da residência.
- Fornecer relatórios de:
  - **Saldo individual** → ganhos – despesas individuais – participação em despesas compartilhadas.
  - **Saldo coletivo da residência** → ganhos totais – despesas compartilhadas – investimentos.

---

## 🏛 Estrutura do Sistema

### Entidades Principais

**Residência**  
- Representa o grupo de pessoas que compartilham finanças.  
- Contém os indivíduos e as transações **compartilhadas**.

**Indivíduo**  
- Membro da residência.  
- Registra ganhos, despesas e investimentos individuais.  
- Participa das divisões de despesas compartilhadas.

**Categoria**  
- Classificação de transações.  
- Tipos:
  - **Compartilhada** → despesas coletivas (moradia, contas, alimentação).  
  - **Individual** → despesas pessoais (lazer, hobbies, saúde, etc.).  

**Transação**  
- Registro financeiro (ganho, despesa ou investimento).  
- Pode ser individual (ligada a um indivíduo) ou compartilhada (ligada à residência).  
- Atributos:
  - `tipo` → [ganho | despesa | investimento]  
  - `valor`, `data`, `descricao`  
  - `categoria_id`  
  - `residencia_id` *(se compartilhada)*  
  - `individuo_id` *(se individual)*  

**Divisão**  
- Representa como uma transação compartilhada é repartida entre os membros.  
- Pode ser:
  - **Igualitária** → todos pagam o mesmo valor.  
  - **Proporcional** → baseado em pesos (ex.: renda de cada indivíduo).  

---

## 📐 Modelo Conceitual

### MER (Entidade-Relacionamento)

```mermaid
erDiagram
    RESIDENCIA ||--o{ INDIVIDUO : possui
    RESIDENCIA ||--o{ TRANSACAO : "transações compartilhadas"
    INDIVIDUO ||--o{ TRANSACAO : "transações individuais"
    CATEGORIA ||--o{ TRANSACAO : classifica
    TRANSACAO ||--o{ DIVISAO : gera
    INDIVIDUO ||--o{ DIVISAO : participa

    RESIDENCIA {
        int id
        string nome
        datetime data_criacao
    }

    INDIVIDUO {
        int id
        string nome
        string email
        int residencia_id
    }

    CATEGORIA {
        int id
        string nome
        string tipo
    }

    TRANSACAO {
        int id
        string tipo
        decimal valor
        datetime data
        string descricao
        int categoria_id
        int residencia_id
        int individuo_id
    }

    DIVISAO {
        int id
        int transacao_id
        int individuo_id
        decimal percentual
        decimal valor
    }
