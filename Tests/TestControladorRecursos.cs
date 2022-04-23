using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Controladores;
using ServicioHydrate.Data;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace ServicioHydrate.Tests
{
    public class ControladorRecursosTests
    {

        private DTORecursoInformativo CrearRecurso(int id) 
        {
            return new RecursoInformativo()
            {
                Id = id,
                Titulo = "Titulo del recurso informativo",
                Descripcion = "Descripcion.",
                FechaPublicacion = "2022-04-22T19:49:20Z",
                Url = "https://url-del-recurso.net"
            }.ComoDTO();
        }

        [Fact]
        public async Task GetRecursosInformativos_SinRecursos_RetornaColeccionVacia()
        {
            // Dado
            var coleccionRecursos = new List<DTORecursoInformativo>();

            var stubRepositorio = new Mock<IServicioRecursos>();
            
            stubRepositorio.Setup(repo => repo.GetRecursosAsync())
                .ReturnsAsync(coleccionRecursos);

            var stubLogger = new Mock<ILogger<ControladorRecursosInformativos>>();

            var controlador = new ControladorRecursosInformativos(stubRepositorio.Object, stubLogger.Object);

            // Cuando
            var resultadoHttp = await controlador.GetRecursosInformativos(); 
        
            // Entonces
            Assert.NotNull(resultadoHttp);

            var resultado = resultadoHttp as ObjectResult;

            if (resultado is not null) 
            {
                Assert.IsType<List<DTORecursoInformativo>>(resultado.Value);
                Assert.Empty(resultado.Value as List<DTORecursoInformativo>);
            }
        }

        [Fact]
        public async Task GetRecursosInformativos_RecursosDisponibles_RetornaTodosLosRecursos()
        {
            // Arrange
            var coleccionRecursos = new List<DTORecursoInformativo> {
                CrearRecurso(0),
                CrearRecurso(1),
                CrearRecurso(2),
            };

            var stubRepositorio = new Mock<IServicioRecursos>();
            
            stubRepositorio.Setup(repo => repo.GetRecursosAsync())
                .ReturnsAsync(coleccionRecursos);

            var stubLogger = new Mock<ILogger<ControladorRecursosInformativos>>();

            var controlador = new ControladorRecursosInformativos(stubRepositorio.Object, stubLogger.Object);
            
            // Act
            var resultadoHttp = await controlador.GetRecursosInformativos();

            // Assert
            Assert.NotNull(resultadoHttp);

            var resultado = resultadoHttp as ObjectResult;

            if (resultado is not null) 
            {
                Assert.IsType<List<DTORecursoInformativo>>(resultado.Value);
                Assert.Equal(coleccionRecursos, resultado.Value);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public async void GetRecursoPorId_ExisteUnRecursoConElId_RetornaElRecurso(int idRecurso)
        {
            // Arrange
            var recurso = CrearRecurso(idRecurso);

            var stubRepositorio = new Mock<IServicioRecursos>();
            
            stubRepositorio.Setup(repo => repo.GetRecursoPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(recurso);

            var stubLogger = new Mock<ILogger<ControladorRecursosInformativos>>();

            var controlador = new ControladorRecursosInformativos(stubRepositorio.Object, stubLogger.Object);
            
            // Act
            var resultadoHttp = await controlador.GetRecursoPorId(idRecurso);
        
            // Assert
            Assert.NotNull(resultadoHttp);

            var resultado = resultadoHttp as ObjectResult;

            if (resultado is not null) 
            {
                Assert.Equal(StatusCodes.Status200OK, resultado.StatusCode);
                Assert.IsType<DTORecursoInformativo>(resultado.Value);
                Assert.Equal(recurso, resultado.Value);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(9999)]
        public async void GetRecursoPorId_NoExisteUnRecursoConElId_RetornaResultado404(int idRecurso)
        {
            // Arrange
            var recurso = CrearRecurso(idRecurso + 10);

            var stubRepositorio = new Mock<IServicioRecursos>();
            
            stubRepositorio.Setup(repo => repo.GetRecursoPorIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException());

            var stubLogger = new Mock<ILogger<ControladorRecursosInformativos>>();

            var controlador = new ControladorRecursosInformativos(stubRepositorio.Object, stubLogger.Object);
            
            // Act
            var resultadoHttp = await controlador.GetRecursoPorId(idRecurso);
        
            // Assert
            Assert.NotNull(resultadoHttp);

            var resultado = resultadoHttp as ObjectResult;

            if (resultado is not null) 
            {
                Assert.Equal(StatusCodes.Status404NotFound, resultado.StatusCode);
            }
        }

        [Fact]
        public async Task AgregarRecurso_RegistraRecurso_CuandoEsValido() 
        {
            // Arrange
            var nuevoRecurso = CrearRecurso(0);

            var stubRepositorio = new Mock<IServicioRecursos>();
            
            stubRepositorio.Setup(repo => repo.AgregarNuevoRecursoAsync(It.IsAny<DTORecursoInformativo>()))
                .ReturnsAsync(nuevoRecurso);

            var stubLogger = new Mock<ILogger<ControladorRecursosInformativos>>();

            var controlador = new ControladorRecursosInformativos(stubRepositorio.Object, stubLogger.Object);

            // Act
            var resultadoHttp = await controlador.AgregarRecurso(nuevoRecurso);

            // Assert
            Assert.NotNull(resultadoHttp);

            var resultado = resultadoHttp as ObjectResult;
            
            if (resultado is not null) 
            {
                Assert.Equal(StatusCodes.Status201Created, resultado.StatusCode);
                var recursoCreado = resultado.Value as DTORecursoInformativo;

                if (recursoCreado is not null) 
                {
                    Assert.Equal(nuevoRecurso, recursoCreado);
                    Assert.True(recursoCreado.Id >= 0);
                }
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async Task EliminarRecurso_NoHayUnRecursoConElId_RetornaResultado404(int idRecurso)
        {
            // Arrange
            var stubRepositorio = new Mock<IServicioRecursos>();
            
            stubRepositorio.Setup(repo => repo.EliminarRecursoAsync(It.IsAny<int>()));

            var stubLogger = new Mock<ILogger<ControladorRecursosInformativos>>();

            var controlador = new ControladorRecursosInformativos(stubRepositorio.Object, stubLogger.Object);
            
            // Act
            var resultadoHttp = await controlador.EliminarRecurso(idRecurso);
        
            // Assert
            Assert.NotNull(resultadoHttp);

            var resultado = resultadoHttp as ObjectResult;
            
            if (resultado is not null) 
            {
                Assert.Equal(StatusCodes.Status404NotFound, resultado.StatusCode);
            }
        }
    }
}
