using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DesafioItix.Data;
using DesafioItix.Models;
using DesafioItix.Controllers;

namespace UnitTests
{
    [TestClass]
    public class ConsultasControllerTest
    {
        [TestMethod]
        public async Task TestGetConsultas()
        {
            //Arrange   
            DbContextOptions<DesafioContext> options = new DbContextOptionsBuilder<DesafioContext>()
                            .UseInMemoryDatabase(databaseName: "DesafioContext").Options;
            DesafioContext desafioContext = new DesafioContext(options);
            DataRepository<Consulta> consultaRepository = new DataRepository<Consulta>(desafioContext);
            ConsultasController consultasController = new ConsultasController(desafioContext, consultaRepository);

            Consulta expectedConsulta1 = new Consulta
            {
                Nome = "Cartola",
                DataFinal = DateTime.Now.AddDays(8),
                DataInicial = DateTime.Now.AddDays(7),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste Cartola"
            };

            Consulta expectedConsulta2 = new Consulta
            {
                Nome = "Vinícius de Moraes",
                DataFinal = DateTime.Now.AddDays(10),
                DataInicial = DateTime.Now.AddDays(9),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste Vinícius de Moraes"
            };

            consultaRepository.Add(expectedConsulta1);
            consultaRepository.Add(expectedConsulta2);
            Consulta savedConsulta1 = await consultaRepository.SaveAsync(expectedConsulta1);
            Consulta savedConsulta2 = await consultaRepository.SaveAsync(expectedConsulta2);

            //Act
            IEnumerable<Consulta> consultas = consultasController.GetConsultas();

            //Assert            
            Assert.AreEqual(expectedConsulta1, consultas.First(c => c.Id == savedConsulta1.Id));
            Assert.AreEqual(expectedConsulta2, consultas.First(c => c.Id == savedConsulta2.Id));
        }


        [TestMethod]
        public async Task TestGetConsulta()
        {
            //Arrange   
            DbContextOptions<DesafioContext> options = new DbContextOptionsBuilder<DesafioContext>()
                            .UseInMemoryDatabase(databaseName: "DesafioContext").Options;
            DesafioContext desafioContext = new DesafioContext(options);
            DataRepository<Consulta> consultaRepository = new DataRepository<Consulta>(desafioContext);
            ConsultasController consultasController = new ConsultasController(desafioContext, consultaRepository);

            Consulta expectedConsulta = new Consulta
            {
                Nome = "José de Alencar",
                DataFinal = DateTime.Now.AddDays(10),
                DataInicial = DateTime.Now.AddDays(5),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste"
            };

            consultaRepository.Add(expectedConsulta);
            Consulta savedConsulta = await consultaRepository.SaveAsync(expectedConsulta);

            //Act
            IActionResult actionResult = consultasController.GetConsulta(savedConsulta.Id).Result;

            //Assert
            var result = actionResult as OkObjectResult;
            Consulta consulta = result.Value as Consulta;
            Assert.AreEqual(expectedConsulta, consulta);
        }

        [TestMethod]
        public async Task TestPutConsulta()
        {
            //Arrange   
            DbContextOptions<DesafioContext> options = new DbContextOptionsBuilder<DesafioContext>()
                            .UseInMemoryDatabase(databaseName: "DesafioContext").Options;
            DesafioContext desafioContext = new DesafioContext(options);
            DataRepository<Consulta> consultaRepository = new DataRepository<Consulta>(desafioContext);
            ConsultasController consultasController = new ConsultasController(desafioContext, consultaRepository);

            Consulta originalConsulta = new Consulta
            {
                Nome = "João Bosco",
                DataFinal = DateTime.Now.AddDays(10),
                DataInicial = DateTime.Now.AddDays(9),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste João Bosco"
            };

            consultaRepository.Add(originalConsulta);
            Consulta savedConsulta = await consultaRepository.SaveAsync(originalConsulta);
            desafioContext.Entry(savedConsulta).State = EntityState.Detached;

            Consulta updatedConsulta = new Consulta
            {
                Id = savedConsulta.Id,
                Nome = "João Gilberto",
                DataFinal = DateTime.Now.AddDays(5),
                DataInicial = DateTime.Now.AddDays(4),
                DataNascimento = DateTime.Now.AddDays(365),
                Observacoes = "Teste João Gilberto"
            };

            //Act
            await consultasController.PutConsulta(updatedConsulta.Id, updatedConsulta);

            //Assert
            IActionResult actionResult = consultasController.GetConsulta(updatedConsulta.Id).Result;
            var result = actionResult as OkObjectResult;
            Consulta consulta = result.Value as Consulta;
            Assert.AreEqual(updatedConsulta, consulta);
        }

