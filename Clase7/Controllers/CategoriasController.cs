using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Clase7.Models;
using System.Web.Management;
using Microsoft.Ajax.Utilities;

namespace Clase7.Controllers
{
    public class CategoriasController : Controller
    {
        // GET: Categorias
        private string Cadena = ConfigurationManager.ConnectionStrings["Cadena"].ConnectionString;

        #region Métodos
        public int GenerarId()
        {
            int codigo = 0;

            SqlConnection cnn = new SqlConnection(Cadena);
            SqlCommand cmd = new SqlCommand("usp_Categoria_GenerarCodigo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cnn.Open();
            codigo = Convert.ToInt32(cmd.ExecuteScalar());
            cnn.Close();

            return codigo;
        }
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            SqlConnection cnn = new SqlConnection(Cadena);
            SqlCommand cmd = new SqlCommand("usp_Categoria_Listar", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Categoria oCategoria = new Categoria()
                {
                    IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                    NombreCategoria = dr["NombreCategoria"].ToString(),
                    Descripcion = dr["Descripcion"].ToString()
                };
                lista.Add(oCategoria);
            }
            cnn.Close();
            return lista;
        }
        public Categoria Seleccionar(int idCategoria)
        {
            Categoria oCategoria = new Categoria();

            SqlConnection cnn = new SqlConnection(Cadena);
            SqlCommand cmd = new SqlCommand("usp_Categoria_Seleccionar", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);
            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                oCategoria.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                oCategoria.NombreCategoria = dr["NombreCategoria"].ToString();
                oCategoria.Descripcion = dr["Descripcion"].ToString();
            }
            cnn.Close();

            return oCategoria;
        }
        public bool Insertar(Categoria oCategoria)
        {
            bool rpta = false;
            try
            {
                SqlConnection cnn = new SqlConnection(Cadena);
                SqlCommand cmd = new SqlCommand("usp_Categoria_Insertar", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategoria", oCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("@NombreCategoria", oCategoria.NombreCategoria);
                cmd.Parameters.AddWithValue("@Descripcion", oCategoria.Descripcion);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                rpta = true;
            }
            catch (Exception)
            {
                rpta= false;
            }
            return rpta;
        }
        public bool Actualizar(Categoria oCategoria)
        {
            bool rpta = false;
            try
            {
                SqlConnection cnn = new SqlConnection(Cadena);
                SqlCommand cmd = new SqlCommand("usp_Categoria_Actualizar", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategoria", oCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("@NombreCategoria", oCategoria.NombreCategoria);
                cmd.Parameters.AddWithValue("@Descripcion", oCategoria.Descripcion);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                rpta = true;
            }
            catch (Exception)
            {
                rpta = false;
            }
            return rpta;
        }
        public bool Eliminar(int idCategoria) 
        { 
            bool rpta = false;
            SqlConnection cnn = null;
            try
            {
                cnn = new SqlConnection(Cadena);
                SqlCommand cmd = new SqlCommand("usp_Categoria_Eliminar", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);
                cnn.Open();
                cmd.ExecuteNonQuery();
                rpta = true;
            }
            catch (Exception)
            {
                rpta = false;
            }
            finally
            {
                cnn.Close();
            }
            return rpta;
        }
        #endregion

        public ActionResult Index()
        {
            return View(Listar());
        }
        public ActionResult Create()
        {
            Categoria oCategoria = new Categoria()
            {
                IdCategoria = GenerarId()
            };
            return View(oCategoria);
        }
        [HttpPost]
        public ActionResult Create(Categoria oCategoria)
        {
            // REALIZAR EL REGISTRO DE LA CATEGORIA
            if (Insertar(oCategoria)){
                // REDIRECCIONAR A LA ACCION Index
                return RedirectToAction("Index");
            }
            else
            {
                return View(oCategoria);
            }            
        }
        public ActionResult Edit(int id)
        {
            return View(Seleccionar(id));
        }
        [HttpPost]
        public ActionResult Edit(Categoria oCategoria)
        {
            if (Actualizar(oCategoria))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Ocurrió un error en la operación.";
                return View(oCategoria);
            }
        }
        public ActionResult Details(int id) 
        {
            return View(Seleccionar(id));
        }
        public ActionResult Delete(int id)
        {
            return View(Seleccionar(id));
        }
        [HttpPost]
        public ActionResult Delete(Categoria oCategoria)
        {
            if (Eliminar(oCategoria.IdCategoria))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Ocurrió un error en la operación.";
                return View(oCategoria);
            }
        }
    }
}