using Bombones2025.Entidades.Entidades;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Bombones2025.DatosSql.Repositorios
{
    public class FormaDePagoRepositorio
    {
        private readonly bool _usarCache;
        private List<FormaDePago> FormaPagoCache = new();
        private readonly string _connectionString = null!;
        public FormaDePagoRepositorio(bool usarCache = false)
        {
            _usarCache = usarCache;
            _connectionString = ConfigurationManager.ConnectionStrings["MiConexion"].ToString();
            if (_usarCache)
            {
                LeerDatos();

            }
        }
        public void RecargarCache()
        {
            FormaPagoCache.Clear();
            LeerDatos();
        }
        private void LeerDatos()
        {
            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    cnn.Open();
                    var query = @"SELECT FormaDePagoId, Descripcion FROM FormasDePago ORDER BY Descripcion";
                    using (var cmd = new SqlCommand(query, cnn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FormaDePago fs = ConstruirFormaDePago(reader);
                                FormaPagoCache.Add(fs);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Error al intentar leer las formas de Pago", ex);
            }
        }

        private FormaDePago ConstruirFormaDePago(SqlDataReader reader)
        {
            return new FormaDePago
            {
                FormaDePagoId = reader.GetInt32(0),
                Descripcion = reader.GetString(1)
            };
        }
        public List<FormaDePago> GetLista()
        {
            if (_usarCache)
            {
                return FormaPagoCache;

            }
            List<FormaDePago> lista = new List<FormaDePago>();
            using (var cnn = new SqlConnection(_connectionString))
            {
                cnn.Open();
                string query = @"SELECT FormaDePagoId, Descripcion
                        FROM FormasDePago ORDER BY Descripcion";
                using (var cmd = new SqlCommand(query, cnn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormaDePago fs = ConstruirFormaDePago(reader);
                            lista.Add(fs);
                        }
                    }
                }
            }
            return lista;
        }

        public bool Existe(FormaDePago formaPago)
        {
            if (_usarCache)
            {
                return formaPago.FormaDePagoId == 0 ? FormaPagoCache
                    .Any(c => c.Descripcion.ToLower() == formaPago.Descripcion.ToLower())
                    : FormaPagoCache.Any(c => c.Descripcion.ToLower() == formaPago.Descripcion.ToLower()
                    && c.FormaDePagoId != formaPago.FormaDePagoId);
            }

            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    cnn.Open();
                    string query = formaPago.FormaDePagoId == 0 ? @"SELECT COUNT(*) FROM FormasDePago
                                WHERE LOWER(Descripcion)=@Descripcion"
                        : @"SELECT COUNT(*) FROM FormasDePago
                                WHERE LOWER(Descripcion)=@Descripcion
                                AND FormaDePagoId<>@FormaDePagoId";
                    using (var cmd = new SqlCommand(query, cnn))
                    {
                        cmd.Parameters.AddWithValue("@Descripcion", formaPago.Descripcion);
                        if (formaPago.FormaDePagoId > 0)
                        {
                            cmd.Parameters.AddWithValue("@FormaDePagoId", formaPago.FormaDePagoId);
                        }
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Error al intentar ver si existe una Forma de Pago", ex);
            }
        }

        public void Agregar(FormaDePago formaPago)
        {
            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    cnn.Open();
                    string query = @"INSERT INTO FormasDePago (Descripcion) VALUES (@Descripcion);
                                SELECT SCOPE_IDENTITY();";
                    using (var cmd = new SqlCommand(query, cnn))
                    {
                        cmd.Parameters.AddWithValue("@Descripcion", formaPago.Descripcion);
                        int id = (int)(decimal)cmd.ExecuteScalar();
                        formaPago.FormaDePagoId = id;
                    }
                }
                if (_usarCache)
                {
                    RecargarCache();
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error al intentar agregar una Forma de Pago", ex);
            }
        }

        public void Borrar(int formaDePagoId)
        {
            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    cnn.Open();
                    string query = @"DELETE FROM FormasDePago WHERE FormaDePagoId=@FormaDePagoId";
                    using (var cmd = new SqlCommand(query, cnn))
                    {
                        cmd.Parameters.AddWithValue("@FormaDePagoId", formaDePagoId);
                        cmd.ExecuteNonQuery();
                    }
                }
                if (_usarCache)
                {
                    RecargarCache();
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error al intentar borrar la Forma de Pago", ex);
            }
        }

        public void Editar(FormaDePago formaPago)
        {
            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    cnn.Open();
                    string query = @"UPDATE FormasDePago SET Descripcion=@Descripcion
                    WHERE FormaDePagoId=@FormaDePagoId";
                    using (var cmd = new SqlCommand(query, cnn))
                    {
                        cmd.Parameters.AddWithValue("@Descripcion", formaPago.Descripcion);
                        cmd.Parameters.AddWithValue("@FormaDePagoId", formaPago.FormaDePagoId);
                        cmd.ExecuteNonQuery();
                    }

                }
                if (_usarCache)
                {
                    RecargarCache();
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error al intentar editar una Forma de Pago", ex);
            }
        }







    }
}