        [TestMethod]
        public void TestPostConsulta()
        {
            //Arrange   
            DbContextOptions<DesafioContext> options = new DbContextOptionsBuilder<DesafioContext>()
                            .UseInMemoryDatabase(databaseName: "DesafioContext").Options;
            DesafioContext desafioContext = new DesafioContext(options);
            DataRepository<Consulta> consultaRepository = new DataRepository<Consulta>(desafioContext);
            ConsultasController consultasController = new ConsultasController(desafioContext, consultaRepository);

            Consulta expectedConsulta = new Consulta
            {
                Nome = "Milton Nascimento",
                DataFinal = DateTime.Now.AddDays(10),
                DataInicial = DateTime.Now.AddDays(5),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste"
            };

            //Act
            IActionResult actionResult = consultasController.PostConsulta(expectedConsulta).Result;

            //Assert
            var result = actionResult as OkObjectResult;
            Consulta consulta = result.Value as Consulta;

            Assert.AreEqual(expectedConsulta.Nome, consulta.Nome);
            Assert.AreEqual(expectedConsulta.DataFinal, consulta.DataFinal);
            Assert.AreEqual(expectedConsulta.DataInicial, consulta.DataInicial);
            Assert.AreEqual(expectedConsulta.DataNascimento, consulta.DataNascimento);
            Assert.AreEqual(expectedConsulta.Observacoes, consulta.Observacoes);
        }

        [TestMethod]
        public async Task TestPostValidateConsultaAsync()
        {
            //Arrange   
            DbContextOptions<DesafioContext> options = new DbContextOptionsBuilder<DesafioContext>()
                            .UseInMemoryDatabase(databaseName: "DesafioContext").Options;
            DesafioContext desafioContext = new DesafioContext(options);
            DataRepository<Consulta> consultaRepository = new DataRepository<Consulta>(desafioContext);
            ConsultasController consultasController = new ConsultasController(desafioContext, consultaRepository);

            Consulta existingConsulta = new Consulta
            {
                Nome = "Dorival Caymmi",
                DataFinal = DateTime.Now.AddDays(10),
                DataInicial = DateTime.Now.AddDays(9),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste"
            };

            consultaRepository.Add(existingConsulta);
            Consulta savedConsulta = await consultaRepository.SaveAsync(existingConsulta);

            Consulta consultaWithAnotherAtTheSameTime = new Consulta
            {
                Nome = "Pixinguinha",
                DataFinal = DateTime.Now.AddDays(10),
                DataInicial = DateTime.Now.AddDays(9),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste"
            };

            Consulta noProblemConsulta = new Consulta
            {
                Nome = "Elis Regina",
                DataFinal = DateTime.Now.AddDays(2),
                DataInicial = DateTime.Now.AddDays(3),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste"
            };

            //Act
            bool consultaWithAnotherAtTheSameTimeValidation = consultasController.PostValidateConsulta(consultaWithAnotherAtTheSameTime);
            bool noProblemConsultaValidation = consultasController.PostValidateConsulta(noProblemConsulta);

            //Assert            
            Assert.AreEqual(consultaWithAnotherAtTheSameTimeValidation, false);
            Assert.AreEqual(noProblemConsultaValidation, true);
        }

        [TestMethod]
        public async Task TestDeleteConsulta()
        {
            //Arrange   
            DbContextOptions<DesafioContext> options = new DbContextOptionsBuilder<DesafioContext>()
                            .UseInMemoryDatabase(databaseName: "DesafioContext").Options;
            DesafioContext desafioContext = new DesafioContext(options);
            DataRepository<Consulta> consultaRepository = new DataRepository<Consulta>(desafioContext);
            ConsultasController consultasController = new ConsultasController(desafioContext, consultaRepository);

            Consulta expectedConsulta = new Consulta
            {
                Nome = "Luiz Gonzaga",
                DataFinal = DateTime.Now.AddDays(10),
                DataInicial = DateTime.Now.AddDays(5),
                DataNascimento = DateTime.Now.AddDays(3650),
                Observacoes = "Teste"
            };

            consultaRepository.Add(expectedConsulta);
            Consulta savedConsulta = await consultaRepository.SaveAsync(expectedConsulta);

            //Act
            IActionResult actionResult = consultasController.DeleteConsulta(savedConsulta.Id).Result;

            //Assert
            Consulta consulta = await desafioContext.Consulta.FindAsync(savedConsulta.Id);
            Assert.IsTrue(savedConsulta != null);
            Assert.AreEqual(consulta, null);
        }
    }
}
