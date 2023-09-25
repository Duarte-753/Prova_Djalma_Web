using MySql.Data.MySqlClient;
using Prova_web.Models;
using System;
using MySql.Data.MySqlClient;
using Prova_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;


namespace Prova_web.Controllers
{
    public class MarcaController : Controller
    {

        public ActionResult Index()
        {
            using (var conexao = new Conexao())
            {
                string strMarca = "SELECT * FROM marcas  WHERE isExcluido = false;";
                using (var comando = new MySqlCommand(strMarca, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                    {
                        var lstMarca = new List<Marca>();

                        while (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Marca_M = Convert.ToString(dr["marca_M"]),

                            };

                            lstMarca.Add(marca);
                        }
                        return View(lstMarca);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Index");
                    }
                }
            }

        }

        public ActionResult NovoMarca()
        {

            return View();
        }

        public ActionResult Edit(int Id)
        {
            using (var conexao = new Conexao())
            {
                string strMarcas = "SELECT * FROM marcas " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strMarcas, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var marca = new Marca
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Marca_M = Convert.ToString(dr["marca_M"]),

                        };
                        return View(marca);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        public ActionResult Visualizar(int Id)
        {
            using (var conexao = new Conexao())
            {
                string strMarcas = "SELECT * FROM marcas " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strMarcas, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var marca = new Marca
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Marca_M = Convert.ToString(dr["marca_M"]),

                        };
                        return View(marca);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        public ActionResult Excluir(int Id)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM marcas " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var marca = new Marca
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Marca_M = Convert.ToString(dr["marca_M"]),

                        };
                        return View(marca);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Excluir(Marca marca)
        {   
            // Não exclue de fato no banco de dados

            using (var conexao = new Conexao())
            {
                string strLogin = "UPDATE marcas SET isExcluido = true " +
                                    "where id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@id", marca.Id);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }


        }

        [HttpPost]
        public ActionResult SalvarAlteracoes(Marca marca)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "UPDATE marcas SET " +

                                    "Marca_M = @marca_M " +

                                    "where id = @Id;";


                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {

                    comando.Parameters.AddWithValue("@marca_M", marca.Marca_M);
                    comando.Parameters.AddWithValue("@id", marca.Id);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public ActionResult SalvarMarca(Marca marca)
        {
            using (var conexao = new Conexao())
            {
                string strMarcas = "INSERT INTO marcas ( Marca_M) " +
                                  "values (" +
                                  " @marca_M);";

                using (var comando = new MySqlCommand(strMarcas, conexao.conn))
                {

                    comando.Parameters.AddWithValue("@marca_M", marca.Marca_M);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }
    }
}