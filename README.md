# TesteAcesso

<h3>Abaixo segue o que utilizei para fazer esse projeto:</h3>

<ul>
  <li>Dot net core 2.2</li>
  <li>Injeção de dependencias nativa do framework .net core.</li>
  <li>Banco de dados MySql</li>
  <li>Entity Framework Code First</li>
  <li>Swagger para exibição dos contratos</li>
  <li>Arquitetura em DDD.</li>
  <li>Biblioteca MediatR para cuidar das instancias dos handlers</li>
  <li>FluentValidation para validação dos requests</li>
  <li>Serilog com ElasticSearch para logs</li>
  <li>Kibana para visualização dos logs</li>
  <li>Docker(imagem do MySql, do ElasticSearch e do Kibana)</li>
</ul>

<h3>Resumo da solução:</h3>

<p>Criei 3 endpoints para as seguintes funcionalidades:<p/>

<ul>
  <li>Criar uma nova tranferência</li>
  <li>Consultar o status de uma transferência</li>
  <li>Executar um transferencia. 
    Criei esse endpoint para separar a responsabilidade de criar e executar a transferência, dessa forma a execução pode ser rodada em background offline por algum robo ou alguma outra aplicação interna. </li>  
</ul>

<p>Toda ação é logada no Elastic Search e pode ser consultada no Kibana. Para gravar as transferencias criadas e seus status utilizei o MySql.</p>

<h3>Configuração para subir os containers:</h3>

<p>Executar o comando "docker-compose up -d" dentro da pasta que contem o arquivo <b>docker-compose.yml</b></p>

<h3>Configuração para criação do banco MySql:</h3>

<p>Após subir os containers, rodar o comando abaixo no Package Manager Console substituindo {caminho_da_aplicacao} pelo caminho que o projeto foi salvo na maquina. Esse comando irá criar o banco de dados no container do MySql

"dotnet ef database update --project "{caminho_da_aplicacao}\TesteAcesso.Repository\TesteAcesso.Repository.csproj" --startup-project "{caminho_da_aplicacao}\TesteAcesso.API\TesteAcesso.API.csproj""

Obs.: Criei o Banco Mysql na porta 3310 para evitar conflito caso já exista algum banco na porta default 3306</p>

<h3>Swagger:</h3>

http://localhost:54000/swagger

<h3>Kibana:</h3>

http://localhost:5601

<h3>Elasticsearch:</h3>

http://localhost:9200 
