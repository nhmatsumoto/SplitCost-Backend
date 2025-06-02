# Requisitos funcionais
ID: RF-0001
Título: Criação de Usuários
Descrição:
O sistema deve permitir que usuários se cadastrem
O cadastro será feito no Keycloak e uma cópia do usuário sera registrada no SQL Server da aplicação.

ID: RF-0002
Título: Criação de Residências
Descrição: 
O sistema deve permitir que cada usuário cadastre uma residência.
Uma residência deve ter um campo chamado OwnerId (que vai armazenar o ID do usuário criador)

ID: RF-0003
Título: Associação de Usuário a uma Residência Única via Membro
Descrição:
O sistema deve permitir que cada usuário esteja vinculado a uma única residência por meio de uma entidade intermediária chamada Member, enquanto uma residência pode conter múltiplos usuários como membros.

