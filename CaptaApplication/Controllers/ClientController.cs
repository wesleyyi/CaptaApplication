using CaptaApplication.Model;
using CaptaApplication.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CaptaApplication.Controllers
{
    public class ClientController : Controller
    {
        [HttpPost]
        [Route("/v1/CreateClient")]
        public IActionResult CreateClient([FromBody] ClientModel client, [FromHeader] string Authorization)
        {

            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            if (Authorization != configuration.GetSection("Authorization").Value)
                return StatusCode(Convert.ToInt32(HttpStatusCode.Unauthorized), new { message = "Invalid credentials." });

            try
            {

                //Instancia classe de conexão com sql server
                SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(configuration.GetSection("ConnectionString").Value);

                //Faz o primeiro insert na tabela principal, caso o cpf ja exista no banco ele vai retornar bad request no catch
                string query = "INSERT clientes(nome, cpf, sexo) VALUES('" + client.Nome + "', '" + client.Cpf + "', '" + client.Sexo + "')";
                if (!sqlConnectionManager.InsertData(query))
                    return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Cpf already registered" });

                //Busca o id do cliente inserido anteriormente com base no cpf
                query = "SELECT id from clientes where cpf = '" + client.Cpf + "'";
                int id_client = sqlConnectionManager.GetIdClient(query);

                //Insere na segunda tabela com base no tipo_cliente enviado no request e no get retornado do banco
                query = "INSERT tipo_cliente(id_cliente, tipo_cliente) VALUES(" + id_client + ", '" + client.TipoCliente + "')";
                sqlConnectionManager.InsertData(query);

                //Insere na terceira tabela com base no situacao_cliente enviado no request e no get retornado do banco
                query = "INSERT situacao_cliente(id_cliente, situacao_cliente) VALUES(" + id_client + ", '" + client.SituacaoCliente + "')";
                sqlConnectionManager.InsertData(query);

                //retorna a confirmação da inserção do cliente
                return StatusCode(Convert.ToInt32(HttpStatusCode.Created), new { message = "Client inserted" });

            }
            catch
            {
                //retorna a confirmação de erro ao tentar inserir o cliente
                return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Problem inserting the client" });
            }

        }

        [HttpPost]
        [Route("/v1/UpdateClient")]
        public IActionResult UpdateClient([FromBody] ClientModel client, [FromHeader] string Authorization)
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            if (Authorization != configuration.GetSection("Authorization").Value)
                return StatusCode(Convert.ToInt32(HttpStatusCode.Unauthorized), new { message = "Invalid credentials." });

            try
            {

                //Instancia classe de conexão com sql server
                SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(configuration.GetSection("ConnectionString").Value);

                //Busca o id do cliente inserido anteriormente com base no cpf
                string query = "SELECT id from clientes where cpf = '" + client.Cpf + "'";
                int id_client = sqlConnectionManager.GetIdClient(query);
                if (id_client == 0)
                {
                    return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Client not exist" });
                }
                if (client.Nome != null)
                {
                    query = "update clientes set nome = '" + client.Nome + "' where cpf = '" + client.Cpf + "'";
                    sqlConnectionManager.UpdateData(query);
                }
                if (client.Sexo != null)
                {
                    query = "update clientes set sexo = '" + client.Sexo + "' where cpf = '" + client.Cpf + "'";
                    sqlConnectionManager.UpdateData(query);
                }
                if (client.SituacaoCliente != null)
                {
                    query = "update situacao_cliente set situacao_cliente = '" + client.SituacaoCliente + "' where id_cliente = '" + id_client + "'";
                    sqlConnectionManager.UpdateData(query);
                }
                if (client.TipoCliente != null)
                {
                    query = "update tipo_cliente set tipo_cliente = '" + client.TipoCliente + "' where id_cliente = '" + id_client + "'";
                    sqlConnectionManager.UpdateData(query);
                }

                return StatusCode(Convert.ToInt32(HttpStatusCode.Created), new { message = "Updated client" });

            }
            catch
            {
                //retorna a confirmação de erro ao tentar atualizar o cliente
                return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Problem updating client data" });
            }
        }

        [HttpGet]
        [Route("/v1/GetClient/{cpf}")]
        public IActionResult GetClient([FromRoute] string cpf, [FromHeader] string Authorization)
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            if (Authorization != configuration.GetSection("Authorization").Value)
                return StatusCode(Convert.ToInt32(HttpStatusCode.Unauthorized), new { message = "Invalid credentials." });

            try
            {

                //Instancia classe de conexão com sql server
                SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(configuration.GetSection("ConnectionString").Value);

                string query = "select nome,cpf,sexo,tipo_cliente, situacao_cliente from clientes inner join tipo_cliente on tipo_cliente.id_cliente = clientes.id inner join situacao_cliente on situacao_cliente.id_cliente = clientes.id where cpf = '" + cpf + "'";
                ClientModel client = sqlConnectionManager.GetClient(query);
                if (client.Cpf == null)
                {
                    return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Client does not exist" });
                }
                return StatusCode(Convert.ToInt32(HttpStatusCode.Created), client);

            }
            catch
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Problem looking for client" });
            }
        }
        [HttpGet]
        [Route("/v1/GetAllClients")]
        public IActionResult GetAllClients([FromHeader] string Authorization)
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            if (Authorization != configuration.GetSection("Authorization").Value)
                return StatusCode(Convert.ToInt32(HttpStatusCode.Unauthorized), new { message = "Invalid credentials." });

            try
            {

                //Instancia classe de conexão com sql server
                SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(configuration.GetSection("ConnectionString").Value);

                string query = "select nome,cpf,sexo,tipo_cliente, situacao_cliente from clientes inner join tipo_cliente on tipo_cliente.id_cliente = clientes.id inner join situacao_cliente on situacao_cliente.id_cliente = clientes.id";
                List<ClientModel> client = sqlConnectionManager.GetClients(query);
                if (client == null)
                {
                    return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Client does not exist" });
                }
                else if (client.Count < 1)
                {
                    return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Client does not exist" });
                }
                return StatusCode(Convert.ToInt32(HttpStatusCode.Created), client);

            }
            catch
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Problem looking for client" });
            }
        }
        [HttpDelete]
        [Route("/v1/DeleteClient/{cpf}")]
        public IActionResult DeleteClient([FromRoute] string cpf, [FromHeader] string Authorization)
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            if (Authorization != configuration.GetSection("Authorization").Value)
                return StatusCode(Convert.ToInt32(HttpStatusCode.Unauthorized), new { message = "Invalid credentials." });

            try
            {

                //Instancia classe de conexão com sql server
                SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(configuration.GetSection("ConnectionString").Value);

                //Busca o id do cliente inserido anteriormente com base no cpf
                string query = "SELECT id from clientes where cpf = '" + cpf + "'";
                int id_client = sqlConnectionManager.GetIdClient(query);
                if (id_client == 0)
                {
                    return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Client not exist" });
                }
                query = "delete tipo_cliente where id_cliente = '" + id_client + "'";
                sqlConnectionManager.DeleteData(query);
                query = "delete situacao_cliente where id_cliente = '" + id_client + "'";
                sqlConnectionManager.DeleteData(query);
                query = "delete clientes where id = '" + id_client + "'";
                sqlConnectionManager.DeleteData(query);

                return StatusCode(Convert.ToInt32(HttpStatusCode.Created), new { message = "Client deleted" });

            }
            catch
            {
                //retorna a confirmação de erro ao tentar inserir o cliente
                return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), new { message = "Problem deleting client" });
            }
        }


    }
}
