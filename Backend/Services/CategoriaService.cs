using Backend.DataAccess;
using Backend.Models;
using System.Collections.Generic;

namespace Backend.Services
{
    public class CategoriaService
    {
        private readonly CategoriaDAL _categoriaDal;

        public CategoriaService(CategoriaDAL categoriaDal)
        {
            _categoriaDal = categoriaDal;
        }

        public List<CategoriaDTO> ListarCategorias()
        {
            return _categoriaDal.ListarCategorias();
        }
    }
}
