# Sistema de Gestão de Gastos Residenciais

## Objetivo

Este sistema tem como objetivo auxiliar na gestão de gastos de uma residência, seja ela alugada ou própria. Ele permite o gerenciamento de moradores e o rateio de custos entre eles.

## Requisitos Funcionais

### 1. Gestão de Usuários

* **1.0 Cadastro de Moradores:** O sistema deve permitir o cadastro de novos moradores.
* **1.1 Perfis de Moradores:** Existem três perfis de moradores:
    * `Administrador`: Possui visualização e acesso total a todas as funcionalidades do sistema.
    * `Proprietário`: Possui visualização apenas da residência que cadastrou e só pode estar vinculado a uma única residência. É o responsável legal pela residência.
    * `Participante`: Possui visualização das informações da residência em que está vinculado, similar ao `Proprietário`, mas não é o responsável legal pela residência.
* **1.2 Permissões do Administrador:** O perfil `Administrador` tem visualização e acesso total ao sistema.
* **1.3 Permissões do Proprietário:** O perfil `Proprietário` tem visualização apenas da residência que cadastrou e só pode estar vinculado a uma residência.
* **1.4 Permissões do Participante:** O perfil `Participante` tem visualização da residência em que está vinculado, assim como o `Proprietário`. A distinção é que o `Proprietário` é o responsável legal pela residência.

### 2. Gestão de Residências

* **2. Cadastro de Residências:** O sistema deve permitir o cadastro de novas residências.
* **2.1 Limite de Participantes:** Cada residência deve ter um número máximo de participantes (`Participantes`).
* **2.2 Visualização de Informações da Residência:** Cada `Participante` pode visualizar informações da residência, como:
    * O responsável legal pela residência (`Proprietário`).
    * Valores de custos compartilhados: água, energia, gás, aluguel.
    * Impostos que são cobrados individualmente de cada morador.
* **2.3 Registro de Despesas:** Cada `Participante` pode registrar despesas que podem ser:
    * Compartilhadas entre os moradores da residência.
    * Individuais de um morador específico.