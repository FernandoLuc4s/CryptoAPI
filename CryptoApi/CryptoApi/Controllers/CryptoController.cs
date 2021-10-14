using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;

namespace CryptoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //  crypto
    public class CryptoController : ControllerBase
    {

        private readonly ILogger<CryptoController> _logger;

        public CryptoController(ILogger<CryptoController> logger)
        {
            this._logger = logger;
        }




        [HttpPost("coin")]

        public ActionResult<CryptoEntity> Add([FromBody] Crypto input)
        {
            _logger.LogInformation("Inicio da tentativa de inserir uma nova crypto. {@crypto}", input);
            if (!input.IsValid())
            {
                // se o que foi enviado está inválido então retorna erro
                return BadRequest();
            }

            var output = Save(input);

            if (output is null)
            {
                // significa que tentou inserir e já existia, logo a api precisa sinalizar isso de alguma forma para o cliente
                return UnprocessableEntity($"{input.Code} already exists!");
            }

            return Ok(output);


        }

        [HttpGet("{id}")]
        public ActionResult<CryptoEntity> GetCrypto([FromBody] Crypto input)
        {


            _logger.LogInformation("Inicio da tentativa de Selecionar uma  crypto. {@crypto}", input);


            var output = Selecionar(input);

            return Ok(output);



        }

        private CryptoEntity Save(Crypto crypto)
        {
            var entity = new CryptoEntity
            {
                Code = crypto.Code,
                Name = crypto.Name,
                Site = crypto.Site,
                CreatedAt = DateTime.Now
            };
            //Data Source=.\SQLEXPRESS;Initial Catalog=Cripto;User ID=sa;Password=***********
            //  var connection = new SqlConnection(@"Server=C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL;Database=Cripto;User Id=DESKTOP-HQN1PB4\User;Password=;");
            // var connection = new SqlConnection("Data Source=DESKTOP-HQN1PB4\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            var connection = new SqlConnection(@"Data Source={.\SQLEXPRESS};Initial Catalog={Cripto};Persist Security Info=True;User ID={sa};Password={1324};");



            using (connection)
            {
                connection.Open();

                const string sql = @"IF (NOT EXISTS (SELECT 1 FROM TB_CRIPTOMOEDA WHERE CODIGO = @Code))
                                    BEGIN 
                                        INSERT INTO TB_CRIPTOMOEDA(CODIGO, NOME, SITE, CRIADO_EM) 
                                        VALUES (@Code, @name, @Site, @CreatedAt)

                                        SELECT 
                                        Code = CODIGO,
                                        Name = NOME,
                                        Site = SITE,
                                        CreatedAt = CRIADO_EM
                                        FROM TB_CRIPTOMOEDA
                                        WHERE CODIGO = @Code

                                        END";

                var entityInserted = connection.QueryFirstOrDefault<CryptoEntity>(sql, entity);

                return entityInserted;

            };
        }

        private CryptoEntity Selecionar(Crypto crypto)
        {
            var entity = new CryptoEntity
            {
                Code = crypto.Code
            };
            var connection = new SqlConnection(@"Server=.\SQLEXPRESS;Database=Cripto;User Id=sa;Password=1234;");

            using (connection)
            {
                connection.Open();

                const string sql = @"
                                        SELECT 
                                        Code = CODIGO,
                                        Name = NOME,
                                        Site = SITE,
                                        CreatedAt = CRIADO_EM
                                        FROM TB_CRIPTOMOEDA
                                        WHERE CODIGO = @Code"; //
                                                               //IF (NOT EXISTS (SELECT 1 FROM TB_CRIPTOMOEDA WHERE CODIGO = @Code))
                                                               // BEGIN INSERT INTO TB_CRIPTOMOEDA(CODIGO, NOME, SITE, CRIADO_EM)
                                                               //VALUES(@Code, @name, @Site, @CreatedAt) END

                var entitySelect = connection.QueryFirstOrDefault<CryptoEntity>(sql, entity);

                return entitySelect;

            };
        }
    }
}
