# Testes HTTP - API Livro

Coleção de arquivos `.http` para testar todos os endpoints da API utilizando a extensão REST Client do VS Code.

## 📋 Pré-requisitos

1. **VS Code** com a extensão **REST Client** instalada
2. **API rodando** em `http://localhost:5000`
3. **Docker** com os containers ativos (ou API rodando localmente)

## 🚀 Como Usar

### Opção 1: Ordem Recomendada (Criação de Dados Completos)

Execute os arquivos nesta ordem para criar um conjunto completo de dados de teste:

1. **`tipos-compra.http`** - Verificar tipos de compra (dados de seed)
2. **`autores.http`** - Criar autores
3. **`assuntos.http`** - Criar assuntos
4. **`livros.http`** - Criar livros (requer IDs de autores e assuntos)
5. **`relatorio.http`** - Visualizar relatório completo

### Opção 2: Teste Individual por Contexto

Você pode testar cada contexto separadamente:

- **Autores**: Abra `autores.http` e execute as requisições
- **Assuntos**: Abra `assuntos.http` e execute as requisições
- **Livros**: Abra `livros.http` (requer autores e assuntos criados previamente)
- **Relatório**: Abra `relatorio.http` (requer livros criados)

## 📁 Arquivos Disponíveis

### `autores.http`
- ✅ Criar múltiplos autores (Machado de Assis, Clarice Lispector, etc.)
- ✅ Listar todos os autores
- ✅ Atualizar autor
- ✅ Deletar autor
- ✅ Testes de validação (nome vazio, muito curto, muito longo)

### `assuntos.http`
- ✅ Criar múltiplos assuntos (Ficção, Romance, Poesia, etc.)
- ✅ Listar todos os assuntos
- ✅ Atualizar assunto
- ✅ Deletar assunto
- ✅ Testes de validação (descrição vazia, muito curta, muito longa)

### `tipos-compra.http`
- ✅ Listar tipos de compra (dados de seed - read-only)
- ℹ️ Retorna 4 registros: Balcão, Self-Service, Internet, Telefone

### `livros.http`
- ✅ Criar múltiplos livros com autores, assuntos e valores
- ✅ Listar todos os livros
- ✅ Buscar livro por ID
- ✅ Atualizar livro (incluindo relacionamentos N:N)
- ✅ Deletar livro
- ✅ Testes de validação completos
- ⚠️ **Requer IDs de autores, assuntos e tipos de compra**

### `relatorio.http`
- ✅ Obter relatório completo de livros
- ✅ Visualizar dados da VIEW `vw_relatorio_livros`
- ℹ️ Mostra autores e assuntos concatenados + preços por tipo de compra

## 🔧 Usando Variáveis

Os arquivos utilizam variáveis para facilitar os testes:

```http
### Definir variável da resposta
# @name createAutor
POST {{baseUrl}}/api/autor
Content-Type: application/json

{
  "nome": "Machado de Assis"
}

### Usar variável em requisição seguinte
@autorId = {{createAutor.response.body.codAu}}

PUT {{baseUrl}}/api/autor/{{autorId}}
```

## ⚠️ Importante para `livros.http`

O arquivo `livros.http` contém **placeholders** que devem ser substituídos pelos IDs reais:

```http
"autoresIds": [
  "AUTOR_ID_AQUI"  // ← Substitua pelo ID real do autor
],
"assuntosIds": [
  "ASSUNTO_ID_AQUI"  // ← Substitua pelo ID real do assunto
],
"valores": [
  {
    "tipoCompraId": "TIPO_COMPRA_ID_BALCAO",  // ← Substitua pelo ID real
    "valor": 45.90
  }
]
```

**Como obter os IDs:**
1. Execute `GET {{baseUrl}}/api/autor` → copie o `codAu`
2. Execute `GET {{baseUrl}}/api/assunto` → copie o `codAs`
3. Execute `GET {{baseUrl}}/api/tipo-compra` → copie o `codTc`

## 📊 Validações Testadas

Cada arquivo inclui seção de **Testes de Validação** que verificam:

- ✅ Campos obrigatórios vazios
- ✅ Tamanhos mínimos e máximos
- ✅ Formatos de dados (ano com 4 dígitos)
- ✅ Regras de negócio (mínimo 1 autor, mínimo 1 assunto)
- ✅ Edição >= 1

## 🎯 Casos de Uso Principais

### Cenário 1: Cadastro Completo de um Livro
```
1. POST /api/autor → Criar "Machado de Assis"
2. POST /api/assunto → Criar "Ficção"
3. GET /api/tipo-compra → Obter IDs dos tipos
4. POST /api/livro → Criar "Dom Casmurro" com todos os relacionamentos
5. GET /api/relatorio/livro → Visualizar no relatório
```

### Cenário 2: Atualização de Livro com Novos Relacionamentos
```
1. POST /api/autor → Criar segundo autor
2. POST /api/assunto → Criar segundo assunto
3. PUT /api/livro/{id} → Adicionar novos autores e assuntos
4. GET /api/livro/{id} → Verificar alterações
```

### Cenário 3: Teste de Validações
```
1. Execute cada requisição da seção "TESTES DE VALIDAÇÃO"
2. Verifique que todas retornam 400 Bad Request
3. Valide as mensagens de erro retornadas
```

## 🐛 Troubleshooting

**Problema**: Erro 404 ao executar requisições
- **Solução**: Verifique se a API está rodando em `http://localhost:5000`

**Problema**: Erro 400 ao criar livro
- **Solução**: Verifique se substituiu os placeholders pelos IDs reais

**Problema**: Relatório vazio
- **Solução**: Certifique-se de ter criado pelo menos 1 livro completo

**Problema**: Variáveis não funcionam
- **Solução**: Execute as requisições na ordem (a variável `@name` precisa ser executada antes)

## 📚 Referências

- [REST Client Extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- [HTTP File Format](https://www.jetbrains.com/help/idea/http-client-in-product-code-editor.html)
- [Swagger UI](http://localhost:5000/swagger)
